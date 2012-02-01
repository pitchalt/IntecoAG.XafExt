using System;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace IntecoaAG.XAFExt.CDS 
{
    public static class CustomCollectionGenerator
    {
        public static CollectionSourceBase Create(XafApplication application, IObjectSpace objectSpace, IModelListView listViewNode) {
            IModelCustomDataSource modelCustomDataSource = ((IModelCollectionDataSource)listViewNode).CollectionDataSource;
            if (modelCustomDataSource == null) return null;

            Type customDataSourceType = modelCustomDataSource.CustomDataSourceType;   // Тип коллекции
            if (customDataSourceType == null) return null;

            Type objectType = modelCustomDataSource.ObjectType;   // Тип выхода запроса
            if (objectType == null) return null;

            // Создание коллекции с типом customDataSourceType
            var outCollection = Activator.CreateInstance(customDataSourceType, objectSpace);
            return outCollection as CollectionSourceBase;
        }

        public static CollectionSourceBase Create(XafApplication application, IObjectSpace objectSpace, string listViewID) {
            IModelListView listViewNode = application.FindModelView(listViewID) as IModelListView;
            return Create(application, objectSpace, listViewNode);
        }
    }
}