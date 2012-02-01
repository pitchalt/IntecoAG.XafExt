using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
//
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;

namespace IntecoaAG.XAFExt.CDS
{
    public class CustomDataSourceNodesGenerator : ModelNodesGeneratorBase {

        public Assembly assembly = Assembly.GetExecutingAssembly();
        //public Assembly assembly = Assembly.GetEntryAssembly();   // Не работает
        //public Assembly assembly = Assembly.GetCallingAssembly();
        //public string nameSpace = "IntecoaAG.XAFExt.CDS.Tests";

        protected override void GenerateNodesCore(ModelNode node) {
            GenerateNodesCoreSub(node, assembly);
        }

        public static void GenerateNodesCoreSub(ModelNode node, Assembly assembly) {
            Type[] typelist = GetTypesInNamespace(assembly);   //, nameSpace);

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

        public static Type[] GetTypesInNamespace(Assembly assembly) {   //, string nameSpace) {
            // Старый вариант - поиск только в указанной сборке.
            Type T = typeof(IQueryDataSource);
            var linqQuertType = typeof(LinqQuery);
            //return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsClass).Where(t => T.IsAssignableFrom(t)).Where(t => !t.IsAbstract).ToArray();
            return assembly.GetTypes().Where(t => t.IsClass).Where(t => T.IsAssignableFrom(t)).Where(t => !t.IsAbstract).Where(p => p.IsSubclassOf(linqQuertType)).ToArray();

            /*
            ArrayList list = new ArrayList();

            Type ti = typeof(IQueryDataSource); 
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies()) { 
                foreach (Type t in asm.GetTypes()) { 
                    if (ti.IsAssignableFrom(t)) { 
                        // here's your type in t 
                        list.Add(t);
                    } 
                } 
            }
            return (System.Type[])list.ToArray();
            */

            /*
            // Новый вариант - просмотр всех загруженных сборок и типов на предмет поиска таких, которые поддерживают нужный интерфейс IQueryDataSource и являются производными от LinqQuery
            // НЕ РАБОТАЕТ, т.к. нужная сборка не подключена и о ней данная компонента ничего не знает
            var type = typeof(IQueryDataSource);
            var linqQuertType = typeof(LinqQuery);
            Type[] res = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p)).Where(p => p.IsSubclassOf(linqQuertType)).ToArray();
            return res;
            */
        } 
        
    }

}

