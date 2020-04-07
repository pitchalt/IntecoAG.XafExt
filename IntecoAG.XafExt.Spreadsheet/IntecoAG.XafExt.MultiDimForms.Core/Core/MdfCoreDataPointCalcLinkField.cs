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

    public enum MdfCoreDataPointCalcLinkFieldType {
        FROM_SOURCE = 0,
        EXPLICIT = 1,
        NOT_USED = 2
    }

    [Persistent("FmMdfCoreDataPointCalcLinkField")]
    public class MdfCoreDataPointCalcLinkField : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private Int32 _CalcIndex;
        public Int32 CalcIndex {
            get { return _CalcIndex; }
            set { SetPropertyValue(ref _CalcIndex, value); }
        }

        private MdfCoreDataPointCalcLink _CalcLink;
        [Association()]
        public MdfCoreDataPointCalcLink CalcLink {
            get { return _CalcLink; }
            set { SetPropertyValue(ref _CalcLink, value); }
        }

        private MdfCoreDataPointCalcLinkFieldType _FieldType;
        public MdfCoreDataPointCalcLinkFieldType FieldType {
            get { return _FieldType; }
            set { SetPropertyValue(ref _FieldType, value); }
        }

        private MdfCoreDimension _Dimension;
        //[Association()]
        //[DataSourceProperty(nameof(CalcLink)nameof(Category) + "." + nameof(MdfCoreCategory.Container) + "." + nameof(MdfCoreContainer.Dimensions))]
        [DataSourceProperty(nameof(DimensionSource))]
        [RuleRequiredField]
        public MdfCoreDimension Dimension {
            get { return _Dimension; }
            set { SetPropertyValue(ref _Dimension, value); }
        }
        [Browsable(false)]
        public XPCollection<MdfCoreDimension> DimensionSource {
            get { return CalcLink.DataPoint.Container.Dimensions; }
        }

        private MdfCoreDimensionMember _DimensionMember;
        //[Association("FmMdfCoreDimensionMember-FmMdfCoreCategoryField")]
        [ExplicitLoading(2)]
        [DataSourceProperty(nameof(Dimension) + "." + nameof(MdfCoreDimension.DimensionMembers))]
        public MdfCoreDimensionMember DimensionMember {
            get { return _DimensionMember; }
            set { SetPropertyValue(ref _DimensionMember, value); }
        }

        public MdfCoreDataPointCalcLinkField(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(FieldType):
                    CalcLink?.UpdateFields();
                    break;
                case nameof(Dimension):
                    CalcLink?.UpdateFields();
                    break;
                case nameof(DimensionMember):
                    CalcLink?.UpdateFields();
                    break;
                case nameof(CalcLink):
//                    CalcLink?.UpdateFields();
                    break;
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