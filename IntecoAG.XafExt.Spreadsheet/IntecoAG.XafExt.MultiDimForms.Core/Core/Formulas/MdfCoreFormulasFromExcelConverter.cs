using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Spreadsheet.Formulas;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core.Formulas {

    public class MdfCoreFormulasFromExcelConverter : ExpressionVisitor {

        private readonly MdfTemplateFormExcel _Book;
        public MdfTemplateFormExcel Book {
            get { return _Book; }
        }

        private readonly MdfTemplateFormExcelSheet _Sheet;
        public MdfTemplateFormExcelSheet Sheet {
            get { return _Sheet; }
        }

        private readonly MdfTemplateFormExcelSheetCell _SheetCell;
        public MdfTemplateFormExcelSheetCell SheetCell {
            get { return _SheetCell; }
        }

        private readonly MdfCoreTable _Table;
        public MdfCoreTable Table {
            get { return _Table; }
        }

        private readonly MdfCoreTableCell _TableCell;
        public MdfCoreTableCell TableCell {
            get { return _TableCell; }
        }

        private readonly MdfCoreDataPointCalc _Calc;
        public MdfCoreDataPointCalc Calc {
            get { return _Calc; }
        }

        private readonly List<MdfCoreDataPointCalcLink> OldLinks;

        public override void Visit(CellErrorReferenceExpression expression) {
            base.Visit(expression);
        }
        //
        public override void Visit(CellReferenceExpression expression) {
            base.Visit(expression);
            //var ref_sheet_cell = Sheet.Rows[expression.CellArea.TopRowIndex][expression.CellArea.LeftColumnIndex];
            //var ref_table_cell = ref_sheet_cell.TableCell;
            //if (ref_table_cell != null && ref_table_cell.DataPoint != null) {
            //    var calc_link = Calc.CalcLinks.FirstOrDefault(x => x.TableCell == ref_table_cell);
            //    if (calc_link == null) {
            //        calc_link = new MdfCoreDataPointCalcLink(Calc.Session);
            //        Calc.CalcLinks.Add(calc_link);
            //        calc_link.TableCell = ref_table_cell;
            //        calc_link.Index = Calc.CalcLinks.Select(x => x.Index).Max() + 1;
            //    }
            //    else {
            //        OldLinks.Remove(calc_link);
            //    }
            //    calc_link.Formula = expression.CellArea.ToString();
            //    calc_link.DataPoint = ref_table_cell.DataPoint;
            //    calc_link.UpdateFields();
            //}
        }
        //
        public override void Visit(ConstantExpression expression) {
            base.Visit(expression);
        }
        //
        public override void Visit(FunctionExternalExpression expression) {
            base.Visit(expression);
        }
        //
        public override void Visit(UnknownFunctionExpression expression) {
            base.Visit(expression);
        }
        //
        public override void Visit(FunctionExpression expression) {
            base.Visit(expression);
        }
        //
        public override void Visit(RangeExpression expression) {
            base.Visit(expression);
        }
        //
        public override void VisitBinary(BinaryOperatorExpression expression) {
 //           base.VisitBinary(expression);
            System.Console.WriteLine("(");
            expression.LeftExpression.Visit(this);
            System.Console.WriteLine(expression.OperatorText);
            expression.RightExpression.Visit(this);
            System.Console.WriteLine(")");
        }
        //
        public override void VisitFunction(FunctionExpressionBase expression) {
            System.Console.WriteLine("(");
            foreach (var exp in expression.InnerExpressions) {
                exp.Visit(this);
            }
            System.Console.WriteLine(")");
//            base.VisitFunction(expression);
        }
        //
        public override void VisitUnary(UnaryOperatorExpression expression) {
//            base.VisitUnary(expression);
            System.Console.WriteLine(expression.OperatorText);
            expression.InnerExpression.Visit(this);
        }

        public MdfCoreFormulasFromExcelConverter(MdfTemplateFormExcelSheetCell cell, MdfCoreDataPointCalc calc) {
            _Book = cell.Sheet.TemplateFormExcel;
            _Sheet = cell.Sheet;
            _SheetCell = cell;
            _Table = calc.Table;
            _TableCell = calc.TableCell;
            _Calc = calc;
            OldLinks = new List<MdfCoreDataPointCalcLink>(calc.CalcLinks);
        }

        public void Clean() {
            Calc.Session.Delete(OldLinks);
        }

    }

}
