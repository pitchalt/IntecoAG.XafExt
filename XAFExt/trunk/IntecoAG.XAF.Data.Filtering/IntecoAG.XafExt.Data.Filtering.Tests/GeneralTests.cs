using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.Data.Filtering;
using NUnit.Framework;

namespace IntecoAG.XafExt.Data.Filtering.Tests {
    [TestFixture]
    public class GeneralTests  {
        [Test]
        public void SimpleTest() {
            String arg1 = "val_arg1";
            String arg2 = "val_arg2";
            CriteriaOperator oper_etalon = null;
            CriteriaOperator oper_parse = null;
            oper_parse = CriteriaExpression<TestObject1>.Parse(
                obj => obj.PropertyOne == "A" + "B"
            );
            oper_etalon = new BinaryOperator(
                new OperandProperty("PropertyOne"), 
                new OperandValue("AB"), 
                BinaryOperatorType.Equal);
            Assert.IsTrue( IsOperatorsEqual( oper_etalon, oper_parse));
            oper_parse = CriteriaExpression<TestObject1>.Parse(obj => obj.PropertyOne == obj.PropertyTwo);
            oper_etalon = new BinaryOperator(
                new OperandProperty("PropertyOne"), 
                new OperandProperty("PropertyTwo"), 
                BinaryOperatorType.Equal);
            Assert.IsTrue( IsOperatorsEqual( oper_etalon, oper_parse));
            oper_parse = CriteriaExpression<TestObject1>.Parse(obj => obj.RefObject.Property == obj.PropertyTwo);
            oper_etalon = new BinaryOperator(
                new OperandProperty("RefObject.Property"), 
                new OperandProperty("PropertyTwo"), 
                BinaryOperatorType.Equal);
            Assert.IsTrue( IsOperatorsEqual( oper_etalon, oper_parse));
            oper_parse = CriteriaExpression<TestObject1>.Parse(obj => obj.PropertyOne == arg1);
            oper_etalon = new BinaryOperator(
                new OperandProperty("PropertyOne"), 
                new OperandValue(arg1), 
                BinaryOperatorType.Equal);
            Assert.IsTrue( IsOperatorsEqual( oper_etalon, oper_parse));
            oper_parse = CriteriaExpression<TestObject1>.Parse(obj => arg1 == arg2);
            oper_etalon = new BinaryOperator(
                new OperandValue(arg1), 
                new OperandValue(arg2), 
                BinaryOperatorType.Equal);
            Assert.IsTrue( IsOperatorsEqual( oper_etalon, oper_parse));
        }

        public Boolean IsOperatorsEqual(CriteriaOperator op_left, CriteriaOperator op_right) {
            Assert.AreEqual(op_left.GetType(), op_right.GetType());
            if (op_left is BinaryOperator) {
                return IsOperatorsEqual((BinaryOperator)op_left, (BinaryOperator)op_right);
            }
            if (op_left is OperandProperty) {
                return IsOperatorsEqual((OperandProperty)op_left, (OperandProperty)op_right);
            }
            if (op_left is OperandValue) {
                return IsOperatorsEqual((OperandValue)op_left, (OperandValue)op_right);
            }
            throw new NotImplementedException("Operator type " + op_left.GetType() + " not implemented");
        }

        protected Boolean IsOperatorsEqual(BinaryOperator op_left, BinaryOperator op_right) {
            Assert.AreEqual(op_left.OperatorType, op_right.OperatorType);
            return IsOperatorsEqual(op_left.LeftOperand, op_right.LeftOperand) &&
                   IsOperatorsEqual(op_left.RightOperand, op_right.RightOperand);
        }

        protected Boolean IsOperatorsEqual(OperandProperty op_left, OperandProperty op_right) {
            Assert.AreEqual(op_left.PropertyName, op_right.PropertyName);
            return true;
        }

        protected Boolean IsOperatorsEqual(OperandValue op_left, OperandValue op_right) {
            Assert.AreEqual(op_left.Value.GetType(), op_right.Value.GetType());
            Assert.AreEqual(op_left.Value, op_right.Value);
            return true;
        }
    }
}
