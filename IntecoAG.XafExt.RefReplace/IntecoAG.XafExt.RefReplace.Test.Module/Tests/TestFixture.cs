using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntecoAG.XafExt.RefReplace.Test.Module.Variant1;
using IntecoAG.XafExt.RefReplace.Module.Win.Logic;
using IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects;
using System.Reflection;
using IntecoAG.XafExt.RefReplace.Test.Module.Tests.Variant2;
using IntecoAG.XafExt.RefReplace.Test.Module.Tests.Variant3;

namespace IntecoAG.XafExt.RefReplace.Test.Module.Tests {
  public  class TestFixture:IDisposable {
     
        public TestApplication Application { get; set; }
        public ModuleBase TestModule { get; set; }
        public TestFixture() {
            Application = new TestApplication();
            Application.ApplicationName = "TestApp";
            Application.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            Application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            TestModule = new ModuleBase();
            TestModule.AdditionalExportedTypes.Add(typeof(Party));
            TestModule.AdditionalExportedTypes.Add(typeof(ReplaceTable));
            TestModule.AdditionalExportedTypes.Add(typeof(Contract));
            TestModule.AdditionalExportedTypes.Add(typeof(ContractRent));
            TestModule.AdditionalExportedTypes.Add(typeof(ContractProduct));
            TestModule.AdditionalExportedTypes.Add(typeof(LegalPerson));
            TestModule.AdditionalExportedTypes.Add(typeof(PartyTest));
            TestModule.AdditionalExportedTypes.Add(typeof(ParentClass));
            TestModule.AdditionalExportedTypes.Add(typeof(Child1));
            TestModule.AdditionalExportedTypes.Add(typeof(Child2));
            TestModule.AdditionalExportedTypes.Add(typeof(ChildV2));
            TestModule.AdditionalExportedTypes.Add(typeof(ChildV22));
            TestModule.AdditionalExportedTypes.Add(typeof(ParentV2));
            TestModule.AdditionalExportedTypes.Add(typeof(MyChild1));
            Application.Modules.Add(TestModule);
            Application.Setup();
            using (IObjectSpace os = Application.CreateObjectSpace()) {

                MyChild1 m1 = os.CreateObject<MyChild1>();

           
                Child1 child1 = os.CreateObject<Child1>();
                Child2 child2 = os.CreateObject<Child2>();
                ChildV2 c2 = os.CreateObject<ChildV2>();
                ChildV2 c21 = os.CreateObject<ChildV2>();
                ChildV22 c22 = os.CreateObject<ChildV22>();

                ParentClass parent = os.CreateObject<ParentClass>();
                PartyTest partyTest2 = os.CreateObject<PartyTest>();
                partyTest2.Nick = "MyParty";
                m1.MyPartySet(partyTest2);
                PartyTest partyOther = os.CreateObject<PartyTest>();
                partyOther.Nick = "Other";
                c2.MyProperty = partyTest2;
                c21.MyProperty = partyTest2;
                PartyTest partyTest = os.CreateObject<PartyTest>();
                partyTest.Nick = "TestLizon";
                c22.MyProperty2 = partyOther;
                parent.GetType().GetField("_party", System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance | BindingFlags.Public
                    ).SetValue(parent, partyTest);

                child1.MyProperty = partyTest;
                child2.MyProperty2 = partyTest;
                //PartyTest partyTest2 = os.CreateObject<PartyTest>();

                //partyTest2.Nick = "Piter";
                //var p = parent.GetType().GetField("_party", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                //p.SetValue(parent, partyTest);



                var party = os.CreateObject<Party>();
                var party2 = os.CreateObject<Party>();
                party2.Nick= "Vasya";
                party.Nick = "LIZON";
                var contractProduct = os.CreateObject<ContractProduct>();
                var contractRent = os.CreateObject<ContractRent>();
                contractRent.Customer = party;
                contractProduct.Customer = party;

                os.CommitChanges();

            }
        }

        public void Dispose() {
            Application.Dispose();
        }
    }
}
