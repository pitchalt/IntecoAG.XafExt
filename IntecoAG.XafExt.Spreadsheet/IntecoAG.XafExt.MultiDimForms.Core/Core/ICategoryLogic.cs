using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public static class ICategoryLogic {

        public static String CategoryKeyGet(this IEnumerable<IDimension> dims) {
            return $@"<{String.Join(",", dims.OrderBy(x => x.Guid).Select(x => x.Code))}>";
        }

//        public static ICategory CategoryGet(this IContainer container, IEnumerable<IDimension> dims) {
//           String key = CategoryKeyGet(dims);
//            if (container.Categorys.ContainsKey(key))
//                return container.Categorys[key];
//            ICategory category = container.CategoryCreate(dims);
//
//        }

        public static String CategoryMemberKeyGet(this IReadOnlyDictionary<IDimension, IDomainMember> cat_member) {

            return $@"<{ String.Join(",", cat_member.Keys.OrderBy(x => x.Guid)
                        .Select(x => $@"{x.Code}={cat_member[x].Code}"))}>";

        }

        //public static ICategoryMember CategoryMemberGet(this ICategory category, IReadOnlyDictionary<IDimension, IDomainMember> cat_member, IDimension dim, IDomainMember dom_member) {

        //}

    }
}
