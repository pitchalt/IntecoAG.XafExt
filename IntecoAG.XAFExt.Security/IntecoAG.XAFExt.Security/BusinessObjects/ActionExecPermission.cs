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
    /// Операция над Action
    /// </summary>
    public static class SecurityActionExecOperations {
        public const String Exec = "Exec";
    }

    /// <summary>
    /// Тип доступа к permission
    /// </summary>
    public enum PermissionAccessTypes {
        /// <summary>
        /// Не обрабатывать (не учитывать) такой permission
        /// </summary>
        UNDEFINED = 0,
        /// <summary>
        /// Доступ разрешён
        /// </summary>
        ALLOW = 1,
        /// <summary>
        /// Доступ запрезён
        /// </summary>
        DENY = 2
    }

    /// <summary>
    /// Объект, описывающий параметры доступа к Action
    /// </summary>
    public class ActionExecPermission : OperationPermissionBase {

        private PermissionAccessTypes _PermissionAccessType;
        private Type _ObjectType;
        private String _Criteria;
        private String _TargetAction;
        /*
        private IList<String> _TargetActions;
        */

        public ActionExecPermission(Type objectType, String targetAction, String criteria, String operation, PermissionAccessTypes permissionAccessType)
            : base(operation) {
            Guard.ArgumentNotNull(objectType, "objectType");
            Guard.ArgumentNotNullOrEmpty(targetAction, "targetAction");
            _PermissionAccessType = permissionAccessType;
            _ObjectType = objectType;
            _Criteria = criteria;
            _TargetAction = targetAction;
        }

        public override bool IsSame(IOperationPermission comparedPermission) {
            if(base.IsSame(comparedPermission)) {
                ActionExecPermission comparedActiomOperationPermission = comparedPermission as ActionExecPermission;
                if (comparedActiomOperationPermission == null) {
                    return _ObjectType == comparedActiomOperationPermission.ObjectType
                        && _Criteria == comparedActiomOperationPermission.Criteria
                        && _PermissionAccessType == comparedActiomOperationPermission.PermissionAccessType
                        && _TargetAction == comparedActiomOperationPermission.TargetAction;
                }
            }
            return false;
        }

        public override IList<String> GetSupportedOperations() {
            return new String[] { SecurityActionExecOperations.Exec };
        }

        public PermissionAccessTypes PermissionAccessType {
            get {
                return _PermissionAccessType;
            }
            set {
                _PermissionAccessType = value;
            }
        }

        public Type ObjectType {
            get {
                return _ObjectType;
            }
        }

        public String Criteria {
            get {
                return _Criteria;
            }
        }

        public String TargetAction {
            get {
                return _TargetAction;
            }
        }

        /*
        public IList<String> TargetActions {
            get {
                return _TargetActions;
            }
        }
        */
    }
}
