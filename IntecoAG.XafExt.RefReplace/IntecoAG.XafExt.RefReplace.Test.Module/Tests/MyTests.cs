using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DevExpress.ExpressApp;
using IntecoAG.XafExt.RefReplace.Test.Module.Variant1;
using IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects;
using IntecoAG.XafExt.RefReplace.Module.Win.Logic;
using IntecoAG.XafExt.RefReplace.Controllers;

using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Data.Filtering;
using System.Reflection;
using DevExpress.Xpo;
using IntecoAG.XafExt.RefReplace.Test.Module.Tests.Variant2;
using IntecoAG.XafExt.RefReplace.Test.Module.Tests.Variant3;

namespace IntecoAG.XafExt.RefReplace.Test.Module.Tests {
   public class MyTests:IDisposable, IClassFixture<TestFixture> {
        private readonly TestFixture fixture;
        private readonly IObjectSpace objectSpace;
        public TestApplication Application {
            get { return fixture.Application; }
        }
        public MyTests(TestFixture fixture) {
            this.fixture = fixture;
            objectSpace = Application.CreateObjectSpace();
        }

        [Fact]
        public void TestReadOnlyField() {
            String s = typeof(Contract).FullName;
    
            var ps = objectSpace.GetObjects<ParentClass>();
            
            ParentClass parent = ps.Last();
            var p = typeof(ParentClass).GetField("_party", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            var cr = p.GetValue(parent);
            var child1 = objectSpace.GetObjects<Child1>();
            foreach(var ch in child1) {
                ch.MyProperty = cr as PartyTest;
            }
            var child2 = objectSpace.GetObjects<Child2>();
            foreach (var ch in child2) {
                ch.MyProperty2 = cr as PartyTest;
            }
            Assert.NotNull(cr);
            var partys = objectSpace.GetObjects(typeof(PartyTest), new BinaryOperator("Nick", "MyParty"));
            var items = objectSpace.GetObjects(typeof(PartyTest), new BinaryOperator("Nick", "TestLizon"));
            PartyTest c = null;
            PartyTest newParty = null;
            foreach (var r in partys) {
                newParty = r as PartyTest;
            }
            foreach (var r in items)
            {
                c = r as PartyTest;
            }
            //parent.GetType().GetField("_party", System.Reflection.BindingFlags.NonPublic |
            //       System.Reflection.BindingFlags.Instance | BindingFlags.Public
            //       ).SetValue(parent, c);
            var info = XafTypesInfo.CastTypeToTypeInfo(typeof(PartyTest).BaseType);
            //Assert.Null(info);
            Assert.NotNull(newParty);
  
            CreateTable(cr, newParty);
            var res = objectSpace.GetObjects<ParentClass>();
            //foreach (var r in res)
            //{
            //    var q = parent.GetType().GetField("_party", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //    Assert.Equal(q.GetValue(parent), party2);
            //}

            var childs = objectSpace.GetObjects<Child2>();
            
            foreach (var r in childs)
            {
                //Assert.Null(r.MyProperty);
                Assert.Equal(r.MyProperty2.Nick, newParty.Nick);
            }

        }
        [Fact]
        public void TestReadOnlyProperty() {
            var items = objectSpace.GetObjects(typeof(PartyTest), new BinaryOperator("Nick", "TestLizon"));

            var v2c = objectSpace.GetObjects<ParentV2>();
            var partys = objectSpace.GetObjects(typeof(PartyTest), new BinaryOperator("Nick", "MyParty"));

          var p =  typeof(MyChild1).GetProperty("MyParty");
            //Assert.Null(p.Name);


            //Assert.Null(partys.Count);
            PartyTest c = null;
            PartyTest newParty = null;
            foreach (var r in items) {
                newParty = r as PartyTest;
            }
            foreach (var r in partys) {
                c = r as PartyTest;
            }
           // CriteriaOperator op = new BinaryOperator(new OperandProperty("MyPart"), new OperandValue(c), BinaryOperatorType.Equal);
           //var objs = objectSpace.GetObjects(typeof(MyChild1), op);
           // Assert.Equal(9, objs.Count);
            CreateTable(c, newParty);
            var res = objectSpace.GetObjects<MyChild1>();
            foreach (var r in res) {
                Assert.Equal(newParty.Nick, r.MyParty.Nick);
            }

        }

        [Fact]
        public void TestMyProperty()
        {
            var items = objectSpace.GetObjects(typeof(PartyTest), new BinaryOperator("Nick", "TestLizon"));
      
            var v2c = objectSpace.GetObjects<ParentV2>();
            var partys = objectSpace.GetObjects(typeof(PartyTest), new BinaryOperator("Nick", "MyParty"));
         
            //Assert.Null(partys.Count);
            PartyTest c = null;
            PartyTest newParty = null;
            foreach (var r in items) {
                newParty = r as PartyTest;
            }
            foreach (var r in partys) {
               c = r as PartyTest;
            }
            //Assert.Null(newParty.Nick);
            CreateTable(c, newParty);

            var res = objectSpace.GetObjects<ChildV2>();
            foreach (var r in res) {
                Assert.Equal(newParty, r.MyProperty);
            }
            var res2 = objectSpace.GetObjects<ChildV22>();
         
            foreach (var r in res2) {
              
                Assert.NotEqual(newParty.Nick, r.MyProperty2.Nick);
            }

        }


        public void CreateTable(object c, object party2) {
            //текущий и объект для замены
            var findVC = new SearchViewController();




            ReferenceTable t = Logic.FindAllRef(objectSpace, c);
            ReplaceTable replace = objectSpace.CreateObject<ReplaceTable>();
            List<ReferenceItem> items = new List<ReferenceItem>();
            items.AddRange(t.Items);
            replace.Items.AddRange(items);
            List<ObjItem> objs = new List<ObjItem>();
            objs.AddRange(t.Objects);
            replace.Objects.AddRange(objs);
            findVC.SetView(Application.CreateDetailView(objectSpace, replace));


            findVC.View.CurrentObject = replace;
            DevExpress.ExpressApp.DC.ITypeInfo info = XafTypesInfo.CastTypeToTypeInfo(typeof(Party));

            IMemberInfo m = info.KeyMember;

            String nameKey = m.Name;
            var y = party2.GetType().GetProperty(nameKey).GetValue(party2);

            replace.NewId = y.ToString();

            var g = System.Guid.Parse(replace.NewId);
            //Assert.NotEqual(party2.Oid, g);
            replace.CurrentType = c.GetType();
            var p = findVC.View.ObjectSpace.GetObjectByKey(replace.CurrentType, g);
            var prop = typeof(ParentClass).GetField("_party", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var ps = objectSpace.GetObjects<ParentClass>();
            ParentClass parent = ps.Last();
            var v = prop.GetValue(parent);
            Assert.NotNull(((PartyTest) v).Nick);
            //Assert.Null(replace.Objects.Last().Obj);

            //Assert.Null(i.Type);
            CriteriaOperator op = new BinaryOperator(new OperandProperty("_party"), new OperandValue(c), BinaryOperatorType.Equal);

         
           
            //Assert.Equal(9, replace.Objects.Count);
            //Assert.Equal(3, ob.Count);
            //Assert.Equal(9, replace.Objects.Count);

            findVC.ApplyAction.DoExecute();
        }
        [Fact]
        public void TestAction() {
          
            var ps=  objectSpace.GetObjects(typeof(Party), new BinaryOperator("Nick", "Vasya"));
            Party party2 = null;
            foreach(var r in ps)
            {
                party2 = r as Party;
            }
          
            //var party = objectSpace.CreateObject<Party>();
            var contracts = objectSpace.GetObjects<Contract>();
           
            var c = contracts.Last().Customer;

            CreateTable(c, party2);
            foreach (var r in contracts) {

                Assert.Equal(party2, r.Customer);
                //Assert.Equal(r.Party.Nick, party2.Nick);
            }

        }

        public void Dispose() {
            objectSpace.Dispose();
        }
    }
}
