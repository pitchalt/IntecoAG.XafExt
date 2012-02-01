using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp.Model;

namespace IntecoaAG.XAFExt.CDS
{
    public interface IModelCustomDataSource : IModelNode {
        [Localizable(true)]
        string Description { get; set; }
        Type CustomDataSourceType { get; set; }
        Type ObjectType { get; set; } // Тип объекта на выходе запроса Linq
    }
    
    [ModelNodesGenerator(typeof(CustomDataSourceNodesGenerator))]
    public interface IModelCustomDataSources : IModelNode, IModelList<IModelCustomDataSource> {
    }

    public interface IModelCustomDataSourceExtension : IModelNode {
        IModelCustomDataSources CustomDataSources { get; }
    }
}
