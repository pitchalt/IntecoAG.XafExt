using System;
using DevExpress.Xpo;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms {

    public class MdfForm : XPObject {
        public MdfForm() : base() {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public MdfForm(Session session) : base(session) {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}