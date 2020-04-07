namespace IntecoAG.XafExt.RefReplace.Controllers {
    partial class SearchViewController {
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
            this.simpleTestAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.showOldAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.showNewAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.singleChoiceAction1 = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.simpleAction1 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ShowLookupAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // simpleTestAction1
            // 
            this.simpleTestAction1.Caption = "Найти ссылки";
            this.simpleTestAction1.Category = "Tools";
            this.simpleTestAction1.ConfirmationMessage = null;
            this.simpleTestAction1.Id = "d8d5a899-03c5-40a7-93ea-08ef77b46ce5";
            this.simpleTestAction1.ToolTip = null;
            this.simpleTestAction1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleTestAction1_Execute);
            // 
            // showOldAction
            // 
            this.showOldAction.AcceptButtonCaption = null;
            this.showOldAction.CancelButtonCaption = null;
            this.showOldAction.Caption = "Показать текущий";
            this.showOldAction.Category = "myCategory";
            this.showOldAction.ConfirmationMessage = null;
            this.showOldAction.Id = "68e40177-c409-469a-9747-5266262a03da";
            this.showOldAction.ToolTip = null;
            this.showOldAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.showOldAction_CustomizePopupWindowParams);
            this.showOldAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.showOldAction_Execute);
            // 
            // showNewAction
            // 
            this.showNewAction.AcceptButtonCaption = null;
            this.showNewAction.CancelButtonCaption = null;
            this.showNewAction.Caption = "Показать замену";
            this.showNewAction.Category = "newCategory";
            this.showNewAction.ConfirmationMessage = null;
            this.showNewAction.Id = "cc8ab73d-b5b6-41ec-9c49-7a63b846f73e";
            this.showNewAction.ToolTip = null;
            this.showNewAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.showNewAction_CustomizePopupWindowParams);
            // 
            // singleChoiceAction1
            // 
            this.singleChoiceAction1.Caption = "Сменить статус";
            this.singleChoiceAction1.Category = "myCategory";
            this.singleChoiceAction1.ConfirmationMessage = null;
            this.singleChoiceAction1.Id = "577a3528-e587-414c-97bd-6fdf783123c3";
            this.singleChoiceAction1.TargetObjectType = typeof(IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects.ReplaceTable);
            this.singleChoiceAction1.ToolTip = null;
            this.singleChoiceAction1.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.singleChoiceAction1_Execute);
            // 
            // simpleAction1
            // 
            this.simpleAction1.Caption = "Применить";
            this.simpleAction1.Category = "Tools";
            this.simpleAction1.ConfirmationMessage = null;
            this.simpleAction1.Id = "d83c0bff-3ded-48e7-b37f-580d52506fef";
            this.simpleAction1.TargetObjectType = typeof(IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects.ReplaceTable);
            this.simpleAction1.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.simpleAction1.ToolTip = null;
            this.simpleAction1.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.simpleAction1.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // ShowLookupAction
            // 
            this.ShowLookupAction.AcceptButtonCaption = null;
            this.ShowLookupAction.CancelButtonCaption = null;
            this.ShowLookupAction.Caption = "Заменить на";
            this.ShowLookupAction.Category = "myCategory";
            this.ShowLookupAction.ConfirmationMessage = null;
            this.ShowLookupAction.Id = "ShowLookup";
            this.ShowLookupAction.TargetObjectType = typeof(IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects.ReplaceTable);
            this.ShowLookupAction.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ShowLookupAction.ToolTip = null;
            this.ShowLookupAction.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ShowLookupAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ShowLookupAction_CustomizePopupWindowParams);
            this.ShowLookupAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ShowLookupAction_Execute);
            // 
            // SearchViewController
            // 
            this.Actions.Add(this.simpleAction1);
            this.Actions.Add(this.simpleTestAction1);
            this.Actions.Add(this.ShowLookupAction);
            this.Actions.Add(this.showOldAction);
            this.Actions.Add(this.showNewAction);
            this.Actions.Add(this.singleChoiceAction1);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ShowLookupAction;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleAction1;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleTestAction1;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction showOldAction;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction showNewAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction1;
    }
}
