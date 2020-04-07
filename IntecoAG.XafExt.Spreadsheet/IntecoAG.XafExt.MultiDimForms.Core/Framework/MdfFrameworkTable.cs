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
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfFrameworkTable")]
    public class MdfFrameworkTable : IagBaseObject {

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class MdfTable0 : MdfCoreTable {

            [Persistent(nameof(FrameworkTable))]
            [ExplicitLoading(1)]
            private MdfFrameworkTable _FrameworkTable;
            [PersistentAlias(nameof(_FrameworkTable))]
            public MdfFrameworkTable FrameworkTable {
                get { return _FrameworkTable; }
            }
            public void FrameworkTableSet(MdfFrameworkTable value) {
                SetPropertyValue<MdfFrameworkTable>(ref _FrameworkTable, value);
            }

            public MdfTable0(Session session) : base(session) {
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
        [Persistent(nameof(Table))]
        [Aggregated]
        private MdfTable0 _Table;
        [PersistentAlias(nameof(_Table))]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public MdfTable0 Table {
            get { return _Table; }
        }
        public void TableSet(MdfTable0 value) {
            SetPropertyValue<MdfTable0>(ref _Table, value);
        }

        private MdfFramework _Framework;
        [Association("FmMdfFramework-FmMdfTable")]
        public MdfFramework Framework {
            get { return _Framework; }
            set { SetPropertyValue<MdfFramework>(ref _Framework, value); }
        }


        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MdfFrameworkTable(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            TableSet(CreateTable());
            Table.FrameworkTableSet(this);
        }

        public virtual MdfTable0 CreateTable() {
            return new MdfTable0(Session);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(Framework):
                    (newValue as MdfFramework)?.Container.Tables.Add(Table);
                    break;
            }
        }

        public override string ToString() {
            return base.ToString();
        }

        //protected override void OnChanged(string propertyName, object oldValue, object newValue) {
        //    base.OnChanged(propertyName, oldValue, newValue);
        //    switch (propertyName) {
        //        case nameof(Framework):
        //            FrameworkAccess = newValue as MdfFramework;
        //            break;
        //    }
        //}
    }
}