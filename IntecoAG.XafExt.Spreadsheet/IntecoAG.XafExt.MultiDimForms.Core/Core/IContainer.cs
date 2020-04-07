using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public interface IContainer : IElement {
        IEnumerable<IDomain> Domains { get; }
        IDomain DomainCreate();
        IDomain DomainCreate(String code);

        IEnumerable<IDimension> Dimensions { get; }

        IReadOnlyDictionary<String, ICategory> Categorys { get; }
        ICategory CategoryCreate();
        ICategory CategoryGet(IEnumerable<IDimension> dims);

        IEnumerable<IDataPoint> DataPoints { get; }
    }

}
