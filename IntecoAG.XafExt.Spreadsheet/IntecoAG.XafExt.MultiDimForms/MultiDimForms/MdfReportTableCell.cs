using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public abstract class MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private readonly MdfReportTable<Tr, Tv, Tt, Tc, Tdp> _Table;
        public MdfReportTable<Tr, Tv, Tt, Tc, Tdp> Table {
            get { return _Table; }
        }

        private readonly MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate[] _Ordinates;
        public MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate[] Ordinates {
            get { return _Ordinates; }
        }

        private Tdp _DataPoint;
        public Tdp DataPoint {
            get { return _DataPoint; }
            set {
                _DataPoint = value;
            }
        }

        private MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> _Calc;
        public MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> Calc {
            get { return _Calc; }
            set { _Calc = value; }
        }
//        protected abstract MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> CalcCreate();

        public override bool Equals(object obj) {
            Tc cell = obj as Tc;
            if (cell == null)
                return false;
            return Equals(cell);
        }

        public bool Equals(Tc cell) {
            if (cell == null || !ReferenceEquals(_Table, cell._Table) || _Ordinates.Length != cell._Ordinates.Length)
                return false;
            for (int i = 0; i < _Ordinates.Length; i++)
                if (!ReferenceEquals(_Ordinates[i], cell._Ordinates[i]))
                    return false;
            return true;
        }

        public override int GetHashCode() {
            int hash = _Table.GetHashCode();
            for (int i = 0; i < _Ordinates.Length; i++)
                hash = hash ^ _Ordinates[i].GetHashCode();
            return hash;
        }

        public delegate void OnCellChangedCallback(MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> table_cell);
        public OnCellChangedCallback OnCellChanged;

        public MdfReportTableCell(MdfReportTable<Tr, Tv, Tt, Tc, Tdp> table) {
            _Table = table;
            _Ordinates = new MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate[table.Axiss.Count];
        }

        public virtual void Store() {
            if (!Table.IsRefreshDisabled)
                OnCellChanged?.Invoke(this);
        }

        public virtual void Restore() {
        }
    }

}
