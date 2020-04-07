using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
//
using DevExpress.Xpo;
using IntecoAG.XpoExt;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms;

namespace IntecoAG.XafExt.Spreadsheet.Test.MultiDimForms.Templates.ReportOrderPlan {


    public enum ReportOrderPlanReportType {
        REPORT = 69,
    }

    [Persistent("FmMdfTestReportOrderPlanReport")]
    public partial class ReportOrderPlanReport : MdfReport {


        [Association]
        public XPCollection<ReportOrderPlanEntity> ReportOrderPlanEntitys {
            get { return GetCollection<ReportOrderPlanEntity>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanDeal> ReportOrderPlanDeals {
            get { return GetCollection<ReportOrderPlanDeal>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanPeriod> ReportOrderPlanPeriods {
            get { return GetCollection<ReportOrderPlanPeriod>(); }
        }

        private ReportOrderPlanReportType _ValueType;
        public ReportOrderPlanReportType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        private System.DateTime _ReportDateBegin;
        public System.DateTime ReportDateBegin {
            get { return _ReportDateBegin; }
            set { SetPropertyValue(ref _ReportDateBegin, value); }
        }

        private System.DateTime _ReportDateEnd;
        public System.DateTime ReportDateEnd {
            get { return _ReportDateEnd; }
            set { SetPropertyValue(ref _ReportDateEnd, value); }
        }

        private System.DateTime _ReportDateFact;
        public System.DateTime ReportDateFact {
            get { return _ReportDateFact; }
            set { SetPropertyValue(ref _ReportDateFact, value); }
        }

        private ReportOrderPlanEntity _ReportEntiry;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanEntity ReportEntiry {
            get { return _ReportEntiry; }
            set { SetPropertyValue(ref _ReportEntiry, value); }
        }

        private ReportOrderPlanPeriod _ReportPeriodPlan;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanPeriod ReportPeriodPlan {
            get { return _ReportPeriodPlan; }
            set { SetPropertyValue(ref _ReportPeriodPlan, value); }
        }

        private ReportOrderPlanPeriod _ReportPeriodFact;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanPeriod ReportPeriodFact {
            get { return _ReportPeriodFact; }
            set { SetPropertyValue(ref _ReportPeriodFact, value); }
        }

        private ReportOrderPlanValuta _ReportValuta;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanValuta ReportValuta {
            get { return _ReportValuta; }
            set { SetPropertyValue(ref _ReportValuta, value); }
        }

        private ReportOrderPlanValuta _ReportSaleValuta;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanValuta ReportSaleValuta {
            get { return _ReportSaleValuta; }
            set { SetPropertyValue(ref _ReportSaleValuta, value); }
        }

        private ReportOrderPlanTemplate _ReportOrderPlanTemplate;
        public ReportOrderPlanTemplate ReportOrderPlanTemplate {
            get { return _ReportOrderPlanTemplate; }
            set { SetPropertyValue(ref _ReportOrderPlanTemplate, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<ReportOrderPlanForm> Forms {
            get { return GetCollection<ReportOrderPlanForm>(); }
        }

        private Dictionary<ReportOrderPlanValueTypeType, ReportOrderPlanValueType> _EnumValueType;
        [Browsable(false)]
        public IDictionary<ReportOrderPlanValueTypeType, ReportOrderPlanValueType> EnumValueType {
            get {
                if (_EnumValueType == null) {
                    _EnumValueType = new Dictionary<ReportOrderPlanValueTypeType, ReportOrderPlanValueType>(ReportOrderPlanTemplate.ReportOrderPlanValueTypes.Count);
                    foreach (var value in ReportOrderPlanTemplate.ReportOrderPlanValueTypes) {
                        _EnumValueType[value.ValueType] = value;
                    }
                }
                return _EnumValueType;
            }
        }

        private Dictionary<ReportOrderPlanScenarioType, ReportOrderPlanScenario> _EnumScenario;
        [Browsable(false)]
        public IDictionary<ReportOrderPlanScenarioType, ReportOrderPlanScenario> EnumScenario {
            get {
                if (_EnumScenario == null) {
                    _EnumScenario = new Dictionary<ReportOrderPlanScenarioType, ReportOrderPlanScenario>(ReportOrderPlanTemplate.ReportOrderPlanScenarios.Count);
                    foreach (var value in ReportOrderPlanTemplate.ReportOrderPlanScenarios) {
                        _EnumScenario[value.ValueType] = value;
                    }
                }
                return _EnumScenario;
            }
        }

        private Dictionary<ReportOrderPlanArticleType, ReportOrderPlanArticle> _EnumArticle;
        [Browsable(false)]
        public IDictionary<ReportOrderPlanArticleType, ReportOrderPlanArticle> EnumArticle {
            get {
                if (_EnumArticle == null) {
                    _EnumArticle = new Dictionary<ReportOrderPlanArticleType, ReportOrderPlanArticle>(ReportOrderPlanTemplate.ReportOrderPlanArticles.Count);
                    foreach (var value in ReportOrderPlanTemplate.ReportOrderPlanArticles) {
                        _EnumArticle[value.ValueType] = value;
                    }
                }
                return _EnumArticle;
            }
        }

        private Dictionary<ReportOrderPlanReportFormType, ReportOrderPlanReportForm> _EnumReportForm;
        [Browsable(false)]
        public IDictionary<ReportOrderPlanReportFormType, ReportOrderPlanReportForm> EnumReportForm {
            get {
                if (_EnumReportForm == null) {
                    _EnumReportForm = new Dictionary<ReportOrderPlanReportFormType, ReportOrderPlanReportForm>(ReportOrderPlanTemplate.ReportOrderPlanReportForms.Count);
                    foreach (var value in ReportOrderPlanTemplate.ReportOrderPlanReportForms) {
                        _EnumReportForm[value.ValueType] = value;
                    }
                }
                return _EnumReportForm;
            }
        }

        private List<ReportOrderPlanCategoryValue> _CategoryValues;
        [Browsable(false)]
        public IList<ReportOrderPlanCategoryValue> CategoryValues {
            get {
                if (_CategoryValues == null) {
                    _CategoryValues = new List<ReportOrderPlanCategoryValue>();
                }
                return _CategoryValues;
            }
        }

        private ReportOrderPlanAxisPeriod _AxisAxisPeriod;
        [Browsable(false)]
        public ReportOrderPlanAxisPeriod AxisAxisPeriod {
            get {
                if (_AxisAxisPeriod == null) {
                    _AxisAxisPeriod = new ReportOrderPlanAxisPeriod(this);
                    _AxisAxisPeriod.Render();
                }
                return _AxisAxisPeriod;
            }
        }

        private ReportOrderPlanAxisIeArticle _AxisAxisIeArticle;
        [Browsable(false)]
        public ReportOrderPlanAxisIeArticle AxisAxisIeArticle {
            get {
                if (_AxisAxisIeArticle == null) {
                    _AxisAxisIeArticle = new ReportOrderPlanAxisIeArticle(this);
                    _AxisAxisIeArticle.Render();
                }
                return _AxisAxisIeArticle;
            }
        }

        private ReportOrderPlanAxisCfArticle _AxisAxisCfArticle;
        [Browsable(false)]
        public ReportOrderPlanAxisCfArticle AxisAxisCfArticle {
            get {
                if (_AxisAxisCfArticle == null) {
                    _AxisAxisCfArticle = new ReportOrderPlanAxisCfArticle(this);
                    _AxisAxisCfArticle.Render();
                }
                return _AxisAxisCfArticle;
            }
        }

        public ReportOrderPlanReport(Session session) : base(session) { }
    }

    public enum ReportOrderPlanTemplateType {
        TEMPLATE = 70,
    }

    [Persistent("FmMdfTestReportOrderPlanTemplate")]
    public partial class ReportOrderPlanTemplate : MdfTemplate {


        [Association]
        public XPCollection<ReportOrderPlanValueType> ReportOrderPlanValueTypes {
            get { return GetCollection<ReportOrderPlanValueType>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanValuta> ReportOrderPlanValutas {
            get { return GetCollection<ReportOrderPlanValuta>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanOrder> ReportOrderPlanOrders {
            get { return GetCollection<ReportOrderPlanOrder>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanScenario> ReportOrderPlanScenarios {
            get { return GetCollection<ReportOrderPlanScenario>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanArticle> ReportOrderPlanArticles {
            get { return GetCollection<ReportOrderPlanArticle>(); }
        }

        [Association]
        public XPCollection<ReportOrderPlanReportForm> ReportOrderPlanReportForms {
            get { return GetCollection<ReportOrderPlanReportForm>(); }
        }

        private ReportOrderPlanTemplateType _ValueType;
        public ReportOrderPlanTemplateType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanTemplate(Session session) : base(session) { }
    }

    public enum ReportOrderPlanEntityType {
    }

    [Persistent("FmMdfTestReportOrderPlanEntity")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanEntity : MdfContainerObject {

        private ReportOrderPlanReport _Report;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanReport Report {
            get { return _Report; }
            set {
                if (SetPropertyValue(ref _Report, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanEntityType _ValueType;
        public ReportOrderPlanEntityType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanEntity(Session session) : base(session) { }
    }

    public enum ReportOrderPlanDealType {
    }

    [Persistent("FmMdfTestReportOrderPlanDeal")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanDeal : MdfContainerObject {

        private ReportOrderPlanReport _Report;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanReport Report {
            get { return _Report; }
            set {
                if (SetPropertyValue(ref _Report, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanDealType _ValueType;
        public ReportOrderPlanDealType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        private ReportOrderPlanEntity _Supplier;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanEntity Supplier {
            get { return _Supplier; }
            set { SetPropertyValue(ref _Supplier, value); }
        }

        private System.String _Number;
        public System.String Number {
            get { return _Number; }
            set { SetPropertyValue(ref _Number, value); }
        }

        private System.DateTime _Date;
        public System.DateTime Date {
            get { return _Date; }
            set { SetPropertyValue(ref _Date, value); }
        }

        public ReportOrderPlanDeal(Session session) : base(session) { }
    }

    public enum ReportOrderPlanPeriodType {
        PERIOD_PLAN = 71,
        PERIOD_PLAN_BALANCE_BEGIN = 72,
        PERIOD_PLAN_BALANCE_END = 73,
        PERIOD_PLAN_YEAR = 74,
        PERIOD_PLAN_YEAR_BALANCE_BEGIN = 75,
        PERIOD_PLAN_YEAR_BALANCE_END = 76,
        PERIOD_PLAN_MONTH = 77,
    }

    [Persistent("FmMdfTestReportOrderPlanPeriod")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanPeriod : MdfContainerObject {

        private ReportOrderPlanReport _Report;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanReport Report {
            get { return _Report; }
            set {
                if (SetPropertyValue(ref _Report, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanPeriodType _ValueType;
        public ReportOrderPlanPeriodType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanPeriod(Session session) : base(session) { }
    }

    public enum ReportOrderPlanValueTypeType {
        VALUE_TYPE_BALANCE = 78,
        VALUE_TYPE_COUNT = 79,
        VALUE_TYPE_SUMM_COST = 80,
        VALUE_TYPE_SUMM_ALL = 81,
        VALUE_TYPE_SUMM_VAT = 82,
    }

    [Persistent("FmMdfTestReportOrderPlanValueType")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanValueType : MdfContainerObject {

        private ReportOrderPlanTemplate _Template;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanValueTypeType _ValueType;
        public ReportOrderPlanValueTypeType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanValueType(Session session) : base(session) { }
    }

    public enum ReportOrderPlanValutaType {
        VALUTA_BUDGET = 83,
        VALUTA_OBLIGATION = 84,
        USD = 85,
        EUR = 86,
        RUB = 87,
    }

    [Persistent("FmMdfTestReportOrderPlanValuta")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanValuta : MdfContainerObject {

        private ReportOrderPlanTemplate _Template;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanValutaType _ValueType;
        public ReportOrderPlanValutaType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanValuta(Session session) : base(session) { }
    }

    public enum ReportOrderPlanOrderType {
    }

    [Persistent("FmMdfTestReportOrderPlanOrder")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanOrder : MdfContainerObject {

        private ReportOrderPlanTemplate _Template;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanOrderType _ValueType;
        public ReportOrderPlanOrderType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanOrder(Session session) : base(session) { }
    }

    public enum ReportOrderPlanScenarioType {
        SCENARIO_PLAN = 88,
        SCENARIO_FACT = 89,
        SCENARIO_FORECAST = 90,
    }

    [Persistent("FmMdfTestReportOrderPlanScenario")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanScenario : MdfContainerObject {

        private ReportOrderPlanTemplate _Template;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanScenarioType _ValueType;
        public ReportOrderPlanScenarioType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanScenario(Session session) : base(session) { }
    }

    public enum ReportOrderPlanArticleType {
        ARTICLE_INCOME = 91,
        ARTICLE_EXPENSE = 92,
        ARTICLE_EXPENSE_GOOD = 93,
        ARTICLE_EXPENSE_GOOD_MATERIAL = 94,
        ARTICLE_EXPENSE_GOOD_COMPONENT = 95,
        ARTICLE_EXPENSE_STAFF = 96,
        ARTICLE_EXPENSE_PARTY = 97,
        ARTICLE_EXPENSE_OTHER = 98,
        ARTICLE_EXPENSE_OTHER_INSURANCE = 99,
        ARTICLE_EXPENSE_OTHER_OTHER = 100,
    }

    [Persistent("FmMdfTestReportOrderPlanArticle")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanArticle : MdfContainerObject {

        private ReportOrderPlanTemplate _Template;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanArticleType _ValueType;
        public ReportOrderPlanArticleType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanArticle(Session session) : base(session) { }
    }

    public enum ReportOrderPlanReportFormType {
        REPORT_IE = 101,
        REPORT_CF = 102,
    }

    [Persistent("FmMdfTestReportOrderPlanReportForm")]
    [DefaultProperty("Code")]
    public partial class ReportOrderPlanReportForm : MdfContainerObject {

        private ReportOrderPlanTemplate _Template;
        [Association]
        [Browsable(false)]
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set {
                if (SetPropertyValue(ref _Template, value) && !IsLoading && value != null) {
                    Container = value;
                }
            }
        }

        private String _Code;
        [Size(64)]
        [VisibleInListView(true)]
        public String Code {
            get { return _Code; }
            set { SetPropertyValue(ref _Code, value); }
        }

        private String _NameShort;
        [ModelDefault("RowCount", "1")]
        [Size(128)]
        [VisibleInListView(true)]
        public String NameShort {
            get { return _NameShort; }
            set { SetPropertyValue(ref _NameShort, value); }
        }

        private ReportOrderPlanReportFormType _ValueType;
        public ReportOrderPlanReportFormType ValueType {
            get { return _ValueType; }
            set { SetPropertyValue(ref _ValueType, value); }
        }

        public ReportOrderPlanReportForm(Session session) : base(session) { }
    }

    public partial class ReportOrderPlanTable : MdfReportTable<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell> {

        public ReportOrderPlanTable(ReportOrderPlanReport report) : base(report) { }

        protected override ReportOrderPlanTableCell CellCreate() {
            return new ReportOrderPlanTableCell(this);
        }

    }

    public partial class ReportOrderPlanTableCell : MdfReportTableCell<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell> {

        public ReportOrderPlanTableCell(ReportOrderPlanTable table) : base(table) { }

    }

    public class ReportOrderPlanCategoryValue : MdfCategoryValue<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell> {

        public static ReportOrderPlanCategoryValue DefaultCategoryValue = new ReportOrderPlanCategoryValue();


        private ReportOrderPlanReport _Report;
        public ReportOrderPlanReport Report {
            get { return _Report; }
            set { _Report = value; }
        }

        private ReportOrderPlanTemplate _Template;
        public ReportOrderPlanTemplate Template {
            get { return _Template; }
            set { _Template = value; }
        }

        private ReportOrderPlanPeriod _DimPeriod;
        public ReportOrderPlanPeriod DimPeriod {
            get { return _DimPeriod; }
            set { _DimPeriod = value; }
        }

        private ReportOrderPlanValueType _DimValueType;
        public ReportOrderPlanValueType DimValueType {
            get { return _DimValueType; }
            set { _DimValueType = value; }
        }

        private ReportOrderPlanValuta _DimValutaBudget;
        public ReportOrderPlanValuta DimValutaBudget {
            get { return _DimValutaBudget; }
            set { _DimValutaBudget = value; }
        }

        private ReportOrderPlanValuta _DimValutaObligation;
        public ReportOrderPlanValuta DimValutaObligation {
            get { return _DimValutaObligation; }
            set { _DimValutaObligation = value; }
        }

        private ReportOrderPlanScenario _DimScenario;
        public ReportOrderPlanScenario DimScenario {
            get { return _DimScenario; }
            set { _DimScenario = value; }
        }

        private ReportOrderPlanArticle _DimArticle;
        public ReportOrderPlanArticle DimArticle {
            get { return _DimArticle; }
            set { _DimArticle = value; }
        }

        private ReportOrderPlanReportForm _DimReportForm;
        public ReportOrderPlanReportForm DimReportForm {
            get { return _DimReportForm; }
            set { _DimReportForm = value; }
        }
        public ReportOrderPlanCategoryValue() { }

        public ReportOrderPlanCategoryValue(ReportOrderPlanCategoryValue category_value) {

            Report = category_value.Report;
            Template = category_value.Template;
            DimPeriod = category_value.DimPeriod;
            DimValueType = category_value.DimValueType;
            DimValutaBudget = category_value.DimValutaBudget;
            DimValutaObligation = category_value.DimValutaObligation;
            DimScenario = category_value.DimScenario;
            DimArticle = category_value.DimArticle;
            DimReportForm = category_value.DimReportForm;

        }

    }

    public partial class ReportOrderPlanAxisPeriod : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell> {

        public enum OrdinateType {
            AXIS_PERIOD_ROOT = 94,
            SCENARIO_FACT = 95,
            SCENARIO_PLAN = 96,
            SCENARIO_FORECAST = 97,
            SCENARIO_FACT_BALANCE = 98,
            SCENARIO_PLAN_BALANCE = 99,
            SCENARIO_PLAN_PERIOD = 100,
            SCENARIO_FORECAST_BALANCE_BEGIN = 101,
            SCENARIO_FORECAST_PERIOD = 102,
            SCENARIO_FORECAST_BALANCE_END = 103,
            SCENARIO_FORECAST_PERIOD_PERIOD = 104,
            SCENARIO_FORECAST_PERIOD_YEAR = 105,
            SCENARIO_FORECAST_PERIOD_YEAR_MONTH = 106,
            SCENARIO_FORECAST_PERIOD_YEAR_PERIOD = 107,
            SCENARIO_FORECAST_PERIOD_YEAR_BALANCE_END = 108,
        }

        public class Ordinate : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate {
            private OrdinateType _OrdinateType;
            public OrdinateType OrdinateType {
                get { return _OrdinateType; }
            }

            public Ordinate(ReportOrderPlanAxisPeriod axis, MdfAxisOrdinateValueType value_type, OrdinateType ord_type) : base(axis, value_type) {
                _OrdinateType = ord_type;
            }
        }

        public ReportOrderPlanAxisPeriod(ReportOrderPlanReport report) : base(report) { }

        public override void OrdinatesFill() {
            Root = OrdinateCreate(null, OrdinateType.AXIS_PERIOD_ROOT);
        }

        protected Ordinate OrdinateCreate(Ordinate ord_up, OrdinateType ord_type) {
            Ordinate ord;
            Ordinate sub_ord;
            Int32 sort_order = 0;
            ReportOrderPlanCategoryValue ord_up_category_value = ord_up?.CategoryValue ?? ReportOrderPlanCategoryValue.DefaultCategoryValue;
            switch (ord_type) {
                case OrdinateType.AXIS_PERIOD_ROOT:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.AXIS_PERIOD_ROOT);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 0;
                    ord.IsIntegrated = true;
                    return ord;
                case OrdinateType.SCENARIO_FACT:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FACT);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimScenarioCategoryValueLocate(ord_up_category_value, DimScenarioEnumGet(ord_up_category_value, ReportOrderPlanScenarioType.SCENARIO_FACT, OrdinateType.SCENARIO_FACT));
                    return ord;
                case OrdinateType.SCENARIO_PLAN:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_PLAN);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimScenarioCategoryValueLocate(ord_up_category_value, DimScenarioEnumGet(ord_up_category_value, ReportOrderPlanScenarioType.SCENARIO_PLAN, OrdinateType.SCENARIO_PLAN));
                    return ord;
                case OrdinateType.SCENARIO_FORECAST:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST);
                    ord.Up = ord_up;
                    ord.SortOrder = 30;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimScenarioCategoryValueLocate(ord_up_category_value, DimScenarioEnumGet(ord_up_category_value, ReportOrderPlanScenarioType.SCENARIO_FORECAST, OrdinateType.SCENARIO_FORECAST));
                    return ord;
                case OrdinateType.SCENARIO_FACT_BALANCE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FACT_BALANCE);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 0;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_fact_balance_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_BEGIN, OrdinateType.SCENARIO_FACT_BALANCE);
                    sort_order = 0;
                    foreach (var scenario_fact_balance_obj in scenario_fact_balance_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FACT_BALANCE);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_fact_balance_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_PLAN_BALANCE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_PLAN_BALANCE);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_plan_balance_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_BEGIN, OrdinateType.SCENARIO_PLAN_BALANCE);
                    sort_order = 0;
                    foreach (var scenario_plan_balance_obj in scenario_plan_balance_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_PLAN_BALANCE);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_plan_balance_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_PLAN_PERIOD:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_PLAN_PERIOD);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_plan_period_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN, OrdinateType.SCENARIO_PLAN_PERIOD);
                    sort_order = 0;
                    foreach (var scenario_plan_period_obj in scenario_plan_period_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_PLAN_PERIOD);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_plan_period_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_BALANCE_BEGIN:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_BALANCE_BEGIN);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_balance_begin_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_BEGIN, OrdinateType.SCENARIO_FORECAST_BALANCE_BEGIN);
                    sort_order = 0;
                    foreach (var scenario_forecast_balance_begin_obj in scenario_forecast_balance_begin_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_BALANCE_BEGIN);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_balance_begin_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_PERIOD:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_PERIOD);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_period_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN, OrdinateType.SCENARIO_FORECAST_PERIOD);
                    sort_order = 0;
                    foreach (var scenario_forecast_period_obj in scenario_forecast_period_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_PERIOD);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_period_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                        OrdinateCreate(sub_ord, OrdinateType.SCENARIO_FORECAST_PERIOD_PERIOD);
                        OrdinateCreate(sub_ord, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR);
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_BALANCE_END:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_BALANCE_END);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 30;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_balance_end_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_END, OrdinateType.SCENARIO_FORECAST_BALANCE_END);
                    sort_order = 0;
                    foreach (var scenario_forecast_balance_end_obj in scenario_forecast_balance_end_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_BALANCE_END);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_balance_end_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_PERIOD_PERIOD:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_PERIOD_PERIOD);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 0;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_period_period_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN, OrdinateType.SCENARIO_FORECAST_PERIOD_PERIOD);
                    sort_order = 0;
                    foreach (var scenario_forecast_period_period_obj in scenario_forecast_period_period_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_PERIOD_PERIOD);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_period_period_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 0;
                    ord.IsIntegrated = true;
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_MONTH:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_MONTH);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_period_year_month_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_MONTH, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_MONTH);
                    sort_order = 0;
                    foreach (var scenario_forecast_period_year_month_obj in scenario_forecast_period_year_month_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_MONTH);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_period_year_month_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_PERIOD:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_PERIOD);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_period_year_period_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_YEAR, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_PERIOD);
                    sort_order = 0;
                    foreach (var scenario_forecast_period_year_period_obj in scenario_forecast_period_year_period_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_PERIOD);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_period_year_period_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                case OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_BALANCE_END:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_BALANCE_END);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 30;
                    ord.IsIntegrated = true;
                    IList<ReportOrderPlanPeriod> scenario_forecast_period_year_balance_end_objs = DimPeriodObjectsGet(ord.CategoryValue, ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_END, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_BALANCE_END);
                    sort_order = 0;
                    foreach (var scenario_forecast_period_year_balance_end_obj in scenario_forecast_period_year_balance_end_objs) {
                        sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.SCENARIO_FORECAST_PERIOD_YEAR_BALANCE_END);
                        sub_ord.Up = ord;
                        sub_ord.CategoryValue = DimPeriodCategoryValueLocate(ord.CategoryValue, scenario_forecast_period_year_balance_end_obj);
                        sub_ord.IsIntegrated = false;
                        sub_ord.SortOrder = sort_order++;
                    }
                    return ord;
                default:
                    return null;
            }
        }

        protected ReportOrderPlanCategoryValue DimScenarioCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanScenario obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(obj, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario) &&
                    ReferenceEquals(context.DimScenario, tmp.DimScenario)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimScenario = obj;
            }
            return value;
        }

        protected ReportOrderPlanScenario DimScenarioEnumGet(ReportOrderPlanCategoryValue context, ReportOrderPlanScenarioType model, OrdinateType ord_type) {
            return Report.EnumScenario[model];
        }
        protected ReportOrderPlanCategoryValue DimPeriodCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanPeriod obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(obj, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod) &&
                    ReferenceEquals(context.DimPeriod, tmp.DimPeriod)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimPeriod = obj;
            }
            return value;
        }

        protected IList<ReportOrderPlanPeriod> DimPeriodObjectsGet(ReportOrderPlanCategoryValue context, ReportOrderPlanPeriodType model, OrdinateType ord_type) {
            return new List<ReportOrderPlanPeriod>();
        }

    }

    public partial class ReportOrderPlanAxisIeArticle : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell> {

        public enum OrdinateType {
            IE_REPORT_IE = 109,
            IE_VALUE_TYPE_BALANCE = 110,
            IE_ARTICLE_EXPENSE = 111,
            IE_ARTICLE_INCOME = 112,
            IE_ARTICLE_EXPENSE_ALL = 113,
            IE_ARTICLE_EXPENSE_GOOD = 114,
            IE_ARTICLE_EXPENSE_STAFF = 115,
            IE_ARTICLE_EXPENSE_PARTY = 116,
            IE_ARTICLE_EXPENSE_OTHER = 117,
            IE_ARTICLE_EXPENSE_GOOD_ALL = 118,
            IE_ARTICLE_EXPENSE_GOOD_MATERIAL = 119,
            IE_ARTICLE_EXPENSE_GOOD_COMPONENT = 120,
            IE_ARTICLE_EXPENSE_OTHER_ALL = 121,
            IE_ARTICLE_EXPENSE_OTHER_INSURANCE = 122,
            IE_ARTICLE_EXPENSE_OTHER_OTHER = 123,
        }

        public new class Ordinate : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate {
            private OrdinateType _OrdinateType;
            public OrdinateType OrdinateType {
                get { return _OrdinateType; }
            }

            public Ordinate(ReportOrderPlanAxisIeArticle axis, MdfAxisOrdinateValueType value_type, OrdinateType ord_type) : base(axis, value_type) {
                _OrdinateType = ord_type;
            }
        }

        public ReportOrderPlanAxisIeArticle(ReportOrderPlanReport report) : base(report) { }

        public override void OrdinatesFill() {
            Root = OrdinateCreate(null, OrdinateType.IE_REPORT_IE);
        }

        protected Ordinate OrdinateCreate(Ordinate ord_up, OrdinateType ord_type) {
            Ordinate ord;
            Ordinate sub_ord;
            Int32 sort_order = 0;
            ReportOrderPlanCategoryValue ord_up_category_value = ord_up?.CategoryValue ?? ReportOrderPlanCategoryValue.DefaultCategoryValue;
            switch (ord_type) {
                case OrdinateType.IE_REPORT_IE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_REPORT_IE);
                    ord.Up = ord_up;
                    ord.SortOrder = 0;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimReportFormCategoryValueLocate(ord_up_category_value, DimReportFormEnumGet(ord_up_category_value, ReportOrderPlanReportFormType.REPORT_CF, OrdinateType.IE_REPORT_IE));
                    return ord;
                case OrdinateType.IE_VALUE_TYPE_BALANCE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_VALUE_TYPE_BALANCE);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimValueTypeCategoryValueLocate(ord_up_category_value, DimValueTypeEnumGet(ord_up_category_value, ReportOrderPlanValueTypeType.VALUE_TYPE_BALANCE, OrdinateType.IE_VALUE_TYPE_BALANCE));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE, OrdinateType.IE_ARTICLE_EXPENSE));
                    return ord;
                case OrdinateType.IE_ARTICLE_INCOME:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_INCOME);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_INCOME, OrdinateType.IE_ARTICLE_INCOME));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_ALL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_ALL);
                    ord.Up = ord_up;
                    ord.SortOrder = 5;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE, OrdinateType.IE_ARTICLE_EXPENSE_ALL));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_GOOD:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_GOOD);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD, OrdinateType.IE_ARTICLE_EXPENSE_GOOD));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_STAFF:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_STAFF);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_STAFF, OrdinateType.IE_ARTICLE_EXPENSE_STAFF));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_PARTY:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_PARTY);
                    ord.Up = ord_up;
                    ord.SortOrder = 30;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_PARTY, OrdinateType.IE_ARTICLE_EXPENSE_PARTY));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_OTHER:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_OTHER);
                    ord.Up = ord_up;
                    ord.SortOrder = 40;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER, OrdinateType.IE_ARTICLE_EXPENSE_OTHER));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_GOOD_ALL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_GOOD_ALL);
                    ord.Up = ord_up;
                    ord.SortOrder = 5;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD, OrdinateType.IE_ARTICLE_EXPENSE_GOOD_ALL));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_GOOD_MATERIAL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_GOOD_MATERIAL);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD_MATERIAL, OrdinateType.IE_ARTICLE_EXPENSE_GOOD_MATERIAL));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_GOOD_COMPONENT:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_GOOD_COMPONENT);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD_COMPONENT, OrdinateType.IE_ARTICLE_EXPENSE_GOOD_COMPONENT));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_OTHER_ALL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_OTHER_ALL);
                    ord.Up = ord_up;
                    ord.SortOrder = 5;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER_INSURANCE, OrdinateType.IE_ARTICLE_EXPENSE_OTHER_ALL));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_OTHER_INSURANCE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_OTHER_INSURANCE);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER_INSURANCE, OrdinateType.IE_ARTICLE_EXPENSE_OTHER_INSURANCE));
                    return ord;
                case OrdinateType.IE_ARTICLE_EXPENSE_OTHER_OTHER:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.IE_ARTICLE_EXPENSE_OTHER_OTHER);
                    ord.Up = ord_up;
                    ord.CategoryValue = ord_up_category_value;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = false;
                    return ord;
                default:
                    return null;
            }
        }

        protected ReportOrderPlanCategoryValue DimReportFormCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanReportForm obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(context.DimReportForm, tmp.DimReportForm) &&
                    ReferenceEquals(obj, tmp.DimReportForm)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimReportForm = obj;
            }
            return value;
        }

        protected ReportOrderPlanReportForm DimReportFormEnumGet(ReportOrderPlanCategoryValue context, ReportOrderPlanReportFormType model, OrdinateType ord_type) {
            return Report.EnumReportForm[model];
        }
        protected ReportOrderPlanCategoryValue DimValueTypeCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanValueType obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(obj, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimValueType = obj;
            }
            return value;
        }

        protected ReportOrderPlanValueType DimValueTypeEnumGet(ReportOrderPlanCategoryValue context, ReportOrderPlanValueTypeType model, OrdinateType ord_type) {
            return Report.EnumValueType[model];
        }
        protected ReportOrderPlanCategoryValue DimArticleCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanArticle obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(obj, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimArticle = obj;
            }
            return value;
        }

        protected ReportOrderPlanArticle DimArticleEnumGet(ReportOrderPlanCategoryValue context, ReportOrderPlanArticleType model, OrdinateType ord_type) {
            return Report.EnumArticle[model];
        }

    }

    public partial class ReportOrderPlanAxisCfArticle : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell> {

        public enum OrdinateType {
            CF_REPORT_CF = 124,
            CF_VALUE_TYPE_BALANCE = 125,
            CF_ARTICLE_EXPENSE = 126,
            CF_ARTICLE_INCOME = 127,
            CF_ARTICLE_EXPENSE_ALL = 128,
            CF_ARTICLE_EXPENSE_GOOD = 129,
            CF_ARTICLE_EXPENSE_STAFF = 130,
            CF_ARTICLE_EXPENSE_PARTY = 131,
            CF_ARTICLE_EXPENSE_OTHER = 132,
            CF_ARTICLE_EXPENSE_GOOD_ALL = 133,
            CF_ARTICLE_EXPENSE_GOOD_MATERIAL = 134,
            CF_ARTICLE_EXPENSE_GOOD_COMPONENT = 135,
            CF_ARTICLE_EXPENSE_OTHER_ALL = 136,
            CF_ARTICLE_EXPENSE_OTHER_INSURANCE = 137,
            CF_ARTICLE_EXPENSE_OTHER_OTHER = 138,
        }

        public class Ordinate : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate {
            private OrdinateType _OrdinateType;
            public OrdinateType OrdinateType {
                get { return _OrdinateType; }
            }

            public Ordinate(ReportOrderPlanAxisCfArticle axis, MdfAxisOrdinateValueType value_type, OrdinateType ord_type) : base(axis, value_type) {
                _OrdinateType = ord_type;
            }
        }

        public ReportOrderPlanAxisCfArticle(ReportOrderPlanReport report) : base(report) { }

        public override void OrdinatesFill() {
            Root = OrdinateCreate(null, OrdinateType.CF_REPORT_CF);
        }

        protected Ordinate OrdinateCreate(Ordinate ord_up, OrdinateType ord_type) {
            Ordinate ord;
            Ordinate sub_ord;
            Int32 sort_order = 0;
            ReportOrderPlanCategoryValue ord_up_category_value = ord_up?.CategoryValue ?? ReportOrderPlanCategoryValue.DefaultCategoryValue;
            switch (ord_type) {
                case OrdinateType.CF_REPORT_CF:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_REPORT_CF);
                    ord.Up = ord_up;
                    ord.SortOrder = 0;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimValueTypeCategoryValueLocate(ord_up_category_value, DimValueTypeEnumGet(ord_up_category_value, ReportOrderPlanValueTypeType.VALUE_TYPE_BALANCE, OrdinateType.CF_REPORT_CF));
                    return ord;
                case OrdinateType.CF_VALUE_TYPE_BALANCE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_VALUE_TYPE_BALANCE);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimValueTypeCategoryValueLocate(ord_up_category_value, DimValueTypeEnumGet(ord_up_category_value, ReportOrderPlanValueTypeType.VALUE_TYPE_BALANCE, OrdinateType.CF_VALUE_TYPE_BALANCE));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE, OrdinateType.CF_ARTICLE_EXPENSE));
                    return ord;
                case OrdinateType.CF_ARTICLE_INCOME:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_INCOME);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_INCOME, OrdinateType.CF_ARTICLE_INCOME));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_ALL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_ALL);
                    ord.Up = ord_up;
                    ord.SortOrder = 5;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE, OrdinateType.CF_ARTICLE_EXPENSE_ALL));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_GOOD:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_GOOD);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD, OrdinateType.CF_ARTICLE_EXPENSE_GOOD));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_STAFF:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_STAFF);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_STAFF, OrdinateType.CF_ARTICLE_EXPENSE_STAFF));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_PARTY:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_PARTY);
                    ord.Up = ord_up;
                    ord.SortOrder = 30;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_PARTY, OrdinateType.CF_ARTICLE_EXPENSE_PARTY));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_OTHER:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_OTHER);
                    ord.Up = ord_up;
                    ord.SortOrder = 40;
                    ord.IsIntegrated = true;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER, OrdinateType.CF_ARTICLE_EXPENSE_OTHER));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_GOOD_ALL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_GOOD_ALL);
                    ord.Up = ord_up;
                    ord.SortOrder = 5;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD, OrdinateType.CF_ARTICLE_EXPENSE_GOOD_ALL));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_GOOD_MATERIAL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_GOOD_MATERIAL);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD_MATERIAL, OrdinateType.CF_ARTICLE_EXPENSE_GOOD_MATERIAL));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_GOOD_COMPONENT:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_GOOD_COMPONENT);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_GOOD_COMPONENT, OrdinateType.CF_ARTICLE_EXPENSE_GOOD_COMPONENT));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_OTHER_ALL:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_OTHER_ALL);
                    ord.Up = ord_up;
                    ord.SortOrder = 5;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER, OrdinateType.CF_ARTICLE_EXPENSE_OTHER_ALL));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_OTHER_INSURANCE:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_OTHER_INSURANCE);
                    ord.Up = ord_up;
                    ord.SortOrder = 10;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER_INSURANCE, OrdinateType.CF_ARTICLE_EXPENSE_OTHER_INSURANCE));
                    return ord;
                case OrdinateType.CF_ARTICLE_EXPENSE_OTHER_OTHER:
                    ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.CF_ARTICLE_EXPENSE_OTHER_OTHER);
                    ord.Up = ord_up;
                    ord.SortOrder = 20;
                    ord.IsIntegrated = false;
                    ord.CategoryValue = DimArticleCategoryValueLocate(ord_up_category_value, DimArticleEnumGet(ord_up_category_value, ReportOrderPlanArticleType.ARTICLE_EXPENSE_OTHER_OTHER, OrdinateType.CF_ARTICLE_EXPENSE_OTHER_OTHER));
                    return ord;
                default:
                    return null;
            }
        }

        protected ReportOrderPlanCategoryValue DimValueTypeCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanValueType obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(obj, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType) &&
                    ReferenceEquals(context.DimValueType, tmp.DimValueType)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimValueType = obj;
            }
            return value;
        }

        protected ReportOrderPlanValueType DimValueTypeEnumGet(ReportOrderPlanCategoryValue context, ReportOrderPlanValueTypeType model, OrdinateType ord_type) {
            return Report.EnumValueType[model];
        }
        protected ReportOrderPlanCategoryValue DimArticleCategoryValueLocate(ReportOrderPlanCategoryValue context, ReportOrderPlanArticle obj) {
            ReportOrderPlanCategoryValue value = null;
            for (int i = 0; i < Report.CategoryValues.Count; i++) {
                ReportOrderPlanCategoryValue tmp = Report.CategoryValues[i];
                if (ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle) &&
                    ReferenceEquals(obj, tmp.DimArticle) &&
                    ReferenceEquals(context.DimArticle, tmp.DimArticle)) {
                    value = tmp;
                    break;
                }
            }
            if (value == null) {
                value = new ReportOrderPlanCategoryValue(context);
                value.DimArticle = obj;
            }
            return value;
        }

        protected ReportOrderPlanArticle DimArticleEnumGet(ReportOrderPlanCategoryValue context, ReportOrderPlanArticleType model, OrdinateType ord_type) {
            return Report.EnumArticle[model];
        }

    }

    //public abstract class ReportOrderPlanSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> : MdfReportFormExcelSheetCell<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>
    //    where Tax : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>
    //    where Tox : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate
    //    where Tay : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>
    //    where Toy : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate
    //    where Tst : ReportOrderPlanSheet<Tax, Tox, Tay, Toy, Tst, Tsc> 
    //    where Tsc : ReportOrderPlanSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> {

    //    private Tst sheet;
    //    private MdfReportFormExcelSheetColumn column;
    //    private MdfReportFormExcelSheetRow row;

    //    protected ReportOrderPlanSheetCell(Tst sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row) : base(sheet, column, row) {
    //        this.sheet = sheet;
    //        this.column = column;
    //        this.row = row;
    //    }
    //}

    //public abstract class ReportOrderPlanSheet<Tax, Tox, Tay, Toy, Tst, Tsc> : MdfReportFormExcelSheet<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell, Tax, Tox, Tay, Toy, Tsc>
    //    where Tax : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>
    //    where Tox : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate
    //    where Tay : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>
    //    where Toy : MdfAxis<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTableCell>.MdfAxisOrdinate
    //    where Tst : ReportOrderPlanSheet<Tax, Tox, Tay, Toy, Tst, Tsc> 
    //    where Tsc : ReportOrderPlanSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> {

    //    public ReportOrderPlanSheet(ReportOrderPlanTable table, Int32 index) : base(index) {
    //        Table = table;
    //    }

    //}

    //public abstract class ReportOrderPlanFormWorkSheetIECell : ReportOrderPlanSheetCell<ReportOrderPlanAxisPeriod, ReportOrderPlanAxisPeriod.Ordinate, ReportOrderPlanAxisIeArticle, ReportOrderPlanAxisIeArticle.Ordinate, ReportOrderPlanFormWorkSheetIE, ReportOrderPlanFormWorkSheetIECell> {

    //    public ReportOrderPlanFormWorkSheetIECell(ReportOrderPlanFormWorkSheetIE sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row) : base(sheet, column, row) {
    //    }
    //}

    //public partial class ReportOrderPlanFormWorkSheetIECellTable : ReportOrderPlanFormWorkSheetIECell { 

    //    private readonly ReportOrderPlanTableCell _TableCell;
    //    public ReportOrderPlanTableCell TableCell {
    //        get { return _TableCell; }
    //    }

    //    public override object Value => throw new NotImplementedException();

    //    public ReportOrderPlanFormWorkSheetIECellTable(ReportOrderPlanFormWorkSheetIE sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row, ReportOrderPlanTableCell table_cell) : base(sheet, column, row) {

    //    }
    //}


    //public partial class ReportOrderPlanFormWorkSheetIECellOrdinateY : ReportOrderPlanFormWorkSheetIECell {

    //    private readonly ReportOrderPlanAxisIeArticle.Ordinate _Ordinate;
    //    public ReportOrderPlanAxisIeArticle.Ordinate Ordinate {
    //        get { return _Ordinate; }
    //    }

    //    public override object Value => throw new NotImplementedException();

    //    public ReportOrderPlanFormWorkSheetIECellOrdinateY(ReportOrderPlanFormWorkSheetIE sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row, ReportOrderPlanAxisIeArticle.Ordinate ordinate) : base(sheet, column, row) {
    //        _Ordinate = ordinate;
    //    }
    //}

    //public partial class ReportOrderPlanFormWorkSheetIECellOrdinateX : ReportOrderPlanFormWorkSheetIECell {

    //    private readonly ReportOrderPlanAxisPeriod.Ordinate _Ordinate;
    //    public ReportOrderPlanAxisPeriod.Ordinate Ordinate {
    //        get { return _Ordinate; }
    //    }

    //    public override object Value => throw new NotImplementedException();

    //    public ReportOrderPlanFormWorkSheetIECellOrdinateX(ReportOrderPlanFormWorkSheetIE sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row, ReportOrderPlanAxisPeriod.Ordinate ordinate) : base(sheet, column, row) {
    //        _Ordinate = ordinate;
    //    }
    //}

    //public class ReportOrderPlanFormWorkSheetIE : ReportOrderPlanSheet<ReportOrderPlanAxisPeriod, ReportOrderPlanAxisPeriod.Ordinate, ReportOrderPlanAxisIeArticle, ReportOrderPlanAxisIeArticle.Ordinate, ReportOrderPlanFormWorkSheetIE, ReportOrderPlanFormWorkSheetIECell> {

    //    public ReportOrderPlanFormWorkSheetIE(ReportOrderPlanTable table, Int32 index) : base(table,  index) {
    //    }

    //    protected override ReportOrderPlanFormWorkSheetIECell OrdinateXCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, ReportOrderPlanAxisPeriod.Ordinate ordinate) {
    //        return new ReportOrderPlanFormWorkSheetIECellOrdinateX(this, column, row, ordinate);
    //    }

    //    protected override ReportOrderPlanFormWorkSheetIECell OrdinateYCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, ReportOrderPlanAxisIeArticle.Ordinate ordinate) {
    //        return new ReportOrderPlanFormWorkSheetIECellOrdinateY(this, column, row, ordinate);
    //    }

    //    protected override ReportOrderPlanFormWorkSheetIECell TableCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, ReportOrderPlanTableCell table_cell) {
    //        return new ReportOrderPlanFormWorkSheetIECellTable(this, column, row, table_cell);
    //    }
    //}



    [MapInheritance(MapInheritanceType.OwnTable)]
    [Persistent("FmMdfTestReportOrderPlanForm")]
    public abstract partial class ReportOrderPlanForm : MdfReportFormExcel {

        private ReportOrderPlanReport _Report;
        [Association]
        public ReportOrderPlanReport Report {
            get { return _Report; }
            set { SetPropertyValue(ref _Report, value); }
        }

        protected ReportOrderPlanForm(Session session) : base(session) { }

    }

    public partial class ReportOrderPlanFormWork : ReportOrderPlanForm {


        public ReportOrderPlanFormWork(Session session) : base(session) {
        }

        protected override void OnLoaded() {
            base.OnLoaded();
            ReportOrderPlanTable table = new ReportOrderPlanTable(Report);
            table.Axiss.Add(Report.AxisAxisPeriod);
            table.Axiss.Add(Report.AxisAxisIeArticle);
            _Sheets.Add(new ReportOrderPlanFormWorkSheetIE(table, 0));
            
        }
    }
}
