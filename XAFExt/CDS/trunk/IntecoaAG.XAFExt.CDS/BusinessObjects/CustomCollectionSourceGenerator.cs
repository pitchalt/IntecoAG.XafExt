using System;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace IntecoaAG.XAFExt.CDS 
{
    public static class CustomCollectionSourceGenerator
    {
        public static CollectionSourceBase Create(XafApplication application, IObjectSpace objectSpace, IModelListView listViewNode) {
            IModelCustomDataSource modelCustomDataSource = ((IModelCollectionDataSource)listViewNode).CollectionDataSource;
            if (modelCustomDataSource == null) return null;

            Type customDataSourceType = modelCustomDataSource.CustomDataSourceType;   // Тип коллекции
            if (customDataSourceType == null) return null;

            // Две строки ниже стали не нужны, т.к. тип вычисляется из запроса ниже: query.ElementType
            // В модели поле тоже как бы не нужно - пусть останется зарезервированным пока что.
            //Type objectType = modelCustomDataSource.ObjectType;   // Тип выхода запроса
            //if (objectType == null) return null;

            // Создание объекта запроса 
            var query = ((IQueryDataSource)Activator.CreateInstance(customDataSourceType, objectSpace)).GetQuery();  // as IQueryable;

            // Создание коллекции с типом customDataSourceType
            //var outCollection = Activator.CreateInstance(customDataSourceType, objectSpace);
            var outCollection = Activator.CreateInstance(typeof(LinqCollectionSource<>).MakeGenericType(query.ElementType), objectSpace, query);
            return outCollection as CollectionSourceBase;
        }

        public static CollectionSourceBase Create(XafApplication application, IObjectSpace objectSpace, string listViewID) {
            IModelListView listViewNode = application.FindModelView(listViewID) as IModelListView;
            return Create(application, objectSpace, listViewNode);
        }
    }
}