using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace IntecoaAG.XAFExt.CDS {
    public sealed partial class CDSModule : ModuleBase {
        public CDSModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            Application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
        }

        void Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            CollectionSourceBase collectionSourceBase = CustomCollectionGenerator.Create((XafApplication)sender, e.ObjectSpace, e.ListViewID);
            if (collectionSourceBase != null) e.CollectionSource = collectionSourceBase;
        }

        // ¬ставка в модель узла дл€ указани€ источника данных
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);

            extenders.Add<IModelApplication, IModelCustomDataSourceExtension>();
            extenders.Add<IModelListView, IModelCollectionDataSource>();
        }
    }
}
