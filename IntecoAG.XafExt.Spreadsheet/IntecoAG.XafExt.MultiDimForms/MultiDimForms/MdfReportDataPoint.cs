using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private readonly Tv _CategoryValue;
        public Tv CategoryValue {
            get { return _CategoryValue; }
        }

        private readonly MdfReportCore<Tr, Tv, Tt, Tc, Tdp> _ReportCore;
        public MdfReportCore<Tr, Tv, Tt, Tc, Tdp> ReportCore {
            get { return _ReportCore; }
        }

        private MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> _Calc;
        public MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> Calc {
            get { return _Calc; }
            set { _Calc = value; }
        }

        private readonly List<MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>> _CalcLinks;
        public IList<MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>> CalcLinks {
            get { return _CalcLinks; }
//            set { _Calc = value; }
        }

        private readonly List<Tc> _Cells;
        public IList<Tc> Cells {
            get { return _Cells; }
        }

        private MdfDataType _DataType;
        public MdfDataType DataType {
            get { return _DataType; }
            protected set { _DataType = value; }
        }

        public Boolean IsScale {
            get { return CategoryValue.Concept?.IsReportScale ?? false; }
        }

        public int Precision {
            get {
                return CategoryValue.Concept?.Precision ?? 4;
            }
        }

        private Decimal _ValueDecimal;
        public Decimal ValueDecimal {
            get { return _ValueDecimal; }
        }

        public void ValueSet(Decimal value) {
            Decimal old = _ValueDecimal;
            _ValueDecimal = Decimal.Round(value, Precision, MidpointRounding.ToEven);
            if (old != _ValueDecimal) {
                CellRefresh();
            }
        }

        public void ValueSet(Object value) {
            if (DataType == MdfDataType.DT_DECIMAL || DataType == MdfDataType.DT_MONETARY) {
                switch (value) {
                    case Decimal value_decimal:
                        ValueSet(value_decimal);
                        break;
                    case Double value_double:
                        ValueSet((Decimal) value_double);
                        break;
                    case Single value_single:
                        ValueSet((Decimal) value_single);
                        break;
                    case Int32 value_int32:
                        ValueSet((Decimal)value_int32);
                        break;
                    case Int64 value_int64:
                        ValueSet((Decimal) value_int64);
                        break;
                    default:
                        break;
                        throw new InvalidCastException("Invalid value type: " + value.GetType());
                }
            }
            else {
                Object old = _Value;
                _Value = value;
                if (old != _Value) {
                    CellRefresh();
                }
            }

        }

        private Object _Value;
        public Object Value {
            get {
                if (DataType == MdfDataType.DT_DECIMAL || DataType == MdfDataType.DT_MONETARY) {
                    return _ValueDecimal;
                }
                return _Value;
            }
            set {
                ValueSet(value);
                if (CalcLinks.Count != 0 && !ReportCore.IsCalcDisabled) {
                    var calc_task = new MdfReportDataPointCalcTask<Tr, Tv, Tt, Tc, Tdp>(ReportCore);
                    calc_task.CalculateUp((Tdp)this);
                }
            }
        }

        public MdfReportDataPoint(MdfReportCore<Tr, Tv, Tt, Tc, Tdp> report_core,  Tv value) {
            _CalcLinks = new List<MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>>(4);
            _Cells = new List<Tc>(8);
            _CategoryValue = value;
            DataType = value.Concept?.DataType ?? MdfDataType.DT_UNDEFINED;
            _ReportCore = report_core;
        }

        protected void CellRefresh() {
            foreach (var cell in Cells) {
                cell.Store();
            }
        }
    }

}
