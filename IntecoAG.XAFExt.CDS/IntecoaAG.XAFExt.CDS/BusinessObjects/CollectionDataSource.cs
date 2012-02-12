using System.Collections;
using System.ComponentModel;
//
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;

namespace IntecoaAG.XAFExt.CDS 
{
    public abstract class CollectionDataSource : CollectionSourceBase
    {
        protected IBindingList collectionCore;
        protected ITypeInfo objectTypeInfoCore;
        
        protected CollectionDataSource(IObjectSpace objectSpace)
            : base(objectSpace) {
        }
        
        public override bool? IsObjectFitForCollection(object obj) {
            return collectionCore.Contains(obj);
        }
        
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) { }

        public override ITypeInfo ObjectTypeInfo {
            get { return objectTypeInfoCore; }
        }
    }
}