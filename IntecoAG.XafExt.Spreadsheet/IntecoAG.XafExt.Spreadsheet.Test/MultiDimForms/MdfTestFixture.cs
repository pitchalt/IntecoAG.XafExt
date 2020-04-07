using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Xpo;

using IntecoAG.XafExt.Spreadsheet.MultiDimForms;
using IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core;

namespace IntecoAG.XafExt.Spreadsheet.Test.MultiDimForms {

    public class TestApplication : XafApplication {
        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return null;
        }
    }

    public class MdfTestFixture : IDisposable {

        public IObjectSpace ObjectSpace;
//        PostponeController controller;
        public TestApplication Application;

        public MdfTestFixture() {
            IObjectSpaceProvider objectSpaceProvider =
                new XPObjectSpaceProvider(new MemoryDataStoreProvider());
            Application = new TestApplication();
            ModuleBase testModule = new ModuleBase();
            testModule.AdditionalExportedTypes.Add(typeof(MdfCoreTemplate));

            Application.Modules.Add(testModule);
//            application.Modules[0].AdditionalExportedTypes.Add(typeof(Task));
            Application.Setup("TestApplication", objectSpaceProvider);
            ObjectSpace = objectSpaceProvider.CreateObjectSpace();
//            controller = new PostponeController();
        }

        public void Dispose() {
            ObjectSpace = null;
            Application.Exit();
        }

    }

}
