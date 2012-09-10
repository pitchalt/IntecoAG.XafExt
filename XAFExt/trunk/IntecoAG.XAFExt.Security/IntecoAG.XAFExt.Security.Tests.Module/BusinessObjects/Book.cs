using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;

namespace IntecoAG.XAFExt.Security.Tests.Module.BusinessObjects {

    public enum BookState {
        Prochitana = 1,
        Porvana = 2,
        Sohzena = 3,
        Obgazhena = 4,
        Reabilitirovana = 5
    }

    [DefaultClassOptionsAttribute]
    public class Book : BaseObject {
        public Book(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private Int32 _Number;
        private DateTime _Date;
        private String _Name;
        private String _Description;
        private BookState _Status;

        public Int32 Number {
            get {
                return _Number;
            }
            set {
                SetPropertyValue<Int32>("Number", ref _Number, value);
            }
        }

        public DateTime Date {
            get {
                return _Date;
            }
            set {
                SetPropertyValue<DateTime>("Date", ref _Date, value);
            }
        }

        [Size(300)]
        public String Name {
            get {
                return _Name;
            }
            set {
                SetPropertyValue<String>("Name", ref _Name, value);
            }
        }

        [Size(10000)]
        public String Description {
            get {
                return _Description;
            }
            set {
                SetPropertyValue<String>("Description", ref _Description, value);
            }
        }

        public BookState Status {
            get {
                return _Status;
            }
            set {
                SetPropertyValue<BookState>("Status", ref _Status, value);
            }
        }

    }

}