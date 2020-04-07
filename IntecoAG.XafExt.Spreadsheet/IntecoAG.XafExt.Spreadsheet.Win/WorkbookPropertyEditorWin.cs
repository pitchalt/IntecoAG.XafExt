using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraSpreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.ExpressApp;

namespace IntecoAG.XafExt.Spreadsheet.Win {

    [PropertyEditor(typeof(IWorkbookValue), true)]
    public class WorkbookWinPropertyEditor : WinPropertyEditor {

        private SpreadsheetControl spreadsheet = null;

        IWorkbookValue value;

        protected override void ReadValueCore() {
            if (spreadsheet != null) {
                if (CurrentObject != null) {
                    spreadsheet.ReadOnly = false;
                    value = (IWorkbookValue)PropertyValue;
                    value.Load(spreadsheet.Document);
                }
            }
        }

        protected override object CreateControlCore() {
            spreadsheet = new SpreadsheetControl();
//           spreadsheet.ModifiedChanged += spreadsheet_ModifiedChanged;
            spreadsheet.Leave += Spreadsheet_Leave;
//            control.ValueChanged += control_ValueChanged;
            return spreadsheet;
        }

        private void Spreadsheet_Leave(object sender, EventArgs e) {
            WriteValue();
        }

        private void spreadsheet_ModifiedChanged(object sender, EventArgs e) {
            if (!IsValueReading) {
                OnControlValueChanged();
            }
        }

        protected override void OnControlCreated() {
            base.OnControlCreated();
            ReadValue();
        }

        public WorkbookWinPropertyEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info) {
        }

        protected override void Dispose(bool disposing) {
            if (spreadsheet != null) {
                spreadsheet.ModifiedChanged -= spreadsheet_ModifiedChanged;
                spreadsheet = null;
                value = null;
            }
            base.Dispose(disposing);
        }
        protected override object GetControlValueCore() {
            if (spreadsheet != null) {
                if (spreadsheet.Modified) {
                    return new WorkbookValue(spreadsheet.Document);
                }
                else {
                    return value;
                }
            }
            return null;
        }

        protected override void WriteValueCore() {
            //if (!AllowEdit) {
            //    throw new InvalidOperationException(String.Format(SystemExceptionLocalizer.GetExceptionMessage(ExceptionId.UnableToSetReadOnlyProperty), PropertyName));
            //}
            //if (CurrentObject == null) {
            //    throw new InvalidOperationException(String.Format(SystemExceptionLocalizer.GetExceptionMessage(ExceptionId.CurrentObjectIsNotSet), PropertyName));
            //}
            var value = GetControlValueCore();
            MemberInfo.SetValue(CurrentObject, value);
        }
    }
}
