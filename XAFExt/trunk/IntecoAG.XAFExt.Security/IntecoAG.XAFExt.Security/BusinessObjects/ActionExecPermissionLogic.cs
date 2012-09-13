using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
//

namespace IntecoAG.XAFExt.Security {

    /// <summary>
    /// Класс, инкапсулирующий методы, применяемые в контроллере ActionExecPermissionViewController
    /// </summary>
    public static class ActionExecPermissionLogic {

        //public ActionExecPermissionHelper(Type objectType, String targetAction, String criteria, String operation, PermissionAccessTypes permissionAccessType) {
        //}

        public static Boolean IsGrantedActionExec(View view, ActionBase action) {
            Object currentObject = view.CurrentObject;
            if (currentObject != null) {
                Type currentObjectType = currentObject.GetType();
                String ActionId = action.Id;
                String operation = SecurityActionExecOperations.Exec.ToString();
                Boolean isGrantedBySecurity = SecuritySystem.IsGranted(new ActionExecPermissionRequest(currentObject, currentObjectType, ActionId, operation));
                return isGrantedBySecurity;
            }
            return false;
        }

        public static List<String> GetActionIdList(IRequestSecurityStrategy securityInstance) {
            List<String> actionIds = new List<string>();

            if (securityInstance == null)
                return actionIds;

            foreach (IOperationPermission perm in securityInstance.Permissions) {

                ActionExecPermission ap = perm as ActionExecPermission;
                if (ap == null
                    || ap.PermissionAccessType == PermissionAccessTypes.UNDEFINED
                    //|| ap.Operation != SecurityActionExecOperations.Exec.ToString()
                    )
                    continue;

                if (!actionIds.Contains(ap.TargetAction)) {
                    actionIds.Add(ap.TargetAction);
                }
            }

            return actionIds;
        }
    }
}
