using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraSpreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.ExpressApp;

namespace IntecoAG.XafExt.Spreadsheet {

    public abstract class WorkbookPropertyEditor : PropertyEditor, IComplexViewItem {
        
        //protected override void ReadValueCore() {
            //if (control != null) {
            //    if (CurrentObject != null) {
            //        control.ReadOnly = false;
            //        IWorkbookStore work_book = (IWorkbookStore)PropertyValue;
            //        IWorkbook workbook = control.Document;
            //        workbook.LoadDocument(work_book.GetDocument(), DocumentFormat.Xlsx);
            //        //workbook.LoadDocument(work_book.FileName, DocumentFormat.Xlsx);
            //        //control.Value = (int)PropertyValue;
            //    }
            //}
        //}
        //private void control_ValueChanged(object sender, EventArgs e) {
        //    if (!IsValueReading) {
        //        OnControlValueChanged();
        //        WriteValueCore();
        //    }
        //}
//        protected override object CreateControlCore() {
//            control = new SpreadsheetControl();
//            control.ModifiedChanged += Control_ModifiedChanged;
////            control.ValueChanged += control_ValueChanged;
//            return control;
//        }

        //private void Control_ModifiedChanged(object sender, EventArgs e) {
        //    if (!IsValueReading) {
        //        OnControlValueChanged();
        //    }
        //}

        protected override void OnControlCreated() {
            base.OnControlCreated();
            ReadValue();
        }
        public WorkbookPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info) {
        }
        //protected override void Dispose(bool disposing) {
        //    if (control != null) {
        //        //control.ValueChanged -= control_ValueChanged;
        //        control = null;
        //    }
        //    base.Dispose(disposing);
        //}
//        protected override object GetControlValueCore() {
//            //if (control != null) {
//            //    return (int)control.Value;
//            //}
//            //if (control != null) {
//            //    if (CurrentObject != null) {
//            //        control.ReadOnly = false;
//            //        IWorkbookStore work_book = (IWorkbookStore)PropertyValue;
//            //        IWorkbook workbook = control.Document;
//            //        workbook.LoadDocument(work_book.GetDocument(), DocumentFormat.Xlsx);
//            //        //workbook.LoadDocument(work_book.FileName, DocumentFormat.Xlsx);
//            //        //control.Value = (int)PropertyValue;
//            //    }
//            //}
////            return null;
//        }

        public void Setup(IObjectSpace objectSpace, XafApplication application) {
            //throw new NotImplementedException();
        }

    }
}
