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
    //[Persistent("FmMdfCoreCategoryTypeField")]
    [Persistent("FmMdfCoreCategoryField")]
    [RuleCombinationOfPropertiesIsUnique(null, DefaultContexts.Save, nameof(MdfCoreCategoryField.Category) +"," + nameof(MdfCoreCategoryField.Dimension))]
    public class MdfCoreCategoryField : MdfCoreElement { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreCategory _Category;
        [Association("MdfCoreCategory-MdfCoreCategoryField")]
        public MdfCoreCategory Category {
            get { return _Category; }
            set { SetPropertyValue(ref _Category, value); }
        }

        private MdfCoreDimension _Dimension;
        [Association("FmMdfCoreDimension-FmMdfCoreCategoryFields")]
        [DataSourceProperty(nameof(Category)+"."+nameof(MdfCoreCategory.Container)+"."+nameof(MdfCoreContainer.Dimensions))]
        [RuleRequiredField]
        public MdfCoreDimension Dimension {
            get { return _Dimension; }
            set { SetPropertyValue(ref _Dimension, value); }
        }

        private MdfCoreHierarchy _Hierarchy;
        [DataSourceProperty(nameof(Dimension)+"."+nameof(MdfCoreDimension.Domain))]
        public MdfCoreHierarchy Hierarchy {
            get { return _Hierarchy; }
            set { SetPropertyValue(ref _Hierarchy, value); }
        }

        private Int32 _Order;
        public Int32 Order {
            get { return _Order; }
            set { SetPropertyValue(ref _Order, value); }
        }

        [Association("FmMdfCoreCategoryType-FmMdfCoreCategoryTypeField")]
        public XPCollection<MdfCoreCategoryMemberField> CategoryMemberFields {
            get { return GetCollection<MdfCoreCategoryMemberField>(); }
        }

        public MdfCoreCategoryField(Session session) : base(session) {
        }
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(Dimension):
                    Code = Dimension?.Code;
                    NameShort = Dimension?.NameShort;
                    this.Category?.KeyUpdate();
                    break;
                case nameof(Category):
                    this.Category?.KeyUpdate();
                    break;
            }
        }
        //public override string ToString() {
        //    return base.ToString();
        //}
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

    }
}