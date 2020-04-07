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
    [Persistent("FmMdfCoreCalcVariant")]
    [ModelDefault("IsCloneable", "True")]
    public class MdfCoreCalcVariant : IagBaseObject {

        private MdfCoreContainer _Container;
        [Association]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue<MdfCoreContainer>(ref _Container, value); }
        }

        private String _Code;
        [Size(32)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<MdfCoreDataPointCalc> Calcs {
            get { return GetCollection<MdfCoreDataPointCalc>(nameof(Calcs)); }
        }

        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MdfCoreCalcVariant(Session session)
            : base(session) {
        }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
        public override String ToString() {
            return Code;
        }

        [Action(Caption = "LinkFreeCalcs")]
        public void LinkFreeCalcs() {
            foreach (var table in Container?.Tables) {
                foreach (var calc in table.Calcs) {
                    if (calc.CalcVariant == null) {
                        Calcs.Add(calc);
                    }
                }
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