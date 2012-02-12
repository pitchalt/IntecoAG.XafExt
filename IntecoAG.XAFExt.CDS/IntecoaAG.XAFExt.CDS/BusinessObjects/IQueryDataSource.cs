using System.Linq;
using System.Collections.Generic;

namespace IntecoaAG.XAFExt.CDS
{
    public interface IQueryDataSource {
        IQueryable GetQuery();
    }
}