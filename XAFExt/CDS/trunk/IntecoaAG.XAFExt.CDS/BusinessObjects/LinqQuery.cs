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

namespace IntecoaAG.XAFExt.CDS 
{
    [NonPersistent]
    public class LinqQuery : IQueryDataSource
    {
        //private IObjectSpace objectSpace = null;
        protected Session session = null;

        public LinqQuery(IObjectSpace os) {
            //objectSpace = os;
            session = ((ObjectSpace)os).Session;
        }

        public virtual IQueryable GetQuery() {
            return null;
        }
    }
}