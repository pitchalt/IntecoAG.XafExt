using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
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
    [Persistent("FmMdfCoreCategoryMember")]
    public class MdfCoreCategoryMember : MdfCoreValue { //, ICategoryMember { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreCategory _Category;
        [Association("FmMdfCoreCategory-FmMdfCoreCategoryMember")]
        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public MdfCoreCategory Category {
            get { return _Category; }
            set { SetPropertyValue(ref _Category, value); }
        }

        public override MdfCoreType CoreType {
            get { return Category; }
        }

        [Size(1024)]
        [Persistent(nameof(Key))]
        private String _Key;
        [ModelDefault("RowCount", "1")]
        [PersistentAlias(nameof(_Key))]
        [RuleUniqueValue]
        public String Key {
            get { return _Key; }
        }

        private MdfCoreDataPoint _DataPoint;
        public MdfCoreDataPoint DataPoint {
            get { return _DataPoint; }
            set {
                MdfCoreDataPoint old = _DataPoint;
                if (SetPropertyValue(ref _DataPoint, value) && !IsLoading) {
                    if (old != null) {
                        old.CategoryMember = null;
                    }
                    if (value != null) {
                        value.CategoryMember = this;
                    }
                }
            }
        }

        [Association("FmMdfCoreCategoryMember-FmMdfCoreCategoryMemberField")]
        [Aggregated]
        [ModelDefault("AllowNew", "False")]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("AllowDelete", "False")]
        public XPCollection<MdfCoreCategoryMemberField> CategoryMemberFields {
            get { return GetCollection<MdfCoreCategoryMemberField>(); }
        }

        [Association("FmMdfCoreCategory-FmMdfCoreAxisOrdinate")]
        public XPCollection<MdfCoreAxisOrdinate> AxisOrdinates {
            get { return GetCollection<MdfCoreAxisOrdinate>(); }
        }

        //private Dictionary<IDimension, IDomainMember> _Dict;

        //protected IReadOnlyDictionary<IDimension, IDomainMember> Dict {
        //    get {
        //        if (_Dict == null) {
        //            _Dict = new Dictionary<IDimension, IDomainMember>(CategoryMemberFields.Count);
        //            foreach (var field in CategoryMemberFields) {
        //                _Dict[field.CategoryTypeField.Dimension] = field.DimensionMember.DomainMember;
        //            }
        //        }
        //        return _Dict;
        //    }
        //}

        //ICategory ICategoryMember.Category => Category;

        //IEnumerable<IDimension> IReadOnlyDictionary<IDimension, IDomainMember>.Keys => Dict.Keys;

        //IEnumerable<IDomainMember> IReadOnlyDictionary<IDimension, IDomainMember>.Values => Dict.Values;

        //int IReadOnlyCollection<KeyValuePair<IDimension, IDomainMember>>.Count => Dict.Count;

        //String ICategoryMember.Key => Key;

        //IDomainMember IReadOnlyDictionary<IDimension, IDomainMember>.this[IDimension key] => Dict[key];

        public MdfCoreCategoryMember(Session session)
            : base(session) {
        }

        public void KeyUpdate() {
            String value = this.CategoryMemberKeyGet();
            //this.CategoryMemberKeyGet();
            SetPropertyValue(nameof(Key), ref _Key, value);
        }

        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(Category):
                    FieldsUpdate();
                    break;
            }
        }

        public void FieldsUpdate() {
            if (Category != null) {
                Session.Delete(CategoryMemberFields);
                foreach (var type_field in Category.CategoryFields) {
                    MdfCoreCategoryMemberField field = new MdfCoreCategoryMemberField(Session);
                    CategoryMemberFields.Add(field);
                    field.CategoryTypeField = type_field;
                }
            }
            KeyUpdate();
        }

        //bool IReadOnlyDictionary<IDimension, IDomainMember>.ContainsKey(IDimension key) {
        //    return Dict.ContainsKey(key);
        //}

        //bool IReadOnlyDictionary<IDimension, IDomainMember>.TryGetValue(IDimension key, out IDomainMember value) {
        //    return Dict.TryGetValue(key, out value);
        //}

        //IEnumerator<KeyValuePair<IDimension, IDomainMember>> IEnumerable<KeyValuePair<IDimension, IDomainMember>>.GetEnumerator() {
        //    return Dict.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator() {
        //    return Dict.GetEnumerator();
        //}

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

        //protected override void OnChanged(String property_name, Object old_value, Object new_value) {
        //    base.OnChanged(property_name, old_value, new_value);
        //}
    }
}