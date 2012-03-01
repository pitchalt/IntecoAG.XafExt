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
    [Persistent("testCountry")]
    public partial class testCountry : BaseObject
    {
        public testCountry(Session ses) : base(ses) { }
        
        public override void AfterConstruction() {
            NameShort = String.Empty;
            NameFull = String.Empty;
        }

        #region ПОЛЯ КЛАССА

        private string _NameShort;
        private string _NameFull;
        private string _Comment;

        #endregion

        #region СВОЙСТВА КЛАССА

        public string NameShort {
            get { return _NameShort; }
            set { SetPropertyValue("NameShort", ref _NameShort, value); }
        }

        public string NameFull {
            get { return _NameFull; }
            set { SetPropertyValue("NameFull", ref _NameFull, value); }
        }

        public string Comment {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }
        #endregion

        #region МЕТОДЫ

        #endregion
    }

}