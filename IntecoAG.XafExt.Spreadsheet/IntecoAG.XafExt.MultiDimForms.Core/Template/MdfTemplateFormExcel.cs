using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using Newtonsoft.Json;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using IntecoAG.XpoExt;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core.Formulas;
using DevExpress.Spreadsheet.Formulas;
using Newtonsoft.Json.Converters;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [MapInheritance(MapInheritanceType.OwnTable)]
    [FileAttachment("File")]
    [Persistent("FmMdfCoreTemplateFormExcel")]
    public class MdfTemplateFormExcel : MdfTemplateForm, IWorkbookStore { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).


        private FileData _File;
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public FileData File {
            get { return _File; }
            set { SetPropertyValue(ref _File, value); }
        }

        //IWorkbookValue _WorkbookValue;

        public event EventHandler<CellChangedEventArgs> CellChanged;
        public event EventHandler<CellChangedEventArgs> BookCellChanged;

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

        [Association("FmMdfTemplateFormExcel-FmMdfTemplateFormExcelSheet")]
        [Aggregated]
        public XPCollection<MdfTemplateFormExcelSheet> TemplateFormExcelSheets {
            get { return GetCollection<MdfTemplateFormExcelSheet>(); }
        }

        private class CollectionAsReadOnlyList : IReadOnlyList<MdfTemplateFormExcelSheet> {

            private XPCollection<MdfTemplateFormExcelSheet> _Col;

            public IEnumerator<MdfTemplateFormExcelSheet> GetEnumerator() {
                return _Col.OrderBy(x => x.Index).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            public Int32 Count {
                get { return _Col.Count; }
            }

            public MdfTemplateFormExcelSheet this[Int32 index] {
                get { return _Col.FirstOrDefault(x => x.Index == index); }
            }

            public CollectionAsReadOnlyList(XPCollection<MdfTemplateFormExcelSheet> col) {
                _Col = col;
            }
        }

        [Browsable(false)]
        public IReadOnlyList<MdfTemplateFormExcelSheet> Sheets {
            get { return new CollectionAsReadOnlyList(TemplateFormExcelSheets); }
        }

        public MdfTemplateFormExcelSheet SheetGet(Int32 index) {
            return index < Sheets.Count ? Sheets[index] : null;
        }

        public void SheetsReindex() {
            Int32 index = 0;
            foreach (var sheet in TemplateFormExcelSheets.OrderBy(x => x.SortIndex)) {
                sheet.IndexSet(index++);
            }
        }

        public MdfTemplateFormExcel(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override XPCollection<T> CreateCollection<T>(XPMemberInfo property) {
            if (property.Name == nameof(TemplateFormExcelSheets)) {
                var col = new XPCollection<T>(Session, this, property);
                var sorting = new SortingCollection();
                sorting.Add(new SortProperty(nameof(MdfTemplateFormExcelSheet.Index), SortingDirection.Ascending));
                col.Sorting = sorting;
                return col;
            }
            return base.CreateCollection<T>(property);
        }

        public override string ToString() {
            return base.ToString();
        }

        public virtual void ImportPrepare(IWorkbook book) {
        }

        public virtual void Import(IWorkbook book) {
        }

        public virtual void Export(IWorkbook book) {
        }

        public void Load(IWorkbook book) {
            Stream stream = new MemoryStream();
            IFileData source = File;
            if (source != null) {
                source.SaveToStream(stream);
                stream.Position = 0;
                book.LoadDocument(stream, DocumentFormat.Xlsx);
            }
            for (Int32 sheet_index = book.Worksheets.Count; sheet_index < Sheets.Count; sheet_index++) {
                book.Worksheets.Add();
            }
            foreach (var sheet in Sheets) {
                var book_sheet = book.Worksheets[sheet.Index];
                book_sheet.Name = String.IsNullOrWhiteSpace(sheet.NameShort) ? sheet.Code : sheet.NameShort;
                foreach (var row in sheet.Rows) {
                    book_sheet.Rows[row.Index].Height = row.Height == 0.0 ? book_sheet.Rows[row.Index].Height : row.Height;
                }
                foreach (var column in sheet.Columns) {
                    book_sheet.Columns[column.Index].Width = column.Width == 0.0 ? book_sheet.Columns[column.Index].Width : column.Width;
                }
                foreach (var cell in sheet.Cells) {
                    if (cell.AxisOrdinate != null)
                        book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].SetValue(cell.AxisOrdinate.Code);
                    else {
                        //if (!String.IsNullOrWhiteSpace(cell.Formula)) {
                        //    book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].Formula = cell.Formula;
                        //}
                        //else {
                        //    book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].SetValue(cell.Value);
                        //}
                        book_sheet.Cells[cell.RowIndex, cell.ColumnIndex].SetValue(cell.Value);
                    }
                }
            }
        }

        //public class ParsedExpressionConverter : CustomCreationConverter<ParsedExpression> {

        //    private readonly IWorkbook Workbook;

        //    public override ParsedExpression Create(Type objectType) {
        //        return new ParsedExpression(Workbook);
        //    }

        //    public ParsedExpressionConverter(IWorkbook book) {
        //        Workbook = book;
        //    }
        //}

        public void Save(IWorkbook book) {
            Stream stream = new MemoryStream();
            IFileData target = File;
            if (target == null) {
                target = File = new FileData(Session);
            }
            book.SaveDocument(stream, DocumentFormat.Xlsx);
            stream.Position = 0;
            target.LoadFromStream(".xlsx", stream);
            //var json_ser = new JsonSerializer();
            foreach (var sheet in Sheets) {
                var book_sheet = book.Worksheets[sheet.Index];
                sheet.NameShort = book_sheet.Name;
                foreach (var row in sheet.Rows) {
                    row.Height = book_sheet.Rows[row.Index].Height;
                }
                foreach (var column in sheet.Columns) {
                    column.Width = book_sheet.Columns[column.Index].Width;
                }
                foreach (var cell in sheet.Cells) {
                    var sheet_cell = book_sheet.Cells[cell.RowIndex, cell.ColumnIndex];
                    //if (!String.IsNullOrWhiteSpace(sheet_cell.Formula)) {
                    //    cell.Formula = sheet_cell.Formula;
                    //    if (cell.TableCell != null) {
                    //        if (cell.TableCell.Calc == null) {
                    //            cell.TableCell.Calc = new MdfCoreDataPointCalc(this.Session);
                    //        }
                    //        cell.TableCell.Calc.Table = cell.TableCell.Table;
                    //        cell.TableCell.DataPoint.Calc = cell.TableCell.Calc;
                    //        cell.TableCell.Calc.Formula = sheet_cell.Formula;
                    //        //
                    //        MdfCoreFormulasFromExcelConverter visitor = new MdfCoreFormulasFromExcelConverter(cell, cell.TableCell.Calc);
                    //        sheet_cell.ParsedExpression.Expression.Visit(visitor);
                    //        visitor.Clean();
                    //        //
                    //    }
                    //    else {
                    //    }
                    //    continue;
                    //}
                    //else {
                    //    if (cell.TableCell != null) {
                    //        cell.TableCell.Calc = null;
                    //    }
                    //}
                    if (sheet_cell.Value.IsBoolean) {
                        cell.Value = sheet_cell.Value.BooleanValue;
                    }
                    else if (sheet_cell.Value.IsNumeric) {
                        cell.Value = (Decimal)sheet_cell.Value.NumericValue;
                    }
                    else if (sheet_cell.Value.IsText) {
                        cell.Value = sheet_cell.Value.TextValue;
                    }
                    else if (sheet_cell.Value.IsDateTime) {
                        cell.Value = sheet_cell.Value.DateTimeValue;
                    }
                    else if (sheet_cell.Value.IsEmpty || sheet_cell.Value.IsError) {
                        cell.Value = null;
                    }
                }
            }
        }

        public static String ParsedExpression2Formula(ParsedExpression exp) {
            return exp.ToString();
        }

        public override void Render() {
            foreach (var sheet in Sheets) {
                sheet.Render();
            }
        }

        public void OnBookCellChanged(CellChangedEventArgs args) {
//            throw new NotImplementedException();
        }

        public object CellDataGet(int sheet_id, int col_id, int row_id) {
            return SheetGet(sheet_id)?.RowGet(row_id)?.CellGet(col_id);
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