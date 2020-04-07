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
    [Persistent("FmMdfReportDomainMember")]
    public abstract class MdfReportDomainMember : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [Persistent(nameof(Report))]
        private MdfReport _Report;
        [PersistentAlias(nameof(_Report))]
        [Browsable(false)]
        public abstract MdfReport Report { get; }
        public void ReportSet(MdfReport value) {
            SetPropertyValue(ref _Report, value);
        }

        private String _Code;
        [Size(64)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>(ref _Code, value); }
        }

        private String _NameShort;
        [Size(128)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue<String>(ref _NameShort, value); }
        }

        [Association("FmMdfReportDomainMemberCoreLink-MdfReportDomainMember")]
        [Persistent(nameof(DomainMemberCoreLink))]
        private MdfReportDomainMemberCoreLink _DomainMemberCoreLink;
        [PersistentAlias(nameof(_DomainMemberCoreLink))]
        public MdfReportDomainMemberCoreLink DomainMemberCoreLink {
            get { return _DomainMemberCoreLink; }
        }
        public void DomainMemberCoreLinkSet(MdfReportDomainMemberCoreLink  value) {
            SetPropertyValue(ref _DomainMemberCoreLink, value);
        }

        private MdfCoreDomain _CoreDomain;
        [DataSourceProperty(nameof(Report) + "." + nameof(MdfReport.Template) + "." + nameof(MdfTemplate.Container) + "." + nameof(MdfCoreContainer.Domains))]
        public MdfCoreDomain CoreDomain {
            get { return _CoreDomain; }
            set { SetPropertyValue(ref _CoreDomain, value); }
        }

        private MdfCoreDomainMember _CoreDomainMember;
        [DataSourceProperty(nameof(CoreDomain) + "." + nameof(MdfCoreDomain.Members))]
        public MdfCoreDomainMember CoreDomainMember {
            get { return _CoreDomainMember; }
            set { SetPropertyValue(ref _CoreDomainMember, value); }
        }

        protected MdfReportDomainMember(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(CoreDomain):
                    if (!ReferenceEquals(CoreDomain, CoreDomainMember?.Domain)) {
                        CoreDomainMember = null;
                    }
                    break;
                case nameof(CoreDomainMember):
                    if (!ReferenceEquals(DomainMemberCoreLink?.CoreDomain, CoreDomain) ||
                        !ReferenceEquals(DomainMemberCoreLink?.CoreDomainMember, CoreDomainMember)) {
                        DomainMemberCoreLinkUnSet();
                    }
                    if (CoreDomainMember != null) {
                        CoreDomain = CoreDomainMember.Domain;
                        DomainMemberCoreLinkSet(CoreDomain, CoreDomainMember);
                        Code = CoreDomainMember.Code;
                        NameShort = CoreDomainMember.NameShort;
                    }
                    break;
                case nameof(DomainMemberCoreLink):
                    break;
            }
        }

        protected override void OnDeleting() {
            base.OnDeleting();
            DomainMemberCoreLinkUnSet();
        }

        protected void DomainMemberCoreLinkSet(MdfCoreDomain domain, MdfCoreDomainMember domain_member) {
            MdfReportDomainMemberCoreLink link = Report.DomainCoreLinks.FirstOrDefault(
                                                     x => ReferenceEquals(x.CoreDomain, CoreDomain) && ReferenceEquals(x.CoreDomainMember, CoreDomainMember));
            if (link == null) {
                link = new MdfReportDomainMemberCoreLink(Session) {
                    CoreDomain = CoreDomain,
                    CoreDomainMember = CoreDomainMember
                };
                Report.DomainCoreLinks.Add(link);
            }
            link.ReportDomainMembers.Add(this);
        }

        protected void DomainMemberCoreLinkUnSet() {
            if (DomainMemberCoreLink != null) {
                var link = DomainMemberCoreLink;
                link.ReportDomainMembers.Remove(this);
                if (link.ReportDomainMembers.Count == 0) {
                    link.Delete();
                }
            }
        }

        public override string ToString() {
            return base.ToString();
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