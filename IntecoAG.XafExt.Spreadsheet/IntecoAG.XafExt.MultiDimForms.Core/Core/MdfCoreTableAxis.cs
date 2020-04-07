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
    [Persistent("FmMdfCoreTableAxis")]
//    [RuleCombinationOfPropertiesIsUnique(DefaultContexts.Save, nameof(Table) + "," + nameof(Axis))]
    [RuleCombinationOfPropertiesIsUnique(DefaultContexts.Save, nameof(Table) + "," + nameof(AxisIndex))]
    public class MdfCoreTableAxis : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreTable _Table;
        [Association("FmMdfTable-FmMdfTableAxis")]
        public MdfCoreTable Table {
            get { return _Table; }
            set { SetPropertyValue(ref _Table, value); }
        }

        private MdfCoreAxis _Axis;
        [DataSourceProperty(nameof(Table) + "." + nameof(MdfCoreTable.Container) + "." + nameof(MdfCoreContainer.Axiss))]
        [RuleRequiredField]
        public MdfCoreAxis Axis {
            get { return _Axis; }
            set { SetPropertyValue(ref _Axis, value); }
        }

        private Int32 _AxisIndex;
        [RuleRange(DefaultContexts.Save, 0, 2)]
        public Int32 AxisIndex {
            get { return _AxisIndex; }
            set { SetPropertyValue(ref _AxisIndex, value); }
        }

        public MdfCoreTableAxis(Session session)
            : base(session) {
        }
        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
        public override string ToString() {
            return $"{AxisIndex} - {Axis?.Code}";
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