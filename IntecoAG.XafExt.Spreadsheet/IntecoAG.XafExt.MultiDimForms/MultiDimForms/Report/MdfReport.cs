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

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [NavigationItem("MDF")]
    [Persistent("iag_xaf_ext_mdf.report")]
    public abstract class MdfReport : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        public abstract MdfTemplate Template { get; }

        private String _Code;
        [Size(64)]
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

        [Association("FmMdfReport-FmMdfReportDomainCoreLink")]
        [Aggregated]

        public XPCollection<MdfReportDomainMemberCoreLink> DomainCoreLinks {
            get { return GetCollection<MdfReportDomainMemberCoreLink>(); }
        }
        [Association("FmMdfReport-FmMdfReportForm")]
        [Aggregated]

        public XPCollection<MdfReportForm> Forms {
            get { return GetCollection<MdfReportForm>(); }
        }

        protected MdfReport(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected virtual void Init() {
            foreach (var tmpl_form in Template.Forms) {
                if (tmpl_form is MdfTemplateFormExcel) {
                    var form_excel = new MdfReportFormExcel(Session);
                    Forms.Add(form_excel);
                    var tmpl_form_excel = tmpl_form as MdfTemplateFormExcel;
                    form_excel.TemplateFormExcelSet(tmpl_form_excel);
                }
             }
        }
        //protected override void OnChanged(String property_name, Object old_value, Object new_value) {
        //    base.OnChanged(property_name, old_value, new_value);
        //}
        public override string ToString() {
            return base.ToString();
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

    }
}