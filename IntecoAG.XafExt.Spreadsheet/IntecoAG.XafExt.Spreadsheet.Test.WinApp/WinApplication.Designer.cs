namespace IntecoAG.XafExt.Spreadsheet.Test.WinApp.Win {
    partial class WinAppWindowsFormsApplication {
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
            this.treeListEditorsModuleBase = new DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase();
            this.treeListEditorsWindowsFormsModule = new DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule();
            this.iagXafExtSpreadsheetModuleWin1 = new IntecoAG.XafExt.Spreadsheet.Module.Win.IagXafExtSpreadsheetModuleWin();
            this.iagXafExtSpreadsheetModule1 = new IntecoAG.XafExt.Spreadsheet.Module.IagXafExtSpreadsheetModule();
            this.fileAttachmentsWindowsFormsModule1 = new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule();
            this.winModule1 = new IntecoAG.XafExt.Spreadsheet.Test.Win.WinModule();
            this.cloneObjectModule1 = new DevExpress.ExpressApp.CloneObject.CloneObjectModule();
            this.conditionalAppearanceModule1 = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.validationModule1 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.viewVariantsModule1 = new DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule();
            this.multiDimFormsModule1 = new IntecoAG.XafExt.MultiDimForms.MultiDimFormsModule();
            this.multiDimFormsCoreModule1 = new IntecoAG.XafExt.MultiDimForms.Core.MultiDimFormsCoreModule();
            this.testModule1 = new IntecoAG.XafExt.Spreadsheet.Test.TestModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // validationModule1
            // 
            this.validationModule1.AllowValidationDetailsAccess = true;
            this.validationModule1.IgnoreWarningAndInformationRules = false;
            // 
            // WinAppWindowsFormsApplication
            // 
            this.ApplicationName = "IntecoAG.XafExt.Spreadsheet.Test.WinApp";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.treeListEditorsModuleBase);
            this.Modules.Add(this.treeListEditorsWindowsFormsModule);
            this.Modules.Add(this.iagXafExtSpreadsheetModule1);
            this.Modules.Add(this.fileAttachmentsWindowsFormsModule1);
            this.Modules.Add(this.iagXafExtSpreadsheetModuleWin1);
            this.Modules.Add(this.cloneObjectModule1);
            this.Modules.Add(this.conditionalAppearanceModule1);
            this.Modules.Add(this.validationModule1);
            this.Modules.Add(this.viewVariantsModule1);
            this.Modules.Add(this.multiDimFormsModule1);
            this.Modules.Add(this.multiDimFormsCoreModule1);
            this.Modules.Add(this.testModule1);
            this.Modules.Add(this.winModule1);
            this.UseOldTemplates = false;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.WinAppWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.WinAppWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase treeListEditorsModuleBase;
        private DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule treeListEditorsWindowsFormsModule;
        private Module.Win.IagXafExtSpreadsheetModuleWin iagXafExtSpreadsheetModuleWin1;
        private Module.IagXafExtSpreadsheetModule iagXafExtSpreadsheetModule1;
        private DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule fileAttachmentsWindowsFormsModule1;
        private Test.Win.WinModule winModule1;
        private DevExpress.ExpressApp.CloneObject.CloneObjectModule cloneObjectModule1;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule1;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule1;
        private DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule viewVariantsModule1;
        private XafExt.MultiDimForms.MultiDimFormsModule multiDimFormsModule1;
        private XafExt.MultiDimForms.Core.MultiDimFormsCoreModule multiDimFormsCoreModule1;
        private TestModule testModule1;
    }
}
