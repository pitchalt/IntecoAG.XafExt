using System;
using System.Linq;
using System.Collections.Generic;

namespace IntecoAG.XafExt.CDS
{
    public interface IQueryDataSource {

        Type ElementType { get; }
        Type SourceType { get; }

        IQueryable GetQuery();
    }
}