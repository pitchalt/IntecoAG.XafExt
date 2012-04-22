using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace IntecoAG.XAFExt.StateMachine {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }
    }
}
