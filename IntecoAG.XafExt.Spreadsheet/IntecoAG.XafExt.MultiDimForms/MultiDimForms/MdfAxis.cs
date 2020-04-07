using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfAxis<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp> 
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        public class MdfAxisLevel {

            private Int32 _Index;
            public Int32 Index {
                get { return _Index; }
            }
            public void IndexSet(Int32 value) {
                _Index = value;
            }

            private readonly MdfAxis<Tr, Tv, Tt, Tc, Tdp> _Axis;
            public MdfAxis<Tr, Tv, Tt, Tc, Tdp> Axis {
                get { return _Axis; }
            }

            List<MdfAxisOrdinate> _Ordinates;
            public IList<MdfAxisOrdinate> Ordinates {
                get {
                    if (_Ordinates == null) {
                        _Ordinates = new List<MdfAxisOrdinate>();
                    }
                    return _Ordinates;
                }
            }

            public MdfAxisLevel(MdfAxis<Tr, Tv, Tt, Tc, Tdp> axis) {
                _Axis = axis;
            }
        }

        public enum MdfAxisOrdinateValueType {
            MODEL = 0,
            OBJECT = 1,
        } 

        public class MdfAxisOrdinate {

            private readonly MdfAxisOrdinateValueType _ValueType;
            public MdfAxisOrdinateValueType ValueType {
                get { return _ValueType; }
            }

            private Int32 _SortOrder;
            public Int32 SortOrder {
                get { return _SortOrder; }
                set { _SortOrder = value; }
            }

            private Boolean _IsIntegrated;
            public Boolean IsIntegrated {
                get { return _IsIntegrated; }
                set { _IsIntegrated = value; }
            }

            private String _Name;
            public String Name {
                get { return _Name; }
                set { _Name = value; }
            }

            private MdfAxisLevel _Level;
            public MdfAxisLevel Level {
                get { return _Level; }
                set {
                    MdfAxisLevel old = _Level;
                    if (value != old) {
                        _Level = value;
                        if (value != null) {
                            value.Ordinates.Add(this);
                        }
                        if (old != null) {
                            old.Ordinates.Remove(this);
                        }
                    }
                }
            }

            //private MdfAxis<Tr,Tv> _AxisLine;
            //public MdfAxis<Tr, Tv> AxisLine {
            //    get { return _AxisLine; }
            //    set {
            //        MdfAxis<Tr, Tv> old = _AxisLine;
            //        if (old != value) {
            //            _AxisLine = value;
            //            if (value != null) {
            //            }
            //        }
            //    }
            //}

            private Int32 _Order;
            public Int32 Order {
                get { return _Order; }
            }
            public void OrderSet(Int32 value) {
                _Order = value;
            }

            private Int32 _LevelLength;
            public Int32 LevelLength {
                get { return _LevelLength; }
            }
            public void LevelLengthSet(Int32 value) {
                _LevelLength = value;
            }

            //[Persistent(nameof(LevelOrder))]
            //private Int32 _LevelOrder;
            //[PersistentAlias(nameof(_LevelOrder))]
            //public Int32 LevelOrder {
            //    get { return _LevelOrder; }
            //}
            //public void LevelOrderSet(Int32 value) {
            //    SetPropertyValue<Int32>(ref _LevelOrder, value);
            //}

            private Int32 _LevelIndex;
            public Int32 LevelIndex {
                get { return _LevelIndex; }
            }
            public void LevelIndexSet(Int32 value) {
                _LevelIndex = value;
            }

            private readonly MdfAxis<Tr, Tv, Tt, Tc, Tdp> _Axis;
            public MdfAxis<Tr, Tv, Tt, Tc, Tdp> Axis {
                get { return _Axis; }
            }

            private MdfAxisOrdinate _Up;
            public MdfAxisOrdinate Up {
                get { return _Up; }
                set {
                    MdfAxisOrdinate old = _Up;
                    if (old != value) {
                        _Up = value;
                        if (value != null) {
                            value.Downs.Add(this);
                        }
                        if (old != null) {
                            old.Downs.Remove(this);
                        }
                    }
                }
            }

            List<MdfAxisOrdinate> _Downs;
            public IList<MdfAxisOrdinate> Downs {
                get {
                    if (_Downs == null) {
                        _Downs = new List<MdfAxisOrdinate>();
                    }
                    return _Downs;
                }
            }

            private Tv _CategoryValue;
            public Tv CategoryValue {
                get { return _CategoryValue; }
                set { _CategoryValue = value; }
            }

            public MdfAxisOrdinate(MdfAxis<Tr, Tv, Tt, Tc, Tdp> axis, MdfAxisOrdinateValueType value_type) {
                _Axis = axis;
                _ValueType = value_type;
                _Axis.Ordinates.Add(this);
            }

            public void Render(Int32 level, IList<MdfAxisLevel> levels, ref Int32 index) {
                if (this.IsIntegrated) {
                    this.Level = null;
                    this.LevelLengthSet(0);
                    this.LevelIndexSet(0);
                    this.OrderSet(0);
                    foreach (MdfAxisOrdinate sub_ord in this.Downs.OrderBy(x => x.SortOrder)) {
                        sub_ord.Render(level, levels, ref index);
                    }
                }
                else {
                    Int32 new_level = level + 1;
                    if (new_level == levels.Count) {
                        levels.Add(this.Axis.LevelGet(new_level));
                        //                   levels_index.Add(0);
                    }
                    if (this.Downs.Count > 0) {

                        Int32 length = 0;
                        Int32 old_index = index;
                        foreach (MdfAxisOrdinate sub_ord in this.Downs.OrderBy(x => x.SortOrder)) {
                            sub_ord.Render(new_level, levels, ref index);
//                            length += sub_ord.LevelLength;
                        }
                        length = index - old_index;
                        this.LevelIndexSet(old_index);
                        this.LevelLengthSet(length);
                        this.OrderSet(0);
                        this.Axis.OrdinateLine.Remove(this);
//                        this.AxisLine = null;
                    }
                    else {
                        //index++;
                        //this.AxisLine = this.Axis;
                        this.Axis.OrdinateLine.Add(this);
                        this.OrderSet(index);
                        this.LevelIndexSet(index++);
                        this.LevelLengthSet(1);
                    }
                    this.Level = levels[level];
//                    levels[level].Ordinates.Add(this);
                }
            }
        }

        private readonly List<MdfAxisOrdinate> _Ordinates;
        public IList<MdfAxisOrdinate> Ordinates {
            get { return _Ordinates; }
        }

        private readonly List<MdfAxisOrdinate> _OrdinateLine;
        public IList<MdfAxisOrdinate> OrdinateLine {
            get { return _OrdinateLine; }
        }

        private readonly List<MdfAxisLevel> _Levels;
        public IList<MdfAxisLevel> Levels {
            get { return _Levels; }
        }

        private MdfAxisOrdinate _Root;
        public MdfAxisOrdinate Root {
            get { return _Root; }
            set { _Root = value; }
        }

        private readonly Tr _Report;
        public Tr Report {
            get { return _Report; }
        }

        public MdfAxis(Tr report) {
            _Levels = new List<MdfAxisLevel>();
            _Ordinates = new List<MdfAxisOrdinate>();
            _OrdinateLine = new List<MdfAxisOrdinate>();
            _Report = report;
//            _Root = new MdfAxisOrdinate(this);
//            _Root.IsIntegrated = true;
        }

        public MdfAxisLevel LevelGet(Int32 index) {
            for (Int32 cur = 0; cur < Levels.Count; cur++) {
                if (Levels[cur].Index == index)
                    return Levels[cur];
            }
            return CreateLevel(index);
        }

        protected virtual MdfAxisLevel CreateLevel(Int32 index) {
            MdfAxisLevel res = new MdfAxisLevel(this);
            Levels.Add(res);
            res.IndexSet(index);
            return res;
        }

        public virtual void OrdinatesFill() { }

        public void Render() {
            OrdinatesFill();
            IList<MdfAxisLevel> levels = new List<MdfAxisLevel>(10) {
                this.LevelGet(0)
            };
            Int32 index = 0;
            this.Root.Render(0, levels, ref index);
            foreach (MdfAxisLevel level in this.Levels.ToList()) {
                if (level.Ordinates.Count == 0) {
                    Levels.Remove(level);
                }
            }
        }
        public IList<IList<Tuple<MdfAxisOrdinate, Tv>>> RenderCellAxis
                    (IList<IList<Tuple<MdfAxisOrdinate, Tv>>> axis_mult_list) {
            IList<Tuple<MdfAxisOrdinate, Tv>> axis_linear = RenderCellAxisLinear();
            if (axis_linear.Count != 0) {
                var result = new List<IList<Tuple<MdfAxisOrdinate, Tv>>>(16);
                if (axis_mult_list.Count == 0) {
                    foreach (var axis_item in axis_linear) {
                        var res_item = new List<Tuple<MdfAxisOrdinate, Tv>>(1) {
                            axis_item
                        };
                        result.Add(res_item);
                    }
                }
                else {
                    foreach (var axis_mult_item in axis_mult_list) {
                        foreach (var axis_item in axis_linear) {
                            var res_item = new List<Tuple<MdfAxisOrdinate, Tv>>(axis_mult_item) {
                                //axis_mult_item.Add(.CopyTo(res_item,0);
                                axis_item
                            };
                            result.Add(res_item);
                        }
                    }
                }
                return result;
            }
            else {
                return axis_mult_list;
            }
        }

        public IList<Tuple<MdfAxisOrdinate, Tv>> RenderCellAxisLinear() {
            var result = new List<Tuple<MdfAxisOrdinate, Tv>>(128);
            foreach (var ord in OrdinateLine) {
                result.Add(new Tuple<MdfAxisOrdinate, Tv>(ord, ord.CategoryValue));
            }
            return result;
        }

    }

}
