using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.SystemModule;

namespace IntecoAG.XAFExt.Security.Controllers {

    /*
        // Версия контроллера, образованная наследованием с отключением родительского контролллера
        // Просто подключиться к событию проверки Action нельзя, т.к. такого события нет
        public partial class ActionExecPermissionViewController : ActionsCriteriaViewController {

            public ActionExecPermissionViewController() {
                InitializeComponent();
                RegisterActions(components);
            }

            public const string EnabledByActionExecPermissionKey = "By Action Exec Permission";
            ActionsCriteriaViewController actionsCriteriaViewController;

            protected override void OnFrameAssigned() {
                 base.OnFrameAssigned();

                actionsCriteriaViewController = Frame.GetController<ActionsCriteriaViewController>();
                if (actionsCriteriaViewController != null) {
                    actionsCriteriaViewController.Active[EnabledByActionExecPermissionKey] = false;
                }
            }

            protected override void OnDeactivated() {
                base.OnDeactivated();
                if (actionsCriteriaViewController != null && actionsCriteriaViewController.Active.Contains(EnabledByActionExecPermissionKey)) {
                    actionsCriteriaViewController.Active.RemoveItem(EnabledByActionExecPermissionKey);
                }
            }

            protected override void UpdateAction(ActionBase action, string criteria) {
                base.UpdateAction(action, criteria);

                // Учёт ActionExexPermission

            
                //if (!ActionExecPermissionLogic.IsGrantedActionExec(View, action)) {
                //        //action.Active[EnabledByActionExecPermissionKey] = false;
                //        action.Enabled[EnabledByActionExecPermissionKey] = false;
                //} else {
                //    //if (action.Active.Contains(EnabledByActionExecPermissionKey))
                //    //    action.Active.RemoveItem(EnabledByActionExecPermissionKey);
                //    if (action.Enabled.Contains(EnabledByActionExecPermissionKey))
                //        action.Enabled.RemoveItem(EnabledByActionExecPermissionKey);
                //}
            
            
                bool enable = true;
                if (View != null) {
                    IList selectedObjects = ObjectView.SelectedObjects;
                    if (selectedObjects == null || selectedObjects.Count == 0) {
                        enable = false;
                    } else {
                        foreach (object obj in selectedObjects) {
                            if ((!ActionExecPermissionLogic.IsGrantedActionExec(View, action)) {
                                enable = false;
                                break;
                            }
                        }
                    }
                }
                action.Enabled.SetItemValue(EnabledByActionExecPermissionKey, enable);
            }
        }
    */

    public partial class ActionExecPermissionViewController : ViewController {

        public ActionExecPermissionViewController() {
            InitializeComponent();
            RegisterActions(components);
            TypeOfView = typeof(ObjectView);
        }

        public const string EnabledByActionExecPermissionKey = "By Action Exec Permission";
        private ICollection<ActionBase> actions;
        private List<ActionBase> actionBaseList;
        private List<String> actionIds;

        public ObjectView ObjectView {
            get {
                return (ObjectView)base.View;
            }
        }

        #region OnActivated - OnDeactivated
        protected override void OnActivated() {
            base.OnActivated();
            Frame.ViewChanged += new EventHandler<ViewChangedEventArgs>(Frame_ViewChanged);
            ObjectView.SelectionChanged += new EventHandler(View_SelectionChanged);
            ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            ObjectSpace.ObjectReloaded += new EventHandler<ObjectManipulatingEventArgs>(ObjectSpace_ObjectReloaded);
            UpdateActions();
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            ClearEventActions();
            Frame.ViewChanged -= new EventHandler<ViewChangedEventArgs>(Frame_ViewChanged);
            ObjectView.SelectionChanged -= new EventHandler(View_SelectionChanged);
            ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            ObjectSpace.ObjectReloaded -= new EventHandler<ObjectManipulatingEventArgs>(ObjectSpace_ObjectReloaded);
        }
        #endregion

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            UpdateActions();
        }
        private void ObjectSpace_ObjectReloaded(object sender, ObjectManipulatingEventArgs e) {
            UpdateActions();
        }
        private void View_SelectionChanged(object sender, EventArgs e) {
            UpdateActions();
        }
        private void Frame_ViewChanged(object sender, ViewChangedEventArgs e) {
            Frame.ViewChanged -= new EventHandler<ViewChangedEventArgs>(Frame_ViewChanged);
            UpdateActions();
        }
        private void ClearEventActions() {
            if (actions != null) {
                foreach (ActionBase action in actions) {
                    action.Changed -= new EventHandler<ActionChangedEventArgs>(action_Changed);
                }
                actions.Clear();
                actions = null;
            }
            if (actionBaseList != null) {
                actionBaseList.Clear();
                actionBaseList = null;
            }
        }
        private void action_Changed(object sender, ActionChangedEventArgs e) {
            ActionBase action = sender as ActionBase;
            //if (actionBaseList.Contains(action)) {
            //    actionBaseList.Remove(action);
            //}
            UpdateAction(action);
        }
        private void UpdateActions() {
            //List<ActionBase> actionBases = CollectActions();
            CollectActions();
            foreach (ActionBase action in actionBaseList) {   //actionBases) {
                UpdateAction(action);
            }
        }
        protected ICollection<ActionBase> CollectAllActions() {
            // Собираются только те Action, которые упоминаются в списке настроек безопасности ActionExecPermission
            if (actionIds != null) {
                actionIds.Clear();
                actionIds = null;
            }
            actionIds = ActionExecPermissionLogic.GetActionIdList(SecuritySystem.Instance as IRequestSecurityStrategy);
            List<ActionBase> result = new List<ActionBase>();
            if (Frame != null) {
                foreach (Controller controller in Frame.Controllers) {
                    if (controller is ViewController) {
                        ViewController viewController = (ViewController)controller;
                        if ((viewController.TargetObjectType == null)
                                || viewController.TargetObjectType.IsAssignableFrom(((ObjectView)View).ObjectTypeInfo.Type)) {
                            foreach (ActionBase action in controller.Actions) {
                                if (!actionIds.Contains(action.Id))
                                    continue;
                                if ((action.TargetObjectType == null)
                                        || action.TargetObjectType.IsAssignableFrom(((ObjectView)View).ObjectTypeInfo.Type)) {
                                    result.Add(action);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        protected List<ActionBase> CollectActions() {
            if (actionBaseList == null) {
                actionBaseList = new List<ActionBase>();
            }
            if (ObjectSpace != null) {
                if (actions == null) {
                    actions = CollectAllActions();
                }
                foreach (ActionBase action in actions) {
                    //action.Changed -= new EventHandler<ActionChangedEventArgs>(action_Changed);
                    //action.Changed += new EventHandler<ActionChangedEventArgs>(action_Changed);
                    if (!actionBaseList.Contains(action)) {
                        action.Changed += new EventHandler<ActionChangedEventArgs>(action_Changed);
                        actionBaseList.Add(action);
                    }
                }
            }
            //}
            return actionBaseList;
        }
        protected void UpdateAction(ActionBase action) {
            // Учёт ActionExecPermission

            /*
            if (!ActionExecPermissionLogic.IsGrantedActionExec(View, action)) {
                //action.Active[EnabledByActionExecPermissionKey] = false;
                action.Enabled[EnabledByActionExecPermissionKey] = false;
            } else {
                //if (action.Active.Contains(EnabledByActionExecPermissionKey))
                //    action.Active.RemoveItem(EnabledByActionExecPermissionKey);
                if (action.Enabled.Contains(EnabledByActionExecPermissionKey))
                    action.Enabled.RemoveItem(EnabledByActionExecPermissionKey);
            }
            */

            bool enable = true;
            if (View != null) {
                IList selectedObjects = ObjectView.SelectedObjects;
                if (selectedObjects == null || selectedObjects.Count == 0) {
                    enable = false;
                } else {
                    foreach (object obj in selectedObjects) {
                        if (!ActionExecPermissionLogic.IsGrantedActionExec(View, action)) {
                            enable = false;
                            break;
                        }
                    }
                }
            }
            //action.Active.SetItemValue(EnabledByActionExecPermissionKey, enable);
            if (action.Enabled.Contains(EnabledByActionExecPermissionKey) && action.Enabled[EnabledByActionExecPermissionKey] != enable) {
                action.Enabled.SetItemValue(EnabledByActionExecPermissionKey, enable);
            } else if (!action.Enabled.Contains(EnabledByActionExecPermissionKey) && !enable) {
                action.Enabled.SetItemValue(EnabledByActionExecPermissionKey, enable);
            }
        }
    }

}
