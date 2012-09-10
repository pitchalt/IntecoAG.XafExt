namespace IntecoAG.XAFExt.Security.Tests.Module.Controllers {
    partial class FictionBookViewController {
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
            this.FictionAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FictionAction2 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FictionAction1
            // 
            this.FictionAction1.Caption = "Обгадить";
            this.FictionAction1.Category = "View";
            this.FictionAction1.ConfirmationMessage = null;
            this.FictionAction1.Id = "FictionBookViewController_FictionAction1";
            this.FictionAction1.ImageName = null;
            this.FictionAction1.Shortcut = null;
            this.FictionAction1.Tag = null;
            this.FictionAction1.TargetObjectsCriteria = null;
            this.FictionAction1.TargetViewId = null;
            this.FictionAction1.ToolTip = null;
            this.FictionAction1.TypeOfView = null;
            this.FictionAction1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FictionAction1_Execute);
            // 
            // FictionAction2
            // 
            this.FictionAction2.Caption = "Реабилитировать";
            this.FictionAction2.Category = "View";
            this.FictionAction2.ConfirmationMessage = null;
            this.FictionAction2.Id = "FictionBookViewController_FictionAction2";
            this.FictionAction2.ImageName = null;
            this.FictionAction2.Shortcut = null;
            this.FictionAction2.Tag = null;
            this.FictionAction2.TargetObjectsCriteria = null;
            this.FictionAction2.TargetViewId = null;
            this.FictionAction2.ToolTip = null;
            this.FictionAction2.TypeOfView = null;
            this.FictionAction2.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FictionAction2_Execute);
            // 
            // FictionBookViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.XAFExt.Security.Tests.Module.BusinessObjects.FictionBook);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FictionAction1;
        private DevExpress.ExpressApp.Actions.SimpleAction FictionAction2;
    }
}
