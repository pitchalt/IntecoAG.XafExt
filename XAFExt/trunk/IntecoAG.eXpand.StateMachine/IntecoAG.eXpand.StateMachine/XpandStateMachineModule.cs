using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
//using Xpand.ExpressApp.StateMachine.Security.Improved;
using IntecoAG.eXpand.ExpressApp.StateMachine.Security.Improved;

namespace IntecoAG.eXpand.ExpressApp.StateMachine {

    [ToolboxBitmap(typeof(XpandStateMachineModule))]
    [ToolboxItem(true)]
    public sealed partial class XpandStateMachineModule : ModuleBase {
        public XpandStateMachineModule() {
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