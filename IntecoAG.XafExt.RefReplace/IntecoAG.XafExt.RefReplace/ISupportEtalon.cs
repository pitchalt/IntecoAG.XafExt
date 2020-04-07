using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.RefReplace {

    public interface ISupportEtalon<T> {
        T Etalon { get; set; }
    }

    public interface ISupportEtalon {
        Object Etalon { get; set; }
    }

}
