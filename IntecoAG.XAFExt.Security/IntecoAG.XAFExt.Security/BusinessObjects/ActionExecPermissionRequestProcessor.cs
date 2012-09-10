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
            // Принципы работы IsGranted
            // 1. Производится перебор по всем permission1, доступным через securityInstance
            // 2. Если permission1 не есть ActionExecPermission, то производится переход к анализу следующего permission1
            // 3. Имеем далее дело только с actionExecPermission
            // 4. Если тип actionExecPermission.ObjectType не содержится в совокупности предков типа permissionRequest.ObjectType, 
            //    то производится переход к анализу следующего permission1
            // 5. Если тип actionExecPermission.ObjectType содержится в совокупности предков типа permissionRequest.ObjectType, 
            //    то проверяется PermissionAccessTypes и, если оно есть UNDEFINED, то производится переход к анализу следующего permission1
            // 6. Если тип actionExecPermission.ObjectType содержится в совокупности предков типа permissionRequest.ObjectType, 
            //    но permissionRequest.FitEvaluator() выдал false, то производится переход к анализу следующего permission1 (т.е. не применяем 
            //    это ограничение в этом случае)
            // 7. Если тип actionExecPermission.ObjectType содержится в совокупности предков типа permissionRequest.ObjectType 
            //    и permissionRequest.FitEvaluator() выдал true, то проверяется состояние параметра PermissionAccessType:
            //    7.1. Если оно DENY, то выдаём на гора false (запрет выполнения Action)
            //    7.2. Если оно ALLOW, то выдаём на гора true (разрешаем выполнение Action)

            //if (securityInstance.IsGranted(new ActionExecPermissionRequest(permissionRequest.ObjectType, permissionRequest.TargetAction, permissionRequest.Operation))) {
            //    return true;
            //}

            // Список всех накрывающих типов для типа permissionRequest.ObjectType (возможно потребуется ф-я PatchObjectType(T))
            List<Type> typeList = GetCoveringTypes(permissionRequest.ObjectType);

            // Список всех подходящих ActionExecPermission
            //List<ActionExecPermission> actionExecPermissionList = GetSuitableActionExecPermissions(typeList, securityInstance);

            Boolean resolution = (actionPermissionPolice == ActionPermissionPolices.PERMITS) ? true : false;
            for (Int32 i = 0; i < typeList.Count; i++) {
                Type type = typeList[i];

                Boolean res = true;
                Boolean isExistsPermission = false;
                foreach (OperationPermissionBase perm in securityInstance.Permissions) {

                    if (!IsRequestFit(permissionRequest, perm, securityInstance))
                        continue;

                    ActionExecPermission ap = perm as ActionExecPermission;

                    if (type != ap.ObjectType)
                        continue;

                    /*
                    if (ap.PermissionAccessType == PermissionAccessTypes.ALLOW) {
                        res = res & true;
                    } else */
                    if (ap.PermissionAccessType == PermissionAccessTypes.DENY) {
                        res = false;
                    }

                }

                resolution = resolution & res;
                if (isExistsPermission && !resolution)
                    break;
            }

            return resolution;

            /*
            //return base.IsGranted(permissionRequest, securityInstance);
            if (actionPermissionPolice == ActionPermissionPolices.PERMITS) {
                return true;
            } else if (actionPermissionPolice == ActionPermissionPolices.PROHIBITIVE) {
                return false;
            }

            return false;
            */
        }

        /// <summary>
        /// Проверка применимости permission
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <param name="permission"></param>
        /// <param name="securityInstance"></param>
        /// <returns></returns>
        protected override bool IsRequestFit(ActionExecPermissionRequest permissionRequest, OperationPermissionBase permission, IRequestSecurityStrategy securityInstance) {
            // Правила определения применимости.
            // 1. Тип объекта, зафиксированный в actionExecPermission.ObjectType содержится во множестве предков типа permissionRequest.ObjectType или равен ему
            // 2. Совпадают лперации Operation в actionExecPermission и permissionRequest
            // 3. Совпадают ли целевые объекты, для которых производится проверка:  actionExecPermission.TargetAction == permissionRequest.TargetAction
            // Реализации IFitEvaluator см. PermissionRequestProcessor.cs в исходных кодах
            ActionExecPermission actionExecPermission = permission as ActionExecPermission;
            if (permissionRequest == null || actionExecPermission == null) {
                return false;
            }
            if (!(actionExecPermission.ObjectType.IsAssignableFrom(permissionRequest.ObjectType)
                && actionExecPermission.Operation == permissionRequest.Operation
                && actionExecPermission.TargetAction == permissionRequest.TargetAction
                && actionExecPermission.PermissionAccessType != PermissionAccessTypes.UNDEFINED)) {
                    return false;
            }
            if (permissionRequest.TargetObject != null && permissionRequest.FitEvaluator == null) {
                IObjectSpace objectSpace = ObjectSpace.FindObjectSpaceByObject(permissionRequest.TargetObject);
                // ??? objectSpace.IsObjectFitForCriteria(((ActionExecPermission)permission).ObjectType, permissionRequest.TargetObject, ((ActionExecPermission)permission).Criteria);
                permissionRequest.FitEvaluator = new ObjectSpaceFitCriteriaProcessor(permissionRequest.TargetObject, objectSpace);   //objectSpaceProvider.CreateObjectSpace());
                //return permissionRequest.FitEvaluator.Fit(actionExecPermission.Criteria);
            }
            if (permissionRequest.FitEvaluator != null) {
                return IsActionExecFitsPermission(permissionRequest.FitEvaluator, (ActionExecPermission)permission);
            }
            return false;
        }

        protected bool IsActionExecFitsPermission(IFitEvaluator fitCriteriaProcessor, ActionExecPermission actionExecPermission) {
            if (!actionExecPermission.ObjectType.IsAssignableFrom(fitCriteriaProcessor.TargetObject.GetType())) {
                return false;
            }
            return fitCriteriaProcessor.Fit(actionExecPermission.Criteria);
        }

        /// <summary>
        /// Получение списка накрывающих типов.
        /// Список интерфейсов пока не поддерживается
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<Type> GetCoveringTypes(Type type) {

            // Особенностью данной реализации является то, что чем больше индекс типа в списке типов, тем более далёким предком исходного
            // типа он является. Исходный тип находится на 0-м месте списка (в самом его начале).

            List<Type> typeList = new List<Type>();
            typeList.Add(type);
            List<Type> tempList = new List<Type>();
            for (; ; ) {
                foreach (Type T in typeList) {
                    Type baseType = T.BaseType;
                    if (baseType != null && !typeList.Contains(baseType)) {
                        tempList.Add(baseType);
                    }
                    /*
                    Type[] mInterfaces = T.GetInterfaces();
                    foreach (Type IF in mInterfaces)
                        if (!typeList.Contains(IF)) {
                            tempList.Add(IF);
                    }
                    */
                }
                typeList.AddRange(tempList);
                if (tempList.Count > 0) {
                    tempList.Clear();
                } else {
                    break;
                }
            }
            return typeList;
        }

        /*
        /// <summary>
        /// Получает список всех permissions типа ActionExecPermission, необходимых для анализа доступа
        /// </summary>
        /// <param name="typeList"></param>
        /// <param name="securityInstance"></param>
        /// <returns></returns>
        private List<ActionExecPermission> GetSuitableActionExecPermissions(List<Type> typeList, IRequestSecurityStrategy securityInstance) {
            List<ActionExecPermission> actionExecPermissionList = new List<ActionExecPermission>();

            foreach (IOperationPermission perm in securityInstance.Permissions) {
                ActionExecPermission ap = perm as ActionExecPermission;
                
                if (ap == null)
                    continue;

                if (!typeList.Contains(ap.ObjectType)) {
                    continue;
                }

                // PermissionAccessTypes может проверяться ещё на этапе IsRequestFit, отсекая случаи PermissionAccessTypes.UNDEFINED
                if (ap.PermissionAccessType == PermissionAccessTypes.UNDEFINED) {
                    continue;
                }

                // permissionRequest.FitEvaluator() пока не применяем
            }

            return actionExecPermissionList;
        }
        */
    }
}
