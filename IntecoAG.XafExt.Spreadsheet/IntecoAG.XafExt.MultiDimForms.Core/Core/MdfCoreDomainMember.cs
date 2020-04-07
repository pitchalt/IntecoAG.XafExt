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
    public enum MdfCoreDomainMemberCalcType {
        GENERAL = 0,
        CALCULATED = 1,
        HIERARCHY = 2,
        QUERY = 3
    }

    [Persistent("FmMdfCoreDomainMember")]
    public class MdfCoreDomainMember : MdfCoreValue { //, IDomainMember { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreDomain _Domain;
        [Association("FmMdfCoreDomain-FmMdfCoreDomainMember")]
        public MdfCoreDomain Domain {
            get { return _Domain; }
            set { SetPropertyValue(ref _Domain, value); }
        }

        private MdfCoreDomainMemberCalcType _CalcType;
        public MdfCoreDomainMemberCalcType CalcType {
            get { return _CalcType; }
            set { SetPropertyValue(ref _CalcType, value); }
        }

        [Association("FmMdfCoreDomainMember-FmMdfCoreDimensionMember")]
        public XPCollection<MdfCoreDimensionMember> DimensionMembers {
            get { return GetCollection<MdfCoreDimensionMember>(); }
        }

        [Association("FmMdfCoreDomainMember-FmMdfCoreHierarchyNode")]
        public XPCollection<MdfCoreHierarchyNode> HierarchyNodes {
            get { return GetCollection<MdfCoreHierarchyNode>(); }
        }

        private MdfCoreDimension _CalcDimension;
        [DataSourceProperty(nameof(CalcDimensionSource))]
        public MdfCoreDimension CalcDimension {
            get { return _CalcDimension; }
            set { SetPropertyValue<MdfCoreDimension>(ref _CalcDimension, value); }
        }

        [Browsable(false)]
        public IList<MdfCoreDimension> CalcDimensionSource {
            get {
                var result = new List<MdfCoreDimension>(16);
                foreach (var dim in Domain.Container.Dimensions) {
                    var prop = dim.Domain.Propertys.FirstOrDefault(x => x.PropertyType == Domain);
                    if (prop != null)
                        result.Add(dim);
                }
                return result;
            }
        }

        private MdfCoreDomainProperty _CalcProperty;
        [DataSourceProperty(nameof(CalcPropertySource))]
        public MdfCoreDomainProperty CalcProperty {
            get { return _CalcProperty;  }
            set { SetPropertyValue(ref _CalcProperty, value); }
        }

        [Browsable(false)]
        public IList<MdfCoreDomainProperty> CalcPropertySource {
            get {
                var result = new List<MdfCoreDomainProperty>(16);
                if (CalcDimension != null) {
                    foreach (var prop in CalcDimension.Domain.Propertys) {
                        if (prop.PropertyType == Domain) {
                            result.Add(prop);
                        }
                    }
                }
                return result;
            }
        }

        private MdfCoreHierarchy _CalcHierarchy;
        [DataSourceProperty(nameof(CalcHierarchySource))]
        public MdfCoreHierarchy CalcHierarchy {
            get { return _CalcHierarchy; }
            set { SetPropertyValue(ref _CalcHierarchy, value); }
        }

        [Browsable(false)]
        public IList<MdfCoreHierarchy> CalcHierarchySource {
            get {
                return Domain.Container.Hierarchys.Where(x => x.Domain == Domain).ToList();
            }
        }

        private MdfCoreDataType _DataPointDataType;
        public MdfCoreDataType DataPointDataType {
            get { return _DataPointDataType; }
            set { SetPropertyValue(ref _DataPointDataType, value); }
        }

        public override MdfCoreType CoreType {
            get { return Domain; }
        }

        public MdfCoreDomainMember(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            CalcType = MdfCoreDomainMemberCalcType.GENERAL;
        }

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(Name):
                case nameof(Code):
                    UpdateDimensionMembers();
                    break;
                case nameof(CalcDimension):
                case nameof(CalcProperty):
                    UpdateCode();
                    break;
            }
        }

        private void UpdateDimensionMembers() {
            foreach (var member in DimensionMembers) {
                member.Code = Code;
                member.NameShort = NameShort;
            }
        }

        private void UpdateCode() {
            if (CalcDimension != null && CalcProperty != null) {
                CalcType = MdfCoreDomainMemberCalcType.CALCULATED;
                Code = CalcDimension.Code + "_" + CalcProperty.Code;
            }
            else {
                CalcType = MdfCoreDomainMemberCalcType.CALCULATED;
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

        //protected override void OnChanged(String property_name, Object old_value, Object new_value) {
        //    base.OnChanged(property_name, old_value, new_value);
        //}
    }
}