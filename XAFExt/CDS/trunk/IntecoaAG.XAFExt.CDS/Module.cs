using System;
using System.Collections.Generic;
using System.Reflection;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
//
using IntecoaAG.XAFExt.CDS.Model;
//
namespace IntecoaAG.XAFExt.CDS {
    public sealed partial class XAFExtCDSModule : ModuleBase {
        public XAFExtCDSModule() {
            InitializeComponent();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            Application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
        }
        //
        void Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            CollectionSourceBase collectionSourceBase = CustomCollectionSourceManager.Create((XafApplication)sender, e.ObjectSpace, e.ListViewID);
            if (collectionSourceBase != null) e.CollectionSource = collectionSourceBase;
        }
        // ¬ставка в модель узла дл€ указани€ источника данных
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);

            extenders.Add<IModelApplication, IModelApplicationExtension>();
            extenders.Add<IModelListView, IModelListViewExtension>();
        }

    }
}
