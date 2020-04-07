using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    [Persistent("FmMdfCoreTableCell")]
    public class MdfCoreTableCell : IagBaseObject {

        private MdfCoreTable _Table;
        [Association("FmMdfTable-FmMdfTableCell")]
        public MdfCoreTable Table {
            get { return _Table; }
            set { SetPropertyValue<MdfCoreTable>(ref _Table, value); }
        }

//        private MdfCoreDataPointCalc _Calc;
//        [ExplicitLoading(1)]
//        [Aggregated]
//        [ExpandObjectMembers(ExpandObjectMembers.Never)]
////        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
//        public MdfCoreDataPointCalc Calc {
//            get { return _Calc; }
//            set { SetPropertyValue(ref _Calc, value); }
//        }
        [Association]
        public XPCollection<MdfCoreDataPointCalc> Calcs {
            get { return GetCollection<MdfCoreDataPointCalc>(nameof(Calcs)); }
        }

        //private Boolean _IsCalculated;
        //[ImmediatePostData]
        //public Boolean IsCalculated {
        //    get { return _IsCalculated; }
        //    set { SetPropertyValue(ref _IsCalculated, value); }
        //}

        [Association]
        public XPCollection<MdfCoreDataPointCalcLink> CalcLinks {
            get { return GetCollection<MdfCoreDataPointCalcLink>(); }
        }

        [Persistent(nameof(AxisOrdinate0))]
        [ExplicitLoading(1)]
        private MdfCoreAxisOrdinate _AxisOrdinate0;
        [PersistentAlias(nameof(_AxisOrdinate0))]
        public MdfCoreAxisOrdinate AxisOrdinate0 {
            get { return _AxisOrdinate0; }
        }
        public void AxisOrdinate0Set(MdfCoreAxisOrdinate value) {
            SetPropertyValue<MdfCoreAxisOrdinate>(ref _AxisOrdinate0, value);
        }

        [Persistent(nameof(AxisOrdinate1))]
        [ExplicitLoading(1)]
        private MdfCoreAxisOrdinate _AxisOrdinate1;
        [PersistentAlias(nameof(_AxisOrdinate1))]
        public MdfCoreAxisOrdinate AxisOrdinate1 {
            get { return _AxisOrdinate1; }
        }
        public void AxisOrdinate1Set(MdfCoreAxisOrdinate value) {
            SetPropertyValue<MdfCoreAxisOrdinate>(ref _AxisOrdinate1, value);
        }

        [Persistent(nameof(AxisOrdinate2))]
        [ExplicitLoading(1)]
        private MdfCoreAxisOrdinate _AxisOrdinate2;
        [PersistentAlias(nameof(_AxisOrdinate2))]
        public MdfCoreAxisOrdinate AxisOrdinate2 {
            get { return _AxisOrdinate2; }
        }
        public void AxisOrdinate2Set(MdfCoreAxisOrdinate value) {
            SetPropertyValue<MdfCoreAxisOrdinate>(ref _AxisOrdinate2, value);
        }

        private MdfCoreDataPoint _DataPoint;
        [Association("FmMdfDataPoint-FmMdfTableCell")]
        [ExplicitLoading(1)]
        public MdfCoreDataPoint DataPoint {
            get { return _DataPoint; }
            set { SetPropertyValue(ref _DataPoint, value); }
        }
        //
        public XPCollection<MdfCoreCategoryMemberField> DataPointFields {
            get { return DataPoint.CategoryMember.CategoryMemberFields; }
        }

        public IList<MdfCoreAxisOrdinate> AxisOrdinates {
            get {
                //var result = new MdfCoreAxisOrdinate[3];
                //result[0] = AxisOrdinate0;
                //result[1] = AxisOrdinate1;
                //result[2] = AxisOrdinate2;
                var result = new List<MdfCoreAxisOrdinate>();
                if (AxisOrdinate0 != null) result.Add(AxisOrdinate0);
                if (AxisOrdinate1 != null) result.Add(AxisOrdinate1);
                if (AxisOrdinate2 != null) result.Add(AxisOrdinate2);
                return result;
            }
        }

        public MdfCoreTableCell(Session session) : base(session) {
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                //case nameof(Calc):
                //    var old_calc = (MdfCoreDataPointCalc)oldValue;
                //    if (old_calc != null && old_calc.TableCell == this) {
                //        old_calc.TableCell = null;
                //    }
                //    if (Calc != null) {
                //        Calc.TableCell = this;
                //    }
                //    break;
                //case nameof(IsCalculated):
                //    if (IsCalculated) {
                //        if (Calc == null) {
                //            Calc = new MdfCoreDataPointCalc(Session) {
                //                TableCell = this
                //            };
                //        }
                //    }
                //    else {
                //        if (Calc != null) {
                //            var calc = Calc;
                //            Calc = null;
                //            calc.Table = null;
                //            calc.DataPoint = null;
                //            Session.Delete(calc);
                //        }
                //    }
                //    break;
                case nameof(DataPoint):
                    foreach (var calc in Calcs) {
                        calc.DataPoint = DataPoint;
                        calc.Update();
                    }
                    //if (Calc != null) {
                    //    Calc.DataPoint = DataPoint;
                    //    Calc.Update();
                    //}
                    break;
            }
        }

        public override string ToString() {
            return $@"CELL({AxisOrdinate0?.Code}, {AxisOrdinate1?.Code}, {AxisOrdinate2?.Code})";
        }

    }

}