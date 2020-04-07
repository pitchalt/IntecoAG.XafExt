using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.XafExt.RefReplace.BusinessObjects {

    [Persistent("XafExtRefReplaceObjItem")]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ObjItem : BaseObject {
    
        public ObjItem(Session session)
             : base(session) {
        }
        //private Boolean _isProp;
        //public Boolean IsProperty {
        //    get { return _isProp; }
        //    set { SetPropertyValue(nameof(IsProperty), ref _isProp, value); }
        //}
        // Add this property as the key member in the CustomizeTypesInfo event

       
        private String _namePropDB;
        public String NamePropOnDB {
            get { return _namePropDB; }
            set { SetPropertyValue(nameof(NamePropOnDB), ref _namePropDB, value); }
        }
        private String _namePropLocal;
        public String NamePropLocal {
            get { return _namePropLocal; }
            set { SetPropertyValue(nameof(NamePropLocal), ref _namePropLocal, value); }
        }



        private String _nameTableDB;
        public String NameTableOnDB {
            get { return _nameTableDB; }
            set { SetPropertyValue(nameof(NameTableOnDB), ref _nameTableDB, value);}

        }
        private String _nameTableLocal;
        public String NameTableLocal {
            get { return _nameTableLocal; }
            set { SetPropertyValue(nameof(NameTableLocal), ref _nameTableLocal, value); }

        }
        private String _nameType;
        public String NameType {
            get { return _nameType; }
            set { SetPropertyValue(nameof(NameType), ref _nameType, value); }
        }
        private String _nameProp;
        public String NameProp {
            get { return _nameProp; }
            set { SetPropertyValue(nameof(NameProp), ref _nameProp, value); }
        }
        private String _id;
        public String ID {
            get { return _id; }
            set { SetPropertyValue(nameof(ID), ref _id, value); }
        }

        private Boolean _IsForbidden;
        public Boolean IsForbidden {
            get { return _IsForbidden; }
            set { SetPropertyValue(nameof(IsForbidden), ref _IsForbidden, value); }
        }

        private Boolean _IsAggregated;
        public Boolean IsAggregated {
            get { return _IsAggregated; }
            set { SetPropertyValue(nameof(IsAggregated), ref _IsAggregated, value); }
        }

        private ReferenceTable _table;
        [Association("Table-Objects")]
        public ReferenceTable Table {
            get { return _table; }
            set { SetPropertyValue(nameof(Table), ref _table, value); }
        }
       



        #region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}