using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace IntecoAG.XAFExt.CDS.Model
{
    [ModelNodesGenerator(typeof(CustomDataSourceNodesGenerator))]
    public interface IModelCustomDataSources : IModelNode, IModelList<IModelCustomDataSource> {
    }

    public interface IModelApplicationExtension : IModelNode {
        IModelCustomDataSources CustomDataSources { get; }
    }

    public interface IModelListViewExtension : IModelNode { //, IModelList<IModelBOModel> {
        [DataSourceProperty("Application.CustomDataSources")]   //IntecoAG.XAFExt.CDS")]
        IModelCustomDataSource CollectionDataSource { get; set; }
    }
}
