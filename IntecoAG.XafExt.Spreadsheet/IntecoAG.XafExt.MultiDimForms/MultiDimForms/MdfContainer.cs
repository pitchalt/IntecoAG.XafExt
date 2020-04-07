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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfContainer")]
    [DefaultProperty(nameof(MdfContainer.Code))]
    public abstract class MdfContainer : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        protected MdfContainer(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            Containers.Add(this);
        }

        [Persistent(nameof(Code))]
        private String _Code;
        [Size(64)]
        [PersistentAlias(nameof(_Code))]
        public String Code {
            get { return _Code; }
        }
        public void CodeSet(String value) {
            SetPropertyValue(nameof(Code), ref _Code, value);
        }

        [Browsable(false)]
        [Association("FmMdfContainerUsedIn", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<MdfContainer> Containers {
            get { return GetCollection<MdfContainer>(nameof(Containers)); }
        }

        [Browsable(false)]
        [Association("FmMdfContainerUsedIn",UseAssociationNameAsIntermediateTableName =true)]
        public XPCollection<MdfContainer> UsedInContainers {
            get { return GetCollection<MdfContainer>(nameof(Containers)); }
        }

        [Browsable(false)]
        public CriteriaOperator ContainersCritery {
            get {
                return new InOperator(nameof(MdfContainerObject.Container), Containers); 
            }
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