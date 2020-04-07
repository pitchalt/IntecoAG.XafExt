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
    [Persistent("FmMdfCoreDomain")]
    public class MdfCoreDomain : MdfCoreType {//, IDomain { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).


        public enum MdfCoreDomainType {
            DOMAIN_NOT_PERSISTENT = 0,
            DOMAIN_OBJECT = 1,
            DOMAIN_ENUM = 3,
            DOMAIN_REPORT = 4,
            DOMAIN_TEMPLATE = 5,
            DOMAIN_CONCEPT = 6
        }

        private MdfCoreDomainType _DomainType;
//        [ModelDefault("AllowEdit", "False")]
        public MdfCoreDomainType DomainType {
            get { return _DomainType; }
            set { SetPropertyValue(ref _DomainType, value); }
        }

        private MdfCoreDataType _DataType;
        public MdfCoreDataType DataType {
            get { return _DataType; }
            set { SetPropertyValue(ref _DataType, value); }
        }

        [DataSourceProperty(nameof(DomainContainersSource))]
        [Association]
        public XPCollection<MdfCoreDomainContainer> DomainContainers {
            get { return GetCollection<MdfCoreDomainContainer>(); }
        }
        [Browsable(false)]
        public IList<MdfCoreDomainContainer> DomainContainersSource {
            get {
                var result = new List<MdfCoreDomainContainer>();
                foreach (var domain in Container.Domains) {
                    MdfCoreDomainContainer cont = domain as MdfCoreDomainContainer;
                    if (cont != null) {
                        result.Add(cont);
                    }
                }
                return result;
            }
        }

        //private MdfCoreDomainContainer _DomainContainer;
        //[DataSourceProperty(nameof(Container) + "." + nameof(MdfCoreContainer.Domains))]
        //[Association]
        //public MdfCoreDomainContainer DomainContainer {
        //    get { return _DomainContainer; }
        //    set { SetPropertyValue(ref _DomainContainer, value); }
        //}

        [Association]
        [Aggregated]
        public XPCollection<MdfCoreDomainProperty> Propertys {
            get { return GetCollection<MdfCoreDomainProperty>(); }
        }

        [Association("FmMdfCoreDomain-FmMdfCoreDomainMember")]
        [Aggregated]
        public XPCollection<MdfCoreDomainMember> Members{
            get { return GetCollection<MdfCoreDomainMember>(); }
        }

        [Association("FmMdfCoreDomain-FmMdfCoreHierarchy")]
        [Aggregated]
        public XPCollection<MdfCoreHierarchy> Hierarchys {
            get { return GetCollection<MdfCoreHierarchy>(); }
        }

        [Association("FmMdfCoreDomain-FmMdfCoreDimension")]
        [Aggregated]
        public XPCollection<MdfCoreDimension> Dimensions {
            get { return GetCollection<MdfCoreDimension>(); }
        }

        private MdfCoreContainer _Container;
        [Association("FmMdfContainer-FmMdfCoreDomain")]
        public MdfCoreContainer Container {
            get { return _Container; }
            set { SetPropertyValue<MdfCoreContainer>(ref _Container, value); }
        }

        //IEnumerable<IDomainMember> IDomain.Members => Members;

        //IEnumerable<IDimension> IDomain.Dimensions => Dimensions;

        //IContainer IContainerized.Container => Container;

        public MdfCoreDomain(Session session)
            : base(session) {
        }

        //IDomainMember IDomain.MemberCreate() {
        //    throw new NotImplementedException();
        //}

        //IDomainMember IDomain.MemberCreate(string code) {
        //    throw new NotImplementedException();
        //}

        //IDimension IDomain.DimensionCreate() {
        //    throw new NotImplementedException();
        //}

        //IDimension IDomain.DimensionCreate(string code) {
        //    throw new NotImplementedException();
        //}

        public override void AfterConstruction() {
            base.AfterConstruction();
            DomainType = MdfCoreDomainType.DOMAIN_NOT_PERSISTENT;
        }

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(DataType):
                    if (DomainType != MdfCoreDomainType.DOMAIN_REPORT && DomainType != MdfCoreDomainType.DOMAIN_TEMPLATE && DomainType != MdfCoreDomainType.DOMAIN_CONCEPT) {
                        if (DataType == MdfCoreDataType.DT_OBJECT)
                            DomainType = MdfCoreDomainType.DOMAIN_OBJECT;
                        else if (DataType == MdfCoreDataType.DT_ENUM)
                            DomainType = MdfCoreDomainType.DOMAIN_ENUM;
                        else
                            DomainType = MdfCoreDomainType.DOMAIN_NOT_PERSISTENT;
                    }
                    break;
            }
        }

        public override string ToString() {
            return Code;
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

    }
}