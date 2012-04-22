using System;
using System.ComponentModel;

namespace XAFExt.StateMachine {
    public interface IStringLookupPropertyEditor {
        event EventHandler<HandledEventArgs> ItemsCalculating;
    }
}