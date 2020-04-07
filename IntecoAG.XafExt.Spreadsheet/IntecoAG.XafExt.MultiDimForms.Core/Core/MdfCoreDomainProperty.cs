using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.XpoExt;
using System.Collections;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreDomainProperty")]
    public class MdfCoreDomainProperty : MdfCoreType {//, IDomain { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreDomain _Domain;
        [Association]
        public MdfCoreDomain Domain {
            get { return _Domain; }
            set { SetPropertyValue<MdfCoreDomain>(ref _Domain, value); }
        }

        private MdfCoreDomain _PropertyType;
        [RuleRequiredField]
        public MdfCoreDomain PropertyType {
            get { return _PropertyType; }
            set { SetPropertyValue<MdfCoreDomain>(ref _PropertyType, value); }
        }

        private Boolean _IsRequired;
        public Boolean IsRequired {
            get { return _IsRequired; }
            set { SetPropertyValue<Boolean>(ref _IsRequired, value); }
        }

        private String _RequiredCondition;
        [Size(1024)]
        public String RequiredCondition {
            get { return _RequiredCondition; }
            set { SetPropertyValue<String>(ref _RequiredCondition, value); }
        }

        private Boolean _IsAssociation;
        public Boolean IsAssociation {
            get { return _IsAssociation; }
            set { SetPropertyValue<Boolean>(ref _IsAssociation, value); }
        }

        public MdfCoreDomainProperty(Session session)
            : base(session) {
        }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
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
        //public override string ToString() {
        //    return base.ToString();
        //}
    }
}