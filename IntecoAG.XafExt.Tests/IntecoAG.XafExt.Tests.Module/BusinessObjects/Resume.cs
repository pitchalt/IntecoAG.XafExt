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
using IntecoAG.XafExt.Tests.Module.BusinessObjects;
using IntecoAG.XafExt.Documents.Store;

namespace IntecoAG.XafExt.Tests.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [FileAttachment(nameof(StoreFile))]
    public class ResumeCard : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
     
        
        public ResumeCard(Session session) : base(session) { }
        //private Contact contact;
        //private FileData file;
        //[DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        //public FileData File {
        //    get { return file; }
        //    set {
        //        SetPropertyValue(nameof(File), ref file, value);
        //    }
        //}

        private TestDocument _storeFile;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public TestDocument StoreFile {
            get { return _storeFile; }
            set { SetPropertyValue(nameof(StoreFile), ref _storeFile, value); }
        }
        //public Contact Contact {
        //    get {
        //        return contact;
        //    }
        //    set {
        //        SetPropertyValue(nameof(Contact), ref contact, value);
        //    }
        //}
        [DevExpress.Xpo.Aggregated, Association("Resume-PortfolioFileData")]
        public XPCollection<PortfolioFile> Portfolio {
            get { return GetCollection<PortfolioFile>(nameof(Portfolio)); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}