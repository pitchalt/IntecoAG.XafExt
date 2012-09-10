namespace IntecoAG.XAFExt.Security.Tests.Module.Controllers {
    partial class BookViewController {
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
            this.Action1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Action2 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Action3 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Action1
            // 
            this.Action1.Caption = "Прочитать";
            this.Action1.Category = "View";
            this.Action1.ConfirmationMessage = null;
            this.Action1.Id = "BookViewController_Action1";
            this.Action1.ImageName = null;
            this.Action1.Shortcut = null;
            this.Action1.Tag = null;
            this.Action1.TargetObjectsCriteria = null;
            this.Action1.TargetViewId = null;
            this.Action1.ToolTip = null;
            this.Action1.TypeOfView = null;
            this.Action1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Action1_Execute);
            // 
            // Action2
            // 
            this.Action2.Caption = "Порвать";
            this.Action2.Category = "View";
            this.Action2.ConfirmationMessage = null;
            this.Action2.Id = "BookViewController_Action2";
            this.Action2.ImageName = null;
            this.Action2.Shortcut = null;
            this.Action2.Tag = null;
            this.Action2.TargetObjectsCriteria = null;
            this.Action2.TargetViewId = null;
            this.Action2.ToolTip = null;
            this.Action2.TypeOfView = null;
            this.Action2.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Action2_Execute);
            // 
            // Action3
            // 
            this.Action3.Caption = "Сжечь";
            this.Action3.Category = "View";
            this.Action3.ConfirmationMessage = null;
            this.Action3.Id = "BookViewController_Action3";
            this.Action3.ImageName = null;
            this.Action3.Shortcut = null;
            this.Action3.Tag = null;
            this.Action3.TargetObjectsCriteria = null;
            this.Action3.TargetViewId = null;
            this.Action3.ToolTip = null;
            this.Action3.TypeOfView = null;
            this.Action3.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Action3_Execute);
            // 
            // BookViewController
            // 
            this.TargetObjectType = typeof(IntecoAG.XAFExt.Security.Tests.Module.BusinessObjects.Book);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Action1;
        private DevExpress.ExpressApp.Actions.SimpleAction Action2;
        private DevExpress.ExpressApp.Actions.SimpleAction Action3;
    }
}
