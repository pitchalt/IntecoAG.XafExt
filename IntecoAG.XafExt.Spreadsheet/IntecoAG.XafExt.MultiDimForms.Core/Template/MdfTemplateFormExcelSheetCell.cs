using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;
//
using IntecoAG.XpoExt;
//
namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    //[DomainComponent]
    //[DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreTemplateFormExcelSheetCell")]
    public class MdfTemplateFormExcelSheetCell : IagBaseObject {

        private MdfTemplateFormExcelSheet _Sheet;
        [Association("FmMdfCoreTemplateFormExcelSheet-FmMdfCoreTemplateFormExcelSheetCell")]
        public MdfTemplateFormExcelSheet Sheet {
            get { return _Sheet; }
            set { SetPropertyValue(ref _Sheet, value); }
        }

        private MdfTemplateFormExcelSheetRow _Row;
        [Association]
        public MdfTemplateFormExcelSheetRow Row {
            get { return _Row; }
            set { SetPropertyValue(ref _Row, value); }
        }

        [PersistentAlias(nameof(Row) + "." + nameof(MdfTemplateFormExcelSheetRow.Index))]
        public Int32 RowIndex {
            get { return Row.Index; }
        }

        private MdfTemplateFormExcelSheetColumn _Column;
        [Association]
        public MdfTemplateFormExcelSheetColumn Column {
            get { return _Column; }
            set { SetPropertyValue(ref _Column, value); }
        }

        [PersistentAlias(nameof(Column) + "." + nameof(MdfTemplateFormExcelSheetColumn.Index))]
        public Int32 ColumnIndex {
            get { return Column.Index; }
        }

        private MdfCoreTableCell _TableCell;
        public MdfCoreTableCell TableCell {
            get { return _TableCell; }
            set { SetPropertyValue(ref _TableCell, value); }
        }

        private MdfCoreAxisOrdinate _AxisOrdinate;
        public MdfCoreAxisOrdinate AxisOrdinate {
            get { return _AxisOrdinate; }
            set { SetPropertyValue(ref _AxisOrdinate, value); }
        }

        private String _Formula;
        public String Formula {
            get { return _Formula; }
            set { SetPropertyValue(ref _Formula, value); }
        }

        private MdfCoreDataType _ValueDataType;
        public MdfCoreDataType ValueDataType {
            get { return _ValueDataType; }
            set { SetPropertyValue(ref _ValueDataType, value); }
        }

        [NonPersistent]
        public Object Value {
            get {
                switch (ValueDataType) {
                    case MdfCoreDataType.DT_BOOLEAN:
                        return ValueBoolean;
                    case MdfCoreDataType.DT_INTEGER:
                        return ValueInteger;
                    case MdfCoreDataType.DT_DATE:
                        return ValueDateTime;
                    case MdfCoreDataType.DT_DECIMAL:
                        return ValueDecimal;
                    case MdfCoreDataType.DT_STRING:
                        return ValueString;
                    default:
                        return null;
                }
            }
            set {
                ValueDataType = MdfCoreDataType.DT_UNDEFINED;
                ValueBoolean = false;
                ValueInteger = 0;
                ValueDecimal = 0M;
                ValueDateTime = default(DateTime);
                ValueString = null;
                switch (value) {
                    case null:
                        ValueDataType = MdfCoreDataType.DT_UNDEFINED;
                        break;
                    case Boolean valueBoolean:
                        ValueBoolean = valueBoolean;
                        ValueDataType = MdfCoreDataType.DT_BOOLEAN; 
                        break;
                    case Decimal valueDecimal:
                        ValueDecimal = valueDecimal;
                        ValueDataType = MdfCoreDataType.DT_DECIMAL;
                        break;
                    case Int16 valueInt16:
                        ValueInteger = valueInt16;
                        ValueDataType = MdfCoreDataType.DT_INTEGER;
                        break;
                    case Int32 valueInt32:
                        ValueInteger = valueInt32;
                        ValueDataType = MdfCoreDataType.DT_INTEGER;
                        break;
                    case Int64 valueInt64:
                        ValueInteger = valueInt64;
                        ValueDataType = MdfCoreDataType.DT_INTEGER;
                        break;
                    case DateTime valueDateTime:
                        ValueDateTime = valueDateTime;
                        ValueDataType = MdfCoreDataType.DT_DATE;
                        break;
                    case String valueString:
                        ValueString = valueString;
                        ValueDataType = MdfCoreDataType.DT_STRING;
                        break;
                }
            }
        }

        private Boolean _ValueBoolean;
        public Boolean ValueBoolean {
            get { return _ValueBoolean; }
            set { SetPropertyValue(ref _ValueBoolean, value); }
        }
        private Int64 _ValueInteger;
        public Int64 ValueInteger {
            get { return _ValueInteger; }
            set { SetPropertyValue(ref _ValueInteger, value); }
        }
        private Decimal _ValueDecimal;
        public Decimal ValueDecimal {
            get { return _ValueDecimal; }
            set { SetPropertyValue(ref _ValueDecimal, value); }
        }
        private DateTime _ValueDateTime;
        public DateTime ValueDateTime {
            get { return _ValueDateTime; }
            set { SetPropertyValue(ref _ValueDateTime, value); }
        }
        private String _ValueString;
        public String ValueString {
            get { return _ValueString; }
            set { SetPropertyValue(ref _ValueString, value); }
        }

        public MdfTemplateFormExcelSheetCell(Session session) : base(session) { }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(Row):
                case nameof(Column):
                    if (Row != null && Column != null) {
                        Column.CellsAdd(this);
                        Row.CellsAdd(this);
                    }
                    break;
                case nameof(AxisOrdinate):
                    if (AxisOrdinate != null) {
                        Value = AxisOrdinate.Code;
                    }
                    break;
            }
        }

        //protected MdfTemplateFormExcelSheetCell(Session session, MdfTemplateFormExcelSheetRow row, MdfTemplateFormExcelSheetColumn column): base(session) {
        //    Row = row;
        //    Column = column;
        //}
    }
/*
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class MdfTemplateFormExcelSheetCellTableCell1: MdfTemplateFormExcelSheetCell {

        private MdfCoreTableCell _TableCell;
        public MdfCoreTableCell TableCell {
            get { return _TableCell; }
            set { SetPropertyValue(ref _TableCell, value); }
        }

        public override object Value {
            get { return TableCell.DataPoint.Code; }
            set { }
        }

        public MdfTemplateFormExcelSheetCellTableCell1(Session session) : base(session) { }
        //public  MdfTemplateFormExcelSheetCellTableCell(Session session, MdfTemplateFormExcelSheetRow row, MdfTemplateFormExcelSheetColumn column,
        //    MdfCoreTableCell cell) : base(session, row, column){
        //    _TableCell = cell;
        //}
    }

    [MapInheritance(MapInheritanceType.ParentTable)]
    public class MdfTemplateFormExcelSheetCellAxisOrdinate1 : MdfTemplateFormExcelSheetCell {

        private MdfCoreAxisOrdinate _AxisOrdinate;
        public MdfCoreAxisOrdinate AxisOrdinate {
            get { return _AxisOrdinate; }
            set { SetPropertyValue(ref _AxisOrdinate, value); }
        }

        public override object Value {
            get { return AxisOrdinate.Code; }
            set { }
        }

        public MdfTemplateFormExcelSheetCellAxisOrdinate1(Session session):base(session) { }
        //public MdfTemplateFormExcelSheetCellAxisOrdinate(Session session, MdfTemplateFormExcelSheetRow row, MdfTemplateFormExcelSheetColumn column,
        //    MdfCoreAxisOrdinate ordinate) : base(session, row, column) {
        //    _AxisOrdinate = ordinate;
        //}
    }
*/
}