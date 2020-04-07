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
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [NavigationItem("MDF")]
    [Persistent("FmMdfCoreTemplate")]
    public class MdfCoreTemplate : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [MapInheritance(MapInheritanceType.ParentTable)]
        public class MdfCoreContainer1 : MdfCoreContainer {

            [Persistent(nameof(Template))]
            [ExplicitLoading(1)]
            private MdfCoreTemplate _Template;
            [Browsable(false)]
            [PersistentAlias(nameof(_Template))]
            public MdfCoreTemplate Template {
                get { return _Template; }
            }
            public void FrameworkSet(MdfCoreTemplate value) {
                SetPropertyValue<MdfCoreTemplate>(ref _Template, value);
            }

            public MdfCoreContainer1(Session session) : base(session) { }
        }

        [Persistent(nameof(Container))]
        [Aggregated]
        private MdfCoreContainer1 _Container;
        [PersistentAlias(nameof(_Container))]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public MdfCoreContainer1 Container {
            get { return _Container; }
        }
        public void ContainerSet(MdfCoreContainer1 value) {
            SetPropertyValue<MdfCoreContainer1>(ref _Container, value);
        }

        private String _PersistentNamespace;
        [Size(128)]
        public String PersistentNamespace {
            get { return _PersistentNamespace; }
            set { SetPropertyValue(ref _PersistentNamespace, value); }
        }

        private String _PersistentTablePrefix;
        [Size(128)]
        public String PersistentTablePrefix {
            get { return _PersistentTablePrefix; }
            set { SetPropertyValue(ref _PersistentTablePrefix, value); }
        }

        private String _CodeFilePath;
        [Size(128)]
        public String CodeFilePath {
            get { return _CodeFilePath; }
            set { SetPropertyValue(ref _CodeFilePath, value); }
        }

        private MdfFramework _Framework;
        [Association("FmMdfFramework-FmMdfTemplate")]
        public MdfFramework Framework {
            get { return _Framework; }
            set { SetPropertyValue<MdfFramework>(ref _Framework, value); }
        }

        [Persistent(nameof(Axis))]
        [Aggregated]
        private MdfCoreAxis _Axis;
        [PersistentAlias(nameof(_Axis))]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public MdfCoreAxis Axis {
            get { return _Axis; }
        }
        protected void AxisSet(MdfCoreAxis value) {
            SetPropertyValue<MdfCoreAxis>(ref _Axis, value);
        }

        [Association("FmMdfTemplate-FmMdfTemplateTable")]
        [Aggregated]
        public XPCollection<MdfTemplateTable> Tables {
            get { return GetCollection<MdfTemplateTable>(); }
        }

        [Association("FmMdfTemplate-FmMdfTemplateForm")]
        [Aggregated]
        public XPCollection<MdfTemplateForm> Forms {
            get { return GetCollection<MdfTemplateForm>(); }
        }

        private String _SourceCode;
        [Size(SizeAttribute.Unlimited)]
        public String SourceCode {
            get { return _SourceCode; }
            set { SetPropertyValue(ref _SourceCode, value); }
        }

        private MdfCoreDomainContainer _DomainReport;
        public MdfCoreDomainContainer DomainReport {
            get { return _DomainReport; }
            set { SetPropertyValue(ref _DomainReport, value); }
        }

        private MdfCoreDomainContainer _DomainTemplate;
        public MdfCoreDomainContainer DomainTemplate {
            get { return _DomainTemplate; }
            set { SetPropertyValue(ref _DomainTemplate, value); }
        }

        public MdfCoreTemplate(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            ContainerSet(CreateContainer());
            AxisSet(CreateAxis());
            DomainReport = CreateDomainContainer();
            DomainReport.Code = "REPORT";
            DomainReport.DomainType = MdfCoreDomain.MdfCoreDomainType.DOMAIN_REPORT;
            DomainReport.DataType = MdfCoreDataType.DT_OBJECT;
            DomainTemplate = CreateDomainContainer();
            DomainTemplate.Code = "TEMPLATE";
            DomainTemplate.DomainType = MdfCoreDomain.MdfCoreDomainType.DOMAIN_TEMPLATE;
            DomainTemplate.DataType = MdfCoreDataType.DT_OBJECT;
        }

        protected virtual MdfCoreContainer1 CreateContainer() {
            return new MdfCoreContainer1(Session);
        }

        protected virtual MdfCoreAxis CreateAxis() {
            MdfCoreAxis axis = new MdfCoreAxis(Session);
            Container.Axiss.Add(axis);
            return axis;
        }

        protected virtual MdfCoreDomainContainer CreateDomainContainer() {
            var cont = new MdfCoreDomainContainer(Session);
            Container.Domains.Add(cont);
            return cont;
        }

        [Action(Caption = "Compile")]
        public void CompileAction() {
            this.CodeGenerate(null);
        }
        public override String ToString() {
            return Container?.Name ?? "-";
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}