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
    public class testCountry4
    {
        #region ПОЛЯ КЛАССА

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

        #endregion
    }

}