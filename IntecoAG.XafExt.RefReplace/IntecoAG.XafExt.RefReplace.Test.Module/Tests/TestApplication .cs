using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Xpo;


namespace IntecoAG.XafExt.RefReplace.Test.Module.Tests {
    public class TestApplication : XafApplication {
        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return null;
        }
       public TestApplication(): base() {
            this.DatabaseVersionMismatch += new EventHandler<DatabaseVersionMismatchEventArgs>(this.TestApplication_DatabaseVersionMismatch);

        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProviders.Add(new XPObjectSpaceProvider(new MemoryDataStoreProvider()));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }

        private void TestApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}
