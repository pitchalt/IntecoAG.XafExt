using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    //public abstract class MdfCategoryValue<Tv> where Tv : MdfCategoryValue<Tv> {

    //    private readonly MdfCategory<Tv> _Category;
    //    public MdfCategory<Tv> Category {
    //        get { return _Category; }
    //    }

    //    protected MdfCategoryValue(MdfCategory<Tv> category) {
    //        _Category = category;
    //    }
    //}
    public abstract class MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp> : IEquatable<Tv> 
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp> 
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private Tdp _DataPoint;
        public Tdp DataPoint {
            get { return _DataPoint; }
            set { _DataPoint = value; }
        }

        public abstract MdfConcept Concept { get; }

        protected MdfCategoryValue() {
        }

        public override bool Equals(object obj) {
            Tv value = obj as Tv;
            if (value == null)
                return false;
            return Equals(value);
        }

        public abstract bool Equals(Tv other);

        //public override int GetHashCode() {
        //    return base.GetHashCode();
        //}
    }

    public abstract class MdfCategory<Tr, Tv, Tt, Tc, Tdp> : IReadOnlyList<Tv>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> 
        {

        private readonly List<Tv> _Values;

        public Tv this[int index] {
            get { return _Values[index]; }
        }

        public int Count {
            get { return _Values.Count; }
        }

        protected MdfCategory() {
            _Values = new List<Tv>(64);
        }

        public IEnumerator<Tv> GetEnumerator() {
            return _Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public abstract class MdfCategoryValues<Tr, Tv, Tt, Tc, Tdp> : IEnumerable<Tv>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> 
        {

        protected readonly Dictionary<Tv, Tv> _Values;

        //public Tv this[int index] {
        //    get { return _Values[index]; }
        //}

        public int Count {
            get { return _Values.Count; }
        }

        protected MdfCategoryValues() {
            //_Values = new List<Tv>(256);
            _Values = new Dictionary<Tv,Tv>(4096);
        }

        public abstract Tv Union(IEnumerable<Tv> values);

        public Tv Locate(Tv value) {
            if (_Values.TryGetValue(value, out Tv result)) {
                return result;
            }
            _Values[value] = value;
            return value;
        }

        public IEnumerator<Tv> GetEnumerator() {
            return _Values.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }


}
