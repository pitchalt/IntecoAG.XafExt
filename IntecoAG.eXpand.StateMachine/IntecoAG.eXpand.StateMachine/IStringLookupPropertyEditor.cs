using System;
using System.ComponentModel;

namespace IntecoAG.eXpand.ExpressApp.StateMachine {
    public interface IStringLookupPropertyEditor {
        event EventHandler<HandledEventArgs> ItemsCalculating;
    }
}