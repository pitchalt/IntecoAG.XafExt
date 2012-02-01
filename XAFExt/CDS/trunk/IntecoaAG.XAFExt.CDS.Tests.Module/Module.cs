using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
//
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;

namespace IntecoaAG.XAFExt.CDS.Tests.Module {
    public sealed partial class XAFExtCDSTestsModule : ModuleBase {
        public XAFExtCDSTestsModule() {
            InitializeComponent();
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            Application.CreateCustomCollectionSource += new EventHandler<CreateCustomCollectionSourceEventArgs>(Application_CreateCustomCollectionSource);
        }

        void Application_CreateCustomCollectionSource(object sender, CreateCustomCollectionSourceEventArgs e) {
            CollectionSourceBase collectionSourceBase = CustomCollectionSourceGenerator.Create((XafApplication)sender, e.ObjectSpace, e.ListViewID);
            if (collectionSourceBase != null) e.CollectionSource = collectionSourceBase;
        }

        // ������� � ������ ���� ��� �������� ��������� ������
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);

            extenders.Add<IModelApplication, IModelCustomDataSourceExtension>();
            extenders.Add<IModelListView, IModelCollectionDataSource>();
        }

        public override void AddGeneratorUpdaters(DevExpress.ExpressApp.Model.Core.ModelNodesGeneratorUpdaters updaters) {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new CustomChildNodesUpdater());
        }

    }

    public class CustomChildNodesUpdater : ModelNodesGeneratorUpdater<CustomDataSourceNodesGenerator> {

        public Assembly assembly = Assembly.GetExecutingAssembly();
        //public string nameSpace = "IntecoaAG.XAFExt.CDS.Tests";

        //public Type[] GetTypesInNamespace(Assembly assembly, string nameSpace) {
        public Type[] GetTypesInNamespace(Assembly assembly) {
            //Type T = typeof(LinqCollectionSource);
            Type T = typeof(IQueryDataSource);
            // ����� ���-�� ������������ ���������� �������� �� ������������� �����: " && t != T"    return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsClass).Where(t => typeof(T).IsAssignableFrom(t) && t != typeof(T)).ToArray();
            //return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsClass).Where(t => T.IsAssignableFrom(t)).Where(t => !t.IsAbstract).ToArray();
            return assembly.GetTypes().Where(t => t.IsClass).Where(t => T.IsAssignableFrom(t)).Where(t => !t.IsAbstract).ToArray();

            //var type = typeof(IQueryDataSource);
            //var linqQuertType = typeof(LinqQuery);
            //Type[] res = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p)).Where(p => p.IsSubclassOf(linqQuertType)).ToArray();
            //return res;

        }


        public override void UpdateNode(ModelNode node) {
            Type[] typelist = GetTypesInNamespace(assembly);

            for (int i = 0; i < typelist.Length; i++) {
                string childNodeName = typelist[i].Name;
                node.AddNode<IModelCustomDataSource>(childNodeName);
                ((IModelCustomDataSource)node.GetNode(childNodeName)).Description = typelist[i].Name;
                ((IModelCustomDataSource)node.GetNode(childNodeName)).CustomDataSourceType = typelist[i];

                // �������� ��� linq-�������
                Type baseType = typelist[i].BaseType;
                Type[] paramTypes = baseType.GetGenericArguments();
                foreach (Type type in paramTypes) {
                    ((IModelCustomDataSource)node.GetNode(childNodeName)).ObjectType = type;
                    break;
                }
            }
        }
    }
}
