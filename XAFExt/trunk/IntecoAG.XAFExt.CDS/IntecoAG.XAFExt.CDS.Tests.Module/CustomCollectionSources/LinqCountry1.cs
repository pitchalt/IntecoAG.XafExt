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

namespace IntecoAG.XAFExt.CDS.Tests 
{
    public class LinqQueryCountry1 : LinqQuery<testCountry1, testCountry>
    {
        public LinqQueryCountry1(Session ses)
            : base(ses) {
        }

        public override IQueryable<testCountry1> GetQuery() {
            var queryCore = from item in Provider
                        select new testCountry1 {
                            NameShort = item.NameShort,
                            Comment = item.Comment
                        };
            return queryCore;
        }
    }
}