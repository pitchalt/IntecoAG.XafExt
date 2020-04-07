using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.ExpressApp.DC;

using DevExpress.Spreadsheet;

namespace IntecoAG.XafExt.Spreadsheet {

    public class CellChangedEventArgs : EventArgs {
        public Int32 SheetId { get; protected set; }
        public Int32 ColId { get; protected set; }
        public Int32 RowId { get; protected set; }
        public Object Value { get; protected set; }

        public CellChangedEventArgs(Int32 sheet_id, Int32 col_id, Int32 row_id, Object value) {
            SheetId = sheet_id;
            ColId = col_id;
            RowId = row_id;
            Value = value;
        }
    }

    //[NonPersistentDc]
    public interface IWorkbookStore {

        event EventHandler<CellChangedEventArgs> CellChanged;

        //IWorkbookValue WorkbookValue { get; set; }

        void ImportPrepare(IWorkbook book);
        void Import(IWorkbook book);
        void Load(IWorkbook book);
        void Save(IWorkbook book);
        void Export(IWorkbook book);

//        void FileDataLoad(IWorkbook book);
//        void FileDataSave(IWorkbook book);

//        void Render
        void OnBookCellChanged(CellChangedEventArgs args);

        Object CellDataGet(Int32 sheet_id, Int32 col_id, Int32 row_id);

    }

}
