using System;
using System.ComponentModel;
//
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace IntecoaAG.XAFExt.CDS.Model
{
    public interface IModelCustomDataSource : IModelNode {
        [Localizable(true)]
        string Description { get; set; }
        Type CustomDataSourceType { get; set; }
        Type ObjectType { get; set; } // Тип объекта на выходе запроса Linq
    }

}
