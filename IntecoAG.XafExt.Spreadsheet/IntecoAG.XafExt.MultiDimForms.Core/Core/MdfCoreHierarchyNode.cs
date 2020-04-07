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
using DevExpress.Persistent.Base.General;
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
    [Persistent("FmMdfCoreHierarchyNode")]
    public class MdfCoreHierarchyNode : MdfCoreElement, ITreeNode { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private MdfCoreHierarchy _Hierarchy;
        [Association("FmMdfCoreHierarchy-FmMdfCoreHierarchyNode")]
        public MdfCoreHierarchy Hierarchy {
            get { return _Hierarchy; }
            set { SetPropertyValue(ref _Hierarchy, value); }
        }

        private MdfCoreDomainMember _DomainMember;
        [Association("FmMdfCoreDomainMember-FmMdfCoreHierarchyNode")]
        [DataSourceProperty(nameof(Hierarchy) + "." + nameof(MdfCoreHierarchy.Domain) + "." + nameof(MdfCoreDomain.Members))]
//        [DataSourceCriteria(nameof(MdfCoreDomainMember.Domain) + " == This." + nameof(Hierarchy) + "." + nameof(MdfCoreHierarchy.Domain))]
        public MdfCoreDomainMember DomainMember {
            get { return _DomainMember; }
            set { SetPropertyValue(ref _DomainMember, value); }
        }

        private MdfCoreHierarchyNode _Up;
        [Association("FmMdfCoreHierarchyNodeUp-FmMdfCoreHierarchyNodeDowns")]
        public MdfCoreHierarchyNode Up {
            get { return _Up; }
            set { SetPropertyValue(ref _Up, value); }
        }
        [PersistentAlias(nameof(Up))]
        [DataSourceProperty(nameof(Hierarchy) + "." + nameof(MdfCoreHierarchy.Nodes))]
        public MdfCoreHierarchyNode UpUi {
            get { return Up; }
            set { Up = value; }
        }

        private Int32 _SortOrder;
        public Int32 SortOrder {
            get { return _SortOrder; }
            set { SetPropertyValue(ref _SortOrder, value); }
        }

        [Association("FmMdfCoreHierarchyNodeUp-FmMdfCoreHierarchyNodeDowns")]
        [Aggregated]
        public XPCollection<MdfCoreHierarchyNode> Downs {
            get { return GetCollection<MdfCoreHierarchyNode>(); }
        }
        ITreeNode ITreeNode.Parent {
            get { return Up; }
        }

        IBindingList ITreeNode.Children {
            get { return Downs; }
        }

        public MdfCoreHierarchyNode(Session session) : base(session) {
        }
        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}
        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(Up):
                    Hierarchy = Up?.Hierarchy;
                    break;
                case nameof(DomainMember):
                    Code = DomainMember?.Code;
                    break;
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

    }
}