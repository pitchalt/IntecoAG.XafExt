using System;
using System.Collections;
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
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
//

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfReportFormExcelSheetColumn : IReadOnlyList<MdfReportFormExcelSheetCellCore> { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private readonly Int32 _Index;
        public Int32 Index {
            get { return _Index; }
        }

        private readonly List<MdfReportFormExcelSheetCellCore> _Cells;
        public MdfReportFormExcelSheetCellCore this[int index] {
            get { return _Cells[index]; }
            set {
                for (Int32 add_index = _Cells.Count; add_index <= index; add_index++) {
                    _Cells.Add(null);
                }
                _Cells[index] = value;
            }
        }

        public MdfReportFormExcelSheetCellCore CellGet(Int32 index) {
            return index < _Cells.Count ? _Cells[index] : null;
        }

        public MdfReportFormExcelSheetColumn(MdfReportFormExcelSheetCore sheet, Int32 index) {
            _Cells = new List<MdfReportFormExcelSheetCellCore>(128);
            _Index = index;
        }

        public int Count {
            get { return _Cells.Count; }
        }

        public IEnumerator<MdfReportFormExcelSheetCellCore> GetEnumerator() {
            return _Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}