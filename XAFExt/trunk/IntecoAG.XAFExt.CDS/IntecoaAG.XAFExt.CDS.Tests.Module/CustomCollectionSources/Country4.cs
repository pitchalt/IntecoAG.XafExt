using System;
using System.Linq;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace IntecoAG.XAFExt.CDS.Tests
{

    [NavigationItem(true)]
    [NonPersistent]
    public class testCountry4: LinqQuery<testCountry4, testCountry>
    {
        #region ПОЛЯ КЛАССА
        public testCountry4() : base(null) { }
        public testCountry4(Session ses) : base(ses) { }

        private string _NameFull;
        private string _Comment;

        #endregion

        #region СВОЙСТВА КЛАССА

        public string NameFull {
            get { return _NameFull; }
            set { _NameFull = value; }
        }

        public string Comment {
            get { return _Comment; }
            set { _Comment = value; }
        }

        #endregion

        #region МЕТОДЫ

        public override IQueryable<testCountry4> GetQuery() {
            var queryCore = from item in Provider
                            select new testCountry4 {
                                NameFull = item.NameShort + " (" + item.NameFull + "): " + item.Comment,
                                Comment = item.Comment
                            };
            return queryCore;
        }

        #endregion
    }

}