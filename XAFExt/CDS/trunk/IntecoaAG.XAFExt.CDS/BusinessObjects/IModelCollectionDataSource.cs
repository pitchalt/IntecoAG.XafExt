using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;

namespace IntecoaAG.XAFExt.CDS
{
    public interface IModelCollectionDataSource : IModelNode { //, IModelList<IModelBOModel> {
        [DataSourceProperty("Application.CustomDataSources")]   //IntecoaAG.XAFExt.CDS")]
        IModelCustomDataSource CollectionDataSource { get; set; }
    }
}
