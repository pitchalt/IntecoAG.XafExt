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
//

namespace IntecoAG.XAFExt.Security {

    /// <summary>
    /// Политика применения ограничений на выполнение Actions в отношении иерархии типов
    /// </summary>
    public enum ActionPermissionPolices {
        /// <summary>
        /// Разрешительная политика
        /// </summary>
        PERMITS = 1,
        /// <summary>
        /// Запретительная политика
        /// </summary>
        PROHIBITIVE = 2
    }

    public class ActionExecPermissionRequestProcessor : PermissionRequestProcessorBase<ActionExecPermissionRequest> {

        public ActionPermissionPolices actionPermissionPolice;   // Политика применения запрета/разрешения
        public ActionExecPermissionRequestProcessor() { }
        public ActionExecPermissionRequestProcessor(ActionPermissionPolices actionPermissionPolice) {
            this.actionPermissionPolice = actionPermissionPolice;
        }

        //private IObjectSpaceProvider objectSpaceProvider;
        //public ActionExecPermissionRequestProcessor(IObjectSpaceProvider objectSpaceProvider) {
        //    this.objectSpaceProvider = objectSpaceProvider;
        //}

        /// <summary>
        /// Проверка, запрета/разрешения action
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <param name="securityInstance"></param>
        /// <returns></returns>
        public override bool IsGranted(ActionExecPermissionRequest permissionRequest, IRequestSecurityStrategy securityInstance) {

            Boolean permissionPolice = (actionPermissionPolice == ActionPermissionPolices.PERMITS) ? true : false;
            if (permissionRequest == null)
                return permissionPolice;

            if (permissionRequest.TargetObject == null)
                return false;

            // Список всех накрывающих типов для типа permissionRequest.ObjectType
            List<Type> typeList = GetCoveringTypes(permissionRequest.ObjectType, null);

            // Идёт поиск до первого запрещающего Permision для данного объекта (и его типа)
            // Если запрещающего Permission нет, то доступность объекта определяется по значению политики

            Boolean res = true;
            foreach (IOperationPermission perm in securityInstance.Permissions) {

                ActionExecPermission ap = perm as ActionExecPermission;
                if (ap == null
                    || ap.PermissionAccessType == PermissionAccessTypes.UNDEFINED
                    || ap.Operation != permissionRequest.Operation
                    || ap.TargetAction != permissionRequest.TargetAction
                    || !typeList.Contains(ap.ObjectType)
                    )
                    continue;

                if (permissionRequest.FitEvaluator == null) {
                    IObjectSpace objectSpace = ObjectSpace.FindObjectSpaceByObject(permissionRequest.TargetObject);
                    // ??? objectSpace.IsObjectFitForCriteria(((ActionExecPermission)permission).ObjectType, permissionRequest.TargetObject, ((ActionExecPermission)permission).Criteria);
                    permissionRequest.FitEvaluator = new ObjectSpaceFitCriteriaProcessor(permissionRequest.TargetObject, objectSpace);   //objectSpaceProvider.CreateObjectSpace());
                    //return permissionRequest.FitEvaluator.Fit(actionExecPermission.Criteria);
                }
                if (permissionRequest.FitEvaluator != null && !permissionRequest.FitEvaluator.Fit(ap.Criteria)) {
                    continue;
                }

                if (ap.PermissionAccessType == PermissionAccessTypes.DENY) {
                    res = false;
                    break;
                }
            }

            return permissionPolice & res;
        }

        /// <summary>
        /// Проверка применимости permission
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <param name="permission"></param>
        /// <param name="securityInstance"></param>
        /// <returns></returns>
        protected override bool IsRequestFit(ActionExecPermissionRequest permissionRequest, OperationPermissionBase permission, IRequestSecurityStrategy securityInstance) {
            return false;
        }

        /// <summary>
        /// Получение списка накрывающих типов. Получение интерфейсов закомментарено.
        /// Список интерфейсов пока не поддерживается
        /// (возможно потребуется ф-я PatchObjectType(T))
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resultTypeList"></param>
        /// <returns></returns>
        private List<Type> GetCoveringTypes(Type type, List<Type> resultTypeList) {
            if (type == null)
                return resultTypeList;
            if (resultTypeList == null) {
                resultTypeList = new List<Type>();
            }

            // Типы
            resultTypeList.Add(type);
            //resultTypeList = GetCoveringTypes(type.BaseType, resultTypeList);
            GetCoveringTypes(type.BaseType, resultTypeList);

            /*
            // Интерфейсы. - На будущее, но тогда и xxxPermission и xxxPermissionData 
            // должны быть способны получать интерфейсы в выпадающем списке типов.
            foreach (Type IF in type.GetInterfaces()) {
                if (!resultTypeList.Contains(IF)) {
                    resultTypeList.Add(IF);
                }
            }
            */

            return resultTypeList;
        }
    }
}
