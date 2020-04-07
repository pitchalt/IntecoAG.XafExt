using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Spreadsheet;

namespace IntecoAG.XafExt.Spreadsheet {

    public interface IWorkbookValue {

        void Load(IWorkbook book);
        void Save(IWorkbookStore store);

        IWorkbook Workbook{ get; }
        IWorkbookStore Store { get; }

    }
}
