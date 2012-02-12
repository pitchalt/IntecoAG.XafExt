using System;
using System.Collections.Generic;
using System.Linq;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
//
using IntecoaAG.XAFExt.CDS.Model;
//
namespace IntecoaAG.XAFExt.CDS 
{
    public static class CustomCollectionSourceManager
    {
        private static IList<Type> _CollectionTypes = new List<Type>();

        public static IList<Type> CollectionTypes {
            get { return _CollectionTypes; }
        }

        public static void Register(Type type) {
            if (!CollectionTypes.Contains(type))
                CollectionTypes.Add(type);
        }

        public static CollectionSourceBase Create(XafApplication application, IObjectSpace objectSpace, IModelListView listViewNode) {
            IModelCustomDataSource modelCustomDataSource = ((IModelListViewExtension)listViewNode).CollectionDataSource;
            if (modelCustomDataSource == null) return null;

            Type customDataSourceType = modelCustomDataSource.CustomDataSourceType;   // Тип коллекции
            if (customDataSourceType == null) return null;

            // Две строки ниже стали не нужны, т.к. тип вычисляется из запроса ниже: query.ElementType
            // В модели поле тоже как бы не нужно - пусть останется зарезервированным пока что.
            //Type objectType = modelCustomDataSource.ObjectType;   // Тип выхода запроса
            //if (objectType == null) return null;

            // Создание объекта запроса 
            IQueryable query = Activator.CreateInstance(customDataSourceType, objectSpace) as IQueryable;  // as IQueryable;

            // Создание коллекции с типом customDataSourceType
            //var outCollection = Activator.CreateInstance(customDataSourceType, objectSpace);

            LinqCollectionSource outCollection = new LinqCollectionSource(objectSpace, query);
            return outCollection ;
        }

        public static CollectionSourceBase Create(XafApplication application, IObjectSpace objectSpace, string listViewID) {
            IModelListView listViewNode = application.FindModelView(listViewID) as IModelListView;
            return Create(application, objectSpace, listViewNode);
        }
    }
}