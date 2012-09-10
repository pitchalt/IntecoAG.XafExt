using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;

namespace IntecoAG.XAFExt.Security {
    public sealed partial class XafExtSecurityModule : ModuleBase {
        public XafExtSecurityModule() {
            InitializeComponent();
        }

        public static XafApplication xApplication;
        public override void Setup(XafApplication application) {
            base.Setup(application);

            xApplication = application;

            application.SetupComplete += application_SetupComplete;
        }

        void application_SetupComplete(object sender, EventArgs e) {
            if (SecuritySystem.Instance is SecurityStrategyComplex) {
                ((SecurityStrategyComplex)SecuritySystem.Instance).RequestProcessors.Register(
                    new ActionExecPermissionRequestProcessor(ActionPermissionPolices.PERMITS));
            }
        }
    }
}
