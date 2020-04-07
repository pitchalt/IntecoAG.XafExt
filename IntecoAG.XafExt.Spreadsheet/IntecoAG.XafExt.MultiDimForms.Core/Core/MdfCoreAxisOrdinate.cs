using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public enum MdfCoreAxisExpressionType{
        STRING = 0,
        CODE = 1
    }


    [Persistent("FmMdfCoreAxisOrdinate")]
    [DefaultProperty("Code")]
    [ModelDefault("IsCloneable", "True")]
    public class MdfCoreAxisOrdinate : MdfCoreElement, ITreeNode, IObjectSpaceLink {

        private Int32 _SortOrder;
        public Int32 SortOrder {
            get { return _SortOrder; }
            set { SetPropertyValue<Int32>(ref _SortOrder, value); }
        }

        private Boolean _IsIntegrated;
        public Boolean IsIntegrated {
            get { return _IsIntegrated; }
            set { SetPropertyValue<Boolean>(ref _IsIntegrated, value); }
        }

        private MdfCoreAxisLevel _Level;
        [Association("FmMdfAxisLevel-FmMdfAxisOrdinate")]
        public MdfCoreAxisLevel Level {
            get { return _Level; }
            set { SetPropertyValue<MdfCoreAxisLevel>(ref _Level, value); }
        }

        [Persistent(nameof(Order))]
        private Int32 _Order;
        [PersistentAlias(nameof(_Order))]
        public Int32 Order {
            get { return _Order; }
        }
        public void OrderSet(Int32 value) {
            SetPropertyValue<Int32>(ref _Order, value);
        }

        private Boolean _IsForcePersistent;
        public Boolean IsForcePersistent {
            get { return _IsForcePersistent; }
            set { SetPropertyValue<Boolean>(ref _IsForcePersistent, value); }
        }

        private MdfCoreAxisExpressionType _NameExpressionType;
        public MdfCoreAxisExpressionType NameExpressionType {
            get { return _NameExpressionType; }
            set { SetPropertyValue(ref _NameExpressionType, value); }
        }

        private String _NameExpression;
        [Size(1024)]
        [ModelDefault("RowCount", "3")]
        public String NameExpression {
            get { return _NameExpression; }
            set { SetPropertyValue(ref _NameExpression, value); }
        }

        [Persistent(nameof(LevelLength))]
        private Int32 _LevelLength;
        [PersistentAlias(nameof(_LevelLength))]
        public Int32 LevelLength {
            get { return _LevelLength; }
        }
        public void LevelLengthSet(Int32 value) {
            SetPropertyValue<Int32>(ref _LevelLength, value);
        }

        //[Persistent(nameof(LevelOrder))]
        //private Int32 _LevelOrder;
        //[PersistentAlias(nameof(_LevelOrder))]
        //public Int32 LevelOrder {
        //    get { return _LevelOrder; }
        //}
        //public void LevelOrderSet(Int32 value) {
        //    SetPropertyValue<Int32>(ref _LevelOrder, value);
        //}

        [Persistent(nameof(LevelIndex))]
        private Int32 _LevelIndex;
        [PersistentAlias(nameof(_LevelIndex))]
        public Int32 LevelIndex {
            get { return _LevelIndex; }
        }
        public void LevelIndexSet(Int32 value) {
            SetPropertyValue<Int32>(ref _LevelIndex, value);
        }

        private MdfCoreDimension _Dimension;
        [Association]
        [DataSourceProperty(nameof(Axis) + "." + nameof(MdfCoreAxis.Container) + "." + nameof(MdfCoreContainer.Dimensions))]
        public MdfCoreDimension Dimension {
            get { return _Dimension; }
            set { SetPropertyValue(ref _Dimension, value); }
        }

        //private MdfCoreDomainMemberCalcType _CalcType;
        //public MdfCoreDomainMemberCalcType CalcType {
        //    get { return _CalcType; }
        //    set { SetPropertyValue(ref _CalcType, value); }
        //}
        [PersistentAlias(nameof(DimensionMember)+"."+nameof(MdfCoreDimensionMember.DomainMember)+"."+nameof(MdfCoreDomainMember.CalcType))]
        public MdfCoreDomainMemberCalcType CalcType {
            get { return DimensionMember?.DomainMember.CalcType ?? MdfCoreDomainMemberCalcType.GENERAL; }
        }

        private MdfCoreDimensionMember _DimensionMember;
        [DataSourceProperty(nameof(DimensionMemberSource))]
        public MdfCoreDimensionMember DimensionMember {
            get { return _DimensionMember; }
            set { SetPropertyValue(ref _DimensionMember, value); }
        }
        [Browsable(false)]
        public IList<MdfCoreDimensionMember> DimensionMemberSource {
            get {
                if (CalcType == MdfCoreDomainMemberCalcType.GENERAL) {
                    return Dimension?.DimensionMembers;
                }
                else if (CalcType == MdfCoreDomainMemberCalcType.QUERY) {
                    return Dimension?.DimensionMembers.Where(x => x.DomainMember.CalcType == MdfCoreDomainMemberCalcType.QUERY).ToList();
                }
                else if (CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                    return Dimension?.DimensionMembers.Where(x => x.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED &&
                                                                  x.DomainMember.CalcDimension == CalcDimension).ToList();
                }
                else
                    return new List<MdfCoreDimensionMember>();
            }
        }

        [DataSourceProperty(nameof(DomainMemberSource))]
        [PersistentAlias(nameof(DimensionMember)+"."+nameof(MdfCoreDimensionMember.DomainMember))]
        public MdfCoreDomainMember DomainMember {
            get { return DimensionMember?.DomainMember; }
            set {
                var old = DomainMember;
                if (value != null && Dimension != null && DimensionMember?.DomainMember != value) {
                    var dim_member = Dimension.DimensionMembers.FirstOrDefault(x => x.DomainMember == value);
                    if (dim_member == null) {
                        dim_member = new MdfCoreDimensionMember(Session);
                        Dimension.DimensionMembers.Add(dim_member);
                        dim_member.DomainMember = value;
                    }
                    DimensionMember = dim_member;
                }
                if (value == null || Dimension == null) {
                    DimensionMember = null;
                }
                if (old != DomainMember) {
                    OnChanged(nameof(DomainMember), old, DomainMember);
                }
            }
        }
        [Browsable(false)]
        public IList<MdfCoreDomainMember> DomainMemberSource {
            get {
                if (CalcType == MdfCoreDomainMemberCalcType.GENERAL) {
                    return Dimension?.Domain?.Members;
                }
                else if (CalcType == MdfCoreDomainMemberCalcType.QUERY) {
                    return Dimension?.Domain?.Members.Where(x => x.CalcType == MdfCoreDomainMemberCalcType.QUERY).ToList();
                }
                else if (CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                    return Dimension?.Domain.Members.Where(x => x.CalcType == MdfCoreDomainMemberCalcType.CALCULATED &&
                                                                  x.CalcDimension == CalcDimension).ToList();
                }
                else
                    return new List<MdfCoreDomainMember>();
            }
        }

        private MdfCoreDimension _CalcDimension;
        [DataSourceProperty(nameof(CalcDimensionSource))]
        public MdfCoreDimension CalcDimension {
            get {
                if (!IsLoading && CalcType == MdfCoreDomainMemberCalcType.CALCULATED && DimensionMember != null) {
                    return DimensionMember.DomainMember.CalcDimension;
                }
                return _CalcDimension;
            }
            set { SetPropertyValue(ref _CalcDimension, value); }
        }
        [Browsable(false)]
        public IList<MdfCoreDimension> CalcDimensionSource {
            get {
                var result = new List<MdfCoreDimension>(16);
                if (Dimension != null) {
                    foreach (var dim in Dimension.Domain.Container.Dimensions) {
                        var prop = dim.Domain.Propertys.FirstOrDefault(x => x.PropertyType == Dimension.Domain);
                        if (prop != null)
                            result.Add(dim);
                    }
                }
                return result;
            }
        }

        private MdfCoreDomainProperty _CalcProperty;
        [DataSourceProperty(nameof(CalcPropertySource))]
        public MdfCoreDomainProperty CalcProperty {
            get {
                if (!IsLoading && CalcType == MdfCoreDomainMemberCalcType.CALCULATED && DimensionMember != null) {
                    return DimensionMember.DomainMember.CalcProperty;
                }
                return _CalcProperty;
            }
            set { SetPropertyValue(ref _CalcProperty, value); }
        }

        [Browsable(false)]
        public IList<MdfCoreDomainProperty> CalcPropertySource {
            get {
                var result = new List<MdfCoreDomainProperty>(16);
                if (CalcDimension != null) {
                    foreach (var prop in CalcDimension.Domain.Propertys) {
                        if (prop.PropertyType == Dimension.Domain) {
                            result.Add(prop);
                        }
                    }
                }
                return result;
            }
        }

        public MdfCoreHierarchy CalcHierarchy {
            get { return CalcType == MdfCoreDomainMemberCalcType.HIERARCHY ? DomainMember?.CalcHierarchy : null; }
        }

        [Association("FmMdfCoreCategory-FmMdfCoreAxisOrdinate")]
        [ExplicitLoading(2)]
        [Persistent(nameof(CategoryMember))]
        private MdfCoreCategoryMember _CategoryMember;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [PersistentAlias(nameof(_CategoryMember))]
        public MdfCoreCategoryMember CategoryMember {
            get { return _CategoryMember; }
        }
        public void CategoryMemberSet(MdfCoreCategoryMember value) {
            SetPropertyValue(nameof(CategoryMember),  ref _CategoryMember, value);
        }

        private MdfCoreAxis _AxisLine;
        [Association("MdfAxis-MdfAxisOrdinateLine")]
        public MdfCoreAxis AxisLine {
            get { return _AxisLine; }
            set { SetPropertyValue<MdfCoreAxis>(ref _AxisLine, value); }
        }

        private MdfCoreAxis _Axis;
        [Association("MdfAxis-MdfAxisOrdinate")]
        public MdfCoreAxis Axis {
            get { return _Axis; }
            set { SetPropertyValue<MdfCoreAxis>(ref _Axis, value); }
        }

        private MdfCoreAxisOrdinate _Up;
        [Association("MdfAxisOrdinateUp-MdfAxisOrdinateDown")]
        [DataSourceProperty(nameof(Axis)+"."+nameof(MdfCoreAxis.Ordinates))]
        public MdfCoreAxisOrdinate Up {
            get { return _Up; }
            set { SetPropertyValue<MdfCoreAxisOrdinate>(ref _Up, value); }
        }
        [DataSourceProperty(nameof(Axis)+"."+nameof(MdfCoreAxis.Ordinates))]
        [PersistentAlias(nameof(Up))]
        [VisibleInListView(false)]
        public MdfCoreAxisOrdinate UpUi {
            get { return Up;  }
            set { Up = value; }
        }

        [Association("MdfAxisOrdinateUp-MdfAxisOrdinateDown")]
        [Aggregated]
        public XPCollection<MdfCoreAxisOrdinate> Downs {
            get { return GetCollection<MdfCoreAxisOrdinate>(); }
        }

        public MdfCoreAxisOrdinate(Session session) : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
//            CategoryMember = new MdfCoreCategoryMember(Session);
//            CategoryMember.AxisOrdinateSet(this);
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(Up):
                    Axis = Up?.Axis;
                    CategoryMemberUpdate();
                    break;
                case nameof(Dimension):
                    if (Dimension != DimensionMember?.Dimension) {
                        DimensionMember = null;
                    }
                    CategoryMemberUpdate();
                    break;
                case nameof(DimensionMember):
                    if (DimensionMember != null && DimensionMember.Dimension != Dimension) {
                        Dimension = DimensionMember?.Dimension;
                    }
                    else {
                        CategoryMemberUpdate();
                    }
                    if (DimensionMember != null) {
                        CalcDimension = DimensionMember?.DomainMember.CalcDimension;
                        CalcProperty = DimensionMember?.DomainMember.CalcProperty;
                    }
                    else {
                        CalcDimension = null;
                        CalcProperty = null;
                    }
                    break;
                case nameof(CalcDimension):
                    if (CalcDimension != null) {
                        if (DimensionMember?.DomainMember.CalcType != MdfCoreDomainMemberCalcType.CALCULATED) {
                            DimensionMember = null;
                        }
                        if (CalcProperty?.Domain != CalcDimension?.Domain)
                            CalcProperty = null;
                    }
                    break;
                case nameof(CalcProperty):
                    if (CalcProperty != null) {
                        if(CalcProperty?.Domain != CalcDimension?.Domain)
                            CalcDimension = null;
                    }
                    break;
            }
        }

        public void CategoryMemberUpdate() {
            if (Dimension != null ) {
                Dictionary<MdfCoreDimension, MdfCoreDimensionMember> dict;
                if (Up?.CategoryMember != null) {
                    dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>(Up.CategoryMember.CategoryMemberFields.Count + 1);
                    foreach (var field in Up.CategoryMember.CategoryMemberFields) {
                        dict[field.CategoryTypeField.Dimension] = field.DimensionMember;
                    }
                }
                else {
                    dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>(8);
                }
                if (DimensionMember != null) {
                    dict[Dimension] = DimensionMember;
                    foreach (var dim_dep in Dimension.DimensionDependents) {
                        if (!dict.ContainsKey(dim_dep.DimensionDependent)) {
                            dict[dim_dep.DimensionDependent] = dim_dep.DimensionMember;
                        }
                    }
                }
                else if (dict.ContainsKey(Dimension)) {
                    dict.Remove(Dimension);
                }
                MdfCoreCategory category = Axis?.Container?.CategoryGet(dict.Keys);
                CategoryMemberSet(category?.CategoryMemberGet(dict));
            }
            else {
                CategoryMemberSet(Up?.CategoryMember);
            }
            foreach (var down_ord in Downs) {
                down_ord.CategoryMemberUpdate();
            }
        }

        public override string ToString() {
            return $@"{Code} - {NameShort}";
        }

        ITreeNode ITreeNode.Parent {
            get { return Up; }
        }

        IBindingList ITreeNode.Children {
            get { return Downs; }
        }

        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }

    }

}