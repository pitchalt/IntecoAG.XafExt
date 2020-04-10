using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Validation;
using IntecoAG.XafExt.Ecm;

namespace IntecoAG.XafExt.Tests.Module.BusinessObjects
{
    [Persistent]
    [MapInheritance(MapInheritanceType.ParentTable)]
    //[DefaultProperty("FileName")]
    public class TestDocument : EcmDocument, IEmptyCheckable
    {
        private Stream tempSourceStream;
        private string tempFileName = string.Empty;
        private static object syncRoot = new object();

        public static int ReadBytesSize = 0x1000;
        public static string FileSystemStoreLocation = String.Format(@"{0}\FileStore\Templates", PathHelper.GetApplicationFolder());

        public static void CopyFileToStream(string sourceFileName, Stream destination)
        {
            if (string.IsNullOrEmpty(sourceFileName) || destination == null) return;
            using (Stream source = File.OpenRead(sourceFileName))
                CopyStream(source, destination);
        }
        public static void OpenFileWithDefaultProgram(string sourceFileName)
        {
            Guard.ArgumentNotNullOrEmpty(sourceFileName, "sourceFileName");
            System.Diagnostics.Process.Start(sourceFileName);
        }
        public static void CopyStream(Stream source, Stream destination)
        {
            if (source == null || destination == null) return;
            byte[] buffer = new byte[ReadBytesSize];
            int read = 0;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                destination.Write(buffer, 0, read);
        }

        public TestDocument(Session session) : base(session) { }
        public string RealFileName {
            get
            {
                if (!string.IsNullOrEmpty(FileName) /*&& Oid != Guid.Empty*/)
                    return Path.Combine(FileSystemStoreLocation, this.FileId + "." + FileExt);
                return null;
            }
        }
        protected virtual void SaveFileToStore()
        {
            if (!string.IsNullOrEmpty(RealFileName) && TempSourceStream != null)
            {
                try
                {
                    using (Stream destination = File.Create(RealFileName))
                    { //T582918
                        CopyStream(TempSourceStream, destination);
                        Size = (int)destination.Length;
                    }
                }
                catch (DirectoryNotFoundException exc)
                {
                    throw new UserFriendlyException(exc);
                }
            }
        }
        private void RemoveOldFileFromStore()
        {
            //Dennis: We need to remove the old file from the store when saving the current object.
            if (!string.IsNullOrEmpty(tempFileName) && tempFileName != RealFileName)
            {//B222892
                try
                {
                    File.Delete(tempFileName);
                    tempFileName = string.Empty;
                }
                catch (DirectoryNotFoundException exc)
                {
                    throw new UserFriendlyException(exc);
                }
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            FileExt = FileName.Split('.').Last();
            Guard.ArgumentNotNullOrEmpty(FileSystemStoreLocation, "FileSystemStoreLocation");
            lock (syncRoot)
            {
                if (!Directory.Exists(FileSystemStoreLocation))
                    Directory.CreateDirectory(FileSystemStoreLocation);
            }
            SaveFileToStore();
            RemoveOldFileFromStore();
        }
        protected override void OnDeleting()
        {
            //Dennis: We need to remove the old file from the store.
            Clear();
            base.OnDeleting();
        }
        protected override void Invalidate(bool disposing)
        {
            if (disposing && TempSourceStream != null)
            {
                TempSourceStream.Close();
                TempSourceStream = null;
            }
            base.Invalidate(disposing);
        }
        #region IFileData Members
        public void Clear()
        {
            //Dennis: When clearing the file name property we need to save the name of the old file to remove it from the store in the future. You can also setup a separate service for that.
            if (string.IsNullOrEmpty(tempFileName))
                tempFileName = RealFileName;
            FileName = string.Empty;
            Size = 0;
        }
        //[Size(260)]
        //public string FileName {
        //    get { return GetPropertyValue<string>("FileName"); }
        //    set { SetPropertyValue("FileName", value); }
        //}
        [Size(16)]
        public string FileExt {
            get { return GetPropertyValue<string>("FileName"); }
            set { SetPropertyValue("FileName", value); }
        }
        [Browsable(false)]
        public Stream TempSourceStream {
            get { return tempSourceStream; }
            set {
                //Michael: The original Stream might be closed after a while (on the web too - T160753)
                if (value == null)
                {
                    tempSourceStream = null;
                }
                else
                {
                    if (value.Length > (long)int.MaxValue) throw new UserFriendlyException("File is too long");
                    tempSourceStream = new MemoryStream((int)value.Length);
                    CopyStream(value, tempSourceStream);
                    tempSourceStream.Position = 0;
                }
            }
        }
        //Dennis: Fires when uploading a file.
        public override void LoadFromStream(string fileName, Stream source)
        {
            //Dennis: When assigning a new file we need to save the name of the old file to remove it from the store in the future.
            if (fileName != FileName)
            {// updated, old code was: if (string.IsNullOrEmpty(tempFileName))
                tempFileName = RealFileName;
            }
            FileName = fileName;
            TempSourceStream = source;
            Size = (int)TempSourceStream.Length;
            OnChanged();//T582918
        }
        //Dennis: Fires when saving or opening a file.
        public  override void SaveToStream(Stream destination)
        {
            try
            {
                if (!string.IsNullOrEmpty(RealFileName))
                {
                    if (destination == null)
                        OpenFileWithDefaultProgram(RealFileName);
                    else
                        CopyFileToStream(RealFileName, destination);
                }
                else if (TempSourceStream != null)
                    CopyStream(TempSourceStream, destination);
            }
            catch (DirectoryNotFoundException exc)
            {
                throw new UserFriendlyException(exc);
            }
            catch (FileNotFoundException exc)
            {
                throw new UserFriendlyException(exc);
            }
        }
        //[Persistent]
        //public int Size {
        //    get { return GetPropertyValue<int>("Size"); }
        //    private set { SetPropertyValue<int>("Size", value); }
        //}
        #endregion
        #region IEmptyCheckable Members
        public bool IsEmpty {
            //T153149
            get { return FileDataHelper.IsFileDataEmpty(this) || !(this.TempSourceStream != null || File.Exists(RealFileName)); }
        }
        #endregion
    }

}
