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

namespace IntecoaAG.XAFExt.CDS 
{

    public abstract class LinqQuery<TResult, TSource> : IQueryDataSource, IQueryable<TResult>, IQueryable
    {
        //private IObjectSpace objectSpace = null;
        protected Session session = null;
        protected XPQuery<TSource> _Provider;

        public LinqQuery(IObjectSpace os) {
            //objectSpace = os;
            session = ((ObjectSpace)os).Session;
            _Provider = new XPQuery<TSource>(session);
        }

        public abstract IQueryable<TResult> GetQuery();

        public IEnumerator<TResult> GetEnumerator() {
            return GetQuery().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Type ElementType {
            get { return typeof(TResult); }
        }

        public Expression Expression {
            get { return GetQuery().Expression; }
        }

        public XPQuery<TSource> Provider {
            get { return _Provider; }
        }

        IQueryProvider IQueryable.Provider {
            get { return _Provider; }
        }

        IQueryable IQueryDataSource.GetQuery() { 
            return GetQuery();
        }
    }
}