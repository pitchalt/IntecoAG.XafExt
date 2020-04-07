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
    [NonPersistent]
    [DefaultProperty(nameof(Code))]
    public abstract class MdfCoreElement : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [Persistent(nameof(Guid))]
        private Guid _Guid;
        [PersistentAlias(nameof(_Guid))]
        public Guid Guid {
            get { return _Guid; }
        }
        public void GuidSet(Guid value) {
            SetPropertyValue(ref _Guid, value);
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        [Browsable(false)]
//        [RuleUniqueValue]
        public String CodeOrGuid {
            get { return String.IsNullOrWhiteSpace(Code) ? Guid.ToString() : Code; }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        protected MdfCoreElement(Session session)
            : base(session) {
        }
        public override String ToString() {
            return Code;
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            GuidSet(Guid.NewGuid());
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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