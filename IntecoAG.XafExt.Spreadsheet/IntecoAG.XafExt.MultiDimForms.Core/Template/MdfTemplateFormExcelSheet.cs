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
    [Persistent("FmMdfCoreTemplateFormExcelSheet")]
    public class MdfTemplateFormExcelSheet : MdfCoreElement { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [Persistent(nameof(Index))]
        private Int32 _Index;
        [PersistentAlias(nameof(_Index))]
        public Int32 Index {
            get { return _Index; }
        }

        public void IndexSet(Int32 value) {
            SetPropertyValue(ref _Index, value);
        }

        [Persistent(nameof(RowCount))]
        private Int32 _RowCount;
        [PersistentAlias(nameof(_RowCount))]
        public Int32 RowCount {
            get { return _RowCount; }
        }

        public void RowCountSet(Int32 value) {
            if (value < Rows.Count) {
                value = Rows.Count;

            }
            SetPropertyValue(ref _RowCount, value);
        }

        [Persistent(nameof(ColumnCount))]
        private Int32 _ColumnCount;
        [PersistentAlias(nameof(_ColumnCount))]
        public Int32 ColumnCount {
            get { return _ColumnCount; }
        }

        public void ColumnCountSet(Int32 value) {
            if (value < Columns.Count) {
                value = Columns.Count;
            }
            SetPropertyValue(ref _ColumnCount, value);

        }

        private Int32 _SortIndex;
        public Int32 SortIndex {
            get { return _SortIndex; }
            set { SetPropertyValue(ref _SortIndex, value); }
        }

        private MdfTemplateFormExcel _TemplateFormExcel;
        [Association("FmMdfTemplateFormExcel-FmMdfTemplateFormExcelSheet")]
        public MdfTemplateFormExcel TemplateFormExcel {
            get { return _TemplateFormExcel; }
            set { SetPropertyValue(ref _TemplateFormExcel, value); }
        }

        private MdfTemplateTable _TemplateTable;
        public MdfTemplateTable TemplateTable {
            get { return _TemplateTable; }
            set { SetPropertyValue(ref _TemplateTable, value); }
        }

        private Int32 _OffsetCol;
        public Int32 OffsetCol {
            get { return _OffsetCol; }
            set { SetPropertyValue(ref _OffsetCol, value); }
        }
        private Int32 _OffsetRow;
        public Int32 OffsetRow {
            get { return _OffsetRow; }
            set { SetPropertyValue(ref _OffsetRow, value); }
        }

        private MdfCoreTableAxis _AxisX;
        [DataSourceProperty(nameof(TemplateTable) + "." + nameof(MdfTemplateTable.Table) + "." + nameof(MdfCoreTable.TableAxiss))]
        public MdfCoreTableAxis AxisX {
            get { return _AxisX; }
            set { SetPropertyValue(ref _AxisX, value); }
        }

        private MdfCoreTableAxis _AxisY;
        [DataSourceProperty(nameof(TemplateTable) + "." + nameof(MdfTemplateTable.Table) + "." + nameof(MdfCoreTable.TableAxiss))]
        public MdfCoreTableAxis AxisY {
            get { return _AxisY; }
            set { SetPropertyValue(ref _AxisY, value); }
        }

        [Association("FmMdfCoreTemplateFormExcelSheet-FmMdfCoreTemplateFormExcelSheetRow")]
        [Browsable(false)]
        [Aggregated]
        public XPCollection<MdfTemplateFormExcelSheetRow> PersistentRows {
            get { return GetCollection<MdfTemplateFormExcelSheetRow>(); }
        }

        private MdfTemplateFormExcelSheetRow[] _Rows;
        public IReadOnlyList<MdfTemplateFormExcelSheetRow> Rows {
            get {
                if (_Rows == null || RowCount != _Rows.Length) {
                    //_Rows = new List<MdfTemplateFormExcelSheetRow>(RowCount);
                    _Rows = new MdfTemplateFormExcelSheetRow[RowCount];
                    foreach (var row in PersistentRows) {
                        _Rows[row.Index] = row;
                    }
                }
                for (int i = 0; i < RowCount; i++) {
                    //if (i == _Rows.Length)
                    //    _Rows.Add(null);
                    if (_Rows[i] == null)
                        _Rows[i] = new MdfTemplateFormExcelSheetRow(Session) { Sheet = this, Index = i };
                }
                return _Rows;
            }
        }

        public MdfTemplateFormExcelSheetRow RowGet(Int32 index) {
            return index < Rows.Count ? Rows[index] : null;
        }

        [Association("FmMdfCoreTemplateFormExcelSheet-FmMdfCoreTemplateFormExcelSheetColumn")]
        [Browsable(false)]
        [Aggregated]
        public XPCollection<MdfTemplateFormExcelSheetColumn> PersistentColumns {
            get { return GetCollection<MdfTemplateFormExcelSheetColumn>(); }
        }

        private MdfTemplateFormExcelSheetColumn[] _Columns;
        public IReadOnlyList<MdfTemplateFormExcelSheetColumn> Columns {
            get {
                if (_Columns == null || ColumnCount != _Columns.Length) {
                    //_Columns = new List<MdfTemplateFormExcelSheetColumn>(ColumnCount);
                    _Columns = new MdfTemplateFormExcelSheetColumn[ColumnCount];
                    foreach (var column in PersistentColumns) {
                        _Columns[column.Index] = column;
                    }
                }
                for (int i = 0; i < ColumnCount; i++) {
                    //if (i == _Columns.Count)
                    //    _Columns.Add(null);
                    if (_Columns[i] == null)
                        _Columns[i] = new MdfTemplateFormExcelSheetColumn(Session) { Sheet = this, Index = i };
                }
                return _Columns;
            }
        }

        [Association("FmMdfCoreTemplateFormExcelSheet-FmMdfCoreTemplateFormExcelSheetCell")]
        [Aggregated]
        public XPCollection<MdfTemplateFormExcelSheetCell> Cells {
            get { return GetCollection<MdfTemplateFormExcelSheetCell>(); }
        }

        //List<MdfTemplateFormExcelSheetRow> _Rows;
        //public IReadOnlyList<MdfTemplateFormExcelSheetRow> Rows {
        //    get { return _Rows; }
        //}

        //List<MdfTemplateFormExcelSheetColumn> _Columns;
        //public IReadOnlyList<MdfTemplateFormExcelSheetColumn> Columns {
        //    get { return _Columns; }
        //}

        //List<MdfTemplateFormExcelSheetCell> _Cells;
        //public IReadOnlyList<MdfTemplateFormExcelSheetCell> Cells {
        //    get { return _Cells; }
        //}

        public MdfTemplateFormExcelSheet(Session session): base(session) {
        }

        //[Action(Caption ="Render")]
        //public void RenderAction() {
        //    Render();
        //}
        public void Render() {
            //Clear();
            Dictionary<MdfTemplateFormExcelSheetCell, Boolean> ClearList = new Dictionary<MdfTemplateFormExcelSheetCell, Boolean>();
            foreach (var cell in Cells) {
                ClearList[cell] = true;
            }
            Int32 col_index;
            Int32 row_index;
            Int32 loc_col_index;
            Int32 loc_row_index;
            Int32 max_col_index = OffsetCol + AxisY.Axis.Levels.Count + AxisX.Axis.OrdinateLine.Count;
            ColumnCountSet(max_col_index);
            //for (loc_col_index = 0; loc_col_index < max_col_index; loc_col_index++) {
            //    Columns.Add(new MdfTemplateFormExcelSheetColumn(Session) { Sheet = this, Index = loc_col_index });
            //}
            Int32 max_row_index = OffsetRow + AxisX.Axis.Levels.Count + AxisY.Axis.OrdinateLine.Count;
            RowCountSet(max_row_index);
            //for (loc_row_index = 0; loc_row_index < max_row_index; loc_row_index++) {
            //    Rows.Add(new MdfTemplateFormExcelSheetRow(Session) { Sheet = this, Index = loc_row_index });
            //}
            col_index = OffsetCol;
            row_index = OffsetRow + AxisX.Axis.Levels.Count;
            foreach (var level in AxisY.Axis.Levels) {
//                MdfTemplateFormExcelSheetColumn column = Columns[col_index];
                foreach (var ordinate in level.Ordinates) {
                    var cell = Columns[col_index][row_index + ordinate.LevelIndex];
                    cell.AxisOrdinate = ordinate;
                    cell.TableCell = null;
                    ClearList[cell] = false;
                }
                col_index++;
            }
            col_index = OffsetCol + AxisY.Axis.Levels.Count;
            row_index = OffsetRow;
            foreach (var level in AxisX.Axis.Levels) {
//                MdfTemplateFormExcelSheetRow row = Rows[row_index];
                foreach (var ordinate in level.Ordinates) {
                    var cell = Rows[row_index][col_index + ordinate.LevelIndex];
                    cell.AxisOrdinate = ordinate;
                    cell.TableCell = null;
                    ClearList[cell] = false;
                }
                row_index++;
            }
            col_index = OffsetCol + AxisY.Axis.Levels.Count;
            row_index = OffsetRow + AxisX.Axis.Levels.Count;
            foreach (var table_cell in TemplateTable.Table.Cells) {
                MdfCoreAxisOrdinate col_ordinate = table_cell.AxisOrdinates[AxisX.AxisIndex];
                loc_col_index = col_ordinate.LevelIndex;
                MdfTemplateFormExcelSheetColumn column = Columns[loc_col_index + col_index];
                //
                MdfCoreAxisOrdinate row_ordinate = table_cell.AxisOrdinates[AxisY.AxisIndex];
                loc_row_index = row_ordinate.LevelIndex;
                MdfTemplateFormExcelSheetRow row = Rows[loc_row_index + row_index];
                //
                var cell = row[loc_col_index + col_index];
                cell.TableCell = table_cell;
                cell.AxisOrdinate = null;
                var calc = cell.TableCell.Calcs.FirstOrDefault(x => x.CalcVariant == TemplateFormExcel.CalcVariant);
                if (calc != null) {
                    cell.Formula = calc.Formula;
                }
                ClearList[cell] = false;
                //
                //MdfTemplateFormExcelSheetCell cell = new MdfTemplateFormExcelSheetCellTableCell(row, column, table_cell);
                //Cells.Add(cell);
                //row[loc_col_index + col_index] = cell;
                //column[loc_row_index + row_index] = cell;
            }

            foreach (var clear_pair in ClearList) {
                if (clear_pair.Value) {
                    clear_pair.Key.TableCell = null;
                    clear_pair.Key.AxisOrdinate = null;
                }
            }

        }

        //public void Clear() {
        //    foreach (var cell in _Cells) {
        //        cell.Clear();
        //    }
        //    _Cells.Clear();
        //    foreach (var col in Columns) {
        //        col.Clear();
        //    }
        //    _Columns.Clear();
        //    foreach (var row in Rows) {
        //        row.Clear();
        //    }
        //    _Rows.Clear();
        //}
        //public override void AfterConstruction() {
        //    base.AfterConstruction();
        //    // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        //}

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                case nameof(SortIndex):
                    TemplateFormExcel?.SheetsReindex();
                    break;
                case nameof(TemplateFormExcel):
                    Int32 sort_index = 0;
                    if (TemplateFormExcel != null) {
                        foreach (var sheet in TemplateFormExcel.TemplateFormExcelSheets) {
                            if (sort_index < sheet.Index) {
                                sort_index = sheet.Index;
                            }
                        }
                    }
                    sort_index += 10;
                    SortIndex = sort_index;
                    TemplateFormExcel?.SheetsReindex();
                    break;
            }
        }

        protected override void OnLoaded() {
            base.OnLoaded();
//            Render();
        }
        public override String ToString() {
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