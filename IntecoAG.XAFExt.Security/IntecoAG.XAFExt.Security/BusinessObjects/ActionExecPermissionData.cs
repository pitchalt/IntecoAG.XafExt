using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Text;
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

    public interface IActionExecPermissionData : ITypePermissionData {
        StringObject TargetAction {
            get;
            set;
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
        /*
        private String _ActionStrings;
        */

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



        /*
        /// <summary>
        ///  Cериализованнsq набор иденитификаторов Actions, взятых из модели, записанных через точку с запятой: "ActionId1=0;ActionId2=0;ActionId3=1;..."
        ///  где =0 означает, что Action запрещена, а =1 разарешена.
        /// </summary>
        //[Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public String ActionStrings {
            get {
                return _ActionStrings;
            }
            set {
                _ActionStrings = value;
            }
        }

        private String SerializeActionCollection(List<String> actionCols) {
            StringBuilder sb = new StringBuilder();
            foreach (String str in actionCols) {
                if (sb.Length > 0)
                    sb.Append(";");
                sb.Append(str); 
            }
            return sb.ToString();
        }

        private List<String> DeserializeActionStrings(String actionStrings) {
            List<String> res = new List<String>();
            String[] delimiter = { ";" };
            String[] mActionIds = actionStrings.Split(delimiter, StringSplitOptions.None);
            res.AddRange(mActionIds);
            return res;
        }
        */
    }
}
