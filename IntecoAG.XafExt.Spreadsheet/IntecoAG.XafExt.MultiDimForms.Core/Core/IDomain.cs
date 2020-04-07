using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public interface IDomain : IContainerized  {

        IEnumerable<IDomainMember> Members { get; }
        IDomainMember MemberCreate();
        IDomainMember MemberCreate(String code);

        IEnumerable<IDimension> Dimensions { get; }
        IDimension DimensionCreate();
        IDimension DimensionCreate(String code);
    }

}
