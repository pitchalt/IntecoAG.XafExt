using System;
using System.Linq;
using System.Collections.Generic;
//
using DevExpress.ExpressApp;
using IntecoAG.XAFExt.CDS;
//
namespace IntecoAG.XAFExt.CDS.Tests.Module {
    public sealed partial class XAFExtCDSTestsModule : ModuleBase {
        public XAFExtCDSTestsModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            CustomCollectionSourceManager.Register(typeof(LinqQueryCountry1));
            CustomCollectionSourceManager.Register(typeof(LinqQueryCountry2));
            CustomCollectionSourceManager.Register(typeof(LinqQueryCountry3));
            CustomCollectionSourceManager.Register(typeof(testCountry4));
        }
        
    }

}
