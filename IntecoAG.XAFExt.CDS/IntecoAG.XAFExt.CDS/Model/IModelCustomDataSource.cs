using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace IntecoAG.XafExt.CDS.Model
{
    public interface IModelCustomDataSource : IModelNode {
        [Localizable(true)]
        string Description { get; set; }
        Type CustomDataSourceType { get; set; }
        Type ObjectType { get; set; } // Тип объекта на выходе запроса Linq
        Type SourceType { get; set; } // Тип объекта на выходе запроса Linq
    }

}
