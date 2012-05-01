using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.StateMachine;
//
namespace IntecoAG.XAFExt.StateMachine {
    [DomainComponent]
    public class StateMachineProxy {
        public StateMachineProxy(IStateMachine sm) { 
        }

        public IStateMachine StateMachine;

        public String Name {
            get { return StateMachine.Name; }
        }
    }
}
