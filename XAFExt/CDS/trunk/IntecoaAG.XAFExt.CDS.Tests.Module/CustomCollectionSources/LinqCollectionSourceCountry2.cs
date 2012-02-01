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
    public class LinqCollectionSourceCountry2 : LinqCollectionSource<testCountry2>
    {
        public LinqCollectionSourceCountry2(IObjectSpace objectSpace)
            : base(objectSpace) {
        }

        public override IQueryable<testCountry2> GetQuery() {
            XPQuery<testCountry> countries = new XPQuery<testCountry>(session);
            queryCore = from item in countries
                        select new testCountry2 {
                            Name = item.NameShort + " (" + item.NameFull + ")"
                        };
            return queryCore;
        }

    }
}