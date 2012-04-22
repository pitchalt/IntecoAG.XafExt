using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;

namespace IntecoAG.XAFExt.StateMachine {
    [Serializable]
    public class StateMachineTransitionPermissionRequest : OperationPermissionRequestBase, IStateMachineTransitionPermission {
        public StateMachineTransitionPermissionRequest(IStateMachineTransitionPermission permission)
            : base(StateMachineTransitionPermission.OperationName) {
            Modifier = permission.Modifier;
            StateCaption = permission.StateCaption;
            StateMachineName = permission.StateMachineName;
        }

        public StateMachineTransitionModifier Modifier { get; set; }
        public string StateMachineName { get; set; }
        public string StateCaption { get; set; }

        IStateMachine _StateMachine;
        public IStateMachine StateMachine {
            get {
                return _StateMachine;
            }
            set {
                _StateMachine = value;
            }
        }

        ITransition _Transition;
        public ITransition Transition {
            get {
                return _Transition;
            }
            set {
                _Transition = value;
            }
        }


        void IStateMachineTransitionPermission.SyncStateCaptions(IList<string> stateCaptions, string machineName) {

        }
    }
}
