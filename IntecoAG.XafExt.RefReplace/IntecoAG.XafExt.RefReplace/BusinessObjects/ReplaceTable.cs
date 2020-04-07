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
using DevExpress.ExpressApp.ConditionalAppearance;

namespace IntecoAG.XafExt.RefReplace.BusinessObjects {

    [Persistent("XafExtRefReplaceReplaceTable")]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    [NavigationItem("Settings")]
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
        [Association]
        public virtual XPCollection<SimpleAttribute> Attributes{
            get { return GetCollection<SimpleAttribute>(nameof(Attributes)); }

        }

        private String _nameTable;
        public String NameTable {
            get { return _nameTable; }
            set { SetPropertyValue(nameof(NameTable), ref _nameTable, value); }
        }
        private String _keyProp;
        public String KeyPropCurrentType {
            get { return _keyProp; }
            set { SetPropertyValue(nameof(KeyPropCurrentType), ref _keyProp, value); }
        }

        private Type _CurrentType;
        [ValueConverter(typeof(ConverterType2FullNameString))]
        [Size(255)]
        public Type CurrentType {
            get { return _CurrentType; }
            set { SetPropertyValue(nameof(CurrentType), ref _CurrentType, value); }
        }

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

        private Boolean _supportEtalon;
        public Boolean SupportEtalon {
            get { return _supportEtalon; }
            set { SetPropertyValue(nameof(SupportEtalon), ref _supportEtalon, value); }
        }

        private Boolean _replace;
        [Appearance("", Enabled = false, Criteria = "!IsPassed Or ToDelete")]
        [Appearance("", Enabled = true, Criteria = "DeferredDel And !SupportRef Or SupportRef And IsCanDeleted")]


        [Appearance("", Enabled = true, Criteria = "SupportEtalon")]
        [Appearance("", Enabled = false, Criteria = "!SupportEtalon")]

        public Boolean Replace {
            get { return _replace; }
            set { SetPropertyValue(nameof(Replace), ref _replace, value); }
        }

        private Boolean _delete;
       
        [Appearance("",Enabled =false, Criteria ="!IsPassed")]
        [Appearance("", Enabled = true, Criteria = "DeferredDel And !SupportRef Or SupportRef And IsCanDeleted")]
        //[Appearance("", Enabled = false, Criteria = "!(DeferredDel And !SupportRef Or SupportRef And IsCanDeleted)")]
        [Appearance("", Enabled = false, Criteria = "ToClose")]
        [Appearance("", Enabled = true, Criteria = "IsCanDeleted")]
        public Boolean ToDelete {
            get { return _delete; }
            set {
                SetPropertyValue(nameof(ToDelete), ref _delete, value);
                if (value) {
                    ToClose = false;
                    Replace = false;
                }
            }
        }
        private Boolean _defDel;
        public Boolean DeferredDel {
            get { return _defDel; }
            set { SetPropertyValue(nameof(DeferredDel), ref _defDel, value); }
        }
        private Boolean _supporRef;
        public Boolean SupportRef {
            get { return _supporRef; }
            set { SetPropertyValue(nameof(SupportRef), ref _supporRef, value); }
        }

        private Boolean _isCanDel;
        public Boolean IsCanDeleted {
            get { return _isCanDel; }
            set { SetPropertyValue(nameof(IsCanDeleted), ref _isCanDel, value); }
        }

        private Boolean _isCanClose;
        public Boolean IsCanClose {
            get { return _isCanClose; }
            set { SetPropertyValue(nameof(IsCanClose), ref _isCanClose, value); }
        }

        private Boolean _isPass;
        public Boolean IsPassed {
            get { return _isPass; }
            set { SetPropertyValue(nameof(IsPassed), ref _isPass, value); }
        }

        private Boolean _close;
        [Appearance("", Enabled = false, Criteria = "!IsPassed Or !IsCanClose")]
        [Appearance("", Enabled = false, Criteria = "ToDelete")]
        [Appearance("", Enabled = true, Criteria = "DeferredDel And !SupportRef Or SupportRef and IsCanDeleted And IsCanClose")]
        [Appearance("", Enabled = true, Criteria = "SupportRef And IsCanClose")]
        public Boolean ToClose {
            get { return _close; }
            set { SetPropertyValue(nameof(ToClose), ref _close, value); }
        }

    }
   
}