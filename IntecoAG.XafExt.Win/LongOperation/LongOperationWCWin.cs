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
using DevExpress.XtraSplashScreen;

namespace IntecoAG.XafExt.LongOperation.Win
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class LongOperationWCWin : LongOperationWC

    {
        public LongOperationWCWin()
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

        public override void RunSync(LongOperationTask longOperation) {
            SplashScreenManager.ShowForm(typeof(LongOperationSplash));
//            var lo = new SubjectOrdersGenerateLO((Subject)View.CurrentObject, Application.CreateObjectSpace());
            SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.ATTACH, longOperation);
            var task = LongOperationManager.Run(longOperation);
            task.Wait();
            SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.DETTACH, null);
            SplashScreenManager.CloseForm();
//            View.ObjectSpace.ReloadObject(View.CurrentObject);

        }

        public override void SplashShow() {
            SplashScreenManager.ShowForm(typeof(LongOperationSplash));
        }

        public override void SplashClose() {
            SplashScreenManager.CloseForm();
        }

        public override void TaskRunSyn(LongOperationTask longOperation) {
            SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.ATTACH, longOperation);
            LongOperationManager.Run(longOperation).Wait();
            SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.DETTACH, null);
        }

        // private void GenerateOrdersAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        // {
        //     SplashScreenManager.ShowForm(typeof(LongOperationSplash));
        //     var lo = new SubjectOrdersGenerateLO((Subject)View.CurrentObject, Application.CreateObjectSpace());
        //     SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.ATTACH, lo);
        //     var task = TestLongOperationModule.LongOperationManager.Run(lo);
        //     task.Wait();
        //     SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.DETTACH, null);
        //     SplashScreenManager.CloseForm();
        //     View.ObjectSpace.ReloadObject(View.CurrentObject);
        //     //((WinApplication)Application).StartSplash(SplashType.SplashScreen);
        //     //((WinApplication)Application).StopSplash();
        // }
        // private async void GenerateOrdersAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        // {
        //     //            SplashScreenManager.ShowForm(typeof(LongOperationSplash));
        //     var lo = new SubjectOrdersGenerateLO((Subject)View.CurrentObject, Application.CreateObjectSpace());
        //     //            SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.ATTACH, lo);
        //     await TestLongOperationModule.LongOperationManager.Run(lo);
        //     //            task.Wait();
        //     //            SplashScreenManager.Default.SendCommand(LongOperationSplash.SplashCommand.DETTACH, null);
        //     //            SplashScreenManager.CloseForm();
        //     View.ObjectSpace.ReloadObject(View.CurrentObject);
        //     //((WinApplication)Application).StartSplash(SplashType.SplashScreen);
        //     //((WinApplication)Application).StopSplash();
        // }

    }
}
