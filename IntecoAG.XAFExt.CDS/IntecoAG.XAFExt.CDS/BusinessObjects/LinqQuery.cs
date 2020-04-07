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

namespace IntecoAG.XafExt.CDS 
{

    public abstract class LinqQuery<TResult, TSource> : IQueryDataSource, IQueryable<TResult>, IQueryable
    {
        //private IObjectSpace objectSpace = null;
        protected IObjectSpace _ObjectSpace;
        protected IQueryable<TSource> _Provider;

        public LinqQuery(IObjectSpace os) {
            //objectSpace = os;
            //session = ((ObjectSpace)os).Session;
            _ObjectSpace = os;
            if (os != null)
                _Provider = os.GetObjectsQuery<TSource>();
//            if (session != null)
//                _Provider = new XPQuery<TSource>(session);
        }

        public abstract IQueryable<TResult> GetQuery();

        public virtual IEnumerator<TResult> GetEnumerator() {
            return GetQuery().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        //[Browsable(false)]
        public Type ElementType {
            get { return typeof(TResult); }
        }

        public Type SourceType {
            get { return typeof(TSource); }
        }

        //[Browsable(false)]
        public Expression Expression {
            get { 
                return GetQuery().Expression;
            }
        }

        //[Browsable(false)]
        public IQueryable<TSource> Provider {
            get { return _Provider; }
        }

        //[Browsable(false)]
        IQueryProvider IQueryable.Provider {
            get { return _Provider.Provider; }
        }

        IQueryable IQueryDataSource.GetQuery() { 
            return GetQuery();
        }
    }
}