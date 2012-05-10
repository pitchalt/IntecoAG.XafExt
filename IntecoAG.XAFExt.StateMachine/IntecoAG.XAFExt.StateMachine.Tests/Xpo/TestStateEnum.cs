using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace IntecoAG.XAFExt.StateMachine.Tests.Xpo {

    public enum TestStateEnumValue { 
        VALUE_START = 1,
        VALUE_APPROVE = 2,
        VALUE_COMPLETE = 3
    }

    [DefaultClassOptions]
    public class TestStateEnum : BaseObject {
        public TestStateEnum(Session ses) : base(ses) { }

        public override void AfterConstruction() {
            base.AfterConstruction();
            State = TestStateEnumValue.VALUE_START;
        }

        private TestStateEnumValue _State;
        public TestStateEnumValue State {
            get { return _State; }
            set { SetPropertyValue<TestStateEnumValue>("State", ref _State, value); }
        }

    }
}
