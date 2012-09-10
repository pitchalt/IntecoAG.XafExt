using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;

namespace IntecoAG.XAFExt.Security.Tests.Module.BusinessObjects {

    [DefaultClassOptionsAttribute]
    //[MapInheritance(MapInheritanceType.ParentTable)]
    public class FictionBook : Book {
        public FictionBook(Session session)
            : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
        }

        private String _Theme;   // Тема
        private String _Subject;   // Предмет


        [Size(300)]
        public String Theme {
            get {
                return _Theme;
            }
            set {
                SetPropertyValue<String>("Theme", ref _Theme, value);
            }
        }

        [Size(300)]
        public String Subject {
            get {
                return _Subject;
            }
            set {
                SetPropertyValue<String>("Subject", ref _Subject, value);
            }
        }
    }

}