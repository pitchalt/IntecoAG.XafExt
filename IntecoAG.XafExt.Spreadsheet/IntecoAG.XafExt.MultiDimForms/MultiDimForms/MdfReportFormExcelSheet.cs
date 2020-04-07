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
using IntecoAG.XpoExt;
//
namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public abstract class MdfReportFormExcelSheetCore { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        protected readonly List<MdfReportFormExcelSheetColumn> _Columns;
        public IReadOnlyList<MdfReportFormExcelSheetColumn> Columns {
            get { return _Columns; }
        }
        protected readonly List<MdfReportFormExcelSheetRow> _Rows;
        public IReadOnlyList<MdfReportFormExcelSheetRow> Rows {
            get { return _Rows; }
        }

        public MdfReportFormExcelSheetRow RowGet(Int32 index) {
            return index < Rows.Count ? Rows[index] : null;
        }

        public MdfReportFormExcelSheetColumn ColumnGet(Int32 index) {
            return index < Columns.Count ? Columns[index] : null;
        }

        private readonly MdfReportFormExcel _Form;
        public MdfReportFormExcel Form {
            get { return _Form; }
        }

        private readonly Int32 _Index;
        public Int32 Index {
            get { return _Index; }
        }

        public String Code { get; set; }
        public Int32 OffsetCol { get; set; }
        public Int32 OffsetRow { get; set; }

        public abstract IReadOnlyList<MdfReportFormExcelSheetCellCore> Cells { get; }

        protected MdfReportFormExcelSheetCore(MdfReportFormExcel form, Int32 index) {
            _Form = form;
            _Index = index;
            _Columns = new List<MdfReportFormExcelSheetColumn>(128);
            _Rows = new List<MdfReportFormExcelSheetRow>(128);
        }

        public void OnCellChanged(Int32 col, Int32 row, Object value) {
            Form.OnCellChanged(Index, col, row, value);
        }

        public abstract void Render();


        public virtual void FormatHeader(Worksheet sheet, IDictionary<MdfReportFormStyles, Style> def_styles) {

        }

        public virtual void FormatFooter(Worksheet sheet, IDictionary<MdfReportFormStyles, Style> def_styles) {

        }

    }

    public abstract class MdfReportFormExcelSheet<Tr, Tv, Tt, Tc, Tdp, Tax, Tox, Tay, Toy, Tsc> : MdfReportFormExcelSheetCore
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp>
            where Tax : MdfAxis<Tr, Tv, Tt, Tc, Tdp>
            where Tox : MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate
            where Tay : MdfAxis<Tr, Tv, Tt, Tc, Tdp>
            where Toy : MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate
            where Tsc : MdfReportFormExcelSheetCell<Tr, Tv, Tt, Tc, Tdp> {

        public MdfReportTable<Tr, Tv, Tt, Tc, Tdp> Table { get; set; }
        public Tax AxisX { get; set; }
        public Tay AxisY { get; set; }

        public Int32 AxisXIndex { get; set; }
        public Int32 AxisYIndex { get; set; }

        protected readonly List<MdfReportFormExcelSheetCell<Tr,Tv,Tt,Tc,Tdp>> _Cells;

        public override IReadOnlyList<MdfReportFormExcelSheetCellCore> Cells {
            get { return _Cells; }
        }

        protected MdfReportFormExcelSheet(MdfReportFormExcel form, Int32 index) : base(form, index) {
            _Cells = new List<MdfReportFormExcelSheetCell<Tr, Tv, Tt, Tc, Tdp>>(256);
        }

        public override void Render() {
            //Clear();
            Int32 col_index;
            Int32 row_index;
            Int32 loc_col_index;
            Int32 loc_row_index;
            Int32 max_col_index = OffsetCol + AxisY.Levels.Count + AxisX.OrdinateLine.Count;
            for (loc_col_index = 0; loc_col_index <= max_col_index; loc_col_index++) {
                _Columns.Add(new MdfReportFormExcelSheetColumn(this, loc_col_index));
            }
            Int32 max_row_index = OffsetRow + AxisX.Levels.Count + AxisY.OrdinateLine.Count;
            for (loc_row_index = 0; loc_row_index <= max_row_index; loc_row_index++) {
                _Rows.Add(new MdfReportFormExcelSheetRow(this, loc_row_index));
            }
            col_index = OffsetCol;
            row_index = OffsetRow + AxisX.Levels.Count;
            foreach (var level in AxisY.Levels) {
                MdfReportFormExcelSheetColumn column = _Columns[col_index];
                foreach (var ordinate in level.Ordinates) {
                    MdfReportFormExcelSheetRow row = _Rows[row_index + ordinate.LevelIndex];
                    Tsc cell = OrdinateYCellCreate(row, column, (Toy)ordinate);
                    _Cells.Add(cell);
                    row[col_index] = cell;
                    column[row_index + ordinate.LevelIndex - 1] = cell;
                }
                col_index++;
            }
            col_index = OffsetCol + AxisY.Levels.Count;
            row_index = OffsetRow;
            foreach (var level in AxisX.Levels) {
                MdfReportFormExcelSheetRow row = _Rows[row_index];
                foreach (var ordinate in level.Ordinates) {
                    MdfReportFormExcelSheetColumn column = _Columns[col_index + ordinate.LevelIndex];
                    Tsc cell = OrdinateXCellCreate(row, column, (Tox)ordinate);
                    _Cells.Add(cell);
                    row[col_index + ordinate.LevelIndex - 1] = cell;
                    column[row_index] = cell;
                }
                row_index++;
            }
            col_index = OffsetCol + AxisY.Levels.Count;
            row_index = OffsetRow + AxisX.Levels.Count;
            foreach (var table_cell in Table.Cells) {
                //Tox col_ordinate = (Tox)table_cell.Ordinates[AxisXIndex];
                var col_ordinate = table_cell.Ordinates[AxisXIndex];
                loc_col_index = col_ordinate.LevelIndex;
                MdfReportFormExcelSheetColumn column = _Columns[loc_col_index + col_index];
                //
                //Toy row_ordinate = (Toy)table_cell.Ordinates[AxisYIndex];
                var row_ordinate = table_cell.Ordinates[AxisYIndex];
                loc_row_index = row_ordinate.LevelIndex;
                MdfReportFormExcelSheetRow row = _Rows[loc_row_index + row_index];
                //
                Tsc cell = TableCellCreate(row, column, table_cell);
                table_cell.OnCellChanged += cell.OnValueChanged;     
                _Cells.Add(cell);
                row[loc_col_index + col_index] = cell;
                column[loc_row_index + row_index] = cell;
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

        private void Table_cell_OnCellUpdated(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        protected abstract Tsc ReportCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column);
        protected abstract Tsc TableCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, Tc table_cell);
        protected abstract Tsc OrdinateXCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, Tox ordinate);
        protected abstract Tsc OrdinateYCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, Toy ordinate);

    }
}