using System;
using System.Linq;
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
    public class LinqCollectionSource : CollectionSourceBase //, IQueryDataSource
    {
        private IQueryable queryCore = null;
        //private Session session = null;

        
        //private IBindingList collectionCore;
        private ITypeInfo objectTypeInfoCore;
        
        //protected CollectionDataSource(IObjectSpace objectSpace)
        //    : base(objectSpace) {
        //}
        
        public override bool? IsObjectFitForCollection(object obj) {
            return false;
        }
        
        protected override void ApplyCriteriaCore(CriteriaOperator criteria) { }

        public override ITypeInfo ObjectTypeInfo {
            get { return objectTypeInfoCore; }
        }



        public LinqCollectionSource(IObjectSpace objectSpace, IQueryable query)
            : base(objectSpace) {
            //session = ((ObjectSpace)(this.ObjectSpace)).Session;
            queryCore = query;
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(query.ElementType);
        }

        protected override object CreateCollection() {
//            ((XPQueryBase)queryCore).Session = ((ObjectSpace)ObjectSpace).Session;
//            var queryList = Activator.CreateInstance(typeof(List<>).MakeGenericType(queryCore.ElementType), queryCore);
//            return Activator.CreateInstance(typeof(BindingList<>).MakeGenericType(queryCore.ElementType), queryList);
            BindingList<Object> result = new BindingList<Object>();
            foreach (var item in Query) {
                result.Add(item);
            }
            return result;
        }

        public IQueryable Query {
            get { return queryCore; }
            set { queryCore = value; }
        }

        //public virtual IQueryable<T> GetQuery() {
        //    return null;
        //}

    }
}