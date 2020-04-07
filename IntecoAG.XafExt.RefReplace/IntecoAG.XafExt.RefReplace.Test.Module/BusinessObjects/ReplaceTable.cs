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
using IntecoAG.XafExt.RefReplace.Module.Win.Logic;
using DevExpress.Xpo;

namespace IntecoAG.XafExt.RefReplace.Test.Module.BusinessObjects {
 
    [DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReplaceTable : ReferenceTable {

    
        public ReplaceTable(Session session)
           : base(session) {
          
        }

        // Add this property as the key member in the CustomizeTypesInfo event
        private String _oldId;
        public String OldId {
            get { return _oldId; }
            set { SetPropertyValue(nameof(OldId), ref _oldId, value); }
            
        }


        public Type CurrentType { get; set; }

        private String _oldName;
        public String OldName {
            get { return _oldName; }
            set { SetPropertyValue(nameof(OldName), ref _oldName, value); }
        }
        private String _newName;
        public String NewName {
            get { return _newName; }
            set { SetPropertyValue(nameof(NewName), ref _newName, value); }
        }
        private String _newId;
        public String NewId {
            get { return _newId; }
            set { SetPropertyValue(nameof(NewId), ref _newId, value); }

        }


        
    }
   
}