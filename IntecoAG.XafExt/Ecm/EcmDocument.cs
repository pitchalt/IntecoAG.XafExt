using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace IntecoAG.XafExt.Ecm
{
    [Persistent(nameof(EcmDocument))]
    //[DefaultProperty(nameof(Name))]
    [DefaultProperty("FileName")]
    public abstract class EcmDocument : XPObject, IFileData
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        protected EcmDocument(Session session)
            : base(session)
        {
        }
        protected EcmDocument(Session session, String name) : base(session)
        {
            Name = name;
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _FileId = Guid.NewGuid();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string tempFileName = string.Empty;
        private static object syncRoot = new object();
        public static int ReadBytesSize = 0x1000;
        public static string FileSystemStoreLocation = String.Format(@"{0}\FileStore\Templates", PathHelper.GetApplicationFolder());

        public virtual void LoadFromStream(string fileName, Stream stream)
        {
            
        }

        public virtual void SaveToStream(Stream stream)
        {
            
        }

        public virtual  void Clear()
        {
            FileName = string.Empty;
            Size = 0;
        }

        private String _Name;
        public String Name {
            get { return _Name; }
            set { SetPropertyValue(nameof(Name), ref _Name, value); }
        }

        [Persistent(nameof(_FileId))]
        private Guid _FileId;
        [PersistentAlias(nameof(_FileId))]
        public Guid FileId {
            get { return _FileId; }
        }

        private String _fileName;
        [Size(260)]
        public string FileName
        {
            get { return _fileName; }
            set { SetPropertyValue(nameof(FileName), ref _fileName, value); }
        }

        private Int32 _size;
        [Persistent]
        public int Size
        {
            get { return _size; }
           set { SetPropertyValue(nameof(Size), ref _size, value); }
        }

     

        //private IFileData _FileData;
        //public IFileData FileData {
        //    get { return _FileData; }
        //    set { SetPropertyValue(nameof(FileData), ref _FileData, value); }
        //}
    }
}
