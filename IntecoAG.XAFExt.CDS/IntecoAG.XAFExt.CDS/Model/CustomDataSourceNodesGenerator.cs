using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
//
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;

namespace IntecoAG.XafExt.CDS.Model
{
    public class CustomDataSourceNodesGenerator : ModelNodesGeneratorBase {

        public Assembly assembly = Assembly.GetExecutingAssembly();

        protected override void GenerateNodesCore(ModelNode node) {
            GenerateNodesCoreSub(node);
        }

        public static void GenerateNodesCoreSub(ModelNode node) {
            if (CustomCollectionSourceManager.CollectionTypes.Count == 0)
                throw new FieldAccessException("Invalid collection state, typelist.count == 0");
            foreach (Type type in CustomCollectionSourceManager.CollectionTypes.Keys) {

                IQueryDataSource qds = CustomCollectionSourceManager.CollectionTypes[type];

                IModelCustomDataSource child_node = node.AddNode<IModelCustomDataSource>(type.FullName);

                child_node.Description = type.FullName;
                child_node.CustomDataSourceType = type;
                child_node.ObjectType = qds.ElementType;
                child_node.SourceType = qds.SourceType;

                //((IModelCustomDataSource)node.GetNode(childNodeName)).Description = type.Name;
                //((IModelCustomDataSource)node.GetNode(childNodeName)).CustomDataSourceType = type;
                // Паша!!! Переписать правильно для определения типа результата
                //Type baseType = type.BaseType;
                //if (baseType != null) {
                //    Type[] paramTypes = baseType.GetGenericArguments();
                //    foreach (Type typepar in paramTypes) {
                //        ((IModelCustomDataSource)node.GetNode(childNodeName)).ObjectType = typepar;
                //        break;
                //    }
                //}
            }
        }

        public static IList<Type> AutoGetTypes() {   //, string nameSpace) {
            List<Type> list = new List<Type>();

            Type ti = typeof(IQueryDataSource);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (Type t in asm.GetTypes()) {
                    if (ti.IsAssignableFrom(t)) {
                        // here's your type in t 
                        list.Add(t);
                    }
                }
            }
            return list;
        }
        
    }

}

