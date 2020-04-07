using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [DefaultProperty(nameof(Name))]
    [Persistent("FmMdfCoreDataPoint")]
    public class MdfCoreDataPoint : MdfCoreElement { //, IDataPoint {

        private MdfCoreContainer _Container;
        [Association("MdfContainer-MdfDataPoint")]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue(ref _Container, value); }
        }

        public String KeyName {
            get {
                return String.IsNullOrEmpty(Code) ? CategoryMember.Key : Code;
            }
        }

        //private MdfCoreDataPointCalc _Calc;
        //public MdfCoreDataPointCalc Calc {
        //    get { return _Calc; }
        //    set { SetPropertyValue(ref _Calc, value); }
        //}
        [Association]
        public XPCollection<MdfCoreDataPointCalc> Calcs {
            get { return GetCollection<MdfCoreDataPointCalc>(); }
        }

        [Association()]
        public XPCollection<MdfCoreDataPointCalcLink> CalcLinks {
            get { return GetCollection<MdfCoreDataPointCalcLink>(); }
        }

        [Association("FmMdfDataPoint-FmMdfTableCell")]
        public XPCollection<MdfCoreTableCell> Cells {
            get { return GetCollection<MdfCoreTableCell>(); }
        }

        private MdfCoreCategoryMember _CategoryMember;
        [ExplicitLoading(2)]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public MdfCoreCategoryMember CategoryMember {
            get { return _CategoryMember; }
            set {
                MdfCoreCategoryMember old = _CategoryMember;
                if (SetPropertyValue(ref _CategoryMember, value) && !IsLoading) {
                    if (old != null) {
                        old.DataPoint = null;
                    }
                    if (value != null) {
                        value.DataPoint = this;
                    }
                    DataTypeUpdate(false);
                }
            }
        }

        private MdfCoreDataType _DataType;
        public MdfCoreDataType DataType {
            get { return _DataType; }
            set { SetPropertyValue(ref _DataType, value); }
        }

        //ICategoryMember IDataPoint.CategoryMember => CategoryMember;
        //IContainer IContainerized.Container => Container;

        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MdfCoreDataPoint(Session session) : base(session) {
        }

        public void DataTypeUpdate(Boolean is_force) {
            if (CategoryMember != null && (DataType == MdfCoreDataType.DT_UNDEFINED || is_force)) {
                foreach (var field in CategoryMember.CategoryMemberFields) {
                    if (field.DimensionMember.DomainMember.DataPointDataType != MdfCoreDataType.DT_UNDEFINED) {
                        DataType = field.DimensionMember.DomainMember.DataPointDataType;
                        break;
                    }
                }
            }
        }

        [Action]
        public void DataTypeUpdateAction() {
            DataTypeUpdate(true);
        }

        public override string ToString() {
            return $@"DP:{Oid:000000000}";
        }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                //case nameof(Calc):
                //    var old = (MdfCoreDataPointCalc)old_value;
                //    if (old != null && old.DataPoint == this) {
                //        old.DataPoint = null;
                //    }
                //    if (Calc != null) {
                //        Calc.DataPoint = this;
                //    }
                //    break;
            }
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        //protected override void OnChanged(String property_name, Object old_value, Object new_value) {
        //    base.OnChanged(property_name, old_value, new_value);
        //}
    }
}