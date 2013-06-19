using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Data.Filtering;
using NUnit.Framework;

namespace IntecoAG.XafExt.Data.Filtering.Tests {
    [TestFixture]
    public class GeneralTests {
        [Test]
        public void SimpleTest() {
            CriteriaOperator oper1 = CriteriaExpressionOperator<TestObject1>.Parse(obj => obj.PropertyOne == "A" + "B");
            CriteriaOperator oper2 = CriteriaExpressionOperator<TestObject1>.Parse(obj => obj.PropertyOne == obj.PropertyTwo);
            CriteriaOperator oper3 = CriteriaExpressionOperator<TestObject1>.Parse(obj => obj.RefObject.Property == obj.PropertyTwo);
        }
    }
}
