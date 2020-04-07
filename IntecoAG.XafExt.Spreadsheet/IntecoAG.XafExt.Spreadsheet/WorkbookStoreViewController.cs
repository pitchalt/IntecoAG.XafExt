using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
//
//using IntecoAG.XafExt.Spreadsheet.MultiDimForms;

namespace IntecoAG.XafExt.Spreadsheet {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public abstract partial class WorkbookStoreViewController : IagObjectViewController<IWorkbookStore> {
        public SimpleAction ExcelFormShowAction { get; protected set; }

        protected WorkbookStoreViewController() : base() {

            ExcelFormShowAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(ExcelFormShowAction)}", PredefinedCategory.View) {
                Caption = "Excel",
                //ImageName = "BO_Skull",
                PaintStyle = ActionItemPaintStyle.Image,
                ToolTip = "Show in Excel form",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
                //TargetViewType = ViewType.DetailView,
                //TargetViewNesting = Nesting.Root,
                TargetViewType = ViewType.Any,
                TargetViewNesting = Nesting.Any
            };
            ExcelFormShowAction.Execute += ExcelFormShowAction_Execute;
        }

        private IObjectSpace _DocumentObjectSpace;
        protected IObjectSpace DocumentObjectSpace {
            get { return _DocumentObjectSpace; }
        }
        private IWorkbookStore _WorkbookStore;
        protected IWorkbookStore WorkbookStore {
            get { return _WorkbookStore; }
        }

        private void ExcelFormShowAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            if (this.CurrentObject == null)
                return;
            //            ObjectSpace.CommitChanges();
            _DocumentObjectSpace = ObjectSpace.CreateNestedObjectSpace();
            _WorkbookStore = DocumentObjectSpace.GetObject(this.CurrentObject);
            SpreadsheetFormShow(WorkbookStore, DocumentObjectSpace);
        }

        public void SpreadsheetFormShow2(IWorkbookStore store, IObjectSpace os) {
            _DocumentObjectSpace = os;
            _WorkbookStore = store;
            SpreadsheetFormShow(store, os);
        }

        public abstract void SpreadsheetFormShow(IWorkbookStore store, IObjectSpace os);

        protected virtual void OnDocumentLoad(IWorkbook book) {
            WorkbookStore.Load(book);
        }

        protected virtual void OnDocumentSave(IWorkbook book) {
            WorkbookStore.Save(book);
            DocumentObjectSpace.CommitChanges();
        }

        protected virtual void OnDocumentClose() {
            _WorkbookStore = null;
            _DocumentObjectSpace = null;
        }

        protected virtual void OnCellChanged(Cell cell, CellValue old_value, CellValue new_value) {
            WorkbookStore.OnBookCellChanged(new CellChangedEventArgs(cell.Worksheet.Index, cell.ColumnIndex, cell.RowIndex, cell.Value));
        }

        protected virtual void OnCellDataShow(Cell cell) {
            Object data = WorkbookStore.CellDataGet(cell.Worksheet.Index, cell.ColumnIndex, cell.RowIndex);
            if (data != null) {
                //                IObjectSpace os = Application.CreateObjectSpace(data.GetType());
                IObjectSpace os = DocumentObjectSpace.CreateNestedObjectSpace();
                if (os != null) {
                    DetailView dv = Application.CreateDetailView(os, os.GetObject(data));
                    Application.ShowViewStrategy.ShowViewInPopupWindow(dv);
                }
            }
        }
    }
}