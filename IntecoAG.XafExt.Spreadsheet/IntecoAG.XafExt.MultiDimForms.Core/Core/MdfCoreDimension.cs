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

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreDimension")]
    public class MdfCoreDimension : MdfCoreType, IObjectSpaceLink { //, IDimension { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreDomain _Domain;
        [Association("FmMdfCoreDomain-FmMdfCoreDimension")]
        [ExplicitLoading(1)]
        [DataSourceProperty(nameof(Container)+"."+nameof(MdfCoreContainer.Domains))]
        public MdfCoreDomain Domain {
            get { return _Domain; }
            set { SetPropertyValue<MdfCoreDomain>(ref _Domain, value); }
        }

        [PersistentAlias(nameof(Domain))]
        [DataSourceProperty(nameof(Container) + "." + nameof(MdfCoreContainer.Domains))]
        [VisibleInListView(false)]
        public MdfCoreDomain DomainUi {
            get { return Domain; }
            set { Domain = value; }
        }

        private MdfCoreContainer _Container;
        [Association("FmMdfContainer-FmMdfCoreDimension")]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue<MdfCoreContainer>(ref _Container, value); }
        }

        [Association]
        public XPCollection<MdfCoreAxisOrdinate> AxisOrdinates {
            get { return GetCollection<MdfCoreAxisOrdinate>(); }
        }
        //[Persistent(nameof(CategoryType))]
        //[Aggregated]
        //private MdfCoreCategory _CategoryType;
        //[ExpandObjectMembers(ExpandObjectMembers.Never)]
        //[PersistentAlias(nameof(_CategoryType))]
        //public MdfCoreCategory CategoryType {
        //    get { return _CategoryType; }
        //}
        //public void CategoryTypeSet(MdfCoreCategory value) {
        //    SetPropertyValue(ref _CategoryType, value);
        //}

        [Association("FmMdfCoreDimension-FmMdfCoreCategoryFields")]
        public XPCollection<MdfCoreCategoryField> CategoryFields {
            get { return GetCollection<MdfCoreCategoryField>(); }
        }

        [Association("FmMdfCoreDimension-FmMdfCoreDimensionMember")]
        [Aggregated]
        public XPCollection<MdfCoreDimensionMember> DimensionMembers {
            get { return GetCollection<MdfCoreDimensionMember>(); }
        }

        [Association("FmMdfCoreDimension-FmMdfCoreDimensionDependent")]
        [Aggregated]
        public XPCollection<MdfCoreDimensionDependent> DimensionDependents {
            get { return GetCollection<MdfCoreDimensionDependent>(); }
        }

        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }

        //[ManyToManyAlias(nameof(DimensionDependents), nameof(MdfCoreDimensionDependent.DimensionDependent))]
        //public IList<MdfCoreDimension> DimensionDependentsList {
        //    get { return GetList<MdfCoreDimension>(nameof(DimensionDependentsList)); }
        //}

        //IDomain IDimension.Domain => Domain;

        //IContainer IContainerized.Container => Container;

        //Guid IElement.Guid => throw new NotImplementedException();

        //string IElement.Code => throw new NotImplementedException();

        //string IElement.NameShort => throw new NotImplementedException();

        public MdfCoreDimensionMember DimensionMemberGet(MdfCoreDomainMember domain_member) {
            MdfCoreDimensionMember dim_member = DimensionMembers.FirstOrDefault(x => ReferenceEquals(x, domain_member));
            if (dim_member == null) {
                dim_member = new MdfCoreDimensionMember(Session);
                DimensionMembers.Add(dim_member);
                dim_member.DomainMember = domain_member;
            }
            return dim_member;
        }

        public MdfCoreDimension(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
//            CategoryTypeSet(new MdfCoreCategory(Session));
//            CategoryType.CoreDimensionSet(this);
//            MdfCoreCategoryField field = new MdfCoreCategoryField(Session);
//            CategoryType.CategoryFields.Add(field);
//            field.Dimension = this;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(NameShort):
                    foreach (var field in CategoryFields) {
                        field.NameShort = NameShort;
                    }
                    break;
                case nameof(Code):
                    foreach (var field in CategoryFields) {
                        field.Code = Code;
                        field.Category?.KeyUpdate();
                        foreach (var field_member in field.CategoryMemberFields) {
                            field_member.CategoryMember.KeyUpdate();
                        }
                    }
                    break;
                case nameof(Domain):
                    Container = Domain?.Container;
                    break;
                case nameof(Container):
                    //if (!ReferenceEquals(Domain?.Container, Container))
                    //    Domain = null;
                    //if (CategoryType != null)
                    //    CategoryType.Container = Container;
                    break;
            }
        }

        [Action]
        public void OrdinateUpdate() {
            foreach (var ord in AxisOrdinates) {
                ord.CategoryMemberUpdate();
            }
        }

        [Action(Caption = "CategoryClean")]
        public void CategoryCleanAction() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                MdfCoreDimension _this = os.GetObject(this);
                foreach(var field in _this.CategoryFields) {
                    foreach (var category in field.CategoryMemberFields.Select(x => x.CategoryMember).Distinct().ToList()) {
                        os.Delete(category);
                    }

                }
                foreach (var cat in _this.CategoryFields.Select(x => x.Category).Distinct().ToList()) {
                    os.Delete(cat);
                }
                os.CommitChanges();
            }
        }

        [Action(Caption = "OrdinateClean")]
        public void OrdinateCleanAction() {
            using (IObjectSpace os = ObjectSpace.CreateNestedObjectSpace()) {
                MdfCoreDimension _this = os.GetObject(this);
                foreach (var ord in _this.AxisOrdinates.ToList()) {
                    ord.Dimension = null;
                }
                os.CommitChanges();
            }
        }



        //IEnumerator<MdfCoreDomainMember> GetEnumerator() {
        //    return DimensionMembers.Select(x => x.DomainMember).GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator() {
        //    return GetEnumerator();
        //}

        //IEnumerator<IDomainMember> IEnumerable<IDomainMember>.GetEnumerator() {
        //    return GetEnumerator();
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