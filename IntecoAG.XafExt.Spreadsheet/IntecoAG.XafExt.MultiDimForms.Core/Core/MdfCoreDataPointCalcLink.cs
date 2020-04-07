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

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public enum MdfCoreDataPointCalcLinkType {
        SCALAR = 0,
        AGG_SUM = 1,
    }
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreDataPointCalcLink")]
    public class MdfCoreDataPointCalcLink : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private Boolean _IsUsed;
        public Boolean IsUsed {
            get { return _IsUsed; }
            set { SetPropertyValue(ref _IsUsed, value); }
        }

        private MdfCoreDataPointCalcLinkType _LinkType;
        public MdfCoreDataPointCalcLinkType LinkType {
            get { return _LinkType; }
            set { SetPropertyValue(ref _LinkType, value); }
        }

        private Int32 _Index;
        public Int32 Index {
            get { return _Index; }
            set { SetPropertyValue(ref _Index, value); }
        }

        private MdfCoreDataPoint _DataPoint;
        [Association()]
        public MdfCoreDataPoint DataPoint {
            get { return _DataPoint; }
            set { SetPropertyValue(ref _DataPoint, value); }
        }

        private MdfCoreTable _Table;
        [Association]
        public MdfCoreTable Table {
            get { return _Table; }
            set { SetPropertyValue(ref _Table, value); }
        }

        private MdfCoreTableCell _TableCell;
        [DataSourceProperty(nameof(Calc) + "." + nameof(MdfCoreDataPointCalc.Table) + "." + nameof(MdfCoreTable.Cells))]
        [Association]
        public MdfCoreTableCell TableCell {
            get { return _TableCell; }
            set { SetPropertyValue(ref _TableCell, value); }
        }

        private MdfCoreDataPointCalc _Calc;
        [Association()]
        [ExplicitLoading(2)]
        public MdfCoreDataPointCalc Calc {
            get { return _Calc; }
            set { SetPropertyValue(ref _Calc, value); }
        }

        private String _Formula;
        public String Formula {
            get { return _Formula; }
            set { SetPropertyValue(ref _Formula, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<MdfCoreDataPointCalcLinkField> LinkFields {
            get { return GetCollection<MdfCoreDataPointCalcLinkField>(); }
        }

        public MdfCoreDataPointCalcLink(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                //case nameof(TableCell):
                //    if (TableCell != null) {
                //        Table = TableCell.Table;
                //        DataPoint = TableCell.DataPoint;
                //    }
                //    break;
                case nameof(DataPoint):
                    if (DataPoint != null)
                        UpdateFields();
                    else
                        Session.Delete(LinkFields);
                    break;
                case nameof(Calc):
                    Table = Calc?.Table ?? Table;
                    break;
            }
        }

        public override string ToString() {
            return base.ToString();
        }

        private void ResortInternal() {
            Int32 current = 1;
            for (Int32 pass = 0; pass < LinkFields.Count && current < LinkFields.Count; pass++) {
                for (Int32 i = 0; i < LinkFields.Count; i++) {
                    if (LinkFields[i].CalcIndex >= 0 || LinkFields[i].Dimension == null || LinkFields[i].DimensionMember == null)
                        continue;
                    if (LinkFields[i].FieldType == MdfCoreDataPointCalcLinkFieldType.NOT_USED ||
                        LinkFields[i].FieldType == MdfCoreDataPointCalcLinkFieldType.FROM_SOURCE ||
                        LinkFields[i].FieldType == MdfCoreDataPointCalcLinkFieldType.EXPLICIT &&
                        LinkFields[i].DimensionMember.DomainMember.CalcType == MdfCoreDomainMemberCalcType.GENERAL) {
                        LinkFields[i].CalcIndex = current++;
                        continue;
                    }
                    if (LinkFields[i].FieldType == MdfCoreDataPointCalcLinkFieldType.EXPLICIT &&
                        LinkFields[i].DimensionMember.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                        var ref_field = LinkFields.FirstOrDefault(x => x.Dimension == LinkFields[i].DimensionMember.DomainMember.CalcDimension);
                        if (ref_field != null && ref_field.CalcIndex >= 0) {
                            LinkFields[i].CalcIndex = current++;
                            continue;
                        }

                    }
                }
            }
            foreach (var field in LinkFields
                    .Where(x => x.FieldType == MdfCoreDataPointCalcLinkFieldType.EXPLICIT &&
                            x.DimensionMember?.DomainMember.CalcType == MdfCoreDomainMemberCalcType.QUERY)
                    .OrderByDescending(x => x.CalcIndex)) {
                if (field.CalcIndex < 0)
                    field.CalcIndex = current++;
            }
        }

        protected void ResortCalculation() {
            foreach (var field in LinkFields) {
                if (field.CalcIndex == 0)
                    field.CalcIndex = -1;
                if (field.CalcIndex > 0)
                    field.CalcIndex = -field.CalcIndex;
            }
            ResortInternal();
        }

        public void UpdateAction() {
            UpdateFields();
        }

        public void UpdateFields() {
            //            var is_range = false;
            var old_fields = LinkFields.ToList();
            if (Calc?.DataPoint == null)
                return;
            foreach (var field_from in Calc.DataPoint?.CategoryMember.CategoryMemberFields) {
                var dim_from = field_from.CategoryTypeField.Dimension;
                var field = LinkFields.FirstOrDefault(x => x.Dimension == dim_from);
                if (field == null) {
                    field = new MdfCoreDataPointCalcLinkField(Session);
                    LinkFields.Add(field);
                    field.Dimension = dim_from;
                    field.DimensionMember = field_from.DimensionMember;
                    field.FieldType = MdfCoreDataPointCalcLinkFieldType.FROM_SOURCE;
                }
                else {
                    old_fields.Remove(field);
                    if (field.FieldType == MdfCoreDataPointCalcLinkFieldType.FROM_SOURCE)
                        field.DimensionMember = field_from.DimensionMember;
                }
            }
            foreach (var field in old_fields) {
                if (field.FieldType == MdfCoreDataPointCalcLinkFieldType.FROM_SOURCE) {
                    field.FieldType = MdfCoreDataPointCalcLinkFieldType.NOT_USED;
                }
            }
            ResortCalculation();
            Boolean is_range = false;
            Dictionary<MdfCoreDimension, MdfCoreDimensionMember> dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>(LinkFields.Count);
            foreach (var field in LinkFields) {
                if (field.FieldType != MdfCoreDataPointCalcLinkFieldType.NOT_USED && field.DimensionMember != null) {
                    dict[field.Dimension] = field.DimensionMember;
                    if (field.FieldType == MdfCoreDataPointCalcLinkFieldType.EXPLICIT &&
                        (field.DimensionMember.DomainMember.CalcType == MdfCoreDomainMemberCalcType.QUERY ||
                        field.DimensionMember.DomainMember.CalcType == MdfCoreDomainMemberCalcType.HIERARCHY)) {
                        is_range = true;
                    }
                }
            }
            DataPoint = Calc.Table?.Container.DataPointGet(dict);
            if (LinkType == MdfCoreDataPointCalcLinkType.SCALAR && is_range)
                LinkType = MdfCoreDataPointCalcLinkType.AGG_SUM;
            if (LinkType != MdfCoreDataPointCalcLinkType.SCALAR && !is_range)
                LinkType = MdfCoreDataPointCalcLinkType.SCALAR;
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

        //protected override void OnChanged(String property_name, Object old_value, Object new_value) {
        //    base.OnChanged(property_name, old_value, new_value);
        //}
    }
}