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
    [Persistent("FmMdfCoreDimensionDependent")]
//    [RuleCombinationOfPropertiesIsUnique(nameof(Dimension) + "," + nameof(DimensionDependent))]
    [RuleCombinationOfPropertiesIsUnique(nameof(Dimension) + "," + nameof(SortOrder))]
    public class MdfCoreDimensionDependent : IagBaseObject { //, IDimension { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreDimension _Dimension;
        [Association("FmMdfCoreDimension-FmMdfCoreDimensionDependent")]
        public MdfCoreDimension Dimension {
            get { return _Dimension; }
            set { SetPropertyValue<MdfCoreDimension>(ref _Dimension, value); }
        }

        private Int32 _SortOrder;
        [RuleRequiredField]
        public Int32 SortOrder {
            get { return _SortOrder; }
            set { SetPropertyValue<Int32>(ref _SortOrder, value); }
        }

        private MdfCoreDimension _DimensionDependent;
        [RuleRequiredField]
        [DataSourceProperty(nameof(DimensionDependentSource))]
        public MdfCoreDimension DimensionDependent {
            get { return _DimensionDependent; }
            set { SetPropertyValue<MdfCoreDimension>(ref _DimensionDependent, value); }
        }
        [Browsable(false)]
        public IList<MdfCoreDimension> DimensionDependentSource {
            get {
                var dims = new List<MdfCoreDimension>(16);
                dims.Add(Dimension);
                foreach (var dim_dep in Dimension.DimensionDependents) {
                    if (dim_dep.SortOrder < SortOrder) {
                        dims.Add(dim_dep.DimensionDependent);
                    }
                }
                var result = new List<MdfCoreDimension>(16);
                foreach (var dim in Dimension.Container.Dimensions) {
                    if (dim.Domain.Members.Where(
                        x => x.CalcType == MdfCoreDomainMemberCalcType.CALCULATED &&
                            dims.Contains(x.CalcDimension)).Count() > 0) {
                        result.Add(dim);
                    } 
                }
                return result; 
            }
        }

        private MdfCoreDimensionMember _DimensionMember;
        [DataSourceProperty(nameof(DimensionMemberSource))]
        [RuleRequiredField]
        public MdfCoreDimensionMember DimensionMember {
            get { return _DimensionMember; }
            set { SetPropertyValue<MdfCoreDimensionMember>(ref _DimensionMember, value); }
        }


        [Browsable(false)]
        public IList<MdfCoreDimensionMember> DimensionMemberSource {
            get {
                var dims = new List<MdfCoreDimension>(16);
                dims.Add(Dimension);
                foreach (var dim_dep in Dimension.DimensionDependents) {
                    if (dim_dep.SortOrder < SortOrder) {
                        dims.Add(dim_dep.DimensionDependent);
                    }
                }
                return DimensionDependent?.DimensionMembers.Where(
                    x => x.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED && 
                        dims.Contains(x.DomainMember.CalcDimension )).ToList();
            }
        }

        public MdfCoreDimensionDependent(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(DimensionDependent):
                case nameof(Dimension):
                case nameof(DimensionMember):
                    if (Dimension != null && DimensionDependent != null && DimensionMember != null) {
                        foreach (var ord in Dimension.AxisOrdinates) {
                            ord.CategoryMemberUpdate();
                        } 
                    }
                    break;
            }
        }
    }
}