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
//
using DevExpress.Xpo;
using IntecoAG.XpoExt;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms;
using DevExpress.Spreadsheet;

namespace NpoMash.Erm.Mdf.Test.Templates.ReportOrderPlan {


    public partial class ReportOrderPlanFormWork {

        protected override void OnLoading(IWorkbook book) {
            base.OnLoading(book);
            _Sheets.Clear();
            ReportOrderPlanFormWorkSheetIe sheet_ie = new ReportOrderPlanFormWorkSheetIe(this, Report.ReportCore.TableTableIe, 0);
            sheet_ie.AxisX = Report.ReportCore.AxisAxisPeriod;
            sheet_ie.AxisY = Report.ReportCore.AxisAxisIeArticle;
            sheet_ie.OffsetCol = 2;
            sheet_ie.OffsetRow = 2;
            sheet_ie.Code = "БСР";
            sheet_ie.Render();
            _Sheets.Add(sheet_ie);
            ReportOrderPlanFormWorkSheetCf sheet_cf = new ReportOrderPlanFormWorkSheetCf(this, Report.ReportCore.TableTableIe, 1);
            sheet_cf.AxisX = Report.ReportCore.AxisAxisPeriod;
            sheet_cf.AxisY = Report.ReportCore.AxisAxisCfArticle;
            sheet_cf.OffsetCol = 2;
            sheet_cf.OffsetRow = 2;
            sheet_cf.Code = "БДДС";
            sheet_cf.Render();
            _Sheets.Add(sheet_cf);
            ReportOrderPlanFormWorkSheetDeal sheet_deal = new ReportOrderPlanFormWorkSheetDeal(this, Report.ReportCore.TableTableBayDeal, 2);
            sheet_deal.AxisX = Report.ReportCore.AxisAxisPeriod;
            sheet_deal.AxisY = Report.ReportCore.AxisAxisFindeal;
            sheet_deal.OffsetCol = 2;
            sheet_deal.OffsetRow = 2;
            sheet_deal.Code = "Соисполнители";
            sheet_deal.Render();
            _Sheets.Add(sheet_deal);
        }
    }

    public partial class ReportOrderPlanFormWorkSheetCfCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormWorkSheetCfCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_ALL:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_GOOD:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_GOOD_ALL:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_GOOD_COMPONENT:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_GOOD_MATERIAL:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_OTHER:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_OTHER_ALL:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_OTHER_INSURANCE:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_OTHER_OTHER:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_PARTY:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_EXPENSE_STAFF:
                    case ReportOrderPlanAxisCfArticle.OrdinateType.CF_ARTICLE_INCOME:
                        return Ordinate.CategoryValue.DimArticle.Code;
                    default:
                        break;
                }
                return String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormWorkSheetIeCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? "Empty";
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormWorkSheetIeCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_ALL:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_GOOD:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_GOOD_ALL:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_GOOD_COMPONENT:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_GOOD_MATERIAL:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_OTHER:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_ALL:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_INSURANCE:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_PARTY:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_EXPENSE_STAFF:
                    case ReportOrderPlanAxisIeArticle.OrdinateType.IE_ARTICLE_INCOME:
                        return Ordinate.CategoryValue.DimArticle.Code;
                    default:
                        break;
                }
                return String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormWorkSheetDealCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }
    public partial class ReportOrderPlanFormWorkSheetDealCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_REPORT:
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL:
                        return "1.";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_PARTY:
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_NUMBER:
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_EXPENSE:
                        return "Затраты";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_DATE:
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_PAYMENT:
                        return "Оплата";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_VALUTA:
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_PREPAYMENT:
                        return "Аванс";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_VATMODE:
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_ALL_POSTPAYMENT:
                        return "Расчет";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_FINDEAL:
                        return $"1.{this.Ordinate.CategoryValue.DimFindeal?.DealIndex}.";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PARTY:
                        IsEditableSet(true);
                        return Ordinate.CategoryValue.DimFindeal?.PartyName;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PARTY_RUB:
                        return "В рублях...";

                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_NUMBER:
                        IsEditableSet(true);
                        return Ordinate.CategoryValue.DimFindeal?.DealNumber;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_DATE:
                        IsEditableSet(true);
                        return Ordinate.CategoryValue.DimFindeal?.DealDate;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_VALUTA:
                        IsEditableSet(true);
                        return Ordinate.CategoryValue.DimFindeal?.ValutaCode;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_VATMODE:
                        IsEditableSet(true);
                        return Ordinate.CategoryValue.DimFindeal?.VatModeCode;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_NUMBER_RUB:
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_DATE_RUB:
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_VALUTA_RUB:
                        return "РУБ";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_VATMODE_RUB:
                        break;

                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_EXPENSE:
                        return "Затраты";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PAYMENT:
                        return "Оплата";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_POSTPAYMENT:
                        return "Аванс";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PREPAYMENT:
                        return "Расчет";

                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_EXPENSE_RUB:
                        return "Затраты";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PAYMENT_RUB:
                        return "Оплата";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PREPAYMENT_RUB:
                        return "Аванс";
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_POSTPAYMENT_RUB:
                        return "Расчет";
                    default:
                        return Ordinate.CategoryValue.DimArticle.Code;
                }
                return String.Empty;
            }
            set {
            }
        }

    }

}
