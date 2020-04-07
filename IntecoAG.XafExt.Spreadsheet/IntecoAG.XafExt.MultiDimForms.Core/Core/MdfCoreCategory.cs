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
using DevExpress.Xpo.Metadata;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreCategory")]
    [ModelDefault("AllowDelete", "False")]
    public class MdfCoreCategory : MdfCoreType { //, ICategory, ICollectionDictionaryItem<String> { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreContainer _Container;
        [Association("FmMdfContainer-FmMdfCoreCategoryType")]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue<MdfCoreContainer>(ref _Container, value); }
        }

        //[Persistent(nameof(CoreDimension))]
        //private MdfCoreDimension _CoreDimension;
        //[PersistentAlias(nameof(_CoreDimension))]
        //public MdfCoreDimension CoreDimension {
        //    get { return _CoreDimension; }
        //}
        //public void CoreDimensionSet(MdfCoreDimension value) {
        //    SetPropertyValue(ref _CoreDimension, value);
        //}
        [Association("MdfCoreCategory-MdfCoreCategoryField")]
        [Aggregated]
        public XPCollection<MdfCoreCategoryField> CategoryFields {
            get { return GetCollection<MdfCoreCategoryField>(); }
        }

        //protected IEnumerable<IDimension> Dimensions {
        //    get { return CategoryFields.Select(x => x.Dimension); }
        //}

        [Association("FmMdfCoreCategory-FmMdfCoreCategoryMember")]
        [Aggregated]
        //        public XPCollectionDictionary<String,MdfCoreCategoryMember> CategoryMembers {
        public XPCollection<MdfCoreCategoryMember> CategoryMembers {
            get { return GetCollection<MdfCoreCategoryMember>(); }
        }

        private Dictionary<String, MdfCoreCategoryMember> _CategoryMembersDict;
        protected Dictionary<String, MdfCoreCategoryMember> CategoryMembersDict {
            get {
                if (_CategoryMembersDict == null) {
                    _CategoryMembersDict = new Dictionary<string, MdfCoreCategoryMember>(CategoryMembers.Count + 10);
                    foreach (var member in CategoryMembers) {
                        _CategoryMembersDict[member.Key] = member;
                    }
                }
                return _CategoryMembersDict;
            }
        }

        public MdfCoreCategoryMember CategoryMemberGet(IReadOnlyDictionary<MdfCoreDimension, MdfCoreDimensionMember> dim_members) {
            String key = dim_members.CategoryMemberKeyGet();
            if (!CategoryMembersDict.TryGetValue(key, out MdfCoreCategoryMember member)) {
                member = new MdfCoreCategoryMember(Session);
                CategoryMembers.Add(member);
                foreach (var field in member.CategoryMemberFields) {
                    field.DimensionMember = dim_members[field.CategoryTypeField.Dimension];
                }
                CategoryMembersDict[key] = member;
            }
            return member;
        }
        //private ICollectionDictionary<string, ICollectionDictionaryItem<string>> _Dict;
        //ICollectionDictionary<string, ICollectionDictionaryItem<string>> ICollectionDictionaryItem<string>.Dict {
        //    get { return _Dict; }
        //    set { _Dict = value; }
        //}

        [Persistent(nameof(Key))]
        [Size(512)]
        private String _Key;
        [ModelDefault("RowCount", "1")]
        [PersistentAlias(nameof(_Key))]
        [RuleUniqueValue]
        public String Key {
            get { return _Key; }
        }

        //string ICategory.Key => Key;

        //IEnumerable<IDimension> ICategory.Dimensions => Dimensions;

        //IReadOnlyDictionary<string, ICategoryMember> ICategory.Members => (IReadOnlyDictionary<String, ICategoryMember>) CategoryMembers;


        public MdfCoreCategory(Session session)
            : base(session) {
        }

        public void KeyUpdate() {
            String value = this.CategoryKeyGet();
            //    this.CategoryFields.Select(x => x.Dimension).CategoryKeyGet();
            SetPropertyValue(nameof(Key), ref _Key, value);
        }

        //protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property) {
        //    if (property.Name == nameof(CategoryMembers))
        //        return new XPCollectionDictionary<String, T>(Session, this, property);
        //    return base.CreateCollection<T>(property);
        //}

        protected MdfCoreCategoryMember CategoryMemberCreate() {
            MdfCoreCategoryMember member = new MdfCoreCategoryMember(Session);
            CategoryMembers.Add(member);
            return member;
        }
        
        //ICategoryMember ICategory.MemberCreate() {
        //    return CategoryMemberCreate();
        //}

        //ICategoryMember ICategory.MemberGet(IReadOnlyDictionary<IDimension, IDomainMember> category) {
        //    String key = category.CategoryMemberKeyGet();
        //    MdfCoreCategoryMember result;
        //    if (!CategoryMembers.TryGetValue(key, out result)) {
        //        result = CategoryMemberCreate();
        //        foreach (var field in result.CategoryMemberFields) {
        //            MdfCoreDomainMember member = (MdfCoreDomainMember)category[field.CategoryTypeField.Dimension];

        //        }
        //    }
        //    return result;
        //}

        //ICategoryMember ICategory.MemberGet(IReadOnlyDictionary<IDimension, IDomainMember> cat_member, IDimension dim, IDomainMember dim_member) {
        //    throw new NotImplementedException();
        //}

        //IEnumerator<IDimension> IEnumerable<IDimension>.GetEnumerator() {
        //    throw new NotImplementedException();
        //}

        //IEnumerator IEnumerable.GetEnumerator() {
        //    throw new NotImplementedException();
        //}




        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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