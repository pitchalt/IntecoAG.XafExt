using System;
using System.Linq;
using System.IO;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using System.Drawing;

using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
//
using IntecoAG.XpoExt;
//

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    [Flags]
    public enum MdfReportFormMode {

        IMPORT  = 1,
        EXPORT  = 2,
        WORK    = 3        
    }

    public enum MdfReportFormStyles {

        INPUT_CELL = 1,
        DATA_CELL_LEFT = 2,
        DATA_CELL_CENTER = 3,
        DATA_CELL_RIGHT = 4,
    }

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("MdfReportForm")]
    public abstract class MdfReportFormExcel : IagBaseObject, IWorkbookStore { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [Persistent(nameof(DateCreated))]
        private DateTime _DateCreated;
        [PersistentAlias(nameof(_DateCreated))]
        public DateTime DateCreated {
            get { return _DateCreated; }
        }

        private MdfReportFormMode _Mode;
        public MdfReportFormMode Mode {
            get { return _Mode; }
            set { SetPropertyValue<MdfReportFormMode>(ref _Mode, value); }
        }

        public virtual Decimal Scale {
            get { return 1; }
        }

        private String _Code;
        public String Code {
            get { return _Code; }
            set { SetPropertyValue<String>(ref _Code, value); }
        }

        private FileData _File;
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public FileData File {
            get { return _File; }
            set { SetPropertyValue(ref _File, value); }
        }

        [NonPersistent]
        [Browsable(false)]
        public abstract Boolean IsCalcDisabled { get; set; }
        [NonPersistent]
        [Browsable(false)]
        public abstract Boolean IsRefreshDisabled { get; set; }

        //IWorkbookValue _WorkbookValue;
        //[VisibleInListView(false)]
        //public IWorkbookValue WorkbookValue {
        //    get {
        //        if (_WorkbookValue == null)
        //            _WorkbookValue = new WorkbookValue(this);
        //        return _WorkbookValue;
        //    }
        //    set {
        //        if (!Object.ReferenceEquals(value, _WorkbookValue))
        //            Save(value.Workbook);
        //    }
        //}

        protected readonly List<MdfReportFormExcelSheetCore> _Sheets;
        [Browsable(false)]
        public IReadOnlyList<MdfReportFormExcelSheetCore> Sheets {
            get { return _Sheets; }
        }

        public MdfReportFormExcelSheetCore SheetGet(Int32 index) {
            return index < Sheets.Count ? Sheets[index] : null;
        }

        public event EventHandler<CellChangedEventArgs> CellChanged;

        protected MdfReportFormExcel(Session session)
            : base(session) {
            _Sheets = new List<MdfReportFormExcelSheetCore>(8);
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            _DateCreated = DateTime.Now;
        }

        private Boolean _IsRendered;
        public void Render() {
            if (!_IsRendered) {
                RenderCore();
                _IsRendered = true;
            }
        }

        protected abstract void RenderCore();

        protected virtual void OnLoading(IWorkbook book) {
        }

        public void OnCellChanged(Int32 sheet, Int32 col, Int32 row, Object value) {
            CellChanged?.Invoke(this, new CellChangedEventArgs(sheet, col, row, value));
        }

        public void OnBookCellChanged(CellChangedEventArgs args) {
            var cell = SheetGet(args.SheetId)?.RowGet(args.RowId)?.CellGet(args.ColId);
            if (cell == null) {
                return;
            }
            if (args.Value is DevExpress.Spreadsheet.CellValue) {
                var book_cell_value = (DevExpress.Spreadsheet.CellValue)args.Value; 
                if (book_cell_value.IsText) {
                    cell.Value = book_cell_value.TextValue;
                }
                else if (book_cell_value.IsNumeric) {
                    cell.Value = book_cell_value.NumericValue;
                }
                else if (book_cell_value.IsDateTime) {
                    cell.Value = book_cell_value.DateTimeValue;
                }
                else if (book_cell_value.IsBoolean) {
                    cell.Value = book_cell_value.BooleanValue;
                }
            }
            else
                cell.Value = args.Value;
        }

        public void FileDataLoad(IWorkbook book) {
            // IWorkbook book = new Workbook();
            if (File == null)
                return;
            using (Stream stream = new MemoryStream()) {
                IFileData source = File;
                source.SaveToStream(stream);
                stream.Position = 0;
                book.LoadDocument(stream, DocumentFormat.Xlsx);
            }
        }

        public void FileDataSave(IWorkbook book) {
            using (Stream stream = new MemoryStream()) {
                if (File == null)
                    File = new FileData(Session);
                IFileData target = File;
                book.SaveDocument(stream, DocumentFormat.Xlsx);
                stream.Position = 0;
                target.LoadFromStream(Path.GetFileNameWithoutExtension(book.Path) + ".xlsx", stream);
            }

        }

        public virtual void ImportPrepare(IWorkbook book) {

        }

        public void Import() {
            IWorkbook book = new Workbook();
            FileDataLoad(book);
            Import(book);
        }

        public virtual void Import(IWorkbook book) {
            ReportDataSave(book);
            FileDataSave(book);
        }

        public void Export() {
            IWorkbook book = new Workbook();
            Export(book);
            FileDataSave(book);
        }

        public virtual void Export(IWorkbook book) {
            //            Render();
            //            Load(book);
            FileDataLoad(book);
            ReportDataLoad(book);
        }

        public virtual void ReportDataLoad(IWorkbook book) {
            IsCalcDisabled = true;
            IsRefreshDisabled = true;
            try {
                Render();
                CalculateAll();
                var styles = new Dictionary<MdfReportFormStyles, Style>();
                book.BeginUpdate();
                String input_cell_style_name = "Input Cell";
                Style input_cell_style;
                if (!book.Styles.Contains(input_cell_style_name)) {
                    input_cell_style = book.Styles.Add(input_cell_style_name);
                }
                else {
                    input_cell_style = book.Styles[input_cell_style_name];
                }
                input_cell_style.BeginUpdate();
                input_cell_style.Borders.SetAllBorders(System.Drawing.Color.Gold, BorderLineStyle.Medium);
                input_cell_style.Fill.BackgroundColor = System.Drawing.Color.SkyBlue;
                input_cell_style.EndUpdate();
                styles[MdfReportFormStyles.INPUT_CELL] = input_cell_style;
                String data_cell_style_name = "Data Cell Left";
                Style data_cell_style;
                if (!book.Styles.Contains(data_cell_style_name)) {
                    data_cell_style = book.Styles.Add(data_cell_style_name);
                }
                else {
                    data_cell_style = book.Styles[data_cell_style_name];
                }
                data_cell_style.BeginUpdate();
                data_cell_style.Borders.SetAllBorders(Color.Black,  BorderLineStyle.Thin);
                data_cell_style.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
//                data_cell_style.Fill.BackgroundColor = System.Drawing.Color.SkyBlue;
                data_cell_style.EndUpdate();
                styles[MdfReportFormStyles.DATA_CELL_LEFT] = data_cell_style;
                data_cell_style_name = "Data Cell Center";
                if (!book.Styles.Contains(data_cell_style_name)) {
                    data_cell_style = book.Styles.Add(data_cell_style_name);
                }
                else {
                    data_cell_style = book.Styles[data_cell_style_name];
                }
                data_cell_style.BeginUpdate();
                data_cell_style.Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                data_cell_style.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //                data_cell_style.Fill.BackgroundColor = System.Drawing.Color.SkyBlue;
                data_cell_style.EndUpdate();
                styles[MdfReportFormStyles.DATA_CELL_CENTER] = data_cell_style;
                data_cell_style_name = "Data Cell Right";
                if (!book.Styles.Contains(data_cell_style_name)) {
                    data_cell_style = book.Styles.Add(data_cell_style_name);
                }
                else {
                    data_cell_style = book.Styles[data_cell_style_name];
                }
                data_cell_style.BeginUpdate();
                data_cell_style.Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                data_cell_style.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                //                data_cell_style.Fill.BackgroundColor = System.Drawing.Color.SkyBlue;
                data_cell_style.EndUpdate();
                styles[MdfReportFormStyles.DATA_CELL_RIGHT] = data_cell_style;
                //
                if ((Mode & MdfReportFormMode.EXPORT) == MdfReportFormMode.EXPORT) {
                    for (Int32 sheet_index = book.Worksheets.Count; sheet_index < Sheets.Count; sheet_index++) {
                        book.Worksheets.Add();
                    }
                    string author = book.CurrentAuthor;
                    foreach (var sheet in Sheets) {
                        if (sheet == null)
                            continue;
                        var book_sheet = book.Worksheets[sheet.Index];
                        book_sheet.Name = sheet.Code;
                        sheet.FormatHeader(book_sheet, styles);
                        foreach (var cell in sheet.Cells) {
                            //                    if (cell.Comment != String.Empty) {
                            //                        book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].ClearComments()
                            //                        book_sheet.Comments.Add(book_sheet.Cells[cell.RowIndex, cell.ColumnIndex], author, cell.Comment);
                            //                    }
                            //book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].SetValue(cell.Value);
                            cell.SetValue(book_sheet.Cells[cell.RowIndex, cell.ColumnIndex]);
                            cell.Format(book_sheet.Cells[cell.RowIndex, cell.ColumnIndex], styles);
                        }
                        sheet.FormatFooter(book_sheet, styles);
                    }
                    book.Worksheets.ActiveWorksheet = book.Worksheets[0];
                }
            }
            finally {
                book.EndUpdate();
                IsRefreshDisabled = false;
                IsCalcDisabled = false;
            }
        }

        public virtual void ReportDataSave(IWorkbook book) {
            IsCalcDisabled = true;
            IsRefreshDisabled = true;
            try {
                Render();
                for (int i = 0; i < book.Worksheets.Count; i++) {
                    var book_sheet = book.Worksheets[i];
                    var sheet = Sheets.FirstOrDefault(x => x?.Index == i);
                    if (sheet == null)
                        continue;
                    //                    var book_sheet = book.Worksheets[sheet.Index];
                    foreach (var cell in sheet.Cells) {
                        if (cell.IsEditable) {
                            var book_cell_value = book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].Value;
                            if (book_cell_value.IsText) {
                                cell.Value = book_cell_value.TextValue;
                            }
                            else if (book_cell_value.IsNumeric) {
                                cell.Value = book_cell_value.NumericValue;
                            }
                            else if (book_cell_value.IsDateTime) {
                                cell.Value = book_cell_value.DateTimeValue;
                            }
                            else if (book_cell_value.IsBoolean) {
                                cell.Value = book_cell_value.BooleanValue;
                            }
                        }
                    }
                }
                CalculateAll();
            }
            finally {
                IsRefreshDisabled = false;
                IsCalcDisabled = false;
            }
        }

        public virtual void Load(IWorkbook book) {
            FileDataLoad(book);
            ReportDataLoad(book);
        }

        public virtual void Save(IWorkbook book) {
            ReportDataSave(book);
            FileDataSave(book);
        }

        public virtual void CalculateAll() {
            // Report.ReportCore.CalculateAll();
        }

        public object CellDataGet(int sheet_id, int col_id, int row_id) {
            return SheetGet(sheet_id)?.RowGet(row_id)?.CellGet(col_id)?.CellData;
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