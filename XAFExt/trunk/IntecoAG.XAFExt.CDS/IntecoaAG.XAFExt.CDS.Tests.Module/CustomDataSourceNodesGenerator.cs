/*
using System;
using System.Linq;
using System.Reflection;
//
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
//
using IntecoaAG.XAFExt.CDS;

namespace IntecoaAG.XAFExt.CDS
{
    public class CustomCollectionSourceNodesGenerator : CustomDataSourceNodesGenerator {
        public Assembly assembly1 = Assembly.GetExecutingAssembly();
        public string nameSpace1 = "IntecoaAG.XAFExt.CDS.Tests";

        //public CustomCollectionSourceNodesGenerator(Assembly assembly, string nameSpace) {
        //    base.assembly = Assembly.GetExecutingAssembly();
        //    base.nameSpace = "IntecoaAG.XAFExt.CDS.Tests";
        //}

        public override Type[] GetTypesInNamespace(Assembly assembly, string nameSpace) {
            //return base.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "IntecoaAG.XAFExt.CDS.Tests");
            Type T = typeof(IQueryDataSource);
            return assembly1.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace1, StringComparison.Ordinal) && t.IsClass).Where(t => T.IsAssignableFrom(t)).Where(t => !t.IsAbstract).ToArray();
        }
    }

}
*/


using System;
using System.Linq;
using System.Reflection;
//
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;

namespace IntecoaAG.XAFExt.CDS {
    public class CustomDataSourceNodesGenerator1 : ModelNodesGeneratorBase {

        public Assembly assembly = Assembly.GetExecutingAssembly();
        public string nameSpace = "IntecoaAG.XAFExt.CDS.Tests";

        protected override void GenerateNodesCore(ModelNode node) {
            Type[] typelist = GetTypesInNamespace(assembly, nameSpace);

            for (int i = 0; i < typelist.Length; i++) {
                string childNodeName = typelist[i].Name;
                node.AddNode<IModelCustomDataSource>(childNodeName);
                ((IModelCustomDataSource)node.GetNode(childNodeName)).Description = typelist[i].Name;
                //((IModelCustomDataSource)node.GetNode(childNodeName)).CustomDataSourceTypeName = typelist[i].ToString();
                ((IModelCustomDataSource)node.GetNode(childNodeName)).CustomDataSourceType = typelist[i];

                // Выходной тип linq-запроса
                Type baseType = typelist[i].BaseType;
                Type[] paramTypes = baseType.GetGenericArguments();
                foreach (Type type in paramTypes) {
                    ((IModelCustomDataSource)node.GetNode(childNodeName)).ObjectType = type;
                    break;
                }
            }
        }

        public virtual Type[] GetTypesInNamespace(Assembly assembly, string nameSpace) {
            //Type T = typeof(LinqCollectionSource);
            Type T = typeof(IQueryDataSource);
            // чтобы что-то показывалось отменяется проверка на производность типов: " && t != T"    return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsClass).Where(t => typeof(T).IsAssignableFrom(t) && t != typeof(T)).ToArray();
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsClass).Where(t => T.IsAssignableFrom(t)).Where(t => !t.IsAbstract).ToArray();
        }

    }

}






