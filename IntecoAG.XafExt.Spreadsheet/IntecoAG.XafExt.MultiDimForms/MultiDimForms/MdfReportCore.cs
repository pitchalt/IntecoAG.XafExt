using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public abstract class MdfReportCore<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private readonly Tr _Report;
        public Tr Report {
            get { return _Report; }
        }

        private readonly MdfCategoryValues<Tr, Tv, Tt, Tc, Tdp> _CategoryValues;
        public MdfCategoryValues<Tr,Tv,Tt, Tc, Tdp> CategoryValues {
            get { return _CategoryValues; }
        }

        private readonly List<Tdp> _DataPoints;
        public IReadOnlyList<Tdp> DataPoints {
            get { return _DataPoints; }
        }

        public Tdp DataPointGet(Tv cat_value) {
            if (cat_value.DataPoint == null) {
                var dp = DataPointCreate(cat_value);
                cat_value.DataPoint = dp;
                _DataPoints.Add(dp);
            }
            return (Tdp) cat_value.DataPoint;
        }

        public Boolean IsCalcDisabled { get; set; }
        public Boolean IsRefreshDisabled { get; set; }

        public void CalculateAll() {
            var calc_task = new MdfReportDataPointCalcTask<Tr, Tv, Tt, Tc, Tdp>(this);
            calc_task.CalculateAll();
        }

        protected abstract Tdp DataPointCreate(Tv cat_value);

        public MdfReportCore(Tr report, MdfCategoryValues<Tr, Tv, Tt, Tc, Tdp> cat_values) {
            _Report = report;
            _CategoryValues = cat_values;
            _DataPoints = new List<Tdp>(131072);
        }

    }
}
