using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;

namespace IntecoAG.XafExt.DC.Module.Samples {

    [DomainComponent]
    public interface IWorker { }

    [DomainComponent]
    public interface IManager : IWorker { }

    [DomainComponent]
    public interface IEvangelist : IWorker { }

}
