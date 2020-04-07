using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DC = DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using IntecoAG.XpoExt;
using System.Collections;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    [ModelDefault("AllowNew", "False")]
    [Persistent("FmMdfCoreDomainContainer")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class MdfCoreDomainContainer : MdfCoreDomain {

        [Association]
        public XPCollection<MdfCoreDomain> ContainedDomains {
            get {
                return GetCollection<MdfCoreDomain>();
            }
        }

        public MdfCoreDomainContainer(Session session): base(session) { }

    }

}