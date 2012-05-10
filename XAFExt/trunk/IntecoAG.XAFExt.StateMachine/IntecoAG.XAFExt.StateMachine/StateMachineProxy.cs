using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using DevExpress.ExpressApp;
using DC=DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.StateMachine;
//
namespace IntecoAG.XAFExt.StateMachine {
    [DC.DomainComponent]
    [DC.XafDefaultProperty("Name")]
    public class StateMachineProxy {
        public StateMachineProxy(IStateMachine sm) { 
        }

        public IStateMachine StateMachine;

        public String Name {
            get { return StateMachine.Name; }
        }
    }
}
