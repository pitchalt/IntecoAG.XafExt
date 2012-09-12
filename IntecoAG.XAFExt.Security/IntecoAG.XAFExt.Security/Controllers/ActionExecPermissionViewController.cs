using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.SystemModule;

namespace IntecoAG.XAFExt.Security.Controllers {

    public partial class ActionExecPermissionViewController : ActionsCriteriaViewController {

        public ActionExecPermissionViewController() {
            InitializeComponent();
            RegisterActions(components);
        }

        public const string EnabledByActionExecPermissionKey = "By Action Exec Permission";

        protected override void UpdateAction(ActionBase action, string criteria) {
            base.UpdateAction(action, criteria);

            // Ó÷¸ò ActionExexPermission
            if (!ActionExecPermissionLogic.IsGrantedActionExec(View, action)) {
                    //action.Active[EnabledByActionExecPermissionKey] = false;
                    action.Enabled[EnabledByActionExecPermissionKey] = false;
            } else {
                //if (action.Active.Contains(EnabledByActionExecPermissionKey))
                //    action.Active.RemoveItem(EnabledByActionExecPermissionKey);
                if (action.Enabled.Contains(EnabledByActionExecPermissionKey))
                    action.Enabled.RemoveItem(EnabledByActionExecPermissionKey);
            }
        }
    }
}
