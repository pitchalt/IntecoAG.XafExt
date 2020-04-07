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

    public partial class ReportOrderPlanFormImport {

        IList<ReportOrderPlanTable> PersistentTables;

        protected override void OnLoading(IWorkbook book) {
            base.OnLoading(book);
            Report.ReportCore.IsCalcDisabled = true;
            Report.ReportCore.IsRefreshDisabled = true;
            Report.FindealsFill();
            PersistentTables = new List<ReportOrderPlanTable>();
            Report.ReportCore.TablePersIeBalance.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            PersistentTables.Add(Report.ReportCore.TablePersIeBalance);
            Report.ReportCore.TablePersCfBalance.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            PersistentTables.Add(Report.ReportCore.TablePersCfBalance);
            Report.ReportCore.TablePersCheck.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            PersistentTables.Add(Report.ReportCore.TablePersCheck);
            Report.ReportCore.TablePersStandarts.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            PersistentTables.Add(Report.ReportCore.TablePersStandarts);
            Report.UpdateStandarts(CalcVariant);
            _Sheets.Clear();
            // ХАК
            Report.ReportCore.TableTableCalcImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            //
            Report.ReportCore.TableTableIeImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            ReportOrderPlanFormImportSheetIe sheet_ie = new ReportOrderPlanFormImportSheetIe(this, Report.ReportCore.TableTableIeImport, 0);
            sheet_ie.AxisX = Report.ReportCore.AxisAxisPeriodImport;
            sheet_ie.AxisXIndex = 0;
            sheet_ie.AxisY = Report.ReportCore.AxisAxisIeArticleImport;
            sheet_ie.AxisYIndex = 1;
            sheet_ie.OffsetCol = 0;
            sheet_ie.OffsetRow = 7;
            sheet_ie.Code = "БСР";
            sheet_ie.Render();
            _Sheets.Add(sheet_ie);
            Report.ReportCore.TableTableCfImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            ReportOrderPlanFormImportSheetCf sheet_cf = new ReportOrderPlanFormImportSheetCf(this, Report.ReportCore.TableTableCfImport, 1);
            sheet_cf.AxisX = Report.ReportCore.AxisAxisPeriodImport;
            sheet_cf.AxisXIndex = 0;
            sheet_cf.AxisY = Report.ReportCore.AxisAxisCfArticleImport;
            sheet_cf.AxisYIndex = 1;
            sheet_cf.OffsetCol = 0;
            sheet_cf.OffsetRow = 7;
            sheet_cf.Code = "БДДС";
            sheet_cf.Render();
            _Sheets.Add(sheet_cf);
            Report.ReportCore.TableTableBayDealImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            ReportOrderPlanFormImportSheetDeal sheet_deal = new ReportOrderPlanFormImportSheetDeal(this, Report.ReportCore.TableTableBayDealImport, 2);
            sheet_deal.AxisX = Report.ReportCore.AxisAxisPeriodImport;
            sheet_deal.AxisXIndex = 0;
            sheet_deal.AxisY = Report.ReportCore.AxisAxisFindeal;
            sheet_deal.AxisYIndex = 1;
            sheet_deal.OffsetCol = 1;
            sheet_deal.OffsetRow = 7;
            sheet_deal.Code = "Соисполнители";
            sheet_deal.Render();
            _Sheets.Add(sheet_deal);
            //
            Report.ReportCore.TableTableGoodsImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            ReportOrderPlanFormImportSheetGoods sheet_goods = new ReportOrderPlanFormImportSheetGoods(this, Report.ReportCore.TableTableGoodsImport, 3);
            sheet_goods.AxisX = Report.ReportCore.AxisAxisPeriodImport;
            sheet_goods.AxisXIndex = 0;
            sheet_goods.AxisY = Report.ReportCore.AxisAxisGoodsImport;
            sheet_goods.AxisYIndex = 1;
            sheet_goods.OffsetCol = 0;
            sheet_goods.OffsetRow = 7;
            sheet_goods.Code = "ТМЦ";
            sheet_goods.Render();
            _Sheets.Add(sheet_goods);
            //
            _Sheets.Add(null);
            _Sheets.Add(null);
            //
            Report.ReportCore.TableTableStandartsImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            ReportOrderPlanFormImportSheetStandarts sheet_standarts = new ReportOrderPlanFormImportSheetStandarts(this, Report.ReportCore.TableTableStandartsImport, 6);
            sheet_standarts.AxisX = Report.ReportCore.AxisAxisPeriodImport;
            sheet_standarts.AxisXIndex = 0;
            sheet_standarts.AxisY = Report.ReportCore.AxisAxisStandartImport;
            sheet_standarts.AxisYIndex = 1;
            sheet_standarts.OffsetCol = 0;
            sheet_standarts.OffsetRow = 7;
            sheet_standarts.Code = "Нормативы2";
            sheet_standarts.Render();
            _Sheets.Add(sheet_standarts);
            //
            Report.ReportCore.TableTableCalcImport.Render(Report, ReportOrderPlanCalcVariant.IMPORT);
            ReportOrderPlanFormImportSheetCalc sheet_calc = new ReportOrderPlanFormImportSheetCalc(this, Report.ReportCore.TableTableCalcImport, 7);
            sheet_calc.AxisX = Report.ReportCore.AxisAxisCalcImportCol;
            sheet_calc.AxisXIndex = 0;
            sheet_calc.AxisY = Report.ReportCore.AxisAxisCalcImportRow;
            sheet_calc.AxisYIndex = 1;
            sheet_calc.OffsetCol = 0;
            sheet_calc.OffsetRow = 7;
            sheet_calc.Code = "Расчеты";
            sheet_calc.Render();
            _Sheets.Add(sheet_calc);

            Report.ReportCore.IsCalcDisabled = false;
            Report.ReportCore.IsRefreshDisabled = false;
            //var dps = Report.ReportCore.DataPoints.Where(
            //    x =>
            //    x.CategoryValue.DimStandart != null &&
            //    x.CategoryValue.DimStandart.ValueType == ReportOrderPlanStandartType.STD_STAFF_INSURANCE &&
            //    x.CategoryValue.DimPeriod != null &&
            //    x.CategoryValue.DimPeriod?.DateBegin == new DateTime(2018, 09, 01)).ToList();
            //for (int i = 0; i < dps.Count; i++) {
            //    for (int j = 0; j < dps.Count; j++) {
            //        System.Console.WriteLine($"Compare ({i} == {j}) = {dps[i].CategoryValue.Equals(dps[j].CategoryValue)}");
            //    }
            //}

            //foreach (var dp in dps) {

            //}
        }
    }

    public partial class ReportOrderPlanFormImportSheetCfCellData {


    }

    public partial class ReportOrderPlanFormImportSheetCfCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetCfCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_ALL_CODE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_GOOD_CODE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_OTHER_ALL_CODE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_OTHER_INSURANCE_CODE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_OTHER_OTHER_CODE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_PARTY_CODE:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_EXPENSE_STAFF:
                    case ReportOrderPlanAxisCfArticleImport.OrdinateType.CF_ARTICLE_INCOME:
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

    public partial class ReportOrderPlanFormImportSheetIeCellData {


    }

    public partial class ReportOrderPlanFormImportSheetIeCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? "Empty";
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetIeCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_ALL_CODE:
                        return "1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_ALL_NAME:
                        return "Выручка";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_SALE_COUNT_CODE:
                        return "1.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_SALE_COUNT_NAME:
                        return "Выручка (штуки)";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_SALE_VAL_CODE:
                        return "1.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_SALE_VAL_NAME:
                        return "Выручка (валюта)";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_SALE_RUB_CODE:
                        return "1.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_INCOME_SALE_RUB_NAME:
                        return "Выручка (руб)";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_ALL_CODE:
                        return "2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_ALL_NAME:
                        return "'Затраты";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_GOOD_CODE:
                        return "2.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_GOOD_NAME:
                        return "Прямые затраты на материалы и ПКИ";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_CODE:
                        return "2.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_NAME:
                        return "Себестоимость собственных работ";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_CODE:
                        return "2.2.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_NAME:
                        return "- Затраты ЦКБМ";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_FOT_CODE:
                        return "2.2.1.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_FOT_NAME:
                        return "-- ФОТ";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_ZP_CODE:
                        return "2.2.1.1.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_ZP_NAME:
                        return "--- Заработная плата";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_CF_CODE:
                        return "2.2.1.1.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_CF_NAME:
                        return "--- Отчисления в ЦФ";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_COUNT_CODE:
                        return "2.2.1.1.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_COUNT_NAME:
                        return "--- Трудоемкость в н/ч";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_INSURE_CODE:
                        return "2.2.1.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_INSURE_NAME:
                        return "-- Страховые взносы";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_OVER_CODE:
                        return "2.2.1.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_KB_OVER_NAME:
                        return "-- Накладные расходы";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_CODE:
                        return "2.2.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_FOT_CODE:
                        return "2.2.2.1.";
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_ZP_CODE:
                    //return "";
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_CF_CODE:
                    //return "";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_COUNT_CODE:
                        return "2.2.2.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_INSURE_CODE:
                        return "2.2.2.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_OVER_CODE:
                        return "2.2.2.4.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_NAME:
                        return "- Затраты КБ 'Орион'";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_FOT_NAME:
                        return "-- ФОТ";
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_ZP_NAME:
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_CF_NAME:
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_COUNT_NAME:
                        return "--- Трудоемкость в н/ч";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_INSURE_NAME:
                        return "-- Страховые взносы";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_ORION_OVER_NAME:
                        return "-- Накладные расходы";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_CODE:
                        return "2.2.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_NAME:
                        return "- Затраты ОЗМ";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_FOT_CODE:
                        return "2.2.3.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_FOT_NAME:
                        return "-- ФОТ";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_ZP_CODE:
                        return "2.2.3.1.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_ZP_NAME:
                        return "--- Заработная плата";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_CF_CODE:
                        return "2.2.3.1.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_CF_NAME:
                        return "--- Отчисления в ЦФ";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_TH_CODE:
                        return "2.2.3.1.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_TH_NAME:
                        return "--- Трудоемкость в н/ч";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_INSURE_CODE:
                        return "2.2.3.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_INSURE_NAME:
                        return "-- Страховые взносы";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_OVER_CODE:
                        return "2.2.3.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_OZM_OVER_NAME:
                        return "-- Накладные расходы";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_CODE:
                        return "2.2.4.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_NAME:
                        return "- Затраты по договорам подряда";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_FOT_CODE:
                        return "2.2.4.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_FOT_NAME:
                        return "-- ФОТ";
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_ZP_CODE:
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_ZP_NAME:
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF__LC_CF_CODE:
                    //case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_CF_NAME:
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_COUNT_CODE:
                        return "2.2.4.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_COUNT_NAME:
                        return "--- Трудоемкость в н/ч";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_INSURE_CODE:
                        return "2.2.4.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_INSURE_NAME:
                        return "-- Страховые взносы";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_OVER_CODE:
                        return "2.2.4.4.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_STAFF_LC_OVER_NAME:
                        return "-- Накладные расходы";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_PARTY_CODE:
                        return "2.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_PARTY_NAME:
                        return "Затраты соисполнителей";

                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER:
                        break;
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_ALL_CODE:
                        return "2.4.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_ALL_NAME:
                        return "Прочие непроизводственные затраты";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_TRIP_CODE:
                        return "2.4.1.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_TRIP_NAME:
                        return "- Затраты на командировки";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_LIC_CODE:
                        return "2.4.2.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_LIC_NAME:
                        return "- Оформление лицензий";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_PASP_CODE:
                        return "2.4.3.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_PASP_NAME:
                        return "- Оформление паспорт сделки";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_TRANSP_CODE:
                        return "2.4.4.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_TRANSP_NAME:
                        return "- Затраты по транспортировке продукции";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_CUSTOM_CODE:
                        return "2.4.5.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_CUSTOM_NAME:
                        return "- Таможенное оформление груза";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_INSURANCE_CODE:
                        return "2.4.6.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_INSURANCE_NAME:
                        return "- Страхование груза";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_MORF_CODE:
                        return "2.4.7.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_MORF_NAME:
                        return "- Услуги ВП МО РФ по контролю качества и приемки продукции (военно-техническое сопровождение)";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_FAPRID_CODE:
                        return "2.4.8.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_FAPRID_NAME:
                        return "- Отчисления за использование прав РФ на результаты интеллектуальной деятельности (ФАПРИД)";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_GUARANTEE_CODE:
                        return "2.4.9.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_GUARANTEE_NAME:
                        return "- Резерв на гарантийный ремонт и гарантийное обслуживание";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_COMMERC_CODE:
                        return "2.4.10.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_COMMERC_NAME:
                        return "- Коммерческие расходы";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_BANK_CODE:
                        return "2.4.11.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_BANK_NAME:
                        return "- Расходы на банковские гарантии";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_OTHER_CODE:
                        return "2.4.12.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_OTHER_OTHER_NAME:
                        return "- Прочие";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_ITOG_CODE:
                        return "2.5.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_ITOG_NAME:
                        return "Совокупные затраты";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_ITOG_FULL_CODE:
                        return "2.6.";
                    case ReportOrderPlanAxisIeArticleImport.OrdinateType.IE_ARTICLE_EXPENSE_ITOG_FULL_NAME:
                        return "Совокупные затраты нарастающим итогом";

                    //return Ordinate.CategoryValue.DimArticle.Code;
                    default:
                        break;
                }
                return String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetDealCellData {


    }

    public partial class ReportOrderPlanFormImportSheetDealCellOrdinateX {

        public override object Value {
            get {
                //                return $"Level{Ordinate.Level.Index}({Ordinate.LevelIndex},{Ordinate.LevelLength})";
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }
    public partial class ReportOrderPlanFormImportSheetDealCellOrdinateY {

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
                switch (Ordinate.OrdinateType) {
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_PARTY:
                        if (Ordinate.CategoryValue.DimFindeal != null && value is System.String) {
                            Ordinate.CategoryValue.DimFindeal.PartyName = (String)value;
                        }
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_NUMBER:
                        if (Ordinate.CategoryValue.DimFindeal != null && value is System.String) {
                            Ordinate.CategoryValue.DimFindeal.DealNumber = (String)value;
                        }
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_DATE:
                        if (Ordinate.CategoryValue.DimFindeal != null && value is System.DateTime) {
                            Ordinate.CategoryValue.DimFindeal.DealDate = (DateTime)value;
                        }
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_VALUTA:
                        if (Ordinate.CategoryValue.DimFindeal != null && value is System.String) {
                            Ordinate.CategoryValue.DimFindeal.ValutaCode = (String)value;
                        }
                        break;
                    case ReportOrderPlanAxisFindeal.OrdinateType.FINDEAL_BAY_VATMODE:
                        if (Ordinate.CategoryValue.DimFindeal != null && value is System.String) {
                            Ordinate.CategoryValue.DimFindeal.VatModeCode = (String)value;
                        }
                        break;
                }
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetGoodsCellData {


    }

    public partial class ReportOrderPlanFormImportSheetGoodsCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetGoodsCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    default:
                        return Ordinate.OrdinateType.ToString();
                }
 //               return String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetStandartsCellData {


    }

    public partial class ReportOrderPlanFormImportSheetStandartsCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetStandartsCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    default:
                        return Ordinate.OrdinateType.ToString();
                }
                //               return String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetCalcCellData {

        public override string Comment {
            get { return this.TableCell.DataPoint.CategoryValue.ToString(); }
        }

    }

    public partial class ReportOrderPlanFormImportSheetCalcCellOrdinateX {

        public override object Value {
            get {
                return Ordinate.CategoryValue.DimPeriod?.Code ?? String.Empty;
            }
            set {
            }
        }

    }

    public partial class ReportOrderPlanFormImportSheetCalcCellOrdinateY {

        public override object Value {
            get {
                switch (Ordinate.OrdinateType) {
                    //case ReportOrderPlanAxisCalcImportRow.OrdinateType.CCIR_SALEBAY_IE_ENTITY_FINDEAL_ALL_VALREP:
                    //case ReportOrderPlanAxisCalcImportRow.OrdinateType.CCIR_SALEBAY_IE_ENTITY_FINDEAL_COST_VALREP:
                    //case ReportOrderPlanAxisCalcImportRow.OrdinateType.CCIR_SALEBAY_IE_ENTITY_FINDEAL_VAT_VALOBL:
                    //case ReportOrderPlanAxisCalcImportRow.OrdinateType.CCIR_SALEBAY_IE_ENTITY_FINDEAL_VAT_VALREP:
                    //    var cv = Ordinate.CategoryValue;
                    //    return $"FD({cv.DimPartyKind?.Code},{cv.DimParty?.NameShort},{cv.DimDealKind.Code},{cv.DimDeal.Number},{cv.DimValutaObligation},{cv.DimVatModeObligation},{cv.DimArticleObligation.Code})";
                    default:
                        return Ordinate.OrdinateType.ToString();
                }
                //               return String.Empty;
            }
            set {
            }
        }

    }
}
