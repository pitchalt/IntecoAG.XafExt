using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;

namespace IntecoAG.XAFExt.CDS 
{

    public abstract class LinqQuery<TResult, TSource> : IQueryDataSource, IQueryable<TResult>, IQueryable
    {
        //private IObjectSpace objectSpace = null;
        protected Session session = null;
        protected XPQuery<TSource> _Provider;

        public LinqQuery(Session ses) {
            //objectSpace = os;
            //session = ((ObjectSpace)os).Session;
            session = ses;
            if (session != null)
                _Provider = new XPQuery<TSource>(session);
        }

        public abstract IQueryable<TResult> GetQuery();

        public IEnumerator<TResult> GetEnumerator() {
            return GetQuery().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        [Browsable(false)]
        public Type ElementType {
            get { return typeof(TResult); }
        }

        [Browsable(false)]
        public Expression Expression {
            get { 
                if (session != null)
                    return GetQuery().Expression;
                else
                    return null;
            }
        }

        [Browsable(false)]
        public XPQuery<TSource> Provider {
            get { return _Provider; }
        }
        [Browsable(false)]
        IQueryProvider IQueryable.Provider {
            get { return _Provider; }
        }

        IQueryable IQueryDataSource.GetQuery() { 
            return GetQuery();
        }
    }
}