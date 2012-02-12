using System;
using System.Collections.Generic;

using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace IntecoaAG.XAFExt.CDS.Tests
{

    [NavigationItem(true)]
    [NonPersistent]
    public class testCountry1
    {

        #region ПОЛЯ КЛАССА

        private string _NameShort;
        private string _Comment;

        #endregion

        #region СВОЙСТВА КЛАССА

        public string NameShort {
            get { return _NameShort; }
            set { _NameShort = value; }
        }

        public string Comment {
            get { return _Comment; }
            set { _Comment = value; }
        }

        #endregion

        #region МЕТОДЫ

        #endregion
    }

}