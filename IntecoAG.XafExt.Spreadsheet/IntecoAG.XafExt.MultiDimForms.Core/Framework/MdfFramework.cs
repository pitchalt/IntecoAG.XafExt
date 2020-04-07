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

    [NavigationItem("MDF")]
    [Persistent("FmMdfFramework")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class MdfFramework : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class MdfContainer0 : MdfCoreContainer {

            [Persistent(nameof(Framework))]
            [ExplicitLoading(1)]
            private MdfFramework _Framework;
            [Browsable(false)]
            [PersistentAlias(nameof(_Framework))]
            public MdfFramework Framework {
                get { return _Framework; }
            }
            public void FrameworkSet(MdfFramework value) {
                SetPropertyValue<MdfFramework>(ref _Framework, value);
            }


            public MdfContainer0(Session session) : base(session) { }

        }

        [Persistent(nameof(Container))]
        [Aggregated]
        private MdfContainer0 _Container;
        [PersistentAlias(nameof(_Container))]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public MdfContainer0 Container {
            get { return _Container; }
        }
        public void ContainerSet(MdfContainer0 value) {
            SetPropertyValue<MdfContainer0>(ref _Container, value);
        }

        [Association("FmMdfFramework-FmMdfTemplate")]
        [Aggregated]
        public XPCollection<MdfCoreTemplate> Templates {
            get { return GetCollection<MdfCoreTemplate>(); }
        }

        [Association("FmMdfFramework-FmMdfTable")]
        [Aggregated]
        public XPCollection<MdfFrameworkTable> Tables {
            get { return GetCollection<MdfFrameworkTable>(); }
        }

        public MdfFramework(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            ContainerSet(CreateContainer());
        }

        protected virtual MdfContainer0 CreateContainer() {
            return new MdfContainer0(Session);
        }

        public override String ToString() {
            return Container?.Name ?? "-";
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