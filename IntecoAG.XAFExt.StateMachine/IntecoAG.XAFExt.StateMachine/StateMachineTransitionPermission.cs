using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security;
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
        public string StateMachineName { get; set; }
        public string StateCaption { get; set; }

        void IStateMachineTransitionPermission.SyncStateCaptions(IList<string> stateCaptions, string machineName) {

        }
    }

    //[NonPersistent]
    //public class StateMachineTransitionPermission : PermissionBase, INotifyPropertyChanged, IStateMachineTransitionPermission {
    //    public event PropertyChangedEventHandler PropertyChanged;


    //    protected void OnPropertyChanged(PropertyChangedEventArgs e) {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null) handler(this, e);
    //    }

    //    public override IPermission Copy() {
    //        return new StateMachineTransitionPermission(Modifier, StateCaption, StateMachineName);
    //    }

    //    public StateMachineTransitionPermission() {
    //    }
    //    public StateMachineTransitionPermission(StateMachineTransitionModifier modifier, string stateCaption, string stateMachineName) {
    //        Modifier = modifier;
    //        StateCaption = stateCaption;
    //        StateMachineName = stateMachineName;
    //    }
    //    public override bool IsSubsetOf(IPermission target) {
    //        var isSubsetOf = base.IsSubsetOf(target);
    //        if (isSubsetOf) {
    //            var stateMachineTransitionPermission = ((StateMachineTransitionPermission)target);
    //            return stateMachineTransitionPermission.StateCaption == StateCaption &&
    //                   stateMachineTransitionPermission.StateMachineName == StateMachineName;
    //        }
    //        return false;
    //    }

    //    public StateMachineTransitionModifier Modifier { get; set; }
    //    string _stateMachineName;

    //    [ImmediatePostData]
    //    public string StateMachineName {
    //        get { return _stateMachineName; }
    //        set {
    //            _stateMachineName = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("StateMachineName"));
    //        }
    //    }

    //    string _stateCaption;

    //    //[PropertyEditor(typeof(IStringLookupPropertyEditor))]
    //    [DataSourceProperty("StateCaptions")]
    //    public string StateCaption {
    //        get { return _stateCaption; }
    //        set {
    //            _stateCaption = value;
    //            OnPropertyChanged(new PropertyChangedEventArgs("StateCaption"));
    //        }
    //    }

    //    IList<string> _stateCaptions = new List<string>();
    //    [Browsable(false)]
    //    public IList<string> StateCaptions { get { return _stateCaptions; } }

    //    public void SyncStateCaptions(IList<string> stateCaptions, string machineName) {
    //        _stateCaptions = stateCaptions;
    //    }
    //}
}