using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfReportDataPointCalcVariant<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> 
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private readonly List<MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp>> _Calcs;
        public IList<MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp>> Calcs {
            get { return _Calcs; }
        }

        public MdfReportDataPointCalcVariant() {
            _Calcs = new List<MdfReportDataPointCalc<Tr, Tv, Tt, Tc, Tdp>>(1024);
        }


    }

}
