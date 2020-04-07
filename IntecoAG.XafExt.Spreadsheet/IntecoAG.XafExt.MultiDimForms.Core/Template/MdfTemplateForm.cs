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
    [Persistent("FmMdfCoreTemplateForm")]
    [ModelDefault("IsCloneable","True")]
    public abstract class MdfTemplateForm : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        protected MdfTemplateForm(Session session)
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
        private MdfCoreTemplate _Template;
        [Association("FmMdfTemplate-FmMdfTemplateForm")]
        public MdfCoreTemplate Template {
            get { return _Template; }
            set { SetPropertyValue<MdfCoreTemplate>(ref _Template, value); }
        }

        private String _Code;
        [Size(32)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>(ref _Code, value); }
        }

        private String _NameShort;
        [Size(128)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue<String>(ref _NameShort, value); }
        }

        private MdfCoreCalcVariant _CalcVariant;
        public MdfCoreCalcVariant CalcVariant {
            get { return _CalcVariant; }
            set { SetPropertyValue<MdfCoreCalcVariant>(ref _CalcVariant, value); }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        public abstract void Render();

        [Action(Caption = "Render")]
        public void RenderAction() {
            Render();
        }

        public override String ToString() {
            return $@"{Code} - {NameShort}";
        }
    }
}