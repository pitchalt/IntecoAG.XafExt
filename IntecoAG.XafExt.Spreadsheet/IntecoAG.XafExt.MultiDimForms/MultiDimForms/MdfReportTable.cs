using System;
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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public abstract class MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tr : MdfReport
            where Tv : MdfCategoryValue<Tr, Tv, Tt, Tc, Tdp>
            where Tt : MdfReportTable<Tr, Tv, Tt, Tc, Tdp>
            where Tc : MdfReportTableCell<Tr, Tv, Tt, Tc, Tdp>
            where Tdp : MdfReportDataPoint<Tr, Tv, Tt, Tc, Tdp> {

        private readonly List<MdfAxis<Tr, Tv, Tt, Tc, Tdp>> _Axiss;
        public IList<MdfAxis<Tr, Tv, Tt, Tc, Tdp>> Axiss {
            get { return _Axiss; }
        }

        private readonly Dictionary<Tc, Tc> _Cells;
        public IEnumerable<Tc> Cells {
            get { return _Cells.Values; }
        }

        private readonly Tr _Report;
        public Tr Report {
            get { return _Report; }
        }

        public Boolean IsRendered { get; protected set; }

        public MdfReportTable(Tr report) {
            IsRendered = false;
            _Axiss = new List<MdfAxis<Tr, Tv, Tt, Tc, Tdp>>();
            _Report = report;
            _Cells = new Dictionary<Tc, Tc>(4096);
        }

        public Tc Locate(MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate [] ordinates) {
            Tc cell = CellCreateCore(ordinates);
            if (_Cells.TryGetValue(cell, out Tc result)) {
                return result;
            }
            _Cells[cell] = cell;
            return cell;
        }

        protected Tc CellCreateCore(IList<MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate> ordinates) {
            Tc cell = CellCreate();
            for (int i = 0; i < ordinates.Count; i++) {
                cell.Ordinates[i] = ordinates[i];
            }
//            _Cells.Add(cell);
            return cell;
        }

        protected abstract Tc CellCreate();

        protected void RenderCore(MdfReportCore<Tr, Tv, Tt, Tc, Tdp> report_core) {

            if (IsRendered)
                return;
            IsRendered = true;

            IList<IList<Tuple<MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate, Tv>>> axis_mult_list =
                        new List<IList<Tuple<MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate, Tv>>>();
            foreach (var axis in Axiss) {
                axis_mult_list = axis.RenderCellAxis(axis_mult_list);
            }
            //                IDictionary<String, MdfCoreTableCell> cells = new Dictionary<String, MdfCoreTableCell>();
            IList<Tc> old_cells = new List<Tc>(Cells);
            int count = axis_mult_list[0].Count;
            Tv [] values = new Tv[count];
            MdfAxis<Tr, Tv, Tt, Tc, Tdp>.MdfAxisOrdinate[] ordinates = new MdfAxis<Tr, Tv, Tt, Tc,Tdp>.MdfAxisOrdinate[count];
            foreach (var axis_mult_item in axis_mult_list) {
                for (int i = 0; i < count; i++) {
                    values[i] = axis_mult_item[i].Item2;
                    ordinates[i] = axis_mult_item[i].Item1;
                }
                Tv cat_value = report_core.CategoryValues.Union(values);
                Tdp data_point = report_core.DataPointGet(cat_value);
                Tc cell = Locate(ordinates);
                cell.DataPoint = data_point;
                data_point.Cells.Add(cell);
                CellCustom(cell);
                cell.Restore();
            }
        }

        protected abstract void CellCustom(Tc cell);

        [NonPersistent]
        [Browsable(false)]
        public abstract Boolean IsCalcDisabled { get; set; }
        [NonPersistent]
        [Browsable(false)]
        public abstract Boolean IsRefreshDisabled { get; set; }

    }
}