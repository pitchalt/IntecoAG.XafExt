using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
//using Xpand.ExpressApp.PropertyEditors;
//using Xpand.ExpressApp.Security.Permissions;
//using Xpand.Persistent.Base.General.CustomAttributes;
//using IntecoAG.XAFExt.StateMachine;

namespace IntecoAG.XAFExt.StateMachine {
//    public class StateMachineTransitionOperationPermissionData : XpandPermissionData, IStateMachineTransitionPermission {
    public class StateMachineTransitionPermissionData : PermissionData, IStateMachineTransitionPermission {
        public StateMachineTransitionPermissionData(Session session)
            : base(session) {
        }
         IEnumerable<PropertyInfo> _propertyInfos;

        protected override string GetPermissionInfoCaption() {
            String capt = String.Empty;
            foreach (PropertyInfo info in _propertyInfos)
                if (capt == String.Empty)
                    capt = info.GetValue(this, null).ToString();
                else
                    capt = capt + ", " + info.GetValue(this, null);
            return capt;
//            return _propertyInfos.Aggregate<PropertyInfo, string>(null, 
//                (current, propertyInfo) => current + (propertyInfo.GetValue(this, null) + ", ")).TrimEnd(", ".ToCharArray());
        }
        void EnumerateProperties() {
            List<PropertyInfo> infos = new List<PropertyInfo>();
            foreach (PropertyInfo info in GetType().GetProperties()) {
                if (info.GetSetMethod() != null &&
                    info.GetCustomAttributes(typeof(NonPersistentAttribute), true).GetLength(1) == 0)
                    infos.Add(info);
//                _propertyInfos = (GetType().GetProperties()).Where(info => info.GetSetMethod() != null && info.GetCustomAttributes(typeof(NonPersistentAttribute), true).Count() == 0);
            }
            _propertyInfos = infos;
        }


        public override IList<IOperationPermission> GetPermissions() {
            return new IOperationPermission[] { new StateMachineTransitionPermission(this) };
        }
        private StateMachineTransitionModifier _modifier;
        public StateMachineTransitionModifier Modifier {
            get {
                return _modifier;
            }
            set {
                SetPropertyValue("Modifier", ref _modifier, value);
            }
        }
        private string _stateMachineName;
        [ImmediatePostData]
        public string StateMachineName {
            get {
                return _stateMachineName;
            }
            set {
                SetPropertyValue("StateMachineName", ref _stateMachineName, value);
            }
        }
        private string _stateCaption;
        //[PropertyEditor(typeof(IStringLookupPropertyEditor))]
        [DataSourceProperty("StateCaptions")]
        public string StateCaption {
            get {
                return _stateCaption;
            }
            set {
                SetPropertyValue("StateCaption", ref _stateCaption, value);
            }
        }

        IStateMachine _StateMachine;
        public IStateMachine StateMachine {
            get { return _StateMachine; }
            set { _StateMachine = value; }
        }

        ITransition _Transition;
        public ITransition Transition {
            get { return _Transition; }
            set { _Transition = value; }
        }


        IList<string> _stateCaptions = new List<string>();
        [Browsable(false)]
        public IList<string> StateCaptions { get { return _stateCaptions; } }

        public void SyncStateCaptions(IList<string> stateCaptions, string machineName) {
            _stateCaptions = stateCaptions;
        }
    }
}