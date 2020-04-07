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
    [Persistent("FmMdfCoreTable")]
    public abstract class MdfCoreTable : IagBaseObject {

        public enum MdfCoreTableType {
            TABLE_REPORT = 0,
            TABLE_PERSISTENT = 1,
            TABLE_CONSTRAINT = 2
        }

        private MdfCoreContainer _Container;
        [Association("FmMdfContainer-FmMdfTable")]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue<MdfCoreContainer>(ref _Container, value); }
        }

        private MdfCoreTableType _TableType;

        public MdfCoreTableType TableType {
            get { return _TableType; }
            set { SetPropertyValue(ref _TableType, value); }
        }

        private MdfCoreCalcVariant _CalcVariant;
        public MdfCoreCalcVariant CalcVariant {
            get { return _CalcVariant; }
            set { SetPropertyValue(ref _CalcVariant, value); }
        }

        private MdfCoreTableAxis _ColumnAxis;
        [DataSourceProperty(nameof(TableAxiss))]
        public MdfCoreTableAxis ColumnAxis {
            get { return _ColumnAxis; }
            set { SetPropertyValue(ref _ColumnAxis, value); }
        }

        private MdfCoreTableAxis _RowAxis;
        [DataSourceProperty(nameof(TableAxiss))]
        public MdfCoreTableAxis RowAxis {
            get { return _RowAxis; }
            set { SetPropertyValue(ref _RowAxis, value); }
        }

        private MdfCoreCategory _RowCategory;
        [DataSourceProperty(nameof(Container)+"."+nameof(MdfCoreContainer.Categorys))]
        public MdfCoreCategory RowCategory {
            get { return _RowCategory; }
            set { SetPropertyValue(ref _RowCategory, value); }
        }

        private String _Code;
        [Size(32)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [Size(128)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private Boolean _IsPersistent;
        public Boolean IsPersistent {
            get { return _IsPersistent; }
            set { SetPropertyValue(ref _IsPersistent, value); }
        }

        [Association()]
        [Aggregated]
        public XPCollection<MdfCoreDataPointCalc> Calcs {
            get { return GetCollection<MdfCoreDataPointCalc>(); }
        }

        [Association()]
        public XPCollection<MdfCoreDataPointCalcLink> CalcLinks {
            get { return GetCollection<MdfCoreDataPointCalcLink>(); }
        }

        [Association("FmMdfTable-FmMdfTableAxis")]
        [Aggregated]
        public XPCollection<MdfCoreTableAxis> TableAxiss {
            get { return GetCollection<MdfCoreTableAxis>(); }
        }

        [Association("FmMdfTable-FmMdfTableCell")]
        [Aggregated]
        public XPCollection<MdfCoreTableCell> Cells {
            get { return GetCollection<MdfCoreTableCell>(); }
        }

        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        protected MdfCoreTable(Session session)
            : base(session) {
        }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
        public override String ToString() {
            return Code;
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}