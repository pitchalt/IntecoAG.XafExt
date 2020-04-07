using System;
using System.Linq;
using System.IO;
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
using DevExpress.Spreadsheet;
//

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public abstract class MdfReportFormExcelSheetCellCore { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private readonly MdfReportFormExcelSheetCore _Sheet;
        public MdfReportFormExcelSheetCore Sheet {
            get { return _Sheet; }
        }

        private readonly MdfReportFormExcelSheetRow _Row;
        public MdfReportFormExcelSheetRow Row {
            get { return _Row; }
        }

        public Int32 RowIndex {
            get { return Row.Index; }
        }

        private readonly MdfReportFormExcelSheetColumn _Column;
        public MdfReportFormExcelSheetColumn Column {
            get { return _Column; }
        }

        public Int32 ColumnIndex {
            get { return Column.Index; }
        }

        public Style StyleOriginal { get; set; }

        public virtual MdfDataType DataType {
            get { return MdfDataType.DT_UNDEFINED; }
        }

        public virtual Boolean IsScale {
            get { return false; }
        }

        public virtual Decimal Scale {
            get { return Sheet.Form.Scale; }
        }

        public abstract Object Value { get; set; }

        public abstract Object CellData { get; }

        public virtual String Comment {
            get { return String.Empty; }
        }

        private Boolean _IsEditable;
        public virtual Boolean IsEditable { get { return _IsEditable; } }
        protected void IsEditableSet(Boolean value) {
            _IsEditable = value;
        }

        
        public MdfReportFormExcelSheetCellCore(MdfReportFormExcelSheetCore sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row) {
            _Sheet = sheet;
            _Row = row;
            _Column = column;
        }

        public virtual void SetValue(Cell cell) {
            cell.SetValue(Value);
        }

        public virtual void Format(Cell cell, Dictionary<MdfReportFormStyles, Style> styles) {
            if (IsEditable) {
                StyleOriginal = cell.Style;
                cell.Style = styles[MdfReportFormStyles.INPUT_CELL];
            }
        }
    }

    public abstract class MdfReportFormExcelSheetCell<Tr, Tv, Tt, Tc, Tdp> : MdfReportFormExcelSheetCellCore
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> 
        { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        public void OnValueChanged(MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp> table_cell) {
            Sheet.OnCellChanged(ColumnIndex, RowIndex, table_cell?.DataPoint.Value);
        }

        public MdfReportFormExcelSheetCell(MdfReportFormExcelSheetCore sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row): base(sheet, column, row) {
        }
    }

}