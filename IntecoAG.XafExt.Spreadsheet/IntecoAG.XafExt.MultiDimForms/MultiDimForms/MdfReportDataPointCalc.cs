using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        public delegate Decimal CalcExpression(MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> calc);

        private String _Formula;
        public String Formula {
            get { return _Formula; }
            set { _Formula = value; }
        }

        private readonly Tdp _DataPoint;
        public Tdp DataPoint {
            get { return _DataPoint; }
        }

        protected Dictionary<Int16, MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>> _Links;
        public IDictionary<Int16, MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>> Links {
            get { return _Links; }
        }

        protected readonly CalcExpression Expression;

        public void Calculate() {
            DataPoint.ValueSet(Expression(this));
        }

        public MdfReportDataPointCalc(Tdp data_point, CalcExpression exp ) {
            _Links = new Dictionary<Int16, MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>>(8);
            _DataPoint = data_point;
            Expression = exp;
            //DataPoint.Calc = this;
        }

        public void Link() {
            DataPoint.Calc = this;
            foreach (var link in Links.Values) {
                if (link.DataPoint != null) {
                    link.DataPoint.CalcLinks.Add(link);
                }
                if (link.DataPointList != null) {
                    foreach (var point in link.DataPointList) {
                        point.CalcLinks.Add(link);
                    }
                }
            }
        }

        public void UnLink() {
            DataPoint.Calc = null;
            foreach (var link in Links.Values) {
                if (link.DataPoint != null) {
                    link.DataPoint.CalcLinks.Remove(link);
                }
                if (link.DataPointList != null) {
                    foreach (var point in link.DataPointList) {
                        point.CalcLinks.Remove(link);
                    }
                }
            }
        }

        public Boolean CheckNotIsCycle() {
            foreach (var link in Links.Values) {
                if (link.DataPoint != null && DataPoint == link.DataPoint)
                    return false;
                if (link.DataPointList != null) {
                    foreach (var point in link.DataPointList) {
                        if (DataPoint == point)
                            return false;
                    }
                }
            }
            return true;
        }

    }

}
