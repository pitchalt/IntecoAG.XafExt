using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public static class MdfCoreCategoryLogic {

        public static String CategoryKeyGet(this IEnumerable<MdfCoreDimension> dims) {
            return $@"<{String.Join(",", dims.OrderBy(x => x.Guid).Select(x => x.CodeOrGuid))}>";
        }

        public static String CategoryKeyGet(this MdfCoreCategory category) {
            return category.CategoryFields.Select(x => x.Dimension).CategoryKeyGet();
        }

    }
}