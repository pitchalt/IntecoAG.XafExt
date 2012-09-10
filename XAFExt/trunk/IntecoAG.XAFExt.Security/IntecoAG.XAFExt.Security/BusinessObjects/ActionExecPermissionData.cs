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

    public interface IActionExecPermissionData : ITypePermissionData {
        StringObject TargetAction {
            get;
        }

        PermissionAccessTypes PermissionAccessType {
            get;
            set;
        }

        string Criteria {
            get;
            set;
        }
    }

    /// <summary>
    /// Объект, описывающий параметры хранения данных об ActionExecPermission
    /// </summary>
    [Persistent]
    [MapInheritance(MapInheritanceType.ParentTable)]
    //[System.ComponentModel.DisplayName("Action Execute Permission Data")]
    [ImageName("ModelEditor_Action_Open_Object")]   // "Action_Grant"
    public class ActionExecPermissionData : PermissionData, IActionExecPermissionData {

        private PermissionAccessTypes _PermissionAccessType;
        private StringObject _TargetAction;
        private String _Criteria;

        protected override String GetPermissionInfoCaption() {
            return String.Format(@"{0}, ""{1}"", ""{2}"", ""{3}""", CaptionHelper.GetClassCaption(TargetType.FullName), Criteria, TargetAction, PermissionAccessType);
        }

        public ActionExecPermissionData() {
        }
        public ActionExecPermissionData(Session session)
            : base(session) {
        }

        public override IList<IOperationPermission> GetPermissions() {
            IList<IOperationPermission> result = new List<IOperationPermission>();
            if (TargetAction != null) {
                result.Add(new ActionExecPermission(TargetType, TargetAction.Name, Criteria, SecurityActionExecOperations.Exec, PermissionAccessType));
            }
            return result;
        }

        [ValueConverter(typeof(TypeToStringConverter))]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField]
        public Type TargetType {
            get {
                return GetPropertyValue<Type>("TargetType");
            }
            set {
                SetPropertyValue<Type>("TargetType", value);
            }
        }

        public PermissionAccessTypes PermissionAccessType {
            get {
                return _PermissionAccessType;
            }
            set {
                _PermissionAccessType = value;
            }
        }

        [Size(SizeAttribute.Unlimited)]
        [CriteriaOptions("TargetType")]
        [EditorAlias(EditorAliases.ExtendedCriteriaPropertyEditor)]
        public String Criteria {
            get {
                return _Criteria;
            }
            set {
                _Criteria = value;
            }
        }


        // Применить StringObject
        [ValueConverter(typeof(StringObjectToStringConverter))]
        [DataSourceProperty("ActionList")]
        [RuleRequiredField]
        public StringObject TargetAction {
            get {
                return _TargetAction;
            }
            set {
                _TargetAction = value;
            }
        }

        [Browsable(false)]
        public List<StringObject> ActionList {
            get {
                List<StringObject> actionList = new List<StringObject>();
                foreach (IModelAction act in XafExtSecurityModule.xApplication.Model.ActionDesign.Actions) {
                    StringObject stringObject = new StringObject(act.Id);
                    actionList.Add(stringObject);
                }
                return actionList;
            }
        }

    }
}
