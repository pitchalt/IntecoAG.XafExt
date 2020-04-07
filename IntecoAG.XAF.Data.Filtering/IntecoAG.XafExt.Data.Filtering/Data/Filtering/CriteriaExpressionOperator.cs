using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DevExpress.Data.Filtering {
    public class CriteriaExpressionOperator<T> {
        public static CriteriaOperator Parse(Expression<Func<T, Boolean>> exp_tree) {
            return new BinaryOperator();
        }
    }
}
