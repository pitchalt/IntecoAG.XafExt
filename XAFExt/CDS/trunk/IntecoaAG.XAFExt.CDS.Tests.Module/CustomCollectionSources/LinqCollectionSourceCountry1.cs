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
    public class LinqCollectionSourceCountry1 : LinqCollectionSource<testCountry1>
    {
        public LinqCollectionSourceCountry1(IObjectSpace objectSpace)
            : base(objectSpace) {
        }

        public override IQueryable<testCountry1> GetQuery() {
            XPQuery<testCountry> countries = new XPQuery<testCountry>(session);
            queryCore = from item in countries
                        select new testCountry1 {
                            NameShort = item.NameShort,
                            Comment = item.Comment
                        };
            return queryCore;
        }

    }
}