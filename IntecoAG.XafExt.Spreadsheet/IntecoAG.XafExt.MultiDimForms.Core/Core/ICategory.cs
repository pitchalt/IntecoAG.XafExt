using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntecoAG.XafExt;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public interface ICategory: IElement, IEnumerable<IDimension> {

        String Key { get; }
        IEnumerable<IDimension> Dimensions { get;  }

        IReadOnlyDictionary<String, ICategoryMember> Members { get; }
        ICategoryMember MemberCreate();
        ICategoryMember MemberGet(IReadOnlyDictionary<IDimension, IDomainMember> category);
        ICategoryMember MemberGet(IReadOnlyDictionary<IDimension, IDomainMember> cat_member, IDimension dim, IDomainMember dim_member);
    }
}
