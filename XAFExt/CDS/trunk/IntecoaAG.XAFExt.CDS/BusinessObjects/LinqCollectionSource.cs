using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;

namespace IntecoaAG.XAFExt.CDS 
{
    public abstract class LinqCollectionSource<T> : CollectionDataSource, IQueryDataSource
    {
        protected IQueryable<T> queryCore = null;
        protected Session session = null;
        
        protected LinqCollectionSource(IObjectSpace objectSpace)
            : base(objectSpace) {
            session = ((ObjectSpace)(this.ObjectSpace)).Session;
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(typeof(T));
            queryCore = GetQuery();
        }

        protected override object CreateCollection() {
            ((XPQueryBase)queryCore).Session = ((ObjectSpace)ObjectSpace).Session;
            var queryList = Activator.CreateInstance(typeof(List<>).MakeGenericType(queryCore.ElementType), queryCore);
            return Activator.CreateInstance(typeof(BindingList<>).MakeGenericType(queryCore.ElementType), queryList);
        }

        public virtual IQueryable<T> GetQuery() {
            return null;
        }

    }
}