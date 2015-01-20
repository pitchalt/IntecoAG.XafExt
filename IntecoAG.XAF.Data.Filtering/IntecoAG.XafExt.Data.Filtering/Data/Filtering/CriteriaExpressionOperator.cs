using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
//
using DevExpress.Data;
using DevExpress.Data.Filtering;
//
namespace DevExpress.Data.Filtering {

    public enum ParseErrorCodes {
        ERROR_TYPE = 1
    }

    public class ParseErrorException : Exception {
        private ParseErrorCodes _Code;

        public ParseErrorCodes Code {
            get { return _Code; }
            set { _Code = value; }
        }

        public ParseErrorException(String text, ParseErrorCodes error_code) : base(text) {
            _Code = error_code;
        }
    }

    public class CriteriaExpression<T> {
        public static CriteriaOperator Parse(Expression<Func<T, Boolean>> exp_tree) {
            return Parse((Expression)exp_tree.Body);
        }

        protected static CriteriaOperator Parse(Expression exp_tree) {
//            if (exp_tree.Type != typeof(System.Boolean))
//                throw new ParseErrorException("Expression type not is Boolean", ParseErrorCodes.ERROR_TYPE);
            switch (exp_tree.NodeType) { 
                case ExpressionType.And:
                case ExpressionType.Or:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    return Parse((BinaryExpression)exp_tree);
                case ExpressionType.MemberAccess:
                    return Parse((MemberExpression)exp_tree);
                case ExpressionType.Constant:
                    return Parse((ConstantExpression)exp_tree);
                default:
                    throw new NotImplementedException("Not supported expression type: " + exp_tree.NodeType);
            }
        }

        protected static CriteriaOperator Parse(BinaryExpression exp_tree) {
            if (exp_tree.Type != typeof(System.Boolean))
                throw new ParseErrorException("Expression type not is Boolean", ParseErrorCodes.ERROR_TYPE);
            BinaryOperator oper = new BinaryOperator();
            switch (exp_tree.NodeType) {
                case ExpressionType.And:
                    oper.OperatorType = BinaryOperatorType.BitwiseAnd;
                    break;
                case ExpressionType.Or:
                    oper.OperatorType = BinaryOperatorType.BitwiseOr;
                    break;
                case ExpressionType.Equal:
                    oper.OperatorType = BinaryOperatorType.Equal;
                    break;
                case ExpressionType.GreaterThan:
                    oper.OperatorType = BinaryOperatorType.Greater;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    oper.OperatorType = BinaryOperatorType.GreaterOrEqual;
                    break;
                case ExpressionType.LessThan:
                    oper.OperatorType = BinaryOperatorType.Less;
                    break;
                case ExpressionType.LessThanOrEqual:
                    oper.OperatorType = BinaryOperatorType.LessOrEqual;
                    break;
                default:
                    throw new NotImplementedException("Not supported expression type: " + exp_tree.NodeType);
            }
            oper.LeftOperand = Parse(exp_tree.Left);
            oper.RightOperand = Parse(exp_tree.Right);
            return oper;
        }

        protected static CriteriaOperator Parse(MemberExpression exp_tree) {
//            switch (exp_tree.Expression.NodeType) {
//                case ExpressionType.
//            }
//            if (exp_tree.Expression.NodeType)
            return new OperandProperty(ParseMemberName(exp_tree));
        }

        private static String ParseMemberName(MemberExpression exp_tree) {
            if (exp_tree.Expression.NodeType == ExpressionType.Parameter)
                return exp_tree.Member.Name;
            else
                return ParseMemberName((MemberExpression)exp_tree.Expression) + "." + exp_tree.Member.Name;
        }

        protected static CriteriaOperator Parse(ConstantExpression exp_tree) {
            OperandValue val = new OperandValue(exp_tree.Value);
            return val;
        }
    }

}
