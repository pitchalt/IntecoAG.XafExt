using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;

namespace IntecoAG.XafExt.RefReplace {

   public interface ISupportRefReplace {
        Boolean IsCanClose { get; }
        void Close();
        Boolean IsCanDeleted { get; }

    }

}
