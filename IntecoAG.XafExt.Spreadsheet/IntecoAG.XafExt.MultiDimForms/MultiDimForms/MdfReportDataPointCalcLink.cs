using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public enum MdfReportDataPointCalcLinkType {
        SINGLE = 0,
        MULTIPLE = 1,
    }

    public class MdfReportDataPointCalcLink<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private readonly MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> _Calc;
        public MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> Calc {
            get { return _Calc; } 
        }

        private readonly MdfReportDataPointCalcLinkType _LinkType;
        public MdfReportDataPointCalcLinkType LinkType {
            get { return _LinkType; }
        }

        private readonly Tdp _DataPoint;
        public Tdp DataPoint {
            get { return _DataPoint; }
        }

        private readonly List<Tdp> _DataPointList;
        public IList<Tdp> DataPointList {
            get { return _DataPointList; }
        }

        public MdfReportDataPointCalcLink(MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> calc, Tdp data_point) {
            _Calc = calc;
            _LinkType = MdfReportDataPointCalcLinkType.SINGLE;
            _DataPointList = new List<Tdp>(new Tdp[] { data_point });
            _DataPoint = data_point;
            //DataPoint.CalcLinks.Add(this);
        }

        public MdfReportDataPointCalcLink(MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp> calc, IEnumerable<Tdp> data_points) {
            _Calc = calc;
            _LinkType = MdfReportDataPointCalcLinkType.MULTIPLE;
            _DataPointList = new List<Tdp>(data_points);
            //foreach(var dp in _DataPointList) {
            //    dp.CalcLinks.Add(this);
            //}
        }

    }

}
