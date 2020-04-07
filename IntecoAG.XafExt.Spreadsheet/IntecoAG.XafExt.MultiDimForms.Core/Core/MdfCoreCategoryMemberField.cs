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
    [Persistent("FmMdfCoreCategoryMemberField")]
    public class MdfCoreCategoryMemberField : MdfCoreElement { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreCategoryField _CategoryTypeField;
        [Association("FmMdfCoreCategoryType-FmMdfCoreCategoryTypeField")]
        [ExplicitLoading(1)]
        public MdfCoreCategoryField CategoryTypeField {
            get { return _CategoryTypeField; }
            set { SetPropertyValue(ref _CategoryTypeField, value); }
        }

        private MdfCoreCategoryMember _CategoryMember;
        [Association("FmMdfCoreCategoryMember-FmMdfCoreCategoryMemberField")]
        [ExplicitLoading(1)]
        public MdfCoreCategoryMember CategoryMember {
            get { return _CategoryMember; }
            set { SetPropertyValue(ref _CategoryMember, value); }
        }

        private MdfCoreDimensionMember _DimensionMember;
        [Association("FmMdfCoreDimensionMember-FmMdfCoreCategoryField")]
        [ExplicitLoading(2)]
        [DataSourceProperty(nameof(CategoryTypeField) +"."+nameof(MdfCoreCategoryField.Dimension)+"."+nameof(MdfCoreDimension.DimensionMembers))]
        public MdfCoreDimensionMember DimensionMember {
            get { return _DimensionMember; }
            set { SetPropertyValue(ref _DimensionMember, value); }
        }

        public MdfCoreCategoryMemberField(Session session): base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            if (IsDeleted) return;
            switch (property_name) {
                case nameof(CategoryMember):
                case nameof(CategoryTypeField):
                case nameof(DimensionMember):
                    CategoryMember?.KeyUpdate();
                    break;
            }
        }
        //public override string ToString() {
        //    return base.ToString();
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