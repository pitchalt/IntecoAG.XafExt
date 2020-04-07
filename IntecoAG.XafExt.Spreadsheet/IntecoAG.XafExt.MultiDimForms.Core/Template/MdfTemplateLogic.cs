using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public static class MdfTemplateLogic {

        public static void Render(this MdfCoreTemplate _this, IObjectSpace os) {
            foreach (var axis in _this.Container.Axiss) {
                axis.Render(os);
            }
            foreach (var table in _this.Container.Tables) {
                table.RenderCells(os);
            }
            foreach (var form in _this.Forms) {
                form.Render();
            }
        }

        public static void CodeGenerate(this MdfCoreTemplate _this, IObjectSpace os) {
            FileStream stream = new FileStream($@"{_this.CodeFilePath}{MakeCamelName(_this.Container.Code)}.cs", FileMode.OpenOrCreate);
            stream.SetLength(0);
            TextWriter text_writer = new StreamWriter(stream);
            IndentedTextWriter indented_text_writer = new IndentedTextWriter(text_writer);
            CodeGenerate(indented_text_writer, _this);
            indented_text_writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            _this.SourceCode = (new StreamReader(stream)).ReadToEnd();
            stream.Close();
        }

        private static void CodeGenerate(IndentedTextWriter writer, MdfCoreTemplate tmpl) {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.ComponentModel;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("//");
            writer.WriteLine("using DevExpress.ExpressApp;");
            writer.WriteLine("using DC = DevExpress.ExpressApp.DC;");
            writer.WriteLine("using DevExpress.Data.Filtering;");
            writer.WriteLine("using DevExpress.Persistent.Base;");
            writer.WriteLine("using DevExpress.ExpressApp.Model;");
            writer.WriteLine("using DevExpress.Persistent.BaseImpl;");
            writer.WriteLine("using DevExpress.Persistent.Validation;");
            writer.WriteLine("//");
            writer.WriteLine("using DevExpress.Xpo;");
            writer.WriteLine("using IntecoAG.XpoExt;");
            writer.WriteLine("using IntecoAG.XafExt.Spreadsheet.MultiDimForms;");
            writer.WriteLine();
            writer.WriteLine($@"namespace {tmpl.PersistentNamespace}.{MakeCamelName(tmpl.Container.Code)} {{");
            writer.Indent++;
            writer.WriteLine();
            foreach (MdfCoreDomain domain in tmpl.Container.Domains) {
                CodeGenerateDomain(writer, tmpl, domain);
            }
            CodeGenerateCalcVariant(writer, tmpl);
            CodeGenerateReportCore(writer, tmpl);
            CodeGenerateTable(writer, tmpl);
            CodeGenerateCategoryValue(writer, tmpl);
            CodeGenerateCategoryValues(writer, tmpl);
            CodeGenerateDataPoint(writer, tmpl);
            CodeGenerateDataPointCalc(writer, tmpl);
            foreach (MdfCoreAxis axis in tmpl.Container.Axiss) {
                CodeGenerateAxis(writer, tmpl, axis);
            }
            foreach (MdfTemplateTable table in tmpl.Tables.Where(x => x.Table.TableType == MdfCoreTable.MdfCoreTableType.TABLE_PERSISTENT)) {
                CodeGeneratePersistentTable(writer, tmpl, table);
            }
            writer.WriteLine();
            writer.WriteLine("[MapInheritance(MapInheritanceType.OwnTable)]");
            writer.WriteLine($@"[Persistent(""{tmpl.PersistentTablePrefix}{MakeCamelName(tmpl.Container.Code)}Form"")]");
            writer.WriteLine($@"public abstract partial class {MakeCamelName(tmpl.Container.Code)}Form: MdfReportFormExcel {{");
            writer.WriteLine();
            writer.Indent++;
            writer.WriteLine($@"private {MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} _Report;");
            writer.WriteLine("[Association]");
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} Report {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _Report; }}");
            writer.WriteLine($@"set {{ SetPropertyValue(ref _Report, value); }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public override Decimal Scale {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return Report.Scale; }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"private {MakeCamelName(tmpl.Container.Code)}CalcVariant _CalcVariant;");
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}CalcVariant CalcVariant {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _CalcVariant; }}");
            writer.WriteLine($@"set {{ SetPropertyValue(ref _CalcVariant, value); }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"protected {MakeCamelName(tmpl.Container.Code)}Form(Session session): base(session) {{ }}");
            writer.Indent--;
            writer.WriteLine();
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public abstract class {MakeCamelName(tmpl.Container.Code)}FormSheet<Tax, Tox, Tay, Toy, Tst, Tsc> : MdfReportFormExcelSheet<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint, Tax, Tox, Tay, Toy, Tsc>");
            writer.Indent++;
            writer.WriteLine($@"where Tax : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>");
            writer.WriteLine($@"where Tox : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>.MdfAxisOrdinate");
            writer.WriteLine($@"where Tay : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>");
            writer.WriteLine($@"where Toy : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>.MdfAxisOrdinate");
            writer.WriteLine($@"where Tst : {MakeCamelName(tmpl.Container.Code)}FormSheet<Tax, Tox, Tay, Toy, Tst, Tsc>");
            writer.WriteLine($@"where Tsc : {MakeCamelName(tmpl.Container.Code)}FormSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> {{");
            writer.WriteLine();
            writer.WriteLine($"public new {MakeCamelName(tmpl.Container.Code)}Form Form {{");
            writer.Indent++;
            writer.WriteLine($"get {{ return ({MakeCamelName(tmpl.Container.Code)}Form)base.Form; }}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}FormSheet({MakeCamelName(tmpl.Container.Code)}Table table, {MakeCamelName(tmpl.Container.Code)}Form form, Int32 index) : base(form, index) {{");
            writer.Indent++;
            writer.WriteLine($@"Table = table;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.Indent--;
            writer.WriteLine();
            writer.WriteLine($@"}}");
            //
            writer.WriteLine();
            writer.WriteLine($@"public abstract class {MakeCamelName(tmpl.Container.Code)}FormSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> : MdfReportFormExcelSheetCell<{MakeCamelName(tmpl.Container.Code)}Report, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>");
            writer.Indent++;
            writer.WriteLine($@"where Tax : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>");
            writer.WriteLine($@"where Tox : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>.MdfAxisOrdinate");
            writer.WriteLine($@"where Tay : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>");
            writer.WriteLine($@"where Toy : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}Table, {MakeCamelName(tmpl.Container.Code)}TableCell, {MakeCamelName(tmpl.Container.Code)}DataPoint>.MdfAxisOrdinate");
            writer.WriteLine($@"where Tst : {MakeCamelName(tmpl.Container.Code)}FormSheet<Tax, Tox, Tay, Toy, Tst, Tsc>");
            writer.WriteLine($@"where Tsc : {MakeCamelName(tmpl.Container.Code)}FormSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> {{");
            //writer.WriteLine($@"where Tax : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}TableCell>");
            //writer.WriteLine($@"where Tox : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}TableCell>.MdfAxisOrdinate");
            //writer.WriteLine($@"where Tay : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}TableCell>");
            //writer.WriteLine($@"where Toy : MdfAxis<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}, {MakeCamelName(tmpl.Container.Code)}CategoryValue, {MakeCamelName(tmpl.Container.Code)}TableCell>.MdfAxisOrdinate");
            //writer.WriteLine($@"where Tst : {MakeCamelName(tmpl.Container.Code)}FormSheet<Tax, Tox, Tay, Toy, Tst, Tsc>");
            //writer.WriteLine($@"where Tsc : {MakeCamelName(tmpl.Container.Code)}FormSheetCell<Tax, Tox, Tay, Toy, Tst, Tsc> {{");
            writer.WriteLine();
            writer.WriteLine($@"private Tst sheet;");
            writer.WriteLine($@"private MdfReportFormExcelSheetColumn column;");
            writer.WriteLine($@"private MdfReportFormExcelSheetRow row;");
            writer.WriteLine();
            writer.WriteLine($@"protected {MakeCamelName(tmpl.Container.Code)}FormSheetCell(Tst sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row) : base(sheet, column, row) {{");
            writer.Indent++;
            writer.WriteLine($@"this.sheet = sheet;");
            writer.WriteLine($@"this.column = column;");
            writer.WriteLine($@"this.row = row;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.Indent--;
            writer.WriteLine($@"}}");

            foreach (MdfTemplateFormExcel form in tmpl.Forms) {
                if (form == null) continue;
                CodeGenerateFormExcel(writer, tmpl, form);
            }
            writer.Indent--;
            writer.WriteLine("}");

        }

        private static void CodeGenerateFormExcel(IndentedTextWriter writer, MdfCoreTemplate tmpl, MdfTemplateFormExcel form) {
            writer.WriteLine();
            writer.WriteLine($@"[Persistent(""{tmpl.PersistentTablePrefix}{MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code)}"")]");
            writer.WriteLine($@"public partial class {MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code)}: {MakeCamelName(tmpl.Container.Code)}Form {{");
            writer.Indent++;
            foreach (var sheet in form.Sheets.OrderBy(x => x.Index)) {
                writer.WriteLine();
                writer.WriteLine($"private {MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code + "_SHEET_" + sheet.Code)} _{MakeCamelName(sheet.Code)};");
                writer.WriteLine($"public {MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code + "_SHEET_" + sheet.Code)} {MakeCamelName(sheet.Code)} {{ get {{ return _{MakeCamelName(sheet.Code)}; }} }}");
            }
            writer.WriteLine();
            writer.WriteLine("protected override void RenderCore() {");
            writer.Indent++;
            writer.WriteLine();
            foreach (var table in tmpl.Tables.Where( x => x.Table.TableType == MdfCoreTable.MdfCoreTableType.TABLE_PERSISTENT)) {
                writer.WriteLine($"Report.ReportCore.Table{MakeCamelName(table.Table.Code)}.Render(Report, CalcVariant);");
            }
            foreach (var table in tmpl.Tables.Where( x => x.Table.TableType == MdfCoreTable.MdfCoreTableType.TABLE_CONSTRAINT)) {
                writer.WriteLine($"if (CalcVariant == {MakeCamelName(tmpl.Container.Code)}CalcVariant.{table.Table.CalcVariant.Code}) Report.ReportCore.Table{MakeCamelName(table.Table.Code)}.Render(Report, CalcVariant);");
            }
            foreach (var table in form.Sheets.Select(x => x.TemplateTable)) {
                writer.WriteLine($"Report.ReportCore.Table{MakeCamelName(table.Table.Code)}.Render(Report, CalcVariant);");
            }
            writer.WriteLine();
            writer.WriteLine("Int32 index = 0;");
            writer.WriteLine("_Sheets.Clear();");
            foreach (var sheet in form.Sheets.OrderBy(x => x.Index)) {
                writer.WriteLine();
                writer.WriteLine($"_{MakeCamelName(sheet.Code)} = new {MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code + "_SHEET_" + sheet.Code)}(this, Report.ReportCore.Table{MakeCamelName(sheet.TemplateTable.Table.Code)}, index++);");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.AxisX = Report.ReportCore.Axis{MakeCamelName(sheet.AxisX.Axis.Code)};");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.AxisXIndex = {sheet.AxisX.AxisIndex};");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.AxisY = Report.ReportCore.Axis{MakeCamelName(sheet.AxisY.Axis.Code)};");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.AxisYIndex = {sheet.AxisY.AxisIndex};");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.OffsetCol = {sheet.OffsetCol};");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.OffsetRow = {sheet.OffsetRow};");
                writer.WriteLine($@"{MakeCamelName(sheet.Code)}.Code = ""{sheet.NameShort}"";");
                writer.WriteLine($"{MakeCamelName(sheet.Code)}.Render();");
                writer.WriteLine($"_Sheets.Add({MakeCamelName(sheet.Code)});");
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code)}(Session session): base(session) {{ }}");
            writer.Indent--;
            writer.WriteLine();
            writer.WriteLine($@"}}");
            foreach (var sheet in form.Sheets) {
                CodeGenerateFormExcelSheet(writer, tmpl, form, sheet);
            }
        }

        private static void CodeGenerateFormExcelSheet(IndentedTextWriter writer, MdfCoreTemplate tmpl, MdfTemplateFormExcel form, MdfTemplateFormExcelSheet sheet) {
            String form_name = MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code);
            String sheet_name = MakeCamelName(tmpl.Container.Code + "_FORM_" + form.Code + "_SHEET_" + sheet.Code);
            String axis_x_name = MakeCamelName(tmpl.Container.Code + "_" + sheet.AxisX.Axis.Code);
            String axis_y_name = MakeCamelName(tmpl.Container.Code + "_" + sheet.AxisY.Axis.Code);
            String cont_name = MakeCamelName(tmpl.Container.Code);
            writer.WriteLine();
            writer.WriteLine($@"public partial class {sheet_name} : {cont_name}FormSheet <{axis_x_name}, {axis_x_name}.Ordinate, {axis_y_name}, {axis_y_name}.Ordinate, {sheet_name}, {sheet_name}Cell> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($"public new {form_name} Form {{");
            writer.Indent++;
            writer.WriteLine($"get {{ return ({form_name})base.Form; }}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {sheet_name}({form_name} form, {cont_name}Table table, Int32 index) : base(table, form,  index) {{");
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($"protected override {sheet_name}Cell ReportCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column) {{");
            writer.Indent++;
            writer.WriteLine($"return new {sheet_name}CellReport(this, column, row);");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"protected override {sheet_name}Cell OrdinateXCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, {axis_x_name}.Ordinate ordinate) {{");
            writer.Indent++;
            writer.WriteLine($@"return new {sheet_name}CellOrdinateX(this, column, row, ordinate);");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"protected override {sheet_name}Cell OrdinateYCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, {axis_y_name}.Ordinate ordinate) {{");
            writer.Indent++;
            writer.WriteLine($@"return new {sheet_name}CellOrdinateY(this, column, row, ordinate);");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"protected override {sheet_name}Cell TableCellCreate(MdfReportFormExcelSheetRow row, MdfReportFormExcelSheetColumn column, ReportOrderPlanTableCell table_cell) {{");
            writer.Indent++;
            writer.WriteLine($@"return new {sheet_name}CellData(this, column, row, table_cell);");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public abstract class {sheet_name}Cell : {cont_name}FormSheetCell<{axis_x_name}, {axis_x_name}.Ordinate, {axis_y_name}, {axis_y_name}.Ordinate, {sheet_name}, {sheet_name}Cell> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($"public new {sheet_name} Sheet {{");
            writer.Indent++;
            writer.WriteLine($"get {{ return ({sheet_name})base.Sheet; }}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {sheet_name}Cell({sheet_name} sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row) : base(sheet, column, row) {{");
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
            //
            writer.WriteLine();
            writer.WriteLine($@"public partial class {sheet_name}CellReport : {sheet_name}Cell {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine("public override object Value {");
            writer.Indent++;
            writer.WriteLine("get { return null; }");
            writer.WriteLine("set { }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public override Object CellData {");
            writer.Indent++;
            writer.WriteLine("get { return Sheet.Form.Report; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public override bool IsEditable {");
            writer.Indent++;
            writer.WriteLine("get { return false; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {sheet_name}CellReport({sheet_name} sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row) : base(sheet, column, row) {{ }}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
//
            writer.WriteLine();
            writer.WriteLine($@"public partial class {sheet_name}CellData : {sheet_name}Cell {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"private readonly {cont_name}TableCell _TableCell;");
            writer.WriteLine($@"public {cont_name}TableCell TableCell {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _TableCell; }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine("public override MdfDataType DataType {");
            writer.Indent++;
            writer.WriteLine("get { return TableCell.DataPoint?.DataType ?? MdfDataType.DT_UNDEFINED; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public override Boolean IsScale {");
            writer.Indent++;
            writer.WriteLine("get { return TableCell.DataPoint?.IsScale ?? false; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public override object Value {");
            writer.Indent++;
            writer.WriteLine("get { return (DataType == MdfDataType.DT_DECIMAL || DataType == MdfDataType.DT_MONETARY) && IsScale ? TableCell.DataPoint.ValueDecimal / Scale : TableCell.DataPoint.Value; }");
            writer.WriteLine("set {");
            writer.Indent++;
            writer.WriteLine("if (TableCell.DataPoint != null) {");
            writer.Indent++;
            writer.WriteLine("if (DataType == MdfDataType.DT_DECIMAL || DataType == MdfDataType.DT_MONETARY) {");
            writer.Indent++;
            writer.WriteLine("switch (value) {");
//            writer.Indent++;
            writer.WriteLine("case Decimal value_decimal:");
            writer.Indent++;
            writer.WriteLine("TableCell.DataPoint.ValueSet(IsScale ? value_decimal * Scale : value_decimal );");
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("case Double value_double:");
            writer.Indent++;
            writer.WriteLine("TableCell.DataPoint.ValueSet(IsScale ? ((Decimal) value_double) * Scale : ((Decimal) value_double));");
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("case Single value_single:");
            writer.Indent++;
            writer.WriteLine("TableCell.DataPoint.ValueSet(IsScale ? ((Decimal) value_single) * Scale : ((Decimal) value_single));");
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("case Int32 value_int32:");
            writer.Indent++;
            writer.WriteLine("TableCell.DataPoint.ValueSet(IsScale ? ((Decimal) value_int32) * Scale : ((Decimal) value_int32));");
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("case Int64 value_int64:");
            writer.Indent++;
            writer.WriteLine("TableCell.DataPoint.ValueSet(IsScale ? ((Decimal) value_int64) * Scale : ((Decimal) value_int64));");
            writer.WriteLine("break;");
            writer.Indent--;
            writer.WriteLine("default:");
            writer.Indent++;
            writer.WriteLine("break;");
            writer.WriteLine("throw new InvalidCastException(\"Invalid value type: \" + value.GetType());");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine("else {");
            writer.Indent++;
            writer.WriteLine("TableCell.DataPoint.Value = value;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public override Object CellData {");
            writer.Indent++;
            writer.WriteLine("get { return TableCell?.DataPoint; }");
            writer.Indent--;
            writer.WriteLine("}");
//            writer.WriteLine();
//            writer.WriteLine("public override bool IsEditable {");
//            writer.Indent++;
//            writer.WriteLine("get { return TableCell.DataPoint.Calc == null; }");
//            writer.Indent--;
//            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {sheet_name}CellData({sheet_name} sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row, {cont_name}TableCell table_cell) : base(sheet, column, row) {{");
            writer.Indent++;
            writer.WriteLine("_TableCell = table_cell;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public partial class {sheet_name}CellOrdinateX : {sheet_name}Cell {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"private readonly {axis_x_name}.Ordinate _Ordinate;");
            writer.WriteLine($@"public {axis_x_name}.Ordinate Ordinate {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _Ordinate; }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine("public override Object CellData {");
            writer.Indent++;
            writer.WriteLine("get { return Ordinate?.CategoryValue; }");
            writer.Indent--;
            writer.WriteLine("}");
            //            writer.WriteLine();
            //            writer.WriteLine($@"public override object Value => throw new NotImplementedException();");
            writer.WriteLine();
            writer.WriteLine($@"public {sheet_name}CellOrdinateX({sheet_name} sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row, {axis_x_name}.Ordinate ordinate) : base(sheet, column, row) {{");
            writer.Indent++;
            writer.WriteLine($@"_Ordinate = ordinate;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public partial class {sheet_name}CellOrdinateY : {sheet_name}Cell {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"private readonly {axis_y_name}.Ordinate _Ordinate;");
            writer.WriteLine($@"public {axis_y_name}.Ordinate Ordinate {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _Ordinate; }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine("public override Object CellData {");
            writer.Indent++;
            writer.WriteLine("get { return Ordinate?.CategoryValue; }");
            writer.Indent--;
            writer.WriteLine("}");
            //            writer.WriteLine();
            //            writer.WriteLine($@"public override object Value => throw new NotImplementedException();");
            writer.WriteLine();
            writer.WriteLine($@"public {sheet_name}CellOrdinateY({sheet_name} sheet, MdfReportFormExcelSheetColumn column, MdfReportFormExcelSheetRow row, {axis_y_name}.Ordinate ordinate) : base(sheet, column, row) {{");
            writer.Indent++;
            writer.WriteLine($@"_Ordinate = ordinate;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
        }


        private static void CodeGenerateAxis(IndentedTextWriter writer, MdfCoreTemplate tmpl, MdfCoreAxis axis) {
            String category_value_name = $@"{ MakeCamelName(axis.Container.Code) }CategoryValue";
            if (String.IsNullOrEmpty(axis.Code))
                return;
            writer.WriteLine();
            writer.WriteLine($@"public partial class {MakeCamelName(axis.Container.Code + "_" + axis.Code)}: MdfAxis<{MakeCamelName(axis.Container.Code + "_" + tmpl.DomainReport.Code)},{category_value_name},{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(axis.Container.Code)}DataPoint> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine("public enum OrdinateType {");
            writer.Indent++;
            foreach (var ord in axis.Ordinates) {
                writer.WriteLine($@"{ord.Code} =  {ord.Oid},");
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public class Ordinate : MdfAxisOrdinate {");
            writer.Indent++;
            writer.WriteLine($@"private OrdinateType _OrdinateType;");
            writer.WriteLine($@"public OrdinateType OrdinateType{{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _OrdinateType; }}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public Ordinate({MakeCamelName(axis.Container.Code + "_" + axis.Code)} axis, MdfAxisOrdinateValueType value_type, OrdinateType ord_type): base(axis, value_type)  {{");
            writer.Indent++;
            writer.WriteLine($@"_OrdinateType = ord_type;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(axis.Container.Code + "_" + axis.Code)}({MakeCamelName(axis.Container.Code + "_" + tmpl.DomainReport.Code)} report): base(report) {{ }}");
            writer.WriteLine();
            writer.WriteLine("public override void OrdinatesFill() {");
            writer.Indent++;
            writer.WriteLine($@"Root = OrdinateCreate(null, OrdinateType.{axis.Root.Code});");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("protected Ordinate OrdinateCreate(Ordinate ord_up, OrdinateType ord_type) {");
            writer.Indent++;
            writer.WriteLine("Ordinate ord;");
            writer.WriteLine("Ordinate sub_ord;");
            writer.WriteLine("Int32 sort_order = 0;");
            writer.WriteLine($@"{category_value_name} ord_up_category_value = ord_up?.CategoryValue ?? {category_value_name}.DefaultCategoryValue;");
            writer.WriteLine("switch (ord_type) {");
            writer.Indent++;
            foreach (var ord in axis.Ordinates) {
                writer.WriteLine($@"case OrdinateType.{ord.Code}:");
                writer.Indent++;
                if (ord.Dimension == null && ord.DimensionMember == null) {
                    writer.WriteLine($@"ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.{ord.Code});");
                    writer.WriteLine("ord.Up = ord_up;");
                    writer.WriteLine("ord.CategoryValue = ord_up_category_value;");
                    writer.WriteLine($@"ord.SortOrder = {ord.SortOrder};");
                    writer.WriteLine($@"ord.IsIntegrated = {(ord.IsIntegrated ? "true" : "false") };");
                    if (!String.IsNullOrWhiteSpace(ord.NameExpression)) {
                        if (ord.NameExpressionType == MdfCoreAxisExpressionType.STRING) {
                            writer.WriteLine($@"ord.Name = @""{ord.NameExpression.Replace("\"","\"\"").Replace("\r\n", @"\r\n")}"";");
                        }
                        else {
                            writer.WriteLine($@"ord.Name = {ord.NameExpression.Replace("#","ord")};");
                        }
                    }
                    foreach (var sub_ord in ord.Downs) {
                        writer.WriteLine($@"OrdinateCreate(ord, OrdinateType.{sub_ord.Code});");
                    }
                }
                else {
                    if (ord.DimensionMember == null) {
                        writer.WriteLine($@"ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.{ord.Code});");
                        writer.WriteLine("ord.Up = ord_up;");
                        writer.WriteLine($@"ord.SortOrder = {ord.SortOrder};");
                        writer.WriteLine($@"ord.IsIntegrated = {(ord.IsIntegrated ? "true" : "false") };");
                        writer.WriteLine($@"ord.CategoryValue = {MakeCamelName(ord.Dimension.Code)}CategoryValueLocate(ord_up_category_value, null);");
                        if (!String.IsNullOrWhiteSpace(ord.NameExpression)) {
                            if (ord.NameExpressionType == MdfCoreAxisExpressionType.STRING) {
                                writer.WriteLine($@"ord.Name = @""{ord.NameExpression.Replace("\"", "\"\"").Replace("\r\n", @"\r\n")}"";");
                            }
                            else {
                                writer.WriteLine($@"ord.Name = {ord.NameExpression.Replace("#", "ord")};");
                            }
                        }
                        foreach (var sub_ord in ord.Downs) {
                            writer.WriteLine($@"OrdinateCreate(ord, OrdinateType.{sub_ord.Code});");
                        }
                    }
                    else {
                        if (ord.CalcType == MdfCoreDomainMemberCalcType.GENERAL || ord.CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                            writer.WriteLine($@"ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.{ord.Code});");
                            writer.WriteLine("ord.Up = ord_up;");
                            writer.WriteLine($@"ord.SortOrder = {ord.SortOrder};");
                            writer.WriteLine($@"ord.IsIntegrated = {(ord.IsIntegrated ? "true" : "false") };");
                            writer.WriteLine($@"ord.CategoryValue = {MakeCamelName(ord.Dimension.Code)}CategoryValueLocate(ord_up_category_value, {MakeCamelName(ord.Dimension.Code)}SingleValueGet(ord_up_category_value, {MakeCamelName(axis.Container.Code + "_" + ord.Dimension.Domain.Code)}Type.{ord.DimensionMember.DomainMember.Code}, OrdinateType.{ord.Code}));");
                            if (!String.IsNullOrWhiteSpace(ord.NameExpression)) {
                                if (ord.NameExpressionType == MdfCoreAxisExpressionType.STRING) {
                                    writer.WriteLine($@"ord.Name = @""{ord.NameExpression.Replace("\"", "\"\"").Replace("\r\n", @"\r\n")}"";");
                                }
                                else {
                                    writer.WriteLine($@"ord.Name = {ord.NameExpression.Replace("#", "ord")};");
                                }
                            }
                            foreach (var sub_ord in ord.Downs) {
                                writer.WriteLine($@"OrdinateCreate(ord, OrdinateType.{sub_ord.Code});");
                            }
                        }
                        if (ord.CalcType == MdfCoreDomainMemberCalcType.QUERY) {
                            writer.WriteLine($@"ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.{ord.Code});");
                            writer.WriteLine("ord.Up = ord_up;");
                            writer.WriteLine("ord.CategoryValue = ord_up_category_value;");
                            writer.WriteLine($@"ord.SortOrder = {ord.SortOrder};");
                            writer.WriteLine("ord.IsIntegrated = true;");
                            writer.WriteLine($@"IList<{MakeCamelName(axis.Container.Code + "_" + ord.Dimension.Domain.Code)}> {ord.Code.ToLower()}_objs = {MakeCamelName(ord.Dimension.Code)}QueryValueGet(ord.CategoryValue, {MakeCamelName(axis.Container.Code + "_" + ord.Dimension.Domain.Code)}Type.{ord.DimensionMember.DomainMember.Code}, OrdinateType.{ord.Code});");
                            writer.WriteLine("sort_order = 0;");
                            writer.WriteLine($@"foreach (var {ord.Code.ToLower()}_obj in {ord.Code.ToLower()}_objs) {{");
                            writer.Indent++;
                            writer.WriteLine($@"sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.{ord.Code});");
                            writer.WriteLine("sub_ord.Up = ord;");
                            writer.WriteLine($@"sub_ord.CategoryValue = {MakeCamelName(ord.Dimension.Code)}CategoryValueLocate(ord.CategoryValue, {ord.Code.ToLower()}_obj);");
                            writer.WriteLine($@"sub_ord.IsIntegrated = {(ord.IsIntegrated ? "true" : "false") };");
                            writer.WriteLine("sub_ord.SortOrder = sort_order++;");
                            if (!String.IsNullOrWhiteSpace(ord.NameExpression)) {
                                if (ord.NameExpressionType == MdfCoreAxisExpressionType.STRING) {
                                    writer.WriteLine($@"sub_ord.Name = @""{ord.NameExpression.Replace("\"", "\"\"").Replace("\r\n", @"\r\n")}"";");
                                }
                                else {
                                    writer.WriteLine($@"sub_ord.Name = {ord.NameExpression.Replace("#", "sub_ord")};");
                                }
                            }
                            foreach (var sub_ord in ord.Downs) {
                                writer.WriteLine($@"OrdinateCreate(sub_ord, OrdinateType.{sub_ord.Code});");
                            }
                            writer.Indent--;
                            writer.WriteLine("}");
                        }
                    }
                }
                //else if (ord.Dimension.Domain.DataType == MdfCoreDataType.DT_OBJECT) {
                //    writer.WriteLine($@"ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.{ord.Code});");
                //    writer.WriteLine("ord.Up = ord_up;");
                //    writer.WriteLine("ord.CategoryValue = ord_up_category_value;");
                //    writer.WriteLine($@"ord.SortOrder = {ord.SortOrder};");
                //    writer.WriteLine("ord.IsIntegrated = true;");
                //    writer.WriteLine($@"IList<{MakeCamelName(axis.Container.Code + "_" + ord.Dimension.Domain.Code)}> {ord.Code.ToLower()}_objs = {MakeCamelName(ord.Dimension.Code)}ObjectsGet(ord.CategoryValue, {MakeCamelName(axis.Container.Code + "_" + ord.Dimension.Domain.Code)}Type.{ord.DimensionMember.DomainMember.Code}, OrdinateType.{ord.Code});");
                //    writer.WriteLine("sort_order = 0;");
                //    writer.WriteLine($@"foreach (var {ord.Code.ToLower()}_obj in {ord.Code.ToLower()}_objs) {{");
                //    writer.Indent++;
                //    writer.WriteLine($@"sub_ord = new Ordinate(this, MdfAxisOrdinateValueType.OBJECT, OrdinateType.{ord.Code});");
                //    writer.WriteLine("sub_ord.Up = ord;");
                //    writer.WriteLine($@"sub_ord.CategoryValue = {MakeCamelName(ord.Dimension.Code)}CategoryValueLocate(ord.CategoryValue, {ord.Code.ToLower()}_obj);");
                //    writer.WriteLine($@"sub_ord.IsIntegrated = {(ord.IsIntegrated ? "true" : "false") };");
                //    writer.WriteLine("sub_ord.SortOrder = sort_order++;");
                //    foreach (var sub_ord in ord.Downs) {
                //        writer.WriteLine($@"OrdinateCreate(sub_ord, OrdinateType.{sub_ord.Code});");
                //    }
                //    writer.Indent--;
                //    writer.WriteLine("}");
                //}
                //else if (ord.Dimension.Domain.DataType == MdfCoreDataType.DT_ENUM) {
                //    writer.WriteLine($@"ord = new Ordinate(this, MdfAxisOrdinateValueType.MODEL, OrdinateType.{ord.Code});");
                //    writer.WriteLine("ord.Up = ord_up;");
                //    writer.WriteLine($@"ord.SortOrder = {ord.SortOrder};");
                //    writer.WriteLine($@"ord.IsIntegrated = {(ord.IsIntegrated ? "true" : "false") };");
                //    writer.WriteLine($@"ord.CategoryValue = {MakeCamelName(ord.Dimension.Code)}CategoryValueLocate(ord_up_category_value, {MakeCamelName(ord.Dimension.Code)}EnumGet(ord_up_category_value, {MakeCamelName(axis.Container.Code + "_" + ord.Dimension.Domain.Code)}Type.{ord.DimensionMember.DomainMember.Code}, OrdinateType.{ord.Code}));");
                //    foreach (var sub_ord in ord.Downs) {
                //        writer.WriteLine($@"OrdinateCreate(ord, OrdinateType.{sub_ord.Code});");
                //    }
                //}
                writer.WriteLine("return ord;");
                writer.Indent--;
                //
            }
            writer.WriteLine("default:");
            writer.Indent++;
            writer.WriteLine("return null;");
            writer.Indent--;
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            foreach (var dim in axis.Ordinates.Where(x => x.Dimension != null).Select(x => x.Dimension).Distinct()) {
                writer.WriteLine($@"protected {category_value_name} {MakeCamelName(dim.Code)}CategoryValueLocate({category_value_name} context, {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)} obj) {{");
                writer.Indent++;
                writer.WriteLine($@"{category_value_name} value = new {category_value_name}(context);");
                writer.WriteLine($@"value.{MakeCamelName(dim.Code)} = obj;");
                foreach (var dim_dep in dim.DimensionDependents.OrderBy(x => x.SortOrder)) {
                    var dim_dep_calc_dim = dim_dep.DimensionMember.DomainMember.CalcDimension;
                    var dim_dep_calc_prop = dim_dep.DimensionMember.DomainMember.CalcProperty;
                    writer.WriteLine($@"value.{MakeCamelName(dim_dep.DimensionDependent.Code)} = value.{MakeCamelName(dim_dep_calc_dim.Code)}?.{MakeCamelName(dim_dep_calc_prop.Code)};");
                }
                writer.WriteLine($@"value = Report.ReportCore.CategoryValues.Locate(value);");
                writer.WriteLine($@"return value;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                //if (dim.Domain.DataType == MdfCoreDataType.DT_OBJECT) {
                //    writer.WriteLine($@"protected IList<{MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}> {MakeCamelName(dim.Code)}ObjectsGet({category_value_name} context, {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}Type model, OrdinateType ord_type) {{");
                //    writer.WriteLine($@"    return new List<{MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}>();");
                //    writer.WriteLine("}");
                //}
                //else 
                //if (dim.Domain.DataType == MdfCoreDataType.DT_ENUM) {
                //    writer.WriteLine($@"protected {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)} {MakeCamelName(dim.Code)}EnumGet({category_value_name} context,  {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}Type model, OrdinateType ord_type) {{");
                //    writer.Indent++;
                //    writer.WriteLine($@"return Report.Enum{MakeCamelName(dim.Domain.Code)}[model];");
                //    writer.Indent--;
                //    writer.WriteLine("}");
                //}
            }
            foreach (var dim in axis.Ordinates.Where(x => x.Dimension != null && (x.CalcType == MdfCoreDomainMemberCalcType.GENERAL || x.CalcType == MdfCoreDomainMemberCalcType.CALCULATED)).Select(x => x.Dimension).Distinct()) {
                writer.WriteLine($@"protected {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)} {MakeCamelName(dim.Code)}SingleValueGet({category_value_name} context,  {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}Type model, OrdinateType ord_type) {{");
                writer.Indent++;
                writer.WriteLine("switch(ord_type) {");
                writer.Indent++;
                foreach (var ord in axis.Ordinates.Where(x => x.Dimension == dim)) {
                    writer.WriteLine($@"case OrdinateType.{ord.Code}:");
                    writer.Indent++;
                    if (ord.CalcType == MdfCoreDomainMemberCalcType.GENERAL) {
                        writer.WriteLine($@"return Report.ReportCore.Const{MakeCamelName(dim.Domain.Code)}[model];");
                    }
                    if (ord.CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                        writer.WriteLine($@"return context.{MakeCamelName(ord.CalcDimension.Code)}?.{MakeCamelName(ord.CalcProperty.Code)};");
                    }
                    writer.Indent--;
                }
                writer.WriteLine("default:");
                writer.WriteLine("    return null;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.Indent--;
                writer.WriteLine("}");
            }
            foreach (var dim in axis.Ordinates.Where(x => x.Dimension != null && x.CalcType == MdfCoreDomainMemberCalcType.QUERY).Select(x => x.Dimension).Distinct()) {
                writer.WriteLine($@"protected IList<{MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}> {MakeCamelName(dim.Code)}QueryValueGet({category_value_name} context, {MakeCamelName(axis.Container.Code + "_" + dim.Domain.Code)}Type model, OrdinateType ord_type) {{");
                writer.WriteLine($@"    return Report.ReportCore.Query{MakeCamelName(dim.Domain.Code)}(context, model);");
                writer.WriteLine("}");
            }
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
        }

        private static void CodeGenerateDomain(IndentedTextWriter writer, MdfCoreTemplate tmpl, MdfCoreDomain domain) {
            if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_NOT_PERSISTENT)
                return;
            writer.WriteLine();
            writer.WriteLine($@"public enum {MakeCamelName(tmpl.Container.Code + "_" + domain.Code)}Type {{");
            writer.Indent++;
            foreach (MdfCoreDomainMember member in domain.Members) {
                writer.WriteLine($@"{member.Code} = {member.Oid},");
            }
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"[Persistent(""{tmpl.PersistentTablePrefix}{MakeCamelName(domain.Container.Code + "_" + domain.Code)}"")]");
            if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_OBJECT ||
                domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_ENUM) {
                //                writer.WriteLine($@"[DefaultProperty(""Code"")]");
                writer.WriteLine($@"public partial class {MakeCamelName(domain.Container.Code + "_" + domain.Code)}: MdfContainerObject {{");
            }
            else if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_CONCEPT) {
                writer.WriteLine($@"public partial class {MakeCamelName(domain.Container.Code + "_" + domain.Code)}: MdfConcept {{");
            }
            else if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_REPORT) {
                writer.WriteLine($@"public partial class {MakeCamelName(domain.Container.Code + "_" + domain.Code)}: MdfReport {{");
            }
            else if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_TEMPLATE) {
                writer.WriteLine($@"public partial class {MakeCamelName(domain.Container.Code + "_" + domain.Code)}: MdfTemplate {{");
            }
            writer.Indent++;
            if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_OBJECT ||
                domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_ENUM ||
                domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_CONCEPT) {
                foreach (var container in domain.DomainContainers) {
                    writer.WriteLine();
                    writer.WriteLine($@"private {MakeCamelName(tmpl.Container.Code + "_" + container.Code)} _{MakeCamelName(container.Code)};");
                    writer.WriteLine($@"[Association]");
                    writer.WriteLine($@"[Browsable(false)]");
                    writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code + "_" + container.Code)} {MakeCamelName(container.Code)} {{");
                    writer.Indent++;
                    writer.WriteLine($@"get {{ return _{MakeCamelName(container.Code)}; }}");
                    writer.WriteLine($@"set {{");
                    writer.Indent++;
                    writer.WriteLine($@"if (SetPropertyValue(ref _{MakeCamelName(container.Code)}, value) && !IsLoading && value != null) {{");
                    writer.Indent++;
                    writer.WriteLine($@"Container = value;");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                }
                writer.WriteLine();
                writer.WriteLine($@"private String _Code;");
                writer.WriteLine($@"[Size(64)]");
                writer.WriteLine($@"[VisibleInListView(true)]");
                writer.WriteLine($@"public String Code {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _Code; }}");
                writer.WriteLine($@"set {{ SetPropertyValue(ref _Code, value); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.WriteLine();
                writer.WriteLine($@"private String _NameShort;");
                writer.WriteLine($@"[ModelDefault(""RowCount"", ""1"")]");
                writer.WriteLine($@"[Size(128)]");
                writer.WriteLine($@"[VisibleInListView(true)]");
                writer.WriteLine($@"public String NameShort {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _NameShort; }}");
                writer.WriteLine($@"set {{ SetPropertyValue(ref _NameShort, value); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
            }
            if (domain is MdfCoreDomainContainer cont) {
                foreach (var cont_domain in cont.ContainedDomains) {
                    writer.WriteLine();
                    writer.WriteLine($@"[Association]");
                    writer.WriteLine($@"[Aggregated]");
                    writer.WriteLine($@"public XPCollection<{CheckDomainTypeName(cont_domain)}> {CheckDomainTypeName(cont_domain)}s {{");
                    writer.Indent++;
                    writer.WriteLine($@"get {{ return GetCollection<{CheckDomainTypeName(cont_domain)}>(); }}");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                }
            }
            writer.WriteLine();
            writer.WriteLine($@"private {MakeCamelName(domain.Container.Code + "_" + domain.Code)}Type _ValueType;");
            writer.WriteLine($@"public {MakeCamelName(domain.Container.Code + "_" + domain.Code)}Type ValueType {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _ValueType; }}");
            writer.WriteLine($@"set {{ SetPropertyValue(ref _ValueType, value); }}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            foreach (var property in domain.Propertys) {
                writer.WriteLine();
                String property_type_name = CheckDomainTypeName(property.PropertyType);
                writer.WriteLine($@"private {property_type_name} _{MakeCamelName(property.Code)};");
                if (property.IsRequired) {
                    if (String.IsNullOrWhiteSpace(property.RequiredCondition)) {
                        writer.WriteLine("[RuleRequiredField]");
                    }
                    else {
                        writer.WriteLine($@"[RuleRequiredField(TargetCriteria = ""{property.RequiredCondition}"")]");
                    }

                }
                if (property.PropertyType.DataType == MdfCoreDataType.DT_OBJECT || property.PropertyType.DataType == MdfCoreDataType.DT_ENUM) {
                    if (property.IsAssociation) {
                        writer.WriteLine("[Association]");
                    }
                    writer.WriteLine($@"[DataSourceCriteriaProperty(""Container.ContainersCritery"")]");
                }
                writer.WriteLine($@"public {property_type_name} {MakeCamelName(property.Code)} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _{MakeCamelName(property.Code)}; }}");
                writer.WriteLine($@"set {{ SetPropertyValue(ref _{MakeCamelName(property.Code)}, value); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
            }
            foreach (var hierarhy in domain.Container.Hierarchys.Where(x => x.Domain == domain)) {
                String property_type_name = CheckDomainTypeName(domain);
                writer.WriteLine();
                writer.WriteLine($@"private {property_type_name} _{MakeCamelName(hierarhy.Code + "_UP")};");
                writer.WriteLine($@"[DataSourceCriteriaProperty(""Container.ContainersCritery"")]");
                writer.WriteLine($@"[Association(""{MakeCamelName(domain.Container.Code)}-{MakeCamelName(hierarhy.Code)}"")]");
                writer.WriteLine($@"public {property_type_name} {MakeCamelName(hierarhy.Code + "_UP")} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _{MakeCamelName(hierarhy.Code + "_UP")}; }}");
                writer.WriteLine($@"set {{ SetPropertyValue<{property_type_name}>(ref _{MakeCamelName(hierarhy.Code + "_UP")}, value); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.WriteLine();
                writer.WriteLine($@"private Int32 _{MakeCamelName(hierarhy.Code + "_ORDER")};");
                writer.WriteLine($@"public Int32 {MakeCamelName(hierarhy.Code + "_ORDER")} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _{MakeCamelName(hierarhy.Code + "_ORDER")}; }}");
                writer.WriteLine($@"set {{ SetPropertyValue<Int32>(ref _{MakeCamelName(hierarhy.Code + "_ORDER")}, value); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.WriteLine();
                writer.WriteLine($@"[Association(""{MakeCamelName(domain.Container.Code)}-{MakeCamelName(hierarhy.Code)}"")]");
                writer.WriteLine($@"public XPCollection<{property_type_name}> {MakeCamelName(hierarhy.Code + "_DOWNS")} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return GetCollection<{property_type_name}>(""{MakeCamelName(hierarhy.Code + "_DOWNS")}""); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
            }
            if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_TEMPLATE) {
                writer.WriteLine();
                writer.WriteLine($@"public void ConstAllGenerate() {{");
                writer.Indent++;
                var template = (MdfCoreDomainContainer)domain;
                var const_domains = new List<MdfCoreDomain>(128);
                foreach (var const_domain in template.ContainedDomains) {
                    var have_general = const_domain.Members.Where(x => x.CalcType == MdfCoreDomainMemberCalcType.GENERAL).Count();
                    if (have_general > 0) {
                        const_domains.Add(const_domain);
                    }
                }
                foreach (var const_domain in const_domains) {
                    writer.WriteLine();
                    foreach (var member in const_domain.Members.Where(x => x.CalcType == MdfCoreDomainMemberCalcType.GENERAL)) {
                        writer.WriteLine($@"ConstOfType{MakeCamelName(domain.Container.Code + "_" + const_domain.Code)}Generate({MakeCamelName(domain.Container.Code + "_" + const_domain.Code)}Type.{member.Code});");
                    }
                }
                writer.WriteLine();
                writer.Indent--;
                writer.WriteLine($@"}}");
                foreach (var const_domain in const_domains) {
                    String const_domain_name = MakeCamelName(domain.Container.Code + "_" + const_domain.Code);
                    writer.WriteLine();
                    writer.WriteLine($@"public void ConstOfType{const_domain_name}Generate({const_domain_name}Type member) {{");
                    writer.Indent++;
                    writer.WriteLine($@"var enum_type = typeof({const_domain_name}Type);");
                    writer.WriteLine($@"{const_domain_name} value = {const_domain_name}s.FirstOrDefault(x => x.ValueType == member);");
                    writer.WriteLine($@"if (value == null) {{");
                    writer.Indent++;
                    writer.WriteLine($@"value = new {const_domain_name}(Session);");
                    writer.WriteLine($@"value.ValueType = member;");
                    writer.WriteLine($@"value.Code = enum_type.GetEnumName(member);");
                    writer.WriteLine($@"{const_domain_name}s.Add(value);");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                }
                writer.WriteLine();
                writer.WriteLine($@"public void ConstAllHierarchyLink() {{");
                writer.Indent++;
                foreach (var hierarhy in template.Container.Hierarchys) {
                    writer.WriteLine();
                    foreach (var node in hierarhy.Nodes) {
                        if (node.Up != null) {
                            writer.WriteLine($@"ConstHierarhy{MakeCamelName(hierarhy.Code)}Link({MakeCamelName(domain.Container.Code + "_" + hierarhy.Domain.Code)}Type.{node.Up.DomainMember.Code}, {MakeCamelName(domain.Container.Code + "_" + hierarhy.Domain.Code)}Type.{node.DomainMember.Code}, {node.SortOrder});");
                        }
                    }
                }
                writer.WriteLine();
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.WriteLine();
                foreach (var hierarhy in template.Container.Hierarchys) {
                    String const_hierarhy_name = MakeCamelName(hierarhy.Code);
                    String const_domain_name = MakeCamelName(domain.Container.Code + "_" + hierarhy.Domain.Code);
                    writer.WriteLine();
                    writer.WriteLine($@"public void ConstHierarhy{const_hierarhy_name}Link({const_domain_name}Type up, {const_domain_name}Type down, Int32 order) {{");
                    writer.Indent++;
                    writer.WriteLine($@"var enum_type = typeof({const_domain_name}Type);");
                    writer.WriteLine($@"{const_domain_name} value_up = {const_domain_name}s.FirstOrDefault(x => x.ValueType == up);");
                    writer.WriteLine($@"{const_domain_name} value_down = {const_domain_name}s.FirstOrDefault(x => x.ValueType == down);");
                    writer.WriteLine($@"if (!(value_up is null || value_down is null) ) {{");
                    writer.Indent++;
                    writer.WriteLine($@"value_down.{MakeCamelName(hierarhy.Code + "_UP")} = value_up;");
                    writer.WriteLine($@"value_down.{MakeCamelName(hierarhy.Code + "_ORDER")} = order;");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                }
            }
            if (domain.DomainType == MdfCoreDomain.MdfCoreDomainType.DOMAIN_REPORT) {
                writer.WriteLine();
                writer.WriteLine($@"private {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)} _{MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)};");
                writer.WriteLine($@"public {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)} {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _{MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)}; }}");
                writer.WriteLine($@"set {{ SetPropertyValue(ref _{MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)}, value); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
                foreach (var table in domain.Container.Tables.Where(x => x.TableType == MdfCoreTable.MdfCoreTableType.TABLE_PERSISTENT)) {
                    writer.WriteLine();
                    writer.WriteLine($@"[Association]");
                    writer.WriteLine($@"[Aggregated]");
                    writer.WriteLine($@"public XPCollection<{MakeCamelName(tmpl.Container.Code + "_DATA_" + table.Code)}> {MakeCamelName("DATA_" + table.Code)}s {{");
                    writer.Indent++;
                    writer.WriteLine($@"get {{ return GetCollection<{MakeCamelName(tmpl.Container.Code + "_DATA_" + table.Code)}>(); }}");
                    writer.Indent--;
                    writer.WriteLine($@"}}");
                }
                writer.WriteLine();
                writer.WriteLine($@"[Association]");
                writer.WriteLine($@"[Aggregated]");
                writer.WriteLine($@"public XPCollection<{MakeCamelName(tmpl.Container.Code)}Form> Forms {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return GetCollection<{MakeCamelName(tmpl.Container.Code)}Form>(); }}");
                writer.Indent--;
                writer.WriteLine($@"}}");
                //foreach (var enum_domain in domain.Container.Domains.Where(x => x.DataType == MdfCoreDataType.DT_ENUM)) {
                //    writer.WriteLine();
                //    writer.WriteLine($@"private Dictionary<{MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}Type, {MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}> _Enum{MakeCamelName(enum_domain.Code)};");
                //    writer.WriteLine($@"[Browsable(false)]");
                //    writer.WriteLine($@"public IDictionary<{MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}Type, {MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}> Enum{MakeCamelName(enum_domain.Code)} {{");
                //    writer.Indent++;
                //    writer.WriteLine("get {");
                //    writer.Indent++;
                //    writer.WriteLine($@"if (_Enum{MakeCamelName(enum_domain.Code)} == null) {{");
                //    writer.Indent++;
                //    writer.WriteLine($@"_Enum{MakeCamelName(enum_domain.Code)} = new Dictionary<{MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}Type, {MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}>({MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)}.{MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}s.Count);");
                //    writer.WriteLine($@"foreach (var value in {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainTemplate.Code)}.{MakeCamelName(domain.Container.Code + "_" + enum_domain.Code)}s) {{");
                //    writer.Indent++;
                //    writer.WriteLine($@"_Enum{MakeCamelName(enum_domain.Code)}[value.ValueType] = value;");
                //    writer.Indent--;
                //    writer.WriteLine("}");
                //    writer.Indent--;
                //    writer.WriteLine("}");
                //    writer.WriteLine($@"return _Enum{MakeCamelName(enum_domain.Code)};");
                //    writer.Indent--;
                //    writer.WriteLine("}");
                //    writer.Indent--;
                //    writer.WriteLine("}");
                //}
                writer.WriteLine();
                //writer.WriteLine($@"private List<{MakeCamelName(domain.Container.Code)}CategoryValue> _CategoryValues;");
                //writer.WriteLine($@"[Browsable(false)]");
                //writer.WriteLine($@"public IList<{MakeCamelName(domain.Container.Code)}CategoryValue> CategoryValues {{");
                //writer.Indent++;
                //writer.WriteLine($@"get {{");
                //writer.Indent++;
                //writer.WriteLine($@"if (_CategoryValues == null) {{");
                //writer.WriteLine($@"    _CategoryValues = new List<{MakeCamelName(domain.Container.Code)}CategoryValue>();");
                //writer.WriteLine($@"}}");
                //writer.WriteLine($@"return _CategoryValues;");
                //writer.Indent--;
                //writer.WriteLine($@"}}");
                //writer.Indent--;
                //writer.WriteLine($@"}}");
                writer.WriteLine($@"private {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainReport.Code)}Core _ReportCore;");
                writer.WriteLine($@"[Browsable(false)]");
                writer.WriteLine($@"public {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainReport.Code)}Core ReportCore {{");
                writer.Indent++;
                writer.WriteLine($@"get {{");
                writer.Indent++;
                writer.WriteLine($@"if (_ReportCore == null) {{");
                writer.WriteLine($@"    _ReportCore = new  {MakeCamelName(domain.Container.Code + "_" + tmpl.DomainReport.Code)}Core(this);");
                writer.WriteLine($@"}}");
                writer.WriteLine($@"return _ReportCore;");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.Indent--;
                writer.WriteLine($@"}}");
            }
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(domain.Container.Code + "_" + domain.Code)}(Session session): base(session) {{ }}");
            writer.Indent--;
            writer.WriteLine("}");
        }

        private static String CheckDomainTypeName(MdfCoreDomain domain) {
            String name;
            switch (domain.DataType) {
                case MdfCoreDataType.DT_DATE:
                    name = "System.DateTime";
                    break;
                case MdfCoreDataType.DT_BOOLEAN:
                    name = "System.Boolean";
                    break;
                case MdfCoreDataType.DT_DECIMAL:
                    name = "System.Decimal";
                    break;
                case MdfCoreDataType.DT_ENUM:
                    name = MakeCamelName(domain.Container.Code + "_" + domain.Code);
                    break;
                case MdfCoreDataType.DT_MONETARY:
                    name = "System.Decimal";
                    break;
                case MdfCoreDataType.DT_OBJECT:
                    name = MakeCamelName(domain.Container.Code + "_" + domain.Code);
                    break;
                case MdfCoreDataType.DT_PERIOD:
                    name = "System.DateTime";
                    break;
                case MdfCoreDataType.DT_STRING:
                    name = "System.String";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return name;
        }

        private static void CodeGenerateTable(IndentedTextWriter writer, MdfCoreTemplate tmpl) {

            writer.WriteLine();
            writer.WriteLine($@"public abstract partial class {MakeCamelName(tmpl.Container.Code)}Table: MdfReportTable<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}Table({MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} report) : base(report) {{ }}");
            //writer.WriteLine();
            //writer.WriteLine($@"protected override {MakeCamelName(tmpl.Container.Code)}TableCell CellCreate() {{");
            //writer.Indent++;
            //writer.WriteLine($@"return new {MakeCamelName(tmpl.Container.Code)}TableCell(this);");
            //writer.Indent--;
            //writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($"public {MakeCamelName(tmpl.Container.Code)}CalcVariant Variant {{ get; protected set; }}");
            writer.WriteLine();
            writer.WriteLine($"public void Render({MakeCamelName(tmpl.Container.Code)}Report report, {MakeCamelName(tmpl.Container.Code)}CalcVariant variant) {{");
            writer.Indent++;
            writer.WriteLine("Variant = variant;");
            writer.WriteLine("RenderCore(report.ReportCore);");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($"protected override void CellCustom({MakeCamelName(tmpl.Container.Code)}TableCell cell) {{");
            writer.WriteLine("    CellCustom(cell, Variant);");
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($"protected virtual void CellCustom({MakeCamelName(tmpl.Container.Code)}TableCell cell, {MakeCamelName(tmpl.Container.Code)}CalcVariant variant) {{ }}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public abstract partial class {MakeCamelName(tmpl.Container.Code)}TableCell: MdfReportTableCell<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}TableCell({MakeCamelName(tmpl.Container.Code)}Table table) : base(table) {{ }}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");
            //            foreach (var table in tmpl.Tables.Where(x => x.Table.TableType == MdfCoreTable.MdfCoreTableType.TABLE_PERSISTENT)) {
            foreach (var table in tmpl.Tables) {
                writer.WriteLine();
                writer.WriteLine($@"public partial class {MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)}: {MakeCamelName(tmpl.Container.Code)}Table {{");
                writer.Indent++;
                writer.WriteLine();
                writer.WriteLine($@"public  {MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)}({MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} report) : base(report) {{ }}");
                writer.WriteLine();
                writer.WriteLine($@"protected override {MakeCamelName(tmpl.Container.Code)}TableCell CellCreate() {{");
                writer.Indent++;
                writer.WriteLine($@"return new  {MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)}Cell(this);");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.WriteLine();
                writer.WriteLine("void TestCalcExpression() {");
                writer.WriteLine("}");
                writer.WriteLine();
                writer.WriteLine($@"protected override void CellCustom({MakeCamelName(tmpl.Container.Code)}TableCell cell, {MakeCamelName(tmpl.Container.Code)}CalcVariant variant) {{");
                writer.Indent++;
                var name_report = MakeCamelName(tmpl.Container.Code + "_Report");
                var name_category = MakeCamelName(tmpl.Container.Code + "_Category_Value");
                var name_table = MakeCamelName(tmpl.Container.Code + "_Table");
                var name_cell = MakeCamelName(tmpl.Container.Code + "_Table_Cell");
                var name_point = MakeCamelName(tmpl.Container.Code + "_Data_Point");
                var name_gentype = $"<{name_report}, {name_category}, {name_table}, {name_cell}, {name_point}>";
                foreach (var table_axis in table.Table.TableAxiss.OrderBy(x => x.AxisIndex)) {
                    writer.WriteLine($@"var ord{table_axis.AxisIndex} = (({MakeCamelName(tmpl.Container.Code + "_" + table_axis.Axis.Code)}.Ordinate)cell.Ordinates[{table_axis.AxisIndex}]).OrdinateType;");
                }
                writer.WriteLine($@"{name_category} category_value;");
                writer.WriteLine($@"{name_point} data_point;");
                writer.WriteLine($@"IList<{name_category}> category_value_list = new List<{name_category}>(32);");
                writer.WriteLine($@"IList<{name_point}> data_point_list = new List<{name_point}>(32);");
                foreach (var cell in table.Table.Cells) {
                    foreach (var calc in cell.Calcs) {
                        if (calc != null && !calc.IsDisabled && calc.Formula != null && calc.Formula.Length > 1 && calc.Formula[0] == '=') {
                            writer.WriteLine($"if ( variant == {MakeCamelName(tmpl.Container.Code)}CalcVariant.{calc.CalcVariant.Code} ");
                            foreach (var table_axis in table.Table.TableAxiss.OrderBy(x => x.AxisIndex)) {
                                writer.Write($" && ord{table_axis.AxisIndex} == {MakeCamelName(tmpl.Container.Code + "_" + table_axis.Axis.Code)}.OrdinateType.{cell.AxisOrdinates[table_axis.AxisIndex].Code}");
                            }
                            writer.WriteLine(") {");
                            writer.Indent++;
                            //if (cell.Calc.Formula == null || cell.Calc.Formula.Length < 1 || cell.Calc.Formula[0] != '=')
                            //    continue;
                            var formula = calc.Formula.Substring(1);
                            foreach (var call_link in calc.CalcLinks) {
                                if (call_link.LinkType == MdfCoreDataPointCalcLinkType.SCALAR)
                                    formula = formula.Replace(call_link.Formula, $"(x.Links[{call_link.Index}].DataPoint.ValueDecimal)");
                                if (call_link.LinkType == MdfCoreDataPointCalcLinkType.AGG_SUM)
                                    formula = formula.Replace(call_link.Formula, $"(x.Links[{call_link.Index}].DataPointList.Sum(y => y.ValueDecimal))");
                            }
                            writer.WriteLine($@"cell.Calc = new MdfReportDataPointCalc{name_gentype}(cell.DataPoint,");
                            if (calc.IsTested)
                                writer.WriteLine($@"    x => {{ TestCalcExpression(); return ({formula})); }}");
                            else
                                writer.WriteLine($@"    x => ({formula}));");
                            writer.WriteLine($@"cell.Calc.Formula = ""{formula}"";");
                            foreach (var calc_link in calc.CalcLinks.OrderBy(x => x.Index)) {
                                if (!calc_link.IsUsed)
                                    continue;
                                if (calc_link.LinkType != MdfCoreDataPointCalcLinkType.SCALAR) {
                                    writer.WriteLine();
                                    writer.WriteLine("category_value_list.Clear();");
                                    writer.WriteLine("data_point_list.Clear();");
                                    writer.WriteLine($@"category_value_list.Add(new {name_category}());");
                                    var level = 0;
                                    foreach (var link_field in calc_link.LinkFields.OrderBy(x => x.CalcIndex)) {
                                        if (link_field.FieldType == MdfCoreDataPointCalcLinkFieldType.FROM_SOURCE) {
                                            writer.WriteLine($@"category_value_list[category_value_list.Count - 1].{MakeCamelName(link_field.Dimension.Code)} = cell.DataPoint.CategoryValue.{MakeCamelName(link_field.Dimension.Code)};");
                                        }
                                        if (link_field.FieldType == MdfCoreDataPointCalcLinkFieldType.EXPLICIT) {
                                            var member = link_field.DimensionMember;
                                            if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.GENERAL) {
                                                writer.WriteLine($@"category_value_list[category_value_list.Count - 1].{MakeCamelName(link_field.Dimension.Code)} = Report.ReportCore.Const{MakeCamelName(link_field.Dimension.Domain.Code)}[{MakeCamelName(tmpl.Container.Code + "_" + link_field.Dimension.Domain.Code)}Type.{member.DomainMember.Code}];");
                                            }
                                            if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                                                writer.WriteLine($@"category_value_list[category_value_list.Count - 1].{MakeCamelName(link_field.Dimension.Code)} = category_value_list[category_value_list.Count - 1].{MakeCamelName(link_field.DimensionMember.DomainMember.CalcDimension.Code)}.{MakeCamelName(link_field.DimensionMember.DomainMember.CalcProperty.Code)};");
                                            }
                                            if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.QUERY) {
                                                level++;
                                                writer.WriteLine($@"category_value_list.Add(null);");
                                                writer.WriteLine($@"foreach (var {member.Code.ToLower()} in Report.ReportCore.Query{MakeCamelName(member.DomainMember.Domain.Code)}(category_value_list[category_value_list.Count - 2], {MakeCamelName(tmpl.Container.Code + "_" + link_field.Dimension.Domain.Code)}Type.{member.DomainMember.Code})) {{");
                                                writer.Indent++;
                                                writer.WriteLine($@"category_value_list[category_value_list.Count - 1] = new ReportOrderPlanCategoryValue(category_value_list[category_value_list.Count - 2]);");
                                                writer.WriteLine($@"category_value_list[category_value_list.Count - 1].DimPeriod = {member.Code.ToLower()};");
                                            }
                                            if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.HIERARCHY) {
                                                writer.WriteLine($@"throw new NotImplementedException();");
                                            }
                                        }
                                    }
                                    writer.WriteLine($@"category_value = Report.ReportCore.CategoryValues.Locate(category_value_list[category_value_list.Count - 1]);");
                                    writer.WriteLine($@"data_point_list.Add(Report.ReportCore.DataPointGet(category_value));");
                                    while (level-- > 0) {
                                        writer.Indent--;
                                        writer.WriteLine("}");
                                    }
                                    writer.WriteLine($@"cell.Calc.Links[{calc_link.Index}] = new MdfReportDataPointCalcLink{name_gentype}(cell.Calc, data_point_list);");
                                }
                                if (calc_link.LinkType == MdfCoreDataPointCalcLinkType.SCALAR) {
                                    writer.WriteLine();
                                    writer.WriteLine($@"category_value = new {name_category}();");
                                    foreach (var link_field in calc_link.LinkFields.OrderBy(x => x.CalcIndex)) {
                                        if (link_field.FieldType == MdfCoreDataPointCalcLinkFieldType.FROM_SOURCE) {
                                            writer.WriteLine($@"category_value.{MakeCamelName(link_field.Dimension.Code)} = cell.DataPoint.CategoryValue.{MakeCamelName(link_field.Dimension.Code)};");
                                        }
                                        if (link_field.FieldType == MdfCoreDataPointCalcLinkFieldType.EXPLICIT) {
                                            var member = link_field.DimensionMember;
                                            //if (link_field.FieldType == MdfCoreDataPointCalcLinkFieldType.FROM_TARGET) {
                                            //    member = link_field.CalcLink.DataPoint.CategoryMember.CategoryMemberFields.FirstOrDefault(x => x.CategoryTypeField.Dimension == link_field.Dimension).DimensionMember;
                                            //}
                                            if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.GENERAL) {
                                                writer.WriteLine($@"category_value.{MakeCamelName(link_field.Dimension.Code)} = Report.ReportCore.Const{MakeCamelName(link_field.Dimension.Domain.Code)}[{MakeCamelName(tmpl.Container.Code + "_" + link_field.Dimension.Domain.Code)}Type.{member.DomainMember.Code}];");
                                            }
                                            if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED) {
                                                writer.WriteLine($@"category_value.{MakeCamelName(link_field.Dimension.Code)} = category_value.{MakeCamelName(link_field.DimensionMember.DomainMember.CalcDimension.Code)}.{MakeCamelName(link_field.DimensionMember.DomainMember.CalcProperty.Code)};");
                                            }
                                            //if (member.DomainMember.CalcType == MdfCoreDomainMemberCalcType.QUERY) {
                                            //    writer.WriteLine($@"category_value.{MakeCamelName(link_field.Dimension.Code)} = category_value.{MakeCamelName(link_field.DimensionMember.DomainMember.CalcDimension.Code)}.{MakeCamelName(link_field.DimensionMember.DomainMember.CalcProperty.Code)};");
                                            //}
                                        }
                                    }
                                    writer.WriteLine("category_value = Report.ReportCore.CategoryValues.Locate(category_value);");
                                    writer.WriteLine("data_point = Report.ReportCore.DataPointGet(category_value);");
                                    writer.WriteLine($@"cell.Calc.Links[{calc_link.Index}] = new MdfReportDataPointCalcLink{name_gentype}(cell.Calc, data_point);");
                                }
                            }
                            writer.WriteLine("if (cell.Calc.CheckNotIsCycle())");
                            writer.WriteLine("    cell.Calc.Link();");
                            //                        writer.WriteLine("else");
                            //                        writer.WriteLine("    cell.Calc.UnLink();");
                            //    value = new ReportOrderPlanCategoryValue(cell.Calc.DataPoint.CategoryValue);
                            //    value.DimArticle = Report.ReportCore.ConstArticle[ReportOrderPlanArticleType.ARTICLE_EXPENSE];
                            //    data_point = Report.ReportCore.DataPointLocate(value);
                            //    cell.Calc.Links[1] = new MdfReportDataPointCalcLink<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTable, ReportOrderPlanTableCell, ReportOrderPlanDataPoint>(cell.Calc, data_point);
                            writer.Indent--;
                            writer.WriteLine("}");
                        }
                    }
                }
                //var ord0 = ((ReportOrderPlanAxisCalcImportCol.Ordinate)cell.Ordinates[0]).OrdinateType;
                //var ord1 = ((ReportOrderPlanAxisCalcImportCol.Ordinate)cell.Ordinates[1]).OrdinateType;
                //ReportOrderPlanCategoryValue value;
                //ReportOrderPlanDataPoint data_point;
                //if (ord0 == ReportOrderPlanAxisCalcImportCol.OrdinateType.CCIC_PERIOD_PLAN && ord1 == ReportOrderPlanAxisCalcImportCol.OrdinateType.CCIC_PERIOD_YEAR_YEAR) {
                //    cell.Calc = new MdfReportDataPointCalc<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTable, ReportOrderPlanTableCell, ReportOrderPlanDataPoint>(cell.DataPoint,
                //        x => x.Links[1].DataPoint.ValueDecimal + x.Links[2].DataPoint.ValueDecimal);
                //    value = new ReportOrderPlanCategoryValue(cell.Calc.DataPoint.CategoryValue);
                //    value.DimArticle = Report.ReportCore.ConstArticle[ReportOrderPlanArticleType.ARTICLE_EXPENSE];
                //    data_point = Report.ReportCore.DataPointLocate(value);
                //    cell.Calc.Links[1] = new MdfReportDataPointCalcLink<ReportOrderPlanReport, ReportOrderPlanCategoryValue, ReportOrderPlanTable, ReportOrderPlanTableCell, ReportOrderPlanDataPoint>(cell.Calc, data_point);
                //}
                //                writer.WriteLine($@"return new  {MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)}Cell(this);");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.WriteLine();
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine();
                writer.WriteLine($@"public partial class {MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)}Cell: {MakeCamelName(tmpl.Container.Code)}TableCell {{");
                writer.Indent++;
                if (table.Table.TableType == MdfCoreTable.MdfCoreTableType.TABLE_PERSISTENT) {
                    writer.WriteLine();
                    writer.WriteLine($@"public void CellDataPointStore({MakeCamelName(tmpl.Container.Code + "_DATA_" + table.Table.Code)} row) {{");
                    writer.Indent++;
                    writer.WriteLine($@"{MakeCamelName(tmpl.Container.Code + "_" + table.Table.ColumnAxis.Axis.Code)}.OrdinateType ord_type = (({MakeCamelName(tmpl.Container.Code + "_" + table.Table.ColumnAxis.Axis.Code)}.Ordinate)Ordinates[{table.Table.ColumnAxis.AxisIndex}]).OrdinateType;");
                    //ReportOrderPlanAxisBalanceValues.OrdinateType ord_type = ((ReportOrderPlanAxisBalanceValues.Ordinate)Ordinates[0]).OrdinateType;
                    writer.WriteLine("switch (ord_type) {");
                    foreach (var column_ord in table.Table.ColumnAxis.Axis.OrdinateLine) {
                        writer.Indent++;
                        writer.WriteLine($@"case {MakeCamelName(tmpl.Container.Code + "_" + table.Table.ColumnAxis.Axis.Code)}.OrdinateType.{column_ord.Code}:");
                        String value_name;
                        switch (DataPointTypeCheck(column_ord.CategoryMember)) {
                            case MdfCoreDataType.DT_BOOLEAN:
                                value_name = "ValueDecimal";
                                break;
                            case MdfCoreDataType.DT_DATE:
                                value_name = "ValueDate";
                                break;
                            case MdfCoreDataType.DT_DECIMAL:
                            case MdfCoreDataType.DT_MONETARY:
                                value_name = "ValueDecimal";
                                break;
                            case MdfCoreDataType.DT_STRING:
                                value_name = "ValueString";
                                break;
                            default:
                                value_name = $@"UndefinedType{DataPointTypeCheck(column_ord.CategoryMember)}";
                                break;
                        }
                        writer.Indent++;
                        writer.WriteLine($@"row.{MakeCamelName(column_ord.Code)} = DataPoint.{value_name};");
                        writer.WriteLine("break;");
                        writer.Indent--;
                        writer.Indent--;
                    }
                    //    case ReportOrderPlanAxisBalanceValues.OrdinateType.IE_FORECAST_COUNT:
                    //        row.IeForecastCount = DataPoint.ValueDecimal;
                    //        break;
                    writer.WriteLine("}");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    writer.WriteLine($@"public void CellDataPointRestore({MakeCamelName(tmpl.Container.Code + "_DATA_" + table.Table.Code)} row) {{");
                    //ReportOrderPlanAxisBalanceValues.OrdinateType ord_type = ((ReportOrderPlanAxisBalanceValues.Ordinate)Ordinates[0]).OrdinateType;
                    //switch (ord_type) {
                    //    //case ReportOrderPlanAxisBalanceValues.OrdinateType.IE_FORECAST_VALUE_BALANCE:
                    //    //    DataPoint.Value = row.IeForecastValueBalance;
                    //    //    break;
                    //    case ReportOrderPlanAxisBalanceValues.OrdinateType.IE_FORECAST_COUNT:
                    //        DataPoint.Value = row.IeForecastCount;
                    //        break;
                    writer.Indent++;
                    writer.WriteLine($@"{MakeCamelName(tmpl.Container.Code + "_" + table.Table.ColumnAxis.Axis.Code)}.OrdinateType ord_type = (({MakeCamelName(tmpl.Container.Code + "_" + table.Table.ColumnAxis.Axis.Code)}.Ordinate)Ordinates[{table.Table.ColumnAxis.AxisIndex}]).OrdinateType;");
                    writer.WriteLine("switch (ord_type) {");
                    foreach (var column_ord in table.Table.ColumnAxis.Axis.OrdinateLine) {
                        writer.Indent++;
                        writer.WriteLine($@"case {MakeCamelName(tmpl.Container.Code + "_" + table.Table.ColumnAxis.Axis.Code)}.OrdinateType.{column_ord.Code}:");
                        writer.Indent++;
                        writer.WriteLine($@"DataPoint.Value = row.{MakeCamelName(column_ord.Code)};");
                        writer.WriteLine("break;");
                        writer.Indent--;
                        writer.Indent--;
                    }
                    writer.WriteLine("}");
                    writer.Indent--;
                    writer.WriteLine("}");
                    List<MdfCoreDimension> row_dims = new List<MdfCoreDimension>(16);
                    foreach (var row_ord in table.Table.RowAxis.Axis.OrdinateLine) {
                        foreach (var field in row_ord.CategoryMember.CategoryMemberFields) {
                            if (field.DimensionMember.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED)
                                continue;
                            if (!row_dims.Contains(field.CategoryTypeField.Dimension)) {
                                row_dims.Add(field.CategoryTypeField.Dimension);
                            }
                        }
                    }
                    foreach (var row_ord in table.Table.RowAxis.Axis.Ordinates) {
                        if (row_ord.IsForcePersistent) {
                            if (!row_dims.Contains(row_ord.Dimension)) {
                                row_dims.Add(row_ord.Dimension);
                            }
                        }
                    }
                    writer.WriteLine();
                    writer.WriteLine("public override void Store() {");
                    writer.Indent++;
                    writer.WriteLine("base.Store();");
                    writer.WriteLine($@"var row = Table.Report.Data{MakeCamelName(table.Table.Code)}s.FirstOrDefault(");
                    writer.Indent++;
                    writer.WriteLine("x =>");
                    writer.Indent++;
                    foreach (var dim in row_dims) {
                        writer.WriteLine($@"x.{MakeCamelName(dim.Code)} == Ordinates[{table.Table.RowAxis.AxisIndex}].CategoryValue.{MakeCamelName(dim.Code)} &&");
                    }
                    writer.WriteLine("true");
                    writer.Indent--;
                    writer.WriteLine(");");
                    writer.Indent--;
                    writer.WriteLine("if (row == null) {");
                    writer.Indent++;
                    writer.WriteLine("if ((DataPoint.DataType == MdfDataType.DT_DECIMAL ||");
                    writer.Indent++;
                    writer.WriteLine("DataPoint.DataType == MdfDataType.DT_MONETARY) && ");
                    writer.WriteLine("DataPoint.ValueDecimal == 0m )");
                    writer.Indent++;
                    writer.WriteLine("return;");
                    writer.Indent--;
                    writer.Indent--;
                    writer.WriteLine($@"row = new {MakeCamelName(table.Table.Container.Code + "_DATA_" + table.Table.Code)}(Table.Report.Session);");
                    writer.WriteLine($@"Table.Report.Data{MakeCamelName(table.Table.Code)}s.Add(row);");
                    foreach (var dim in row_dims) {
                        writer.WriteLine($@"row.{MakeCamelName(dim.Code)} = Ordinates[{table.Table.RowAxis.AxisIndex}].CategoryValue.{MakeCamelName(dim.Code)};");
                    }
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine("CellDataPointStore(row);");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    writer.WriteLine("public override void Restore() {");
                    writer.Indent++;
                    writer.WriteLine("base.Restore();");
                    writer.WriteLine($@"var row = Table.Report.Data{MakeCamelName(table.Table.Code)}s.FirstOrDefault(");
                    writer.Indent++;
                    writer.WriteLine("x =>");
                    writer.Indent++;
                    foreach (var dim in row_dims) {
                        writer.WriteLine($@"x.{MakeCamelName(dim.Code)} == Ordinates[1].CategoryValue.{MakeCamelName(dim.Code)} &&");
                    }
                    writer.WriteLine("true");
                    writer.Indent--;
                    writer.WriteLine(");");
                    writer.Indent--;
                    writer.WriteLine("if (row == null) {");
                    writer.Indent++;
                    writer.WriteLine("if (DataPoint.DataType == MdfDataType.DT_DECIMAL ||");
                    writer.Indent++;
                    writer.WriteLine("DataPoint.DataType == MdfDataType.DT_MONETARY)");
                    writer.Indent++;
                    writer.WriteLine("DataPoint.Value = 0m;");
                    writer.Indent--;
                    writer.Indent--;
                    writer.WriteLine("return;");
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine("CellDataPointRestore(row);");
                    writer.Indent--;
                    writer.WriteLine("}");
                }
                writer.WriteLine();
                writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)}Cell({MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Table.Code)} table) : base(table) {{ }}");
                writer.WriteLine();
                writer.Indent--;
                writer.WriteLine("}");
            }

        }

        private static void CodeGenerateCategoryValue(IndentedTextWriter writer, MdfCoreTemplate tmpl) {

            writer.WriteLine();
            writer.WriteLine("[DC.DomainComponent]");
            writer.WriteLine($@"public partial class {MakeCamelName(tmpl.Container.Code)}CategoryValue: MdfCategoryValue<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"public static {MakeCamelName(tmpl.Container.Code)}CategoryValue DefaultCategoryValue = new {MakeCamelName(tmpl.Container.Code)}CategoryValue();");
            writer.WriteLine();
            foreach (MdfCoreDimension dim in tmpl.Container.Dimensions) {
                CodeGenerateDomainRef2(writer, dim);
            }
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}CategoryValue() {{ }}");
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}CategoryValue({MakeCamelName(tmpl.Container.Code)}CategoryValue category_value) {{");
            writer.Indent++;
            writer.WriteLine();
            foreach (MdfCoreDimension dim in tmpl.Container.Dimensions) {
                writer.WriteLine($@"{MakeCamelName(dim.Code)} = category_value.{MakeCamelName(dim.Code)};");
            }
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public override String ToString() {{");
            writer.Indent++;
            writer.Write($@"return ");
            Boolean IsFirst = true;
            foreach (MdfCoreDimension dim in tmpl.Container.Dimensions.OrderBy(x => x.Code)) {
                if (IsFirst) {
                    IsFirst = false;
                }
                else {
                    writer.WriteLine($@" +");
                }
                writer.Write($@"$""{MakeCamelName(dim.Code)}={{{MakeCamelName(dim.Code)}}}\n""");
            }
            writer.WriteLine(";");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($@"public override Boolean Equals({MakeCamelName(tmpl.Container.Code)}CategoryValue value) {{");
            writer.Indent++;
            writer.Write($@"return ");
            IsFirst = true;
            foreach (MdfCoreDimension dim in tmpl.Container.Dimensions) {
                if (IsFirst) {
                    IsFirst = false;
                }
                else {
                    writer.WriteLine($@" &&");
                }
                writer.Write($@"ReferenceEquals({MakeCamelName(dim.Code)}, value.{MakeCamelName(dim.Code)})");
            }
            writer.WriteLine(";");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public override int GetHashCode() {{");
            writer.Indent++;
            writer.Write($@"unchecked {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine("int hash = 17;");
            foreach (MdfCoreDimension dim in tmpl.Container.Dimensions) {
                writer.Write($@"hash = hash * 23 + ({MakeCamelName(dim.Code)}?.GetHashCode() ?? 0);");
            }
            writer.WriteLine();
            writer.WriteLine("return hash;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");

        }
        private static void CodeGenerateCategoryValues(IndentedTextWriter writer, MdfCoreTemplate tmpl) {

            writer.WriteLine();
            writer.WriteLine($@"public class {MakeCamelName(tmpl.Container.Code)}CategoryValues: MdfCategoryValues<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint> {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"public override {MakeCamelName(tmpl.Container.Code)}CategoryValue Union(IEnumerable<ReportOrderPlanCategoryValue> values) {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"{MakeCamelName(tmpl.Container.Code)}CategoryValue value = new ReportOrderPlanCategoryValue();");
            writer.WriteLine($@"foreach (var union_value in values) {{");
            writer.Indent++;
            foreach (MdfCoreDimension dim in tmpl.Container.Dimensions) {
                writer.WriteLine($@"value.{MakeCamelName(dim.Code)} = union_value.{MakeCamelName(dim.Code)} ?? value.{MakeCamelName(dim.Code)};");
            }
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine($@"return Locate(value);");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}CategoryValues() {{");
            writer.Indent++;
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");

        }

        private static void CodeGenerateReportCore(IndentedTextWriter writer, MdfCoreTemplate tmpl) {

            writer.WriteLine();
            writer.WriteLine($@"public partial class {MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}Core: MdfReportCore<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint> {{");
            writer.Indent++;
            foreach (var axis in tmpl.Container.Axiss) {
                if (String.IsNullOrEmpty(axis.Code)) continue;
                writer.WriteLine();
                writer.WriteLine($@"private {MakeCamelName(axis.Container.Code + "_" + axis.Code)} _Axis{MakeCamelName(axis.Code)};");
                writer.WriteLine($@"[Browsable(false)]");
                writer.WriteLine($@"public {MakeCamelName(axis.Container.Code + "_" + axis.Code)} Axis{MakeCamelName(axis.Code)} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{");
                writer.Indent++;
                writer.WriteLine($@"if (_Axis{MakeCamelName(axis.Code)} == null) {{ ");
                writer.WriteLine($@"    _Axis{MakeCamelName(axis.Code)} = new {MakeCamelName(axis.Container.Code + "_" + axis.Code)}(this.Report);");
                writer.WriteLine($@"    _Axis{MakeCamelName(axis.Code)}.Render();");
                writer.WriteLine($@"}}");
                writer.WriteLine($@"return _Axis{MakeCamelName(axis.Code)};");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.Indent--;
                writer.WriteLine($@"}}");
            }
            foreach (var table in tmpl.Container.Tables) {
                if (String.IsNullOrEmpty(table.Code)) continue;
                //String table_type_name = table.TableType == MdfCoreTable.MdfCoreTableType.TABLE_PERSISTENT ?
                //    $@"{MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Code)}" :
                //    $@"{MakeCamelName(tmpl.Container.Code)}Table";
                String table_type_name = $@"{MakeCamelName(tmpl.Container.Code + "_TABLE_" + table.Code)}";
                writer.WriteLine();
                writer.WriteLine($@"private {table_type_name} _Table{MakeCamelName(table.Code)};");
                writer.WriteLine($@"[Browsable(false)]");
                writer.WriteLine($@"public {table_type_name} Table{MakeCamelName(table.Code)} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{");
                writer.Indent++;
                writer.WriteLine($@"if (_Table{MakeCamelName(table.Code)} == null) {{ ");
                writer.WriteLine($@"    _Table{MakeCamelName(table.Code)} = new {table_type_name}(this.Report);");
                //                writer.WriteLine($@"    _Axis{MakeCamelName(axis.Code)}.Render();");
                foreach (var axis in table.TableAxiss.OrderBy(x => x.AxisIndex).Select(x => x.Axis)) {
                    writer.WriteLine($@"    _Table{MakeCamelName(table.Code)}.Axiss.Add(Axis{MakeCamelName(axis.Code)});");
                }
                writer.WriteLine($@"}}");
                writer.WriteLine($@"return _Table{MakeCamelName(table.Code)};");
                writer.Indent--;
                writer.WriteLine($@"}}");
                writer.Indent--;
                writer.WriteLine($@"}}");
            }
            foreach (var const_domain in tmpl.DomainTemplate.ContainedDomains.Where(x => x.DataType == MdfCoreDataType.DT_ENUM || x.DataType == MdfCoreDataType.DT_OBJECT)) {
                writer.WriteLine();
                writer.WriteLine($@"private Dictionary<{MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}Type, {MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}> _Const{MakeCamelName(const_domain.Code)};");
                writer.WriteLine($@"[Browsable(false)]");
                writer.WriteLine($@"public IDictionary<{MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}Type, {MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}> Const{MakeCamelName(const_domain.Code)} {{");
                writer.Indent++;
                writer.WriteLine("get {");
                writer.Indent++;
                writer.WriteLine($@"if (_Const{MakeCamelName(const_domain.Code)} == null) {{");
                writer.Indent++;
                writer.WriteLine($@"_Const{MakeCamelName(const_domain.Code)} = new Dictionary<{MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}Type, {MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}>(128);");
                writer.WriteLine($@"foreach (var value in Report.{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainTemplate.Code)}.{MakeCamelName(tmpl.Container.Code + "_" + const_domain.Code)}s) {{");
                writer.Indent++;
                writer.WriteLine($@"_Const{MakeCamelName(const_domain.Code)}[value.ValueType] = value;");
                writer.Indent--;
                writer.WriteLine("}");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine($@"return _Const{MakeCamelName(const_domain.Code)};");
                writer.Indent--;
                writer.WriteLine("}");
                writer.Indent--;
                writer.WriteLine("}");
            }
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)}Core({MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} report) : base(report, NewCategoryValues()) {{ }}");
            writer.WriteLine();
            writer.WriteLine($@"private static {MakeCamelName(tmpl.Container.Code)}CategoryValues NewCategoryValues() {{");
            writer.Indent++;
            writer.WriteLine($@"return new {MakeCamelName(tmpl.Container.Code)}CategoryValues();");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");

        }

        private static void CodeGenerateDomainRef2(IndentedTextWriter writer, MdfCoreDimension dimension) {
            writer.WriteLine();
            writer.WriteLine($@"private {MakeCamelName(dimension.Container.Code + "_" + dimension.Domain.Code)} _{MakeCamelName(dimension.Code)};");
            writer.WriteLine($@"public {MakeCamelName(dimension.Container.Code + "_" + dimension.Domain.Code)} {MakeCamelName(dimension.Code)} {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _{MakeCamelName(dimension.Code)}; }}");
            writer.WriteLine($@"set {{ _{MakeCamelName(dimension.Code)} = value; }}");
            writer.Indent--;
            writer.WriteLine($@"}}");

        }

        private static void CodeGenerateDataPoint(IndentedTextWriter writer, MdfCoreTemplate tmpl) {

            writer.WriteLine();
            writer.WriteLine("[DC.DomainComponent]");
            writer.WriteLine($@"public class {MakeCamelName(tmpl.Container.Code)}DataPoint: MdfReportDataPoint<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint>  {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}DataPoint({MakeCamelName(tmpl.Container.Code)}ReportCore report_core, {MakeCamelName(tmpl.Container.Code)}CategoryValue value): base(report_core, value) {{ }}");
            writer.WriteLine();
            writer.WriteLine("public MdfDataType DCDataType {");
            writer.Indent++;
            writer.WriteLine("get { return DataType; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public Object DCValue {");
            writer.Indent++;
            writer.WriteLine("get { return Value; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("public String Formula {");
            writer.Indent++;
            writer.WriteLine("get { return Calc?.Formula; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine("[ExpandObjectMembers(ExpandObjectMembers.Always)]");
            writer.WriteLine($"public {MakeCamelName(tmpl.Container.Code)}CategoryValue DCCategoryValue {{");
            writer.Indent++;
            writer.WriteLine("get { return CategoryValue; }");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();
            writer.WriteLine($"public IReadOnlyList<{MakeCamelName(tmpl.Container.Code)}DataPoint> CalcData {{");
            writer.Indent++;
            writer.WriteLine("get {");
            writer.Indent++;
            writer.WriteLine($"var result = new List<{MakeCamelName(tmpl.Container.Code)}DataPoint>(32);");
            writer.WriteLine("if (Calc?.Links != null) {");
            writer.Indent++;
            writer.WriteLine("foreach (var calc_link in Calc?.Links.Values) {");
            writer.Indent++;
            writer.WriteLine("foreach (var data_point in calc_link.DataPointList) {");
            writer.Indent++;
            writer.WriteLine("result.Add(data_point);");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine("return result;");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine("}");
            writer.Indent--;
            writer.WriteLine();
            writer.WriteLine("}");
        }

        private static void CodeGenerateDataPointCalc(IndentedTextWriter writer, MdfCoreTemplate tmpl) {

            writer.WriteLine();
            writer.WriteLine("[DC.DomainComponent]");
            writer.WriteLine($@"public class {MakeCamelName(tmpl.Container.Code)}DataPointCalc: MdfReportDataPointCalc<{MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)},{MakeCamelName(tmpl.Container.Code)}CategoryValue,{MakeCamelName(tmpl.Container.Code)}Table,{MakeCamelName(tmpl.Container.Code)}TableCell,{MakeCamelName(tmpl.Container.Code)}DataPoint>  {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code)}DataPointCalc({MakeCamelName(tmpl.Container.Code)}DataPoint data_point, CalcExpression exp ): base(data_point, exp) {{ }}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");

        }

        private static void CodeGenerateDomainRef(IndentedTextWriter writer, MdfCoreDimension dimension) {
            writer.WriteLine();
            writer.WriteLine($@"private {MakeCamelName(dimension.Container.Code + "_" + dimension.Domain.Code)} _{MakeCamelName(dimension.Code)};");
            writer.WriteLine($@"public {MakeCamelName(dimension.Container.Code + "_" + dimension.Domain.Code)} {MakeCamelName(dimension.Code)} {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _{MakeCamelName(dimension.Code)}; }}");
            writer.WriteLine($@"set {{ SetPropertyValue(ref _{MakeCamelName(dimension.Code)}, value); }}");
            writer.Indent--;
            writer.WriteLine($@"}}");

        }

        private static String MakeCamelName(String name) {
            return String.Join(String.Empty, name.Trim().Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Char.ToUpper(x[0]) + x.Substring(1).ToLower()));
        }

        private static void CodeGeneratePersistentTable(IndentedTextWriter writer, MdfCoreTemplate tmpl, MdfTemplateTable table) {

            String class_name = $@"{MakeCamelName(tmpl.Container.Code + "_DATA_" + table.Table.Code)}";
            writer.WriteLine();
            writer.WriteLine($@"[Persistent(""{tmpl.PersistentTablePrefix}{class_name}"")]");
            writer.WriteLine($@"public partial class {class_name} : MdfContainerObject {{");
            writer.Indent++;
            writer.WriteLine();
            writer.WriteLine($@"private {MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} _{MakeCamelName(tmpl.DomainReport.Code)};");
            writer.WriteLine($@"[Association]");
            writer.WriteLine($@"[Browsable(false)]");
            writer.WriteLine($@"public {MakeCamelName(tmpl.Container.Code + "_" + tmpl.DomainReport.Code)} {MakeCamelName(tmpl.DomainReport.Code)} {{");
            writer.Indent++;
            writer.WriteLine($@"get {{ return _{MakeCamelName(tmpl.DomainReport.Code)}; }}");
            writer.WriteLine($@"set {{");
            writer.Indent++;
            writer.WriteLine($@"if (SetPropertyValue(ref _{MakeCamelName(tmpl.DomainReport.Code)}, value) && !IsLoading && value != null) {{");
            writer.Indent++;
            writer.WriteLine($@"Container = value;");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            writer.Indent--;
            writer.WriteLine($@"}}");
            List<MdfCoreDimension> row_dims = new List<MdfCoreDimension>(16);
            foreach (var row_ord in table.Table.RowAxis.Axis.OrdinateLine) {
                foreach (var field in row_ord.CategoryMember.CategoryMemberFields) {
                    if (field.DimensionMember.DomainMember.CalcType == MdfCoreDomainMemberCalcType.CALCULATED)
                        continue;
                    if (!row_dims.Contains(field.CategoryTypeField.Dimension)) {
                        row_dims.Add(field.CategoryTypeField.Dimension);
                    }
                }
            }
            foreach (var row_ord in table.Table.RowAxis.Axis.Ordinates) {
                if (row_ord.IsForcePersistent) {
                    if (!row_dims.Contains(row_ord.Dimension)) {
                        row_dims.Add(row_ord.Dimension);
                    }
                }
            }
            foreach (var dim in row_dims) {
                String data_type_name = MakeCamelName(tmpl.Container.Code + "_" + dim.Domain.Code);
                String col_name = MakeCamelName(dim.Code);
                writer.WriteLine();
                if (dim.Domain.DomainType != MdfCoreDomain.MdfCoreDomainType.DOMAIN_REPORT) {
                    writer.WriteLine($@"private {data_type_name} _{col_name};");
                    writer.WriteLine($@"public {data_type_name} {col_name} {{");
                    writer.Indent++;
                    writer.WriteLine($@"get {{ return _{col_name}; }}");
                    writer.WriteLine($@"set {{ SetPropertyValue(ref _{col_name}, value); }}");
                    writer.Indent--;
                    writer.WriteLine("}");
                }
                else {
                    writer.WriteLine($@"[PersistentAlias(nameof({MakeCamelName(tmpl.DomainReport.Code)}))]");
                    writer.WriteLine($@"public {data_type_name} {col_name} {{");
                    writer.Indent++;
                    writer.WriteLine($@"get {{ return {MakeCamelName(tmpl.DomainReport.Code)}; }}");
                    writer.WriteLine($@"set {{ {MakeCamelName(tmpl.DomainReport.Code)} = value; }}");
                    writer.Indent--;
                    writer.WriteLine("}");
                }
            }
            foreach (var col_ord in table.Table.ColumnAxis.Axis.OrdinateLine) {
                String data_type_name = DataPointTypeNameCheck(col_ord.CategoryMember);
                String col_name = MakeCamelName(col_ord.Code);
                writer.WriteLine();
                writer.WriteLine($@"private {data_type_name} _{col_name};");
                writer.WriteLine($@"public {data_type_name} {col_name} {{");
                writer.Indent++;
                writer.WriteLine($@"get {{ return _{col_name}; }}");
                writer.WriteLine($@"set {{ SetPropertyValue(ref _{col_name}, value); }}");
                writer.Indent--;
                writer.WriteLine("}");
            }
            writer.WriteLine();
            writer.WriteLine($@"public {class_name}(Session session) : base(session) {{ }}");
            writer.WriteLine();
            writer.Indent--;
            writer.WriteLine("}");
        }

        public static String DataPointTypeNameCheck(MdfCoreCategoryMember category) {
            foreach (var field in category.CategoryMemberFields) {
                if (field.DimensionMember.DomainMember.DataPointDataType != MdfCoreDataType.DT_UNDEFINED) {
                    switch (field.DimensionMember.DomainMember.DataPointDataType) {
                        case MdfCoreDataType.DT_DECIMAL:
                        case MdfCoreDataType.DT_MONETARY:
                            return "System.Decimal";
                    }
                }
            }
            return "Unknow";
        }

        public static MdfCoreDataType DataPointTypeCheck(MdfCoreCategoryMember category) {
            foreach (var field in category.CategoryMemberFields) {
                if (field.DimensionMember.DomainMember.DataPointDataType != MdfCoreDataType.DT_UNDEFINED) {
                    return field.DimensionMember.DomainMember.DataPointDataType;
                }
            }
            return MdfCoreDataType.DT_UNDEFINED;
        }

        public static void CodeGenerateCalcVariant(IndentedTextWriter writer, MdfCoreTemplate tmpl) {
            writer.WriteLine();
            writer.WriteLine($"public enum {MakeCamelName(tmpl.Container.Code)}CalcVariant {{");
            foreach (var variant in tmpl.Container.CalcVariants) {
                writer.Indent++;
                writer.WriteLine($"{variant.Code} = {variant.Oid:D},");
                writer.Indent--;
            }
            writer.WriteLine("}");
        }
    }
}
