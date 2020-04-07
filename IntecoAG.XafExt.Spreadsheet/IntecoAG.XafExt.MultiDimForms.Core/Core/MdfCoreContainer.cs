using System;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreContainer")]
    public abstract class MdfCoreContainer : MdfCoreElement { //, IContainer { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [Association("FmMdfContainer-FmMdfAxis")]
        [Aggregated]
        public XPCollection<MdfCoreAxis> Axiss {
            get { return GetCollection<MdfCoreAxis>(); }
        }

        [Association("FmMdfContainer-FmMdfTable")]
        [Aggregated]
        [VisibleInDetailView(false)]
        public XPCollection<MdfCoreTable> Tables {
            get { return GetCollection<MdfCoreTable>(); }
        }

        [Association("FmMdfContainer-FmMdfCoreDomain")]
        [Aggregated]
        public XPCollection<MdfCoreDomain> Domains {
            get { return GetCollection<MdfCoreDomain>(); }
        }

        [Association("MdfContainer-MdfDataPoint")]
        [Aggregated]
        public XPCollection<MdfCoreDataPoint> DataPoints {
            get { return GetCollection<MdfCoreDataPoint>(); }
        }

        //private XPCollection<MdfCoreDimension> _Dimensions;
        [Association("FmMdfContainer-FmMdfCoreDimension")]
        [Aggregated]
        public XPCollection<MdfCoreDimension> Dimensions {
            get {
                return GetCollection<MdfCoreDimension>();
                //return _Dimensions ?? (_Dimensions = new XPCollection<MdfCoreDimension>(
                //           PersistentCriteriaEvaluationBehavior.InTransaction, Session,
                //           new BinaryOperator("Domain.Container", This)));
            }
        }

        //private XPCollection<MdfCoreHierarchy> _Hierarchys;
        [Association("FmMdfContainer-FmMdfCoreHierarchy")]
        [Aggregated]
        public XPCollection<MdfCoreHierarchy> Hierarchys {
            get {
                return GetCollection<MdfCoreHierarchy>();
                //return _Hierarchys ?? (_Hierarchys = new XPCollection<MdfCoreHierarchy>(
                //          PersistentCriteriaEvaluationBehavior.InTransaction, Session,
                //         new BinaryOperator("Domain.Container", This)));
            }
        }

        [Association("FmMdfContainer-FmMdfCoreCategoryType")]
        [Aggregated]
        //        public XPCollectionDictionary<String, MdfCoreCategory> Categorys {
        public XPCollection<MdfCoreCategory> Categorys {
        get {
                return GetCollection<MdfCoreCategory>();
            }
        }

        [Association]
        [Aggregated]
        public XPCollection<MdfCoreCalcVariant> CalcVariants {
            get {
                return GetCollection<MdfCoreCalcVariant>();
            }
        }

        private Dictionary<String, MdfCoreCategory> _CategorysDict;
        protected Dictionary<String, MdfCoreCategory> CategorysDict {
            get {
                if (_CategorysDict == null) {
                    _CategorysDict = new Dictionary<string, MdfCoreCategory>(Categorys.Count + 10);
                    foreach (var cat in Categorys) {
                        _CategorysDict[cat.Key] = cat;
                    }
                }
                return _CategorysDict;
            }
        }

        public MdfCoreCategory CategoryGet(IEnumerable<MdfCoreDimension> dims) {
            String key = dims.CategoryKeyGet();
            if (!CategorysDict.TryGetValue(key, out MdfCoreCategory category)) {
                category = new MdfCoreCategory(Session);
                Categorys.Add(category);
                foreach (var dim in dims) {
                    var category_field = new MdfCoreCategoryField(Session);
                    category.CategoryFields.Add(category_field);
                    category_field.Dimension = dim;
                }
                CategorysDict[key] = category;
            }
            return category;
        }
        private Dictionary<String, MdfCoreDataPoint> _DataPointDict;
        protected Dictionary<String, MdfCoreDataPoint> DataPointDict {
            get {
                if (_DataPointDict == null) {
                    _DataPointDict = new Dictionary<string, MdfCoreDataPoint>(Categorys.Count + 10);
                    foreach (var datapoint in DataPoints) {
                        _DataPointDict[datapoint.CategoryMember.Key] = datapoint;
                    }
                }
                return _DataPointDict;
            }
        }

        public MdfCoreDataPoint DataPointGet(IReadOnlyDictionary<MdfCoreDimension, MdfCoreDimensionMember> dim_members) {
            String key = dim_members.CategoryMemberKeyGet();
            if (!DataPointDict.TryGetValue(key, out MdfCoreDataPoint datapoint)) {
                MdfCoreCategory category = CategoryGet(dim_members.Keys);
                MdfCoreCategoryMember category_member = category.CategoryMemberGet(dim_members);
                datapoint = new MdfCoreDataPoint(Session);
                DataPoints.Add(datapoint);
                datapoint.CategoryMember = category_member;
                DataPointDict[key] = datapoint;
            }
            return datapoint;
        }

        //IEnumerable<IDomain> IContainer.Domains => Domains;

        //IEnumerable<IDimension> IContainer.Dimensions => Dimensions;

        //IReadOnlyDictionary<string, ICategory> IContainer.Categorys => (IReadOnlyDictionary<string, ICategory>)Categorys;

        //IEnumerable<IDataPoint> IContainer.DataPoints => DataPoints;

        protected MdfCoreContainer(Session session) : base(session) {
        }

        //protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property) {
        //    if (property.Name == nameof(Categorys))
        //        return new XPCollectionDictionary<String, T>(Session, this, property);
        //    return base.CreateCollection<T>(property);
        //}

        public override String ToString() {
            return $@"{Code} - {NameShort}";
        }

        //IDomain IContainer.DomainCreate() {
        //    throw new NotImplementedException();
        //}

        //IDomain IContainer.DomainCreate(string code) {
        //    throw new NotImplementedException();
        //}

        //ICategory IContainer.CategoryCreate() {
        //    throw new NotImplementedException();
        //}

        //ICategory IContainer.CategoryGet(IEnumerable<IDimension> dims) {
        //    throw new NotImplementedException();
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

        //public override string ToString() {
        //    return base.ToString();
        //}
    }
}