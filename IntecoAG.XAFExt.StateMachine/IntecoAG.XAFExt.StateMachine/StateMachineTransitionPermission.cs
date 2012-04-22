using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;
//
//using Xpand.ExpressApp.PropertyEditors;
//using Xpand.ExpressApp.Security.Permissions;
//using Xpand.Persistent.Base.General.CustomAttributes;

namespace IntecoAG.XAFExt.StateMachine {
    public enum StateMachineTransitionModifier {
        Allow,
        Deny
    }

    public class StateMachineTransitionPermission : OperationPermissionBase, IStateMachineTransitionPermission {
        public const string OperationName = "StateMachineTransition";

        public StateMachineTransitionPermission(IStateMachineTransitionPermission permission)
            : base(OperationName) {
            StateCaption = permission.StateCaption;
            StateMachineName = permission.StateMachineName;
            Modifier = permission.Modifier;
        }

        public StateMachineTransitionPermission()
            : base(OperationName) {
        }

        public override IList<string> GetSupportedOperations() {
            return new[] { OperationName };
        }
        public StateMachineTransitionModifier Modifier { get; set; }

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

        public string StateMachineName { get; set; }
        public string StateCaption { get; set; }

        void IStateMachineTransitionPermission.SyncStateCaptions(IList<string> stateCaptions, string machineName) {

        }
    }

}