using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
//
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;

namespace IntecoAG.XAFExt.CDS.Model
{
    public class CustomDataSourceNodesGenerator : ModelNodesGeneratorBase {

        public Assembly assembly = Assembly.GetExecutingAssembly();

        protected override void GenerateNodesCore(ModelNode node) {
            GenerateNodesCoreSub(node);
        }

        public static void GenerateNodesCoreSub(ModelNode node) {
            IList<Type> typelist = CustomCollectionSourceManager.CollectionTypes;
            //IList<Type> typelist = AutoGetTypes();
            
            foreach (Type type in typelist) {
                string childNodeName = type.Name;
                node.AddNode<IModelCustomDataSource>(childNodeName);
                ((IModelCustomDataSource)node.GetNode(childNodeName)).Description = type.Name;
                ((IModelCustomDataSource)node.GetNode(childNodeName)).CustomDataSourceType = type;
                // Паша!!! Переписать правильно для определения типа результата
                Type baseType = type.BaseType;
                if (baseType != null) {
                    Type[] paramTypes = baseType.GetGenericArguments();
                    foreach (Type typepar in paramTypes) {
                        ((IModelCustomDataSource)node.GetNode(childNodeName)).ObjectType = typepar;
                        break;
                    }
                }
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

