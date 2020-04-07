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
using IntecoAG.XafExt.Documents.Store;

namespace IntecoAG.XafExt.Tests.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [DefaultProperty("File")]
    [Persistent]
    [FileAttachment("File")]
    public class PortfolioFile : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PortfolioFile(Session session) : base(session) { }
        private DocumentType documentType;
        protected ResumeCard resume;
        [Persistent, Association("Resume-PortfolioFileData")]
        public ResumeCard Resume {
            get { return resume; }
            set {
                SetPropertyValue(nameof(Resume), ref resume, value);
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            documentType = DocumentType.Unknown;
        }


        private TestDocument _storeFile;
        [RuleRequiredField("", "Save", "File should be assigned")]
    
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public TestDocument File
        {
            get { return _storeFile; }
            set { SetPropertyValue(nameof(File), ref _storeFile, value); }
        }

        public DocumentType DocumentType {
            get { return documentType; }
            set { SetPropertyValue(nameof(DocumentType), ref documentType, value); }
        }

     
    }
    public enum DocumentType
    {
        SourceCode = 1, Tests = 2, Documentation = 3,
        Diagrams = 4, ScreenShots = 5, Unknown = 6
    };

}