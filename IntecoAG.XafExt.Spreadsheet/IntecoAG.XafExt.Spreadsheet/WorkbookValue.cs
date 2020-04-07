using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Spreadsheet;

namespace IntecoAG.XafExt.Spreadsheet {

    public class WorkbookValue: IWorkbookValue {

        public void Load(IWorkbook book) {
            Store.Load(book);
        }

        public void Save(IWorkbookStore store) {
            store.Save(Workbook);
        }

        public IWorkbook Workbook { get; protected set; }
        public IWorkbookStore Store { get; protected set; }

        public WorkbookValue(IWorkbook workbook) {
            Workbook = workbook;
        }

        public WorkbookValue(IWorkbookStore store) {
            Store = store;
        }
    }
}
