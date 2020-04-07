using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using DevExpress.ExpressApp;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;
//
using IntecoAG.XpoExt;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms;
using IntecoAG.ERM.CS.Nomenclature;
using IntecoAG.ERM.FM.Standart;
using IntecoAG.ERM.FM.Budget;

namespace NpoMash.Erm.Mdf.Test.Templates.ReportOrderPlan {

    public abstract partial class ReportOrderPlanForm {

        public override void CalculateAll() {
            Report.ReportCore.CalculateAll();
        }

    }

    public partial class ReportOrderPlanCategoryValue {

        public override MdfConcept Concept {
            get { return DimValueType; }
        }


    }

    public partial class ReportOrderPlanTemplate {

        [Action(Caption = "Generate Consts")]
        public void ConstAllGenerateAction() {
            ConstAllGenerate();
            ConstAllHierarchyLink();
        }

    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanArticle {
    }

    [DefaultProperty("NameShort")]
    public partial class ReportOrderPlanCorporator {
    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanValuta {

        private csValuta _Valuta;
        public csValuta Valuta {
            get { return _Valuta; }
            set { SetPropertyValue(ref _Valuta, value); }
        }
    }

    [DefaultProperty("NameCalc")]
    public partial class ReportOrderPlanPeriod {
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public String NameCalc {
            get { return $"({DateBegin:d}, {DateEnd:d})"; } 
        }
    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanVatMode {
    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanOrder {
    }

    [DefaultProperty(nameof(NameShort))]
    public partial class ReportOrderPlanEntity {


        [Association]
        public XPCollection<ReportOrderPlanDeal> Deals {
            get { return GetCollection<ReportOrderPlanDeal>(); }
        }

        private FmBudgetDepGroup _DepGroup;
        public FmBudgetDepGroup DepGroup {
            get { return _DepGroup; }
            set { SetPropertyValue<FmBudgetDepGroup>(ref _DepGroup, value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
            }
        }

    }

    [DefaultProperty(nameof(Code))]
    public partial class ReportOrderPlanEntityKind {
    }

    [DefaultProperty(nameof(NumberDate))]
    public partial class ReportOrderPlanDeal {

        private static DateTime _MinDate = new DateTime(1900, 01, 01);
        public String NumberDate {
            get { return Date > _MinDate ? $"{Number} {Date:D}" : Number; }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(Party):
                    IsOther = Party?.IsOther ?? false || IsOther; 
                    break;
            }
        }
    }

    [DefaultProperty(nameof(Code))]
    public partial class ReportOrderPlanDealKind {
    }

    [NavigationItem("MDF")]
    public partial class ReportOrderPlanReport {

        private Int32 _FinDealCount;
        public Int32 FinDealCount {
            get { return _FinDealCount; }
            set { SetPropertyValue(ref _FinDealCount, value); }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(FinDealCount):
                    //                    FindealListUpdate();
                    break;
                case nameof(Corporator):
                    Entity = Corporator?.Entity;
                    break;
                case nameof(Order):
                    VatMode = Order?.VatMode;
                    break;
            }
        }

        public ReportOrderPlanPeriod PeriodLocate(DateTime date_begin, DateTime date_end) {
            ReportOrderPlanPeriod period = ReportOrderPlanPeriods.FirstOrDefault(x => x.DateBegin == date_begin && x.DateEnd == date_end);
            if (period == null) {
                period = new ReportOrderPlanPeriod(Session);
                ReportOrderPlanPeriods.Add(period);
                period.DateBegin = date_begin;
                period.DateEnd = date_end;
            }
            return period;
        }

        public ReportOrderPlanDeal DealLocate(ReportOrderPlanEntity entity, ReportOrderPlanEntity party, ReportOrderPlanDealKind kind, Boolean is_other, String number,  DateTime date) {
            var deal = ReportOrderPlanDeals.FirstOrDefault(
                        x => x.Entity == entity && x.Party == party && 
                            (is_other && x.IsOther || !is_other && x.Number == number && x.Date == date));
            if (deal == null) {
                deal = ReportOrderPlanTemplate.ReportOrderPlanDeals.FirstOrDefault(
                        x => x.Entity == entity && x.Party == party &&
                            (is_other && x.IsOther || !is_other && x.Number == number && x.Date == date));
            }
            if (deal == null) {
                deal = new ReportOrderPlanDeal(Session) {
                    Entity = entity,
                    Party = party,
                    Kind = kind,
                    IsOther = is_other
                };
                if (is_other) {
                    deal.Number = "Прочие";
                }
                else {
                    deal.Number = number;
                    deal.Date = date;
                }
                ReportOrderPlanDeals.Add(deal);
            }
            return deal;
        }

        public ReportOrderPlanFindeal FindealLocate(ReportOrderPlanDeal deal, ReportOrderPlanValuta valuta, ReportOrderPlanVatMode vat_mode, ReportOrderPlanArticle article, ReportOrderPlanFindealKind kind) {
            var findeal = ReportOrderPlanFindeals.FirstOrDefault(
                        x => x.Deal == deal && x.VatMode == vat_mode && x.Valuta == valuta && x.Article == article && x.Kind == kind);
            if (findeal == null) {
                findeal = ReportOrderPlanTemplate.ReportOrderPlanFindeals.FirstOrDefault(
                            x => x.Deal == deal && x.VatMode == vat_mode && x.Valuta == valuta && x.Article == article && x.Kind == kind);
            }
            if (findeal == null) {
                findeal = new ReportOrderPlanFindeal(Session) {
                    Deal = deal,
                    Valuta = valuta,
                    VatMode = vat_mode,
                    Article = article,
                    Kind = kind
                };
                ReportOrderPlanFindeals.Add(findeal);
            }
            return findeal;
        }

        public void FindealsFill() {
            for (int index = 1; index < FinDealCount; index++) {
                ReportOrderPlanFindeal findeal = ReportOrderPlanFindeals.FirstOrDefault(x => x.DealIndex == index && x.DealSource == ReportOrderPlanFindealSourceType.IMPORTED);
                if (findeal == null) {
                    findeal = new ReportOrderPlanFindeal(Session);
                    ReportOrderPlanFindeals.Add(findeal);
                    findeal.DealIndexSet(index);
                }
                if (findeal.VatMode == null) {
                    if (VatMode == ReportCore.ConstVatMode[ReportOrderPlanVatModeType.NOT_SUBJECT])
                        findeal.VatMode = ReportCore.ConstVatMode[ReportOrderPlanVatModeType.NOT_SUBJECT];
                    else
                        findeal.VatMode = ReportCore.ConstVatMode[ReportOrderPlanVatModeType.NORMAL];
                }
                findeal.DealKind = ReportCore.ConstDealKind[ReportOrderPlanDealKindType.BUY];
                findeal.DealSourceSet(ReportOrderPlanFindealSourceType.IMPORTED);
                findeal.Entity = Entity;
                findeal.Article = ReportCore.ConstArticle[ReportOrderPlanArticleType.ARTICLE_EXPENSE_PARTY_WORK];
                findeal.Kind = ReportCore.ConstFindealKind[ReportOrderPlanFindealKindType.TRADE];
            }
        }

        public void UpdateGeneralValues() {
            var context = new ReportOrderPlanCategoryValue();
            context.DimReport = this;
            context.DimEntity = Entity;
            context.DimOrder = Order;
            context.DimVatMode = Order.VatMode;
            context.DimValueType = ReportCore.ConstValueType[ReportOrderPlanValueTypeType.PRESENTMULT];
            var loc_context = ReportCore.CategoryValues.Locate(context);
            var dp = ReportCore.DataPointGet(loc_context);
            switch (Presentmult?.ValueType) {
                case ReportOrderPlanPresentmultType.MULT_NONE:
                    dp.Value = 1M;
                    break;
                case ReportOrderPlanPresentmultType.MULT_THROUSANT:
                    dp.Value = 1_000M;
                    break;
                case ReportOrderPlanPresentmultType.MULT_MILLION:
                    dp.Value = 1_000_000M;
                    break;
                default:
                    dp.Value = 1M;
                    break;
            }

        }

        public void UpdateStandarts(ReportOrderPlanCalcVariant calcVariant) {
            ReportCore.TablePersStandarts.Render(this, calcVariant);
            //
            UpdateGeneralValues();
            //            var std_table = ReportCore.TableTableStandartsImport;
            var std_types = new XPCollection<FmStandartType>(Session);
            var context = new ReportOrderPlanCategoryValue();
            context.DimReport = this;
            context.DimEntity = Entity;
            context.DimOrder = Order;
            context.DimVatMode = Order.VatMode;
            context.DimValueType = ReportCore.ConstValueType[ReportOrderPlanValueTypeType.STANDART];
            context.DimScenario = ReportCore.ConstScenario[ReportOrderPlanScenarioType.FORECAST];
            context.DimPeriod = PeriodLocate(DateFact, DateEnd);
            IList < ReportOrderPlanPeriod > periods = ReportCore.QueryPeriod(context, ReportOrderPlanPeriodType.PERIOD_PLAN_MONTH);
            foreach (var std in ReportOrderPlanTemplate.ReportOrderPlanStandarts) {
                context.DimStandart = std;
                context.DimVatModeObligation = null;
                context.DimValuta = null;
                context.DimValutaObligation = null;
                context.DimPartyKind = null;
                context.DimParty = null;
                if (std.ValueType == ReportOrderPlanStandartType.STD_EXCHANGE_RATE) {
                    foreach (var std_type in std_types.Where(x => x.ValutaFrom != null)) {
                        UpdateStandartType(std, std_type, context, periods);
                    }
                }
                else if (std.ValueType == ReportOrderPlanStandartType.STD_TAX_VAT) {
                    context.DimVatModeObligation = ReportCore.ConstVatMode[ReportOrderPlanVatModeType.NORMAL];
                    UpdateStandartType(std, std.StandartType, context, periods);
                    context.DimVatModeObligation = ReportCore.ConstVatMode[ReportOrderPlanVatModeType.EXPORT];
                    UpdateStandartType(std, std.StandartType, context, periods);
                }
                else if (std.ValueType == ReportOrderPlanStandartType.STD_STAFF_FOT ||
                        std.ValueType == ReportOrderPlanStandartType.STD_STAFF_INSURANCE ||
                        std.ValueType == ReportOrderPlanStandartType.STD_STAFF_OVERHEAD) {
                    foreach (var party in ReportOrderPlanTemplate.ReportOrderPlanEntitys.Where(x => x.Kind.ValueType == ReportOrderPlanEntityKindType.STAFF )) {
                        context.DimPartyKind = ReportCore.ConstEntityKind[ReportOrderPlanEntityKindType.STAFF];
                        context.DimParty = party;
                        UpdateStandartType(std, std.StandartType, context, periods);
                    }
                }
                else {
                    UpdateStandartType(std, std.StandartType, context, periods);
                }
            }
        }

        public void UpdateStandartType(ReportOrderPlanStandart std, FmStandartType std_type, ReportOrderPlanCategoryValue context, IList<ReportOrderPlanPeriod>  periods) {
            var std_values = new XPCollection<FmStandartValue>(Session, new BinaryOperator(nameof(FmStandartValue.Standart) + "." + nameof(FmStandart.StandartType), std_type)).ToList();
            var valuta_from = ReportOrderPlanTemplate.ReportOrderPlanValutas.FirstOrDefault(x => x.Valuta == std_type.ValutaFrom);
            var valuta_to = ReportOrderPlanTemplate.ReportOrderPlanValutas.FirstOrDefault(x => x.Valuta == std_type.ValutaTo);
            context.DimValutaObligation = valuta_from;
            context.DimValuta = valuta_to;
            foreach (var period in periods) {
                context.DimPeriod = period;
                Int16 priority = -1;
                Decimal value = 0;
                foreach (var std_value in std_values) {
                    var level = std_value.Standart.StandartLevel;
                    if (level.Priority > priority &&
                        std_value.DateBegin <= period.DateBegin &&
                        std_value.DateBegin < std_value.DateEnd) {
                        if (level.IsGlobal ||
                            level.IsEntity ||
                            //level.IsOrder && context.DimOrder == std_value.Standart.Order
                            //level.IsPrjType &&
                            //level.IsSubject &&
                            level.IsDepGroup && context.DimParty.DepGroup == std_value.Standart.DepGroup
                            ) {
                            priority = std_value.Standart.StandartLevel.Priority;
                            value = std_value.Value;
                        }
                    }
                }
                var loc_context = ReportCore.CategoryValues.Locate(context);
                var dp = ReportCore.DataPointGet(loc_context);
                if (context.DimVatModeObligation == ReportCore.ConstVatMode[ReportOrderPlanVatModeType.EXPORT])
                    dp.Value = 0;
                else
                    dp.Value = value;
            }
        }

    }

    public partial class ReportOrderPlanReportCore {

        protected override ReportOrderPlanDataPoint DataPointCreate(ReportOrderPlanCategoryValue cat_value) {
            return new ReportOrderPlanDataPoint(this, cat_value);
        }


        public IList<ReportOrderPlanReport> QueryReport(ReportOrderPlanCategoryValue context, ReportOrderPlanReportType model) {
            var result = new List<ReportOrderPlanReport>();
            if (context.DimVatMode == null || context.DimVatMode == Report.VatMode)
                result.Add(Report);
            return result;
        }

        public IList<ReportOrderPlanEntity> QueryEntity(ReportOrderPlanCategoryValue context, ReportOrderPlanEntityType model) {
            switch (model) {
                case ReportOrderPlanEntityType.PARTY_BAY_STAFF:
                    return Report.ReportOrderPlanTemplate.ReportOrderPlanEntitys.Where(x => x.Kind.ValueType == ReportOrderPlanEntityKindType.STAFF).ToList();
            }
            return new List<ReportOrderPlanEntity>();
        }

        public IList<ReportOrderPlanValuta> QueryValuta(ReportOrderPlanCategoryValue context, ReportOrderPlanValutaType model) {
            var result = new List<ReportOrderPlanValuta>();
            if (model == ReportOrderPlanValutaType.ALL_OBLIGATION) {
                result.AddRange(Report.ReportOrderPlanTemplate.ReportOrderPlanValutas);
                //                result = Report.ReportOrderPlanFindeals.Union(Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals).Select(x => x.Valuta).Distinct().ToList();
            }
            return result;
        }

        public IList<ReportOrderPlanVatMode> QueryVatMode(ReportOrderPlanCategoryValue context, ReportOrderPlanVatModeType model) {
            var result = new List<ReportOrderPlanVatMode>();
            if (model == ReportOrderPlanVatModeType.SUBJECT) {
                result.Add(ConstVatMode[ReportOrderPlanVatModeType.NORMAL]);
                result.Add(ConstVatMode[ReportOrderPlanVatModeType.EXPORT]);
            }
            if (model == ReportOrderPlanVatModeType.ALLRATE) {
                result.Add(ConstVatMode[ReportOrderPlanVatModeType.NORMAL]);
                result.Add(ConstVatMode[ReportOrderPlanVatModeType.EXPORT]);
                result.Add(ConstVatMode[ReportOrderPlanVatModeType.NOT_SUBJECT]);
            }
            return result;
        }

        public IList<ReportOrderPlanDeal> QueryDeal(ReportOrderPlanCategoryValue context, ReportOrderPlanDealType model) {
            return new List<ReportOrderPlanDeal>();
        }

        public IList<ReportOrderPlanFindeal> QueryFindeal(ReportOrderPlanCategoryValue context, ReportOrderPlanFindealType model) {
            switch (model) {
                case ReportOrderPlanFindealType.FINDEAL_NOTREPORT_VALUTA:
                    return Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Union(Report.ReportOrderPlanFindeals).Where(
                        x => (context.DimPartyKind is null || x.PartyKind == context.DimPartyKind) &&
                            (x.Valuta != context.DimReport.Valuta)).ToList();
                case ReportOrderPlanFindealType.FINDEAL_NOTPARTY:
                    return Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Union(Report.ReportOrderPlanFindeals).Where(
                        x => (x.PartyKind.ValueType == ReportOrderPlanEntityKindType.ENTITY) &&
                            (context.DimVatModeObligation == null || x.VatMode == context.DimVatModeObligation) &&
                            !((x.Article.HieArticleIeUp.ValueType == ReportOrderPlanArticleType.ARTICLE_EXPENSE_PARTY ||
                             x.Article.HieArticleIeUp.ValueType == ReportOrderPlanArticleType.ARTICLE_INCOME_SALE))).ToList();
                case ReportOrderPlanFindealType.FINDEAL_PARTY:
                    return Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Union(Report.ReportOrderPlanFindeals).Where(
                        x => (x.PartyKind.ValueType == ReportOrderPlanEntityKindType.ENTITY) &&
                            (context.DimVatModeObligation == null || x.VatMode == context.DimVatModeObligation) &&
                            (x.Article.HieArticleIeUp.ValueType == ReportOrderPlanArticleType.ARTICLE_EXPENSE_PARTY ||
                             x.Article.HieArticleIeUp.ValueType == ReportOrderPlanArticleType.ARTICLE_INCOME_SALE )).ToList();
                case ReportOrderPlanFindealType.FINDEAL:
                    return Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Union(Report.ReportOrderPlanFindeals).Where(
                        x => (context.DimPartyKind == null || x.PartyKind == context.DimPartyKind) &&
                            (context.DimDealKind == null || x.DealKind == context.DimDealKind) &&
                            (context.DimValutaObligation == null || x.Valuta == context.DimValutaObligation) &&
                            (context.DimVatModeObligation == null || x.VatMode == context.DimVatModeObligation) &&
                            (context.DimArticleObligation == null || x.Article == context.DimArticleObligation) &&
                            (context.DimPrimaryValue == null || 
                                (x.PartyKind.ValueType == ReportOrderPlanEntityKindType.ENTITY &&  x.Article.PrimaryValue == context.DimPrimaryValue) ||
                                (x.PartyKind.ValueType == ReportOrderPlanEntityKindType.STAFF && 
                                    (context.DimReport.StaffPrimaryValue == null && x.Article.PrimaryValue == context.DimPrimaryValue ||
                                     context.DimReport.StaffPrimaryValue == context.DimPrimaryValue))) ).ToList();
                case ReportOrderPlanFindealType.FINDEAL_IMPORT:
                    return Report.ReportOrderPlanFindeals.Where(x => x.DealSource == ReportOrderPlanFindealSourceType.IMPORTED).ToList();
                case ReportOrderPlanFindealType.FINDEAL_SALE:
                    return
                        Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Where(x => x.DealKind.ValueType == ReportOrderPlanDealKindType.SALE && x.Entity == context.DimEntity).
                        Union(Report.ReportOrderPlanFindeals.Where(x => x.DealKind.ValueType == ReportOrderPlanDealKindType.SALE && x.Entity == context.DimEntity)).ToList();
                case ReportOrderPlanFindealType.FINDEAL_SALE_PLAN:
                    var sale_deal = Report?.Order?.Deal;
                    if (sale_deal == null) {
                        var sale_party = Report?.Order?.Party ?? ConstEntity[ReportOrderPlanEntityType.PARTY_SALE_PARTY_OTHER];
                        sale_deal = Report.DealLocate(context.DimEntity, sale_party, ConstDealKind[ReportOrderPlanDealKindType.SALE], true, null, default(DateTime)); 
                    }
                    return new List<ReportOrderPlanFindeal>() {
                        Report.FindealLocate(sale_deal, Report.ValutaSale, Report.VatMode, context.DimArticle, Report.ReportCore.ConstFindealKind[ReportOrderPlanFindealKindType.TRADE])
                    };
                case ReportOrderPlanFindealType.FINDEAL_BUY:
                    return
                        Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Where(x => x.DealKind.ValueType == ReportOrderPlanDealKindType.BUY && x.Entity == context.DimEntity).
                        Union(Report.ReportOrderPlanFindeals.Where(x => x.DealKind.ValueType == ReportOrderPlanDealKindType.BUY && x.Entity == context.DimEntity)).ToList();
                case ReportOrderPlanFindealType.FINDEAL_OTHER:
                    ReportOrderPlanFindeal findeal = Report.ReportOrderPlanTemplate.ReportOrderPlanFindeals.Where(
                        x => x.DealKind.ValueType == ReportOrderPlanDealKindType.BUY &&
                            x.Entity == context.DimEntity &&
                            x.Party?.ValueType == ReportOrderPlanEntityType.PARTY_BAY_PARTY_OTHER &&
                            x.Deal != null &&
                            x.Article == context.DimArticle).FirstOrDefault();
                    if (findeal != null)
                        return new List<ReportOrderPlanFindeal>() { findeal };
                    else
                        break;
            }
            return new List<ReportOrderPlanFindeal>();
        }

        public IList<ReportOrderPlanPeriod> QueryPeriod(ReportOrderPlanCategoryValue context, ReportOrderPlanPeriodType model) {
            List<ReportOrderPlanPeriod> result = new List<ReportOrderPlanPeriod>(128);
            if (this.Report.DateBegin == default(DateTime) ||
                this.Report.DateEnd == default(DateTime) ||
                this.Report.DateFact == default(DateTime))
                return result;
            ReportOrderPlanPeriod period;
            DateTime date_begin;
            DateTime date_end;
            switch (model) {
                case ReportOrderPlanPeriodType.PERIOD_BEGIN:
                    period = Report.PeriodLocate(default(DateTime), context.DimPeriod.DateBegin);
                    period.Code = $"На {context.DimPeriod.DateBegin:d}";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_END:
                    period = Report.PeriodLocate(default(DateTime), context.DimPeriod.DateEnd);
                    period.Code = $"На {context.DimPeriod.DateEnd:d}";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN:
                    period = Report.PeriodLocate(this.Report.DateFact.Date, this.Report.DateEnd.Date.AddDays(1));
                    period.Code = "Плановый период";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_YEAR_TOEND:
                    DateTime year_end = context.DimPeriod.DateEnd.AddDays(-1);
                    period = Report.PeriodLocate(year_end.Year == Report.DateFact.Year ? Report.DateFact.Date : new DateTime(year_end.Year, 01, 01), context.DimPeriod.DateEnd);
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_BEGIN:
                    period = Report.PeriodLocate(default(DateTime), this.Report.DateFact.Date);
                    period.Code = "На начало планового";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_BALANCE_END:
                    period = Report.PeriodLocate(default(DateTime), this.Report.DateEnd.Date.AddDays(1));
                    period.Code = "На конец планового";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_YEAR_BALANCE_BEGIN:
                    period = Report.PeriodLocate(default(DateTime), context.DimPeriod.DateBegin);
                    period.Code = $@"На начало {period.DateEnd.Year}";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_YEAR_BALANCE_END:
                    period = Report.PeriodLocate(default(DateTime), context.DimPeriod.DateEnd);
                    period.Code = $@"На конец {period.DateEnd.Year - 1}";
                    result.Add(period);
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_YEAR:
                    for (int year = context.DimPeriod.DateBegin.Year; year <= (context.DimPeriod.DateEnd.AddDays(-1)).Year; year++) {
                        period = Report.PeriodLocate(year == context.DimPeriod.DateBegin.Year ? context.DimPeriod.DateBegin : new DateTime(year, 1, 1),
                                              year == context.DimPeriod.DateEnd.Year ? context.DimPeriod.DateEnd : new DateTime(year + 1, 1, 1));
                        period.Code = year.ToString();
                        result.Add(period);
                    }
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_FIRST_YEAR:
                    date_begin = Report.DateFact.Date;
                    if (Report.DateFact.Month > 6) {
                        if (Report.DateEnd.Year > Report.DateFact.Year) {
                            date_end = new DateTime(Report.DateFact.Year + 2, 1, 1);
                        }
                        else {
                            date_end = Report.DateEnd.Date.AddDays(1);
                        }
                    }
                    else {
                        date_end = new DateTime(Report.DateFact.Year + 1, 1, 1);
                    }
                    for (int year = date_begin.Year; year <= date_end.AddDays(-1).Year; year++) {
                        period = Report.PeriodLocate(year == date_begin.Year ? date_begin : new DateTime(year, 1, 1),
                                              year == date_end.AddDays(-1).Year ? date_end : new DateTime(year + 1, 1, 1));
                        period.Code = year.ToString();
                        result.Add(period);
                    }
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_SECOND_YEAR:
                    if (Report.DateEnd.Year == Report.DateFact.Year) {
                        return new List<ReportOrderPlanPeriod>();
                    }
                    if (Report.DateFact.Month > 6) {
                        date_begin = new DateTime(Report.DateFact.Year + 2, 1, 1);
                    }
                    else {
                        date_begin = new DateTime(Report.DateFact.Year + 1, 1, 1);
                    }
                    date_end = Report.DateEnd.Date.AddDays(1);
                    for (int year = date_begin.Year; year <= date_end.AddDays(-1).Year; year++) {
                        period = Report.PeriodLocate(year == date_begin.Year ? date_begin : new DateTime(year, 1, 1),
                                              year == date_end.AddDays(-1).Year ? date_end : new DateTime(year + 1, 1, 1));
                        period.Code = year.ToString();
                        result.Add(period);
                    }
                    break;
                //case ReportOrderPlanPeriodType.PERIOD_PLAN_QUARTER:
                //    date_begin = context.DimPeriod.DateBegin;
                //    date_end = context.DimPeriod.DateEnd.AddDays(-1);
                //    for (int year = date_begin.Year; year <= date_end.Year; year++) {
                //        int quart_begin = year == date_begin.Year ? (date_begin.Month - 1) / 3 + 1 : 1;
                //        int quart_end = year == date_end.Year ? (date_end.Month - 1) / 3 + 1 : 4;
                //        for (int quart = quart_begin; quart <= quart_end; quart++) {
                //            period = Report.PeriodLocate(new DateTime(year, (quart - 1) * 3 + 1, 1), new DateTime(quart == 4 ? year + 1 : year, quart == 4 ? 1 : (quart - 1) * 3 + 1, 1));
                //            period.Code = $@"кв. {quart}";
                //            result.Add(period);
                //        }
                //    }
                //    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_QUARTER:
                    date_begin = context.DimPeriod.DateBegin;
                    date_end = context.DimPeriod.DateEnd.AddDays(-1);
                    for (int year = date_begin.Year; year <= date_end.Year; year++) {
                        int quart_begin = year == date_begin.Year ? (date_begin.Month - 1) / 3 + 1 : 1;
                        int quart_end = year == date_end.Year ? (date_end.Month - 1) / 3 + 1 : 4;
                        for (int quart = quart_begin; quart <= quart_end; quart++) {
                            period = Report.PeriodLocate(new DateTime(year, (quart - 1) * 3 + 3, 1), new DateTime(quart == 4 ? year + 1 : year, quart == 4 ? 1 : quart * 3 + 1, 1));
                            period.Code = $@"кв. {quart}";
                            result.Add(period);
                        }
                    }
                    break;
                case ReportOrderPlanPeriodType.PERIOD_PLAN_MONTH:
                    date_begin = context.DimPeriod.DateBegin;
                    date_end = context.DimPeriod.DateEnd.AddDays(-1);
                    for (int year = date_begin.Year; year <= date_end.Year; year++) {
                        int month_begin = year == date_begin.Year ? date_begin.Month : 1;
                        int month_end = year == date_end.Year ? date_end.Month : 12;
                        for (int month = month_begin; month <= month_end; month++) {
                            period = Report.PeriodLocate(new DateTime(year, month, 1), new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, 1));
                            period.Code = period.DateBegin.ToString("MMM");
                            result.Add(period);
                        }
                    }
                    break;

            }
            return result;
        }
    }

    public partial class ReportOrderPlanForm {

        public override bool IsCalcDisabled {
            get { return Report.ReportCore.IsCalcDisabled; }
            set { Report.ReportCore.IsCalcDisabled = value; }
        }

        public override bool IsRefreshDisabled {
            get { return Report.ReportCore.IsRefreshDisabled; }
            set { Report.ReportCore.IsRefreshDisabled = value; }
        }
    }

    public partial class ReportOrderPlanTable {

        public override bool IsCalcDisabled {
            get { return Report.ReportCore.IsCalcDisabled; }
            set { Report.ReportCore.IsCalcDisabled = value; }
        }

        public override bool IsRefreshDisabled {
            get { return Report.ReportCore.IsRefreshDisabled; }
            set { Report.ReportCore.IsRefreshDisabled = value; }
        }
    }


    [DefaultProperty(nameof(Code))]
    public partial class ReportOrderPlanPayType {
    }

    public partial class ReportOrderPlanPeriod {

        private DateTime _DateBegin;
        public DateTime DateBegin {
            get { return _DateBegin; }
            set { SetPropertyValue(ref _DateBegin, value); }
        }

        private DateTime _DateEnd;
        public DateTime DateEnd {
            get { return _DateEnd; }
            set { SetPropertyValue(ref _DateEnd, value); }
        }

        public Int32 Year {
            get { return DateBegin.Year; }
        }
        public Int32 Quarter {
            get { return (DateBegin.Month + 2) / 3 ; }
        }
        public Int32 Month {
            get { return DateBegin.Month; }
        }

    }

    public enum ReportOrderPlanFindealSourceType {
        MANUAL = 0,
        IMPORTED = 1,
        AUTOMATIC = 2,
    }

    public partial class ReportOrderPlanFindeal : MdfContainerObject {


        //[Persistent(nameof(MasterDeal))]
        //private ReportOrderPlanFindeal _MasterDeal;
        //[PersistentAlias(nameof(_MasterDeal))]
        //public ReportOrderPlanFindeal MasterDeal {
        //    get { return _MasterDeal; }
        //}
        //public void MasterDealSet(ReportOrderPlanFindeal value) {
        //    SetPropertyValue(ref _MasterDeal, value);
        //}

        [Persistent(nameof(DealSource))]
        private ReportOrderPlanFindealSourceType _DealSource;
        [PersistentAlias(nameof(_DealSource))]
        public ReportOrderPlanFindealSourceType DealSource {
            get { return _DealSource; }
        }
        public void DealSourceSet(ReportOrderPlanFindealSourceType value) {
            SetPropertyValue(ref _DealSource, value);
        }

        [Persistent(nameof(DealIndex))]
        private Int32 _DealIndex;
        [PersistentAlias(nameof(_DealIndex))]
        public Int32 DealIndex {
            get { return _DealIndex; }
        }
        public void DealIndexSet(Int32 value) {
            SetPropertyValue(ref _DealIndex, value);
        }

        private ReportOrderPlanEntity _Entity;
        [DataSourceCriteriaProperty("Container.ContainersCritery")]
        public ReportOrderPlanEntity Entity {
            get { return _Entity; }
            set { SetPropertyValue(ref _Entity, value); }
        }

        private String _PartyName;
        [Size(128)]
        public String PartyName {
            get { return _PartyName; }
            set {
                if (!IsLoading && value.Length > 128)
                    value = value.Substring(0, 128);
                SetPropertyValue(ref _PartyName, value);
            }
        }

        //private String _DealNumber;
        //[Size(128)]
        //public String DealNumber {
        //    get { return _DealNumber; }
        //    set {
        //        if (!IsLoading && value.Length > 128)
        //            value = value.Substring(0, 128);
        //        SetPropertyValue(ref _DealNumber, value);
        //    }
        //}

        //private DateTime _DealDate;
        //public DateTime DealDate {
        //    get { return _DealDate; }
        //    set { SetPropertyValue(ref _DealDate, value); }
        //}

        private String _ValutaCode;
        [Size(3)]
        public String ValutaCode {
            get { return _ValutaCode; }
            set {
                if (!IsLoading && value.Length > 3)
                    value = value.Substring(0, 3);
                SetPropertyValue(ref _ValutaCode, value);
            }
        }

        private String _VatModeCode;
        [Size(8)]
        public String VatModeCode {
            get { return _VatModeCode; }
            set {
                if (!IsLoading && value.Length > 8)
                    value = value.Substring(0, 8);
                SetPropertyValue(ref _VatModeCode, value);
            }
        }

        //private ReportOrderPlanEntity _Entity;
        //public ReportOrderPlanEntity Entity {
        //    get { return _Entity; }
        //    set { SetPropertyValue(ref _Entity, value); }
        //}

        //private ReportOrderPlanArticle _Article;
        //public ReportOrderPlanArticle Article {
        //    get { return _Article; }
        //    set { SetPropertyValue(ref _Article, value); }
        //}

        public override void AfterConstruction() {
            base.AfterConstruction();
            DealSourceSet(ReportOrderPlanFindealSourceType.MANUAL);
        }

        public override string ToString() {
            return $"FD:{DealIndex}";
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue) {
            base.OnChanged(propertyName, oldValue, newValue);
            switch (propertyName) {
                case nameof(Entity):
                    if (Deal != null && Deal.Entity != Entity) {
                        Deal = null;
                    }
                    break;
                case nameof(PartyName):
                    if (!String.IsNullOrWhiteSpace(PartyName)) {
                        UpdateParty();
                    }
                    else {
                        Party = null;
                        Deal = null;
                    }
                    break;
                case nameof(Party):
                    if (Party != null) {
                        PartyKind = Party.Kind;
                        PartyName = Party.NameShort;
                        UpdateDeal();
                    }
                    break;
                case nameof(Deal):
                    if (Deal != null) {
                        var deal = Deal;
                        Entity = deal.Entity;
                        DealKind = deal.Kind;
                        DealNumber = deal.Number;
                        DealDate = deal.Date;
                        Party = deal.Party;
                        Deal = deal;
                    }
                    break;
                case nameof(DealNumber):
                case nameof(DealDate):
                    if (!String.IsNullOrWhiteSpace(PartyName)) {
                        //                        UpdateParty();
                        UpdateDeal();
                    }
                    else {
                        Deal = null;
                    }
                    break;
                case nameof(ValutaCode):
                    if (!String.IsNullOrEmpty(ValutaCode)) {
                        if (Template != null) {
                            Valuta = this.Template.ReportOrderPlanValutas.FirstOrDefault(x => x.Code == ValutaCode);
                        }
                        if (Report != null) {
                            Valuta = Report.ReportOrderPlanTemplate.ReportOrderPlanValutas.FirstOrDefault(x => x.Code == ValutaCode);
                        }
                    }
                    break;
                case nameof(Valuta):
                    if (Valuta != null) {
                        ValutaCode = Valuta.Code;
                    }
                    break;
                case nameof(VatModeCode):
                    if (!String.IsNullOrEmpty(VatModeCode)) {
                        if (Template != null) {
                            VatMode = Template.ReportOrderPlanVatModes.FirstOrDefault(x => x.Code == VatModeCode);
                        }
                        if (Report != null) {
                            VatMode = Report.ReportOrderPlanTemplate.ReportOrderPlanVatModes.FirstOrDefault(x => x.Code == VatModeCode);
                        }
                    }
                    break;
                case nameof(VatMode):
                    if (VatMode != null) {
                        VatModeCode = VatMode.Code;
                    }
                    break;
                case nameof(Report):
                    if (Report != null && Kind == null) {
                        Kind = Report.ReportCore.ConstFindealKind[ReportOrderPlanFindealKindType.TRADE];
                    }
                    break;
                case nameof(Template):
                    if (Template != null && Kind == null) {
                        Kind = Template.ReportOrderPlanFindealKinds.FirstOrDefault(x => x.ValueType == ReportOrderPlanFindealKindType.TRADE);
                    }
                    break;
            }
        }

        protected void UpdateParty() {
            String name_search = PartyName.Replace(" ", "").Replace(".", "");
            if (name_search.Substring(0, 6).ToUpper() == "ПРОЧИЕ") {
                if (Report != null) {
                    if (DealKind.ValueType == ReportOrderPlanDealKindType.SALE) {
                        Party = Report.ReportCore.ConstEntity[ReportOrderPlanEntityType.PARTY_SALE_PARTY_OTHER];
                    }
                    else {
                        Party = Report.ReportCore.ConstEntity[ReportOrderPlanEntityType.PARTY_BAY_PARTY_OTHER];
                    }
                }
            }
            else {
                Party = Template?.ReportOrderPlanEntitys.FirstOrDefault(
                            x => x.NameShort.Replace(" ", "").Replace(".", "") == name_search &&
                            x.Kind?.ValueType == ReportOrderPlanEntityKindType.ENTITY);
                if (Party == null) {
                    Party = Report?.ReportOrderPlanEntitys.FirstOrDefault(
                                x => x.NameShort.Replace(" ", "").Replace(".", "") == name_search &&
                                x.Kind?.ValueType == ReportOrderPlanEntityKindType.ENTITY);
                    if (Party == null && Report != null) {
                        ReportOrderPlanEntity entity = new ReportOrderPlanEntity(Session);
                        entity.Report = Report;
                        entity.NameShort = PartyName;
                        entity.Kind = Report.ReportCore.ConstEntityKind[ReportOrderPlanEntityKindType.ENTITY];
                        Party = entity;
                    }
                }
            }
        }

        protected void UpdateDeal() {
            String number = !String.IsNullOrWhiteSpace(DealNumber) ? DealNumber : "Прочие";
            DateTime date = DealDate > new DateTime(1901, 01, 01) ? DealDate : new DateTime(1901, 01, 01);
            Deal = Template?.ReportOrderPlanDeals.FirstOrDefault(x => x.Entity == Entity && x.Party == Party && x.Number == number && x.Date == date);
            if (Deal == null) {
                Deal = Report?.ReportOrderPlanDeals.FirstOrDefault(x => x.Entity == Report.Entity && x.Party == Party && x.Number == number && x.Date == date);
                if (Deal == null && Report != null && Entity != null && Party != null && DealKind != null) {
                    ReportOrderPlanDeal deal = new ReportOrderPlanDeal(Session);
                    deal.Report = Report;
                    deal.Kind = DealKind;
                    deal.Entity = Report.Entity;
                    deal.Party = Party;
                    deal.Number = number;
                    deal.Date = date;
                    Deal = deal;
                }
            }
        }

    }

    [DefaultProperty(nameof(Code))]
    public partial class ReportOrderPlanFindealKind {
    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanStandart  {

        private FmStandartType _StandartType;
        public FmStandartType StandartType {
            get { return _StandartType; }
            set { SetPropertyValue(ref _StandartType, value); }
        }

    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanValueType {

    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanScenario {

    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanPrimaryValue {

    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanFinrep {

    }

    [DefaultProperty("Code")]
    public partial class ReportOrderPlanPresentmult  {

    }

}
