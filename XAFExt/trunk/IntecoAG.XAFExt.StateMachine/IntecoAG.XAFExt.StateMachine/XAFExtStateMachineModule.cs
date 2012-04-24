using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
//
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

        private StateMachineModule _smModule;

        public override void Setup(ApplicationModulesManager moduleManager) {
            base.Setup(moduleManager);
//            if (RuntimeMode)
            if (Application != null) {
                Application.SetupComplete += new EventHandler<EventArgs>(Application_SetupComplete);
                Application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
            }
        }

        //protected override IEnumerable<Type> GetDeclaredExportedTypes() {
        //    IList<Type> result = new List<Type>(base.GetDeclaredExportedTypes());
        //    result.Add(typeof(IStateMachine));
        //    result.Add(typeof(IState));
        //    result.Add(typeof(ITransition));
        //    return result;
        //}

        void  Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e)
        {
            if (e.ObjectType == typeof(IStateMachine)) {
                e.CollectionSource = new StateMachineCollectionSource(e.ObjectSpace, _smModule.StateMachineRepository, _smModule.StateMachineStorageType);
            }
        }

        void Application_SetupComplete(object sender, EventArgs e) {
            if (SecuritySystem.Instance is SecurityStrategy)
                ((SecurityStrategy)SecuritySystem.Instance).RequestProcessors.Register(new StateMachinePermissionRequestProcessor());
            _smModule = Application.Modules.FindModule<StateMachineModule>();
        }
    }
}