namespace NpoMash.Erm.Mdf.Metadata.Templates {
    partial class MdfMetadataWindowController {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            this.components = new System.ComponentModel.Container();
            this.DBMetadataBackupAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DBMetadataRestoreAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DBMetadataBackupAction
            // 
            this.DBMetadataBackupAction.Caption = "DBMetadataBackup";
            this.DBMetadataBackupAction.Category = "Tools";
            this.DBMetadataBackupAction.ConfirmationMessage = null;
            this.DBMetadataBackupAction.Id = "MdfMetadataWindowController_DBMetadataBackupAction";
            this.DBMetadataBackupAction.ToolTip = null;
            this.DBMetadataBackupAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DBMetadataBackupAction_Execute);
            // 
            // DBMetadataRestoreAction
            // 
            this.DBMetadataRestoreAction.Caption = "DBMetadataRestore";
            this.DBMetadataRestoreAction.Category = "Tools";
            this.DBMetadataRestoreAction.ConfirmationMessage = null;
            this.DBMetadataRestoreAction.Id = "MdfMetadataWindowController_DBMetadataRestoreAction";
            this.DBMetadataRestoreAction.ToolTip = null;
            this.DBMetadataRestoreAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DBMetadataRestoreAction_Execute);
            // 
            // MdfMetadataWindowController
            // 
            this.Actions.Add(this.DBMetadataBackupAction);
            this.Actions.Add(this.DBMetadataRestoreAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction DBMetadataBackupAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DBMetadataRestoreAction;
    }
}
