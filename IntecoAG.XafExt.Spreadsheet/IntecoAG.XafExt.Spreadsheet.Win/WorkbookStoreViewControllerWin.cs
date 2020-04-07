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

namespace IntecoAG.XafExt.Spreadsheet.Win {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class WorkbookStoreViewControllerWin : WorkbookStoreViewController {
        public WorkbookStoreViewControllerWin(): base() {
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();

        }

        protected SpreadsheetForm form;
        public override void SpreadsheetFormShow(IWorkbookStore store, IObjectSpace os) {
            form = new SpreadsheetForm();
            form.CellValueChanged += Form_CellValueChanged;
            form.DocumentSave += Form_DocumentSave;
            form.Closed += Form_Closed;
            OnDocumentLoad(form.Document);
            store.CellChanged += Store_CellChanged;
            form.CellDataShow += Form_CellDataShow;
            form.Show();
        }

        private void Form_CellDataShow(object sender, DevExpress.Spreadsheet.Cell e) {
            OnCellDataShow(e);
        }

        private void Store_CellChanged(object sender, CellChangedEventArgs e) {
            form.Document.Worksheets[e.SheetId].Cells[e.RowId, e.ColId].SetValue(e.Value);
        }

        private void Form_CellValueChanged(object sender, DevExpress.XtraSpreadsheet.SpreadsheetCellEventArgs e) {
            OnCellChanged(e.Cell, e.OldValue, e.Value);
        }

        private void Form_Closed(object sender, EventArgs e) {
            OnDocumentClose();
        }

        private void Form_DocumentSave(object sender, EventArgs e) {
            OnDocumentSave(form.Document);
        }


    }
}
