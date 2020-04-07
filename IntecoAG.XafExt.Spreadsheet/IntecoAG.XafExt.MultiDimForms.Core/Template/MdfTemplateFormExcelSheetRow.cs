using System;
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
using System.Collections;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;
//
using IntecoAG.XpoExt;
using DevExpress.Xpo.Metadata;
//
namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {
    //[DomainComponent]
    //[DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreTemplateFormExcelSheetRow")]
    public class MdfTemplateFormExcelSheetRow : IagBaseObject, IReadOnlyList<MdfTemplateFormExcelSheetCell> {

        private Int32 _Index;
        public Int32 Index {
            get { return _Index; }
            set { SetPropertyValue(ref _Index, value); }
        }

        private Double _Height;
        public Double Height {
            get { return _Height; }
            set { SetPropertyValue(ref _Height, value); }
        }

        private MdfTemplateFormExcelSheet _Sheet;
        [Association("FmMdfCoreTemplateFormExcelSheet-FmMdfCoreTemplateFormExcelSheetRow")]
        public MdfTemplateFormExcelSheet Sheet {
            get { return _Sheet; }
            set { SetPropertyValue(ref _Sheet, value); }
        }

        [Association]
        [Browsable(false)]
        [Aggregated]
        public XPCollection<MdfTemplateFormExcelSheetCell> PersistentCells {
            get { return GetCollection<MdfTemplateFormExcelSheetCell>(); }
        }


        private MdfTemplateFormExcelSheetCell[] _Cells;

        public void CellsAdd(MdfTemplateFormExcelSheetCell cell) {
            ReloadCells();
            _Cells[cell.ColumnIndex] = cell;
        }

        private void ReloadCells() {
            if (Sheet != null && (_Cells == null || _Cells.Length != Sheet.ColumnCount)) {
                _Cells = new MdfTemplateFormExcelSheetCell[Sheet.ColumnCount];
                //_Cells = new List<MdfTemplateFormExcelSheetCell>(Sheet.Columns.Count);
                foreach (var cell in PersistentCells) {
                    _Cells[cell.ColumnIndex] = cell;
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
                    _Cells[index].Row = this;
                    _Cells[index].Column = Sheet.Columns[index];
                }
                return _Cells[index];
            }
        }

        public MdfTemplateFormExcelSheetCell CellGet(Int32 index) {
            return index < Count ? this[index] : null;
        }

        public Int32 Count {
            get { return Sheet.Columns.Count; }
        }

 //       public MdfCoreAxisOrdinate AxisOrdinate { get; set; }


        public MdfTemplateFormExcelSheetRow (Session session): base(session) { }
        //public MdfTemplateFormExcelSheetRow(MdfTemplateFormExcelSheet sheet, Int32 index) {
        //    Index = index;
        //    Sheet = sheet;
        //    _Cells = new List<MdfTemplateFormExcelSheetCell>();
        //}

        //public void Clear() {
        //    _Cells.Clear();
        //}

        //protected override XPCollection CreateCollection(XPMemberInfo property) {
        //    switch (property.Name) {
        //        case nameof(PersistentCells):
        //            XPCollection cells = new XPCollection(Session, this, property);
        //            cells.Sorting.Add(new SortProperty(nameof(MdfTemplateFormExcelSheetCell.Column) + "." + nameof(MdfTemplateFormExcelSheetColumn.Index), DevExpress.Xpo.DB.SortingDirection.Ascending));
        //            return cells;
        //    }
        //    return base.CreateCollection(property);
        //}

        public IEnumerator<MdfTemplateFormExcelSheetCell> GetEnumerator() {
            return (IEnumerator<MdfTemplateFormExcelSheetCell>) _Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}