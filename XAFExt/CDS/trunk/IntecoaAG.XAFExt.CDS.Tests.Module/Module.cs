using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;

namespace IntecoaAG.XAFExt.CDS.Tests.Module {
    public sealed partial class XAFExtCDSTestsModule : ModuleBase {
        public XAFExtCDSTestsModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            Application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
        }

        void Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            CollectionSourceBase collectionSourceBase = CustomCollectionSourceGenerator.Create((XafApplication)sender, e.ObjectSpace, e.ListViewID);
            if (collectionSourceBase != null) e.CollectionSource = collectionSourceBase;
        }

        // ¬ставка в модель узла дл€ указани€ источника данных
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);

            extenders.Add<IModelApplication, IModelCustomDataSourceExtension>();
            extenders.Add<IModelListView, IModelCollectionDataSource>();
        }

        public override void AddGeneratorUpdaters(DevExpress.ExpressApp.Model.Core.ModelNodesGeneratorUpdaters updaters) {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new CustomChildNodesUpdater());
        }
    }

    public class CustomChildNodesUpdater : ModelNodesGeneratorUpdater<CustomDataSourceNodesGenerator> {
        public override void UpdateNode(ModelNode node) {
            CustomDataSourceNodesGenerator.GenerateNodesCoreSub(node, Assembly.GetExecutingAssembly());
        }
    }
}
