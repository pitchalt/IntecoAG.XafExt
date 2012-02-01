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

namespace IntecoaAG.XAFExt.CDS.Tests 
{
    [NonPersistent]
    public class LinqQueryCountry3 : LinqQuery
    {
        public LinqQueryCountry3(IObjectSpace os)
            : base(os) {
        }

        public override IQueryable GetQuery() {
            XPQuery<testCountry> countries = new XPQuery<testCountry>(session);
            var queryCore = from item in countries
                        select new testCountry2 {
                            Name = item.NameShort + " (" + item.NameFull + "): " + item.Comment
                        };
            return queryCore;
        }
    }
}