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
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.XafExt.RefReplace.BusinessObjects {

    [Persistent("XafExtRefReplaceReferenceItem")]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReferenceItem : BaseObject{
        
        public ReferenceItem(Session session)
               : base(session) {
        }

        private ReferenceTable _Table;
        [Association("Table-Items")]
        public ReferenceTable Table {
            get { return _Table; }
            set { SetPropertyValue(nameof(Table), ref _Table, value); }
        }
        //private String _nameField;


        private String _NameModule;
        public String NameModule {
            get { return _NameModule; }
            set { SetPropertyValue(nameof(NameModule), ref _NameModule, value); }
        }

        private Type _Type;
        public Type Type {
            get { return _Type; }
            set { SetPropertyValue(nameof(Type), ref _Type, value); }
        }

        private String _NameTable;
        public String NameTable {
            get { return _NameTable; }
            set { SetPropertyValue(nameof(NameTable), ref _NameTable, value); }

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

        //private Boolean _isProp;
        //public Boolean IsProperty {
        //    get { return _isProp; }
        //    set { SetPropertyValue(nameof(IsProperty), ref _isProp, value); }
        //}


        //public String NameField {
        //    get { return _nameField; }
        //    set { SetPropertyValue(nameof(NameField), ref _nameField, value); }
        //}
        private String _NameType;
        public String NameType {
            get { return _NameType; }
            set { SetPropertyValue(nameof(NameType), ref _NameType, value); }
        }

        private String _NameProp;
        public String NameProp {
            get { return _NameProp; }
            set { SetPropertyValue(nameof(NameProp), ref _NameProp, value); }
        }

        private String _NamePropOnDB;
        public String NamePropOnDB {
            get { return _NamePropOnDB; }
            set { SetPropertyValue(nameof(NamePropOnDB), ref _NamePropOnDB, value); }
        }
       

    }
}