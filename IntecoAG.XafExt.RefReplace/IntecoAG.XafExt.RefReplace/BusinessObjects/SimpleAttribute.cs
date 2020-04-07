using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace IntecoAG.XafExt.RefReplace.BusinessObjects {
    [Persistent("XafExtRefReplaceAttribute")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SimpleAttribute : BaseObject { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SimpleAttribute(Session session)
            : base(session) {
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private ReplaceTable _Table;
        [Association]
        public ReplaceTable Table {
            get { return _Table; }
            set { SetPropertyValue(nameof(Table), ref _Table, value); }
        }
        private String _LocalName;
        public String LocalName {
            get { return _LocalName; }
            set { SetPropertyValue(nameof(LocalName), ref _LocalName, value); }
        }
        private String _NameAtt;
        public String NameAtt {
            get { return _NameAtt; }
            set { SetPropertyValue(nameof(NameAtt), ref _NameAtt, value); }
        }
        private String _NameType;
        public String NameType {
            get { return _NameType; }
            set { SetPropertyValue(nameof(NameType), ref _NameType, value); }
        }
        private String _OldValue;
        public String OldValue {
            get { return _OldValue; }
            set {
                if (!IsLoading && value != null && value.Length > 100)
                    value = value.Substring(0, 100);
                SetPropertyValue(nameof(OldValue), ref _OldValue, value);
            }
        }
        private String _NewValue;
        public String NewValue {
            get { return _NewValue; }
            set {
                if (!IsLoading && value != null && value.Length > 100)
                    value = value.Substring(0, 100);
                SetPropertyValue(nameof(NewValue), ref _NewValue, value);
            }
        }
       
    }
}