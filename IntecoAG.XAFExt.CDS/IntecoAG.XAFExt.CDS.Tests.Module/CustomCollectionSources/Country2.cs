using System;
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
    public class testCountry2
    {

        #region ПОЛЯ КЛАССА

        private string _Name;

        #endregion

        #region СВОЙСТВА КЛАССА

        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }

        #endregion

        #region МЕТОДЫ

        #endregion
    }

}