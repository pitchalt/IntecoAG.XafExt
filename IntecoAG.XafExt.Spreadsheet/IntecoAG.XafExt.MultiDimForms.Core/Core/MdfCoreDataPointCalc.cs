using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Newtonsoft.Json;
using IntecoAG.XpoExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Persistent("FmMdfCoreDataPointCalc")]
    public class MdfCoreDataPointCalc : IagBaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        private String _Code;
        [Size(64)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private Boolean _IsDisabled;
        public Boolean IsDisabled {
            get { return _IsDisabled; }
            set { SetPropertyValue(ref _IsDisabled, value); }
        }

        private Boolean _IsTested;
        public Boolean IsTested {
            get { return _IsTested; }
            set { SetPropertyValue(ref _IsTested, value); }
        }

        private MdfCoreCalcVariant _CalcVariant;
        [Association]
        public MdfCoreCalcVariant CalcVariant {
            get { return _CalcVariant; }
            set { SetPropertyValue(ref _CalcVariant, value); }
        }

        [PersistentAlias(nameof(CalcVariant))]
        public MdfCoreCalcVariant CalcVariantUi {
            get { return CalcVariant; }
            set { CalcVariant = value; }
        }

        [Association]
        [Aggregated]
        public XPCollection<MdfCoreDataPointCalcLink> CalcLinks {
            get { return GetCollection<MdfCoreDataPointCalcLink>(); }
        }

        private MdfCoreDataPoint _DataPoint;
        [DataSourceProperty(nameof(Table) + "." + nameof(MdfCoreTable.Container) + "." + nameof(MdfCoreContainer.DataPoints))]
        [Association]
        public MdfCoreDataPoint DataPoint {
            get { return _DataPoint; }
            set { SetPropertyValue(ref _DataPoint, value); }
        }

        private MdfCoreTable _Table;
        [Association]
        public MdfCoreTable Table {
            get { return _Table; }
            set { SetPropertyValue(ref _Table, value); }
        }

        private MdfCoreTableCell _TableCell;
        [DataSourceProperty(nameof(Table) + "." + nameof(MdfCoreTable.Cells))]
        [Association]
        public MdfCoreTableCell TableCell {
            get { return _TableCell; }
            set { SetPropertyValue(ref _TableCell, value); }
        }

        private String _Formula;
        [Size(SizeAttribute.Unlimited)]
        public String Formula {
            get { return _Formula; }
            set { SetPropertyValue(ref _Formula, value); }
        }

        private String _Expression;
        [Size(SizeAttribute.Unlimited)]
        public String Expression {
            get { return _Expression; }
            set { SetPropertyValue(ref _Expression, value); }
        }

        public MdfCoreDataPointCalc(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnChanged(String property_name, Object old_value, Object new_value) {
            base.OnChanged(property_name, old_value, new_value);
            switch (property_name) {
                //case nameof(DataPoint):
                //    var old = (MdfCoreDataPoint)old_value;
                //    if (old != null && old.Calc == this) {
                //        old.Calc = null;
                //    }
                //    if (DataPoint != null) {
                //        DataPoint.Calc = this;
                //    }
                //    //if (DataPoint != null && Table != null) {
                //    //    foreach (var calc in DataPoint.Calcs.ToList()) {
                //    //        if (calc.Table == Table && calc != this) {
                //    //            calc.DataPoint = null;
                //    //        }
                //    //    }
                //    //}
                //    break;
                case nameof(TableCell):
                    //var old_cell = (MdfCoreTableCell)old_value;
                    //if (old_cell != null && old_cell.Calc == this) {
                    //    old_cell.Calc = null;
                    //}
                    DataPoint = TableCell?.DataPoint;
                    Table = TableCell?.Table;
                    break;
                case nameof(Formula):
                    Update();
                    break;
            }
        }

        public override string ToString() {
            return String.IsNullOrWhiteSpace(Formula) ? base.ToString() : Formula;
        }

        [Action(Caption = "Update")]
        public void UpdateAction() {
            Update();
        }

        public void Update() {
            var calc_links = CalcLinks.ToList();
            if (Formula == null || Formula.Length < 1)
                return;
            var link_names = GetFormulaLinks(Formula);
            foreach (var link_name in link_names) {
                if (link_name.Length < 2)
                    continue;
                Int32 link_index = Int32.Parse(link_name.Substring(1));
                var calc_link = CalcLinks.FirstOrDefault(x => x.Index == link_index);
                if (calc_link != null) {
                    calc_links.Remove(calc_link);
                }
                else {
                    calc_link = new MdfCoreDataPointCalcLink(Session);
                    CalcLinks.Add(calc_link);
                    calc_link.Index = link_index;
                }
                calc_link.Formula = link_name;
                calc_link.IsUsed = true;
            }
            foreach (var calc_link in calc_links) {
                calc_link.IsUsed = false;
            }
            foreach (var calc_link in CalcLinks) {
                if (calc_link.IsUsed)
                    calc_link.UpdateFields();
            }
        }

        static Regex _regex = new Regex(@"#(\d*)");

        public static IEnumerable<String> GetFormulaLinks(String formula) {
            MatchCollection matches = _regex.Matches(formula);
            var result = new List<String>();
            foreach (Match match in matches) {
                if (!result.Contains(match.Value))
                    result.Add(match.Value);
            }
            return result;
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }

}