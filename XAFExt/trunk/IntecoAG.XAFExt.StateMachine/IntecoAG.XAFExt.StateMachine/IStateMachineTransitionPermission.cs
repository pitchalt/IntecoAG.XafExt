using System.Collections.Generic;

using DevExpress.ExpressApp.StateMachine;

namespace IntecoAG.XAFExt.StateMachine {
    public interface IStateMachineTransitionPermission {
        StateMachineTransitionModifier Modifier { get; set; }
        string StateMachineName { get; set; }
        string StateCaption { get; set; }

        IStateMachine StateMachine { get; set; }
        ITransition Transition { get; set; }

        void SyncStateCaptions(IList<string> stateCaptions, string machineName);
    }
}