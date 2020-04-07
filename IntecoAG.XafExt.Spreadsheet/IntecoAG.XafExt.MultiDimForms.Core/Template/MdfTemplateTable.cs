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

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmCoreMdfTemplateTable")]
    public class MdfTemplateTable : IagBaseObject, IObjectSpaceLink { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class MdfTable1 : MdfCoreTable {

            [Persistent(nameof(TemplateTable))]
            [ExplicitLoading(1)]
            private MdfTemplateTable _TemplateTable;
            [PersistentAlias(nameof(_TemplateTable))]
            public MdfTemplateTable TemplateTable {
                get { return _TemplateTable; }
            }
            public void TemplateTableSet(MdfTemplateTable value) {
                SetPropertyValue(ref _TemplateTable, value);
            }

            public MdfTable1(Session session) : base(session) {
            }
        }

        [Persistent(nameof(Table))]
        [Aggregated]
        private MdfTable1 _Table;
        [PersistentAlias(nameof(_Table))]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public MdfTable1 Table {
            get { return _Table; }
        }
        public void TableSet(MdfTable1 value) {
            SetPropertyValue(ref _Table, value);
        }

        private MdfCoreTemplate _Template;
        [Association("FmMdfTemplate-FmMdfTemplateTable")]
        public MdfCoreTemplate Template {
            get { return _Template; }
            set { SetPropertyValue(ref _Template, value); }
        }

        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Action(Caption = "Render")]
        public void RenderAction() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                MdfTemplateTable _this = os.GetObject(this);
                _this.Render(os);
                os.CommitChanges();
            }
        }

        public MdfTemplateTable(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            TableSet(CreateTable());
            Table.TemplateTableSet(this);
        }

        public virtual MdfTable1 CreateTable() {
            return new MdfTable1(Session);
        }


        protected override void OnChanged(string property_name, object old_value, object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(Template):
                    (new_value as MdfCoreTemplate)?.Container.Tables.Add(Table);
                    break;
            }
        }

        public override string ToString() {
            return Table?.Name ?? String.Empty;
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

        //protected override void OnChanged(string propertyName, object oldValue, object newValue) {
        //    base.OnChanged(propertyName, oldValue, newValue);
        //    switch (propertyName) {
        //        case nameof(Template):
        //            FrameworkAccess = (newValue as MdfTemplate)?.Framework;
        //            break;
        //    }
        //}
    }
}