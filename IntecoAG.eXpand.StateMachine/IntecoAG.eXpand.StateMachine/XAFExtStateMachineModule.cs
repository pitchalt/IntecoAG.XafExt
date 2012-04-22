using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
//using Xpand.ExpressApp.StateMachine.Security.Improved;
using XAFExt.StateMachine;

namespace XAFExt.StateMachine {

    [ToolboxBitmap(typeof(XAFExtStateMachineModule))]
    [ToolboxItem(true)]
    public sealed partial class XAFExtStateMachineModule : ModuleBase {
        public XAFExtStateMachineModule() {
            InitializeComponent();
        }
        public override void Setup(ApplicationModulesManager moduleManager) {
            base.Setup(moduleManager);
//            if (RuntimeMode)
            Application.SetupComplete += ApplicationOnSetupComplete;
        }

        void ApplicationOnSetupComplete(object sender, EventArgs eventArgs) {
            if (SecuritySystem.Instance is SecurityStrategy)
                ((SecurityStrategy)SecuritySystem.Instance).RequestProcessors.Register(new StateMachineTransitionRequestProcessor());
        }
    }
}