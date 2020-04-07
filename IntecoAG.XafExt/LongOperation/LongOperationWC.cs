using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace IntecoAG.XafExt.LongOperation
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public abstract partial class LongOperationWC : WindowController
    {
        protected LongOperationWC()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        /// <summary>
        /// TODO Переделать менеджер длинных операций
        /// </summary>
        private static LongOperationManager _LongOperationManager;
        public LongOperationManager LongOperationManager {
            get {
                //IValueManager<LongOperationManager> value_manager =
                //    ValueManager.GetValueManager<LongOperationManager>(typeof(LongOperationManager).FullName);
                //if (!value_manager.CanManageValue)
                //    value_manager.Value = new LongOperationManager();
                //return value_manager.Value;
                return _LongOperationManager ?? (_LongOperationManager = new LongOperationManager());
            }
        }

        public abstract void RunSync(LongOperationTask longOperation);

        public abstract void SplashShow();

        public abstract void SplashClose();

        public abstract void TaskRunSyn(LongOperationTask longOperation);

    }
}
