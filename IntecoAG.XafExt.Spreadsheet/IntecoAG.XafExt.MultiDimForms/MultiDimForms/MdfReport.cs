using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    [NavigationItem("MDF")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [MapInheritance(MapInheritanceType.ParentTable)]
    public abstract class MdfReport : MdfContainer
    {
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        protected MdfReport(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DateCreated = DateTime.Now;
        }

        private DateTime _DateCreated;

        [ModelDefault("AllowEdit", "False")]
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { SetPropertyValue(nameof(DateCreated), ref _DateCreated, value); }
        }

        private MdfTemplate _Template;

        [Association]
        [VisibleInDetailView(false)]
        public MdfTemplate Template
        {
            get { return _Template; }
            set
            {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null)
                {
                    foreach (var cont in value.Containers.ToList())
                    {
                        Containers.Add(cont);
                    }
                }
            }
        }

        public virtual Decimal Scale {
            get { return 1; }
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