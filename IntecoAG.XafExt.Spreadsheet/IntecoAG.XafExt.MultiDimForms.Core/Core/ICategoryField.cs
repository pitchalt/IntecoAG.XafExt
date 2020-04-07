using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {
    public interface ICategoryField: IElement {
        IDimension Dimension { get; }
    }
}
