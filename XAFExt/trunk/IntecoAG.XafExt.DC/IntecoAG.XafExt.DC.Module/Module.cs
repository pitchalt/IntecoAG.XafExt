using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;

using IntecoAG.XafExt.DC.Module.Samples;

namespace IntecoAG.XafExt.DC.Module {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    public sealed partial class DCModule : ModuleBase {
        public DCModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            XafTypesInfo.Instance.RegisterSharedPart(typeof(IOrganization));
            XafTypesInfo.Instance.RegisterSharedPart(typeof(IAccount));
            XafTypesInfo.Instance.RegisterEntity("Person", typeof(IPerson));
            XafTypesInfo.Instance.RegisterEntity("Customer", typeof(ICustomer));
            XafTypesInfo.Instance.RegisterEntity("Contact",typeof(IContact));
            // Manage various aspects of the application UI and behavior at the module level.
//            XafTypesInfo.Instance.RegisterEntity("Manager", typeof(IManager));
//            XafTypesInfo.Instance.RegisterEntity("Evangelist", typeof(IEvangelist));
//            XafTypesInfo.Instance.RegisterSharedPart(typeof(IWorker));
        }
    }
}
