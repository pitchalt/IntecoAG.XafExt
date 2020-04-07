using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

using IntecoAG.XafExt.Documents.Store;

namespace IntecoAG.XafExt.Spreadsheet.Test {
    
    [Persistent("IagMdfTestForm")]
    [NavigationItem("Tests")]
    [FileAttachment("File")]
    public class TestForm : BaseObject, IXafEntityObject, IObjectSpaceLink {
        public TestForm() : base() {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public TestForm(Session session) : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        private WorkbookStore _WorkBookStore;
        [Aggregated]
        public WorkbookStore WorkBookStore {
            get { return _WorkBookStore; }
            set { SetPropertyValue("WorkBookStore", ref _WorkBookStore, value); }
        }

        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        //[PersistentAlias("WorkBookImpl.File")]
        //public FileSystemStoreObject File {
        //    get { return WorkBookImpl?.File;  }
        //    set {
        //        if (WorkBookImpl != null) {
        //            WorkBookImpl.File = value;
        //        }
        //    }
        //}
        ////[ModelDefault("ShowCaption", "False")]
        //public IWorkbookValue WorkBook {
        //    get { return WorkBookImpl?.WorkbookValue; }
        //    set {
        //        if (WorkBookImpl != null) {
        //            WorkBookImpl.WorkbookValue = value;
        //        }
        //    }
        //}

        [Browsable(false)]
        public IObjectSpace ObjectSpace { get ; set; }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public void OnCreated() {
            WorkBookStore = ObjectSpace.CreateObject<WorkbookStore>();
        }

        void IXafEntityObject.OnSaving() {
            //throw new NotImplementedException();
        }

        void IXafEntityObject.OnLoaded() {
            //throw new NotImplementedException();
        }
    }

}