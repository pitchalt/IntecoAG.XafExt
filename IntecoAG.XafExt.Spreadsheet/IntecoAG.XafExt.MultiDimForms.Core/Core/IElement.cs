using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public interface IElement {

        Guid Guid { get; }
        String Code { get; }
        String CodeOrGuid { get; }
        String NameShort { get; }

    }

}
