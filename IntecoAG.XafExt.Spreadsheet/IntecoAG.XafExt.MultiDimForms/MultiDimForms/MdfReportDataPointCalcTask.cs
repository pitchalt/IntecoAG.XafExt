using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfReportDataPointCalcTask<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {


        private readonly MdfReportCore<Tr, Tv, Tt, Tc, Tdp> _Report; 
        public MdfReportCore<Tr, Tv, Tt, Tc, Tdp>  Report {
            get { return _Report; }
        }

        public void Cancel(Tdp data_point) {
            DataPointVisit[data_point] = true;
            foreach (var calc_link in data_point.CalcLinks) {
                if (calc_link.DataPoint != null) {
                    Cancel(calc_link.DataPoint);
                }
                if (calc_link.DataPointList != null) {
                    for (int i = 0; i < calc_link.DataPointList.Count; i++) {
                        Cancel(calc_link.DataPointList[i]);
                    }
                }
            }
        }

        public void CalculateUp(Tdp data_point) {
            CalculateUpCollect(data_point);
            CalculateUpSingle(data_point);
        }

        protected void CalculateUpSingle(Tdp data_point) {
            if (DataPointVisit.TryGetValue(data_point, out Boolean visit) && visit)
                throw new Exception("Calc cycled");
            DataPointVisit[data_point] = true;
            data_point.Calc?.Calculate();
            DataPointRecalc[data_point] = false;
            Boolean[] is_calc_link = new Boolean[data_point.CalcLinks.Count];
            for (int i = 0; i < data_point.CalcLinks.Count; i++) {
                for (int j = 0; j < data_point.CalcLinks.Count; j++) {
                    if (!is_calc_link[j]) {
                        is_calc_link[j] = CalculateUpCheckCanCacl(data_point.CalcLinks[j].Calc.DataPoint);
                        if (is_calc_link[j])
                            CalculateUpSingle(data_point.CalcLinks[j].Calc.DataPoint);
                    }
                }
            }
            for (int i = 0; i < data_point.CalcLinks.Count; i++) {
                if (!is_calc_link[i])
                    throw new Exception("Calc link Up cat not calculate");
            }
            DataPointVisit[data_point] = false;
        }
        protected Boolean CalculateUpCheckCanCacl(Tdp data_point) {
            foreach (var calc_link in data_point.Calc.Links.Values) {
                foreach (var ref_data_point in calc_link.DataPointList) {
                    if (DataPointRecalc.TryGetValue(ref_data_point, out Boolean is_req) && is_req) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected void CalculateUpCollect(Tdp data_point) {
            DataPointRecalc[data_point] = true;
            foreach (var calc_link in data_point.CalcLinks) {
                CalculateUpCollect(calc_link.Calc.DataPoint);
            }
        }

        public void CalculateAll() {
            DataPointRecalc = new Dictionary<Tdp, bool>(Report.DataPoints.Count);
            foreach (var data_point in Report.DataPoints) {
                if (data_point.Calc != null)
                    DataPointRecalc[data_point] = true;
            }
            foreach (var data_point in DataPointRecalc.Keys.ToList()) {
                DataPointVisit.Clear();
                if (DataPointRecalc[data_point]) {
                    CalculateDown(data_point);
                }
            }
        }

        public void CalculateDown(Tdp data_point) {
            DataPointRecalc[data_point] = true;
            CalculateDownSingle(data_point);
        }

        protected void CalculateDownSingle(Tdp data_point) {
            if (DataPointVisit.ContainsKey(data_point)) {
                throw new Exception("Calc cycled");
            }
            DataPointVisit[data_point] = true;
            if (data_point.Calc != null) {
                foreach (var calc_link in data_point.Calc.Links.Values) {
                    foreach (var ref_data_point in calc_link.DataPointList) {
                        if (DataPointRecalc.TryGetValue(ref_data_point, out Boolean recalc) && recalc) {
                            CalculateDownSingle(ref_data_point);
                        }
                    }
                }
                data_point.Calc.Calculate();
            }
            DataPointRecalc[data_point] = false;
        }

        public Dictionary<Tdp, Boolean> DataPointVisit;
        public Dictionary<Tdp, Boolean> DataPointRecalc;

        public MdfReportDataPointCalcTask(MdfReportCore<Tr, Tv, Tt, Tc, Tdp> report) {
            _Report = report;
            DataPointVisit = new Dictionary<Tdp, bool>(16384);
            DataPointRecalc = new Dictionary<Tdp, bool>(16384);
        }

    }

}
