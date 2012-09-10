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
    /// 
    /// </summary>
    public interface IActionExecPermissionRequest {
        Type ObjectType {
            get;
        }

        String TargetAction {
            get;
        }
    }

    /// <summary>
    /// Класс символизирует собой набор информации о сложившемся в Runtime контексте, применяемый для решения вопроса о доступе к Action.
    /// Объект этого класса попадает в RequestProcessor, который его анализирует и выдаёт решение о запрете или разрешении Action с учётом
    /// предписания в ActionExecPermission (типа, операции, условия)
    /// </summary>
    [Serializable]
    public class ActionExecPermissionRequest : OperationPermissionRequestBase, ISerializable, IActionExecPermissionRequest {

        private Type _ObjectType;
        private String _TargetObjectHandle;
        private String _TargetAction;

        public ActionExecPermissionRequest(object targetObject, Type objectType, String targetAction, String operation)
            : base(operation) {
                Guard.ArgumentNotNull(objectType, "objectType");
            Guard.ArgumentNotNull(targetObject, "targetObject");
            Guard.ArgumentNotNullOrEmpty(targetAction, "targetAction");
            ObjectType = objectType;
            TargetObject = targetObject;
            TargetAction = targetAction;
            IObjectSpace objectSpace = ObjectSpace.FindObjectSpaceByObject(targetObject);
            if (objectSpace != null) {
                ((ObjectSpace)objectSpace).TryGetObjectHandle(targetObject, out _TargetObjectHandle);
            }
        }

        /*
        public ActionExecPermissionRequest(IFitEvaluator fitCriteriaProcessor, StringObject targetAction, String criteria, String operation)
            : base(operation) {
            Guard.ArgumentNotNull(fitCriteriaProcessor, "fitCriteriaProcessor");
            Guard.ArgumentNotNull(fitCriteriaProcessor.TargetObject, "fitCriteriaProcessor.TargetObject");
            this.ObjectType = fitCriteriaProcessor.TargetObject.GetType();
            this.FitEvaluator = fitCriteriaProcessor;
            this.TargetAction = targetAction;
            this.Operation = operation;
        }
        public ActionExecPermissionRequest(SerializationInfo info, StreamingContext context) {
            Operation = info.GetString("Operation");
            ObjectType = ReflectionHelper.FindType(info.GetString("ObjectType"));
            TargetAction.Name = info.GetString("TargetAction");
        }
        */

        public Object TargetObject {
            get;
            set;
        }

        public Type ObjectType {
            get {
                return _ObjectType;
            }
            set {
                _ObjectType = PatchObjectType(value);  // Определяется XAF-тип, соответствующий value
            }
        }

        public String TargetObjectHandle {
            get {
                return _TargetObjectHandle;
            }
            set {
                _TargetObjectHandle = value;
            }
        }

        public String TargetAction {
            get {
                return _TargetAction;
            }
            set {
                _TargetAction = value;
            }
        }
        
        public IFitEvaluator FitEvaluator {
            get;
            set;
        }     

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("Operation", Operation);
            info.AddValue("ObjectType", ObjectType.FullName);
            info.AddValue("TargetAction", TargetAction);
        }

        public override String GetHashString() {
            String objectHandle = TargetObjectHandle ?? String.Empty;
            if (String.IsNullOrEmpty(objectHandle) && FitEvaluator != null) {
                objectHandle = FitEvaluator.TargetObjectId;
            }
            return String.Format("{0}/{1}/{2}/{3}", base.GetHashString(), ObjectType, objectHandle, TargetAction);
        }
    }
}
