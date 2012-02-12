using System;
using System.Linq;
using System.Collections.Generic;
//
using DevExpress.ExpressApp;
using IntecoaAG.XAFExt.CDS;
//
namespace IntecoaAG.XAFExt.CDS.Tests.Module {
    public sealed partial class XAFExtCDSTestsModule : ModuleBase {
        public XAFExtCDSTestsModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            CustomCollectionSourceManager.Register(typeof(LinqQueryCountry1));
            CustomCollectionSourceManager.Register(typeof(LinqQueryCountry2));
            CustomCollectionSourceManager.Register(typeof(LinqQueryCountry3));
        }
        
    }

}
