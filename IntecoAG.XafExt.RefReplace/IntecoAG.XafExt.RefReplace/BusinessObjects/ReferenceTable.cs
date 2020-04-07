using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.XafExt.RefReplace.BusinessObjects {
    [Persistent("XafExtRefReplaceReferenceTable")]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[NonPersistent]
    public class ReferenceTable : BaseObject {
    
     
        public ReferenceTable(Session session)
           : base(session) {
        }

        // Add this property as the key member in the CustomizeTypesInfo event

        private DateTime _dateCreate;
        public DateTime DateCreate {
            get { return _dateCreate; }
            set { SetPropertyValue(nameof(DateCreate), ref _dateCreate, value); }
        }

        private DateTime _dateApply;
        public DateTime DateApply {
            get { return _dateApply; }
            set { SetPropertyValue(nameof(DateApply), ref _dateApply, value); }
        }
        private DateTime _dateRej;
        public DateTime DateRejected {
            get { return _dateRej; }
            set { SetPropertyValue(nameof(DateRejected), ref _dateRej, value); }
        }
        private DateTime _datePass;
        public DateTime DatePassed {
            get { return _datePass; }
            set { SetPropertyValue(nameof(DatePassed), ref _datePass, value); }
        }
        private Status _status;
    
        public Status Status {
            get { return _status; }
            set {
         
                SetPropertyValue(nameof(Status), ref _status, value); }
        }

        IList<ReferenceItem> _items = new List<ReferenceItem>();
        [Association("Table-Items")]
        public virtual XPCollection<ReferenceItem> Items {
            get { return GetCollection<ReferenceItem>(nameof(Items)); }
          
        }



        IList<ObjItem> _objs = new List<ObjItem>();
    
        [Association("Table-Objects")]
        public XPCollection<ObjItem> Objects {
            get { return GetCollection<ObjItem>(nameof(Objects)); }
        }

       

    }
   
    public enum Status {
        CREATED = 0,
        PASSED = 1,
        APPLIED = 2,
        CANCELED = 3,
        REJECTED = 4

    }
}