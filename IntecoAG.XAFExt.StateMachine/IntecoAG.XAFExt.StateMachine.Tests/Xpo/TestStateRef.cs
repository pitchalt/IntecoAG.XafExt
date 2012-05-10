using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IntecoAG.XAFExt.StateMachine.Tests.Xpo {

    [DefaultProperty("Code")]
    [DefaultClassOptions]
    public class TestStateRefValue : BaseObject {
        public TestStateRefValue(Session ses) : base(ses) { }
        [Size(10)]
        public String Code;
        [Size(70)]
        public String Name;
    }

    [DefaultClassOptions]
    public class TestStateRef : BaseObject {
        public TestStateRef(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private TestStateRefValue _TestStateRefValue;
        public TestStateRefValue TestStateRefValue {
            get { return _TestStateRefValue; }
            set { SetPropertyValue<TestStateRefValue>("TestStateRefValue", ref _TestStateRefValue, value); }
        }
    }
}
