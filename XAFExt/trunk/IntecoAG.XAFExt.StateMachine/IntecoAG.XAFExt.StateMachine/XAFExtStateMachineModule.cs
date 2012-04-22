using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;
//using Xpand.ExpressApp.StateMachine.Security.Improved;
using IntecoAG.XAFExt.StateMachine;

namespace IntecoAG.XAFExt.StateMachine {

    [ToolboxBitmap(typeof(XAFExtStateMachineModule))]
    [ToolboxItem(true)]
    public sealed partial class XAFExtStateMachineModule : ModuleBase {
        public XAFExtStateMachineModule() {
            InitializeComponent();
        }
        public override void Setup(ApplicationModulesManager moduleManager) {
            base.Setup(moduleManager);
//            if (RuntimeMode)
            Application.SetupComplete += new EventHandler<EventArgs>(Application_SetupComplete);
        }

        void Application_SetupComplete(object sender, EventArgs e) {
            if (SecuritySystem.Instance is SecurityStrategy)
                ((SecurityStrategy)SecuritySystem.Instance).RequestProcessors.Register(new StateMachinePermissionRequestProcessor());
            StateMachineModule sm_module = Application.Modules.FindModule<StateMachineModule>();

        }

    }
}