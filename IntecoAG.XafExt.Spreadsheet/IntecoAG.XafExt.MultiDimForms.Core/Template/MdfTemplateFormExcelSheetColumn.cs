using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;
//
using IntecoAG.XpoExt;
//
namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[DomainComponent]
    //[DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreTemplateFormExcelSheetColumn")]
    public class MdfTemplateFormExcelSheetColumn : IagBaseObject, IReadOnlyList<MdfTemplateFormExcelSheetCell> {

        private Int32 _Index;
        public Int32 Index {
            get { return _Index; }
            set { SetPropertyValue(ref _Index, value); }
        }

        private Double _Width;
        public Double Width {
            get { return _Width; }
            set { SetPropertyValue(ref _Width, value); }
        }

        private MdfTemplateFormExcelSheet _Sheet;
        [Association("FmMdfCoreTemplateFormExcelSheet-FmMdfCoreTemplateFormExcelSheetColumn")]
        public MdfTemplateFormExcelSheet Sheet {
            get { return _Sheet; }
            set { SetPropertyValue(ref _Sheet, value); }
        }

        [Association]
        [Aggregated]
        [Browsable(false)]
        public XPCollection<MdfTemplateFormExcelSheetCell> PersistentCells {
            get { return GetCollection<MdfTemplateFormExcelSheetCell>(); }
        }

        private MdfTemplateFormExcelSheetCell[] _Cells;

        public void CellsAdd(MdfTemplateFormExcelSheetCell cell) {
            ReloadCells();
            _Cells[cell.RowIndex] = cell;
        }

        private void ReloadCells() {
            if (_Cells == null || _Cells.Length != Sheet.RowCount) {
                _Cells = new MdfTemplateFormExcelSheetCell[Sheet.RowCount];
                //_Cells = new List<MdfTemplateFormExcelSheetCell>(Sheet.Columns.Count);
                foreach (var pcell in PersistentCells) {
                    _Cells[pcell.RowIndex] = pcell;
                }
            }
        }

        [Browsable(false)]
        public MdfTemplateFormExcelSheetCell this[int index] {
            get {
                ReloadCells();
                if (_Cells[index] == null) {
                    _Cells[index] = new MdfTemplateFormExcelSheetCell(Session);
                    _Cells[index].Sheet = Sheet;
                    _Cells[index].Column = this;
                    _Cells[index].Row = Sheet.Rows[index];
                }
                return _Cells[index];
            }
        }

        public Int32 Count {
            get { return Sheet.Columns.Count; }
        }

        //        public MdfCoreAxisOrdinate AxisOrdinate { get; set; }

        //        public MdfTemplateFormExcelSheetColumn() { }
        public MdfTemplateFormExcelSheetColumn(Session session) : base(session) { }
        //public MdfTemplateFormExcelSheetColumn(MdfTemplateFormExcelSheet sheet, Int32 index) { //, MdfTemplateTable table, MdfAxisOrdinate ordinate) {
        //    Index = index;
        //    Sheet = sheet;
        //    _Cells = new List<MdfTemplateFormExcelSheetCell>();
        //}

        //public void Clear() {
        //    _Cells.Clear();
        //}

        public IEnumerator<MdfTemplateFormExcelSheetCell> GetEnumerator() {
            return (IEnumerator < MdfTemplateFormExcelSheetCell >)_Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }
}