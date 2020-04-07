namespace NpoMash.Erm.Mdf.Test.Win {
    partial class TestWindowsFormsApplication {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.cloneObjectModule = new DevExpress.ExpressApp.CloneObject.CloneObjectModule();
            this.conditionalAppearanceModule = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.fileAttachmentsWindowsFormsModule = new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule();
            this.treeListEditorsModuleBase = new DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase();
            this.treeListEditorsWindowsFormsModule = new DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationWindowsFormsModule = new DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule();
            this.viewVariantsModule = new DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule();
            this.multiDimFormsModule1 = new IntecoAG.XafExt.MultiDimForms.MultiDimFormsModule();
            this.testWindowsFormsModule1 = new NpoMash.Erm.Mdf.Test.Module.Win.TestWindowsFormsModule();
            this.iagXafExtSpreadsheetModule1 = new IntecoAG.XafExt.Spreadsheet.Module.IagXafExtSpreadsheetModule();
            this.iagXafExtSpreadsheetModuleWin1 = new IntecoAG.XafExt.Spreadsheet.Module.Win.IagXafExtSpreadsheetModuleWin();
            this.testModule1 = new IntecoAG.XafExt.Spreadsheet.TestModule();
            this.businessClassLibraryCustomizationModule1 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.chartModule1 = new DevExpress.ExpressApp.Chart.ChartModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.kpiModule1 = new DevExpress.ExpressApp.Kpi.KpiModule();
            this.xafExtCDSModule1 = new IntecoAG.XafExt.CDS.XAFExtCDSModule();
            this.stateMachineModule1 = new DevExpress.ExpressApp.StateMachine.StateMachineModule();
            this.reportsModule1 = new DevExpress.ExpressApp.Reports.ReportsModule();
            this.reportsModuleV21 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.ermBaseModule1 = new IntecoAG.ERM.Module.ERMBaseModule();
            this.pivotGridModule1 = new DevExpress.ExpressApp.PivotGrid.PivotGridModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // validationModule
            // 
            this.validationModule.AllowValidationDetailsAccess = true;
            this.validationModule.IgnoreWarningAndInformationRules = false;
            // 
            // stateMachineModule1
            // 
            this.stateMachineModule1.StateMachineStorageType = typeof(DevExpress.ExpressApp.StateMachine.Xpo.XpoStateMachine);
            // 
            // reportsModule1
            // 
            this.reportsModule1.EnableInplaceReports = true;
            this.reportsModule1.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportData);
            // 
            // reportsModuleV21
            // 
            this.reportsModuleV21.EnableInplaceReports = true;
            this.reportsModuleV21.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
            // 
            // ermBaseModule1
            // 
            this.ermBaseModule1.AdditionalControllerTypes.Add(typeof(IntecoAG.XafExt.Bpmn.XafExtBpmnAcceptableObjectController));
            // 
            // TestWindowsFormsApplication
            // 
            this.ApplicationName = "NpoMash.Erm.Mdf.Test";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.cloneObjectModule);
            this.Modules.Add(this.conditionalAppearanceModule);
            this.Modules.Add(this.fileAttachmentsWindowsFormsModule);
            this.Modules.Add(this.treeListEditorsModuleBase);
            this.Modules.Add(this.treeListEditorsWindowsFormsModule);
            this.Modules.Add(this.validationModule);
            this.Modules.Add(this.validationWindowsFormsModule);
            this.Modules.Add(this.viewVariantsModule);
            this.Modules.Add(this.multiDimFormsModule1);
            this.Modules.Add(this.iagXafExtSpreadsheetModule1);
            this.Modules.Add(this.iagXafExtSpreadsheetModuleWin1);
            this.Modules.Add(this.businessClassLibraryCustomizationModule1);
            this.Modules.Add(this.chartModule1);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.kpiModule1);
            this.Modules.Add(this.xafExtCDSModule1);
            this.Modules.Add(this.stateMachineModule1);
            this.Modules.Add(this.reportsModule1);
            this.Modules.Add(this.reportsModuleV21);
            this.Modules.Add(this.ermBaseModule1);
            this.Modules.Add(this.pivotGridModule1);
            this.Modules.Add(this.testModule1);
            this.Modules.Add(this.testWindowsFormsModule1);
            this.UseOldTemplates = false;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.TestWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.TestWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private DevExpress.ExpressApp.CloneObject.CloneObjectModule cloneObjectModule;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule;
        private DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule fileAttachmentsWindowsFormsModule;
        private DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase treeListEditorsModuleBase;
        private DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule treeListEditorsWindowsFormsModule;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;
        private DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule validationWindowsFormsModule;
        private DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule viewVariantsModule;
        private IntecoAG.XafExt.MultiDimForms.MultiDimFormsModule multiDimFormsModule1;
        private Module.Win.TestWindowsFormsModule testWindowsFormsModule1;
        private IntecoAG.XafExt.Spreadsheet.Module.IagXafExtSpreadsheetModule iagXafExtSpreadsheetModule1;
        private IntecoAG.XafExt.Spreadsheet.Module.Win.IagXafExtSpreadsheetModuleWin iagXafExtSpreadsheetModuleWin1;
        private IntecoAG.XafExt.Spreadsheet.TestModule testModule1;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule businessClassLibraryCustomizationModule1;
        private DevExpress.ExpressApp.Chart.ChartModule chartModule1;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Kpi.KpiModule kpiModule1;
        private IntecoAG.XafExt.CDS.XAFExtCDSModule xafExtCDSModule1;
        private DevExpress.ExpressApp.StateMachine.StateMachineModule stateMachineModule1;
        private DevExpress.ExpressApp.Reports.ReportsModule reportsModule1;
        private DevExpress.ExpressApp.ReportsV2.ReportsModuleV2 reportsModuleV21;
        private IntecoAG.ERM.Module.ERMBaseModule ermBaseModule1;
        private DevExpress.ExpressApp.PivotGrid.PivotGridModule pivotGridModule1;
    }
}
