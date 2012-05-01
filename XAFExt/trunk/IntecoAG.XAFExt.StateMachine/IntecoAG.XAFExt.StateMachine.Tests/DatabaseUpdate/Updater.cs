using System;

using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;

using IntecoAG.XAFExt.StateMachine.Tests.Xpo;

namespace IntecoAG.XAFExt.StateMachine.Tests.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            TestStateRefValue val1 = ObjectSpace.FindObject<TestStateRefValue>(new BinaryOperator("Code", "Code1"));
            if (val1 == null) {
                val1 = ObjectSpace.CreateObject<TestStateRefValue>();
                val1.Code = "Code1";
                val1.Name = "Name1";
            }
            TestStateRefValue val2 = ObjectSpace.FindObject<TestStateRefValue>(new BinaryOperator("Code", "Код2"));
            if (val2 == null) {
                val2 = ObjectSpace.CreateObject<TestStateRefValue>();
                val2.Code = "Код2";
                val2.Name = "Имя2";
            }

        }
    }
}
