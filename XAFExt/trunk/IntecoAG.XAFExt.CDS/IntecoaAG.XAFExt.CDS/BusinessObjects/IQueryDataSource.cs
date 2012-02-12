using System.Linq;
using System.Collections.Generic;

namespace IntecoAG.XAFExt.CDS
{
    public interface IQueryDataSource {
        IQueryable GetQuery();
    }
}