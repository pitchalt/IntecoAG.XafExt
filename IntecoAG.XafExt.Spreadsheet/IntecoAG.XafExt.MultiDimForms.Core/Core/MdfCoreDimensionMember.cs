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
    [Persistent("FmMdfCoreDimensionMember")]
    public class MdfCoreDimensionMember : MdfCoreValue { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreDimension _Dimension;
        [Association("FmMdfCoreDimension-FmMdfCoreDimensionMember")]
        public MdfCoreDimension Dimension {
            get { return _Dimension; }
            set { SetPropertyValue(ref _Dimension, value); }
        }
        public override MdfCoreType CoreType {
            get { return Dimension; }
        }

        private MdfCoreDomainMember _DomainMember;
        [ExplicitLoading(1)]
        [DataSourceProperty(nameof(Dimension)+"."+nameof(MdfCoreDimension.Domain)+"."+nameof(MdfCoreDomain.Members))]
        [Association("FmMdfCoreDomainMember-FmMdfCoreDimensionMember")]
        public MdfCoreDomainMember DomainMember {
            get { return _DomainMember; }
            set { SetPropertyValue(ref _DomainMember, value); }
        }

        [Association("FmMdfCoreDimensionMember-FmMdfCoreCategoryField")]
        public XPCollection<MdfCoreCategoryMemberField> MdfCoreCategoryMemberFields {
            get { return GetCollection<MdfCoreCategoryMemberField>(); }
        }

        public MdfCoreDimensionMember(Session session)
            : base(session) {
        }
        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            if (IsDeleted)
                return;
            switch (property_name) {
                case nameof(DomainMember):
                    Code = DomainMember?.Code;
                    NameShort = DomainMember?.NameShort;
                    break;
                case nameof(Code):
                    MdfCoreCategoryMemberFieldsUpdate();
                    break;
            }
        }
        protected void MdfCoreCategoryMemberFieldsUpdate() {
            foreach (var field in MdfCoreCategoryMemberFields) {
                field.CategoryMember.KeyUpdate();
            }
        }

        [Action]
        public void CategoryMemberRemove() {
            foreach (var cat_field in this.MdfCoreCategoryMemberFields.ToList()) {
                var cat_member = cat_field.CategoryMember;
                if (cat_member.DataPoint != null) {
                    Session.Delete(cat_member.DataPoint);
                }
                Session.Delete(cat_member);
            }
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

        //public override string ToString() {
        //    return base.ToString();
        //}
    }
}