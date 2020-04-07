using System;
using System.IO;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;

using IntecoAG.XafExt.Documents.Store;

namespace IntecoAG.XafExt.Spreadsheet {

    [Persistent("WorkBookStore")]
    [FileAttachment("File")]
    public class WorkbookStore : BaseObject, IWorkbookStore {

        public WorkbookStore() : base() {
        }

        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public FileSystemStoreObject File {
            get { return GetPropertyValue<FileSystemStoreObject>("File"); }
            set { SetPropertyValue<FileSystemStoreObject>("File", value); }
        }

        public WorkbookStore(Session session) : base(session) {
            
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public virtual void ImportPrepare(IWorkbook book) {

        }

        public virtual void Import(IWorkbook book) {

        }

        public virtual void Export(IWorkbook book) {

        }

        public virtual void Load(IWorkbook book) {
            Stream stream = new MemoryStream();
            IFileData source = File;
            if (source != null) {
                source.SaveToStream(stream);
                stream.Position = 0;
                book.LoadDocument(stream, DocumentFormat.Xlsx);
            }
        }

        public virtual void Save(IWorkbook book) {
            Stream stream = new MemoryStream();
            IFileData target = File;
            if (target != null) {
                book.SaveDocument(stream, DocumentFormat.Xlsx);
                stream.Position = 0;
                target.LoadFromStream(".xlsx", stream);
            }
        }

        public void OnBookCellChanged(CellChangedEventArgs args) {
//            throw new NotImplementedException();
        }

        public object CellDataGet(int sheet_id, int col_id, int row_id) {
            throw new NotImplementedException();
        }

        public event EventHandler<CellChangedEventArgs> CellChanged;

        //IWorkbookValue _WorkbookValue;
        //
        //public IWorkbookValue WorkbookValue {
        //    get {
        //        if (_WorkbookValue == null)
        //            _WorkbookValue = new WorkbookValue(this);
        //        return _WorkbookValue;
        //    }
        //    set {
        //        if (!Object.ReferenceEquals(value, _WorkbookValue) )
        //            Save(value.Workbook);
        //    }
        //}

    }

}