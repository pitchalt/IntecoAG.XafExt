using System;
using System.ComponentModel;

namespace IntecoAG.XAFExt.StateMachine {
    public interface IStringLookupPropertyEditor {
        event EventHandler<HandledEventArgs> ItemsCalculating;
    }
}