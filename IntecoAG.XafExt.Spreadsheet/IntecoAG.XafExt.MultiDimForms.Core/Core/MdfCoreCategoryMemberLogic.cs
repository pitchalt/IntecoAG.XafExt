using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntecoAG.XafExt.Spreadsheet.MultiDimForms.Core {

    public static class MdfCoreCategoryMemberLogic {

        public static String CategoryMemberKeyGet(this IReadOnlyDictionary<MdfCoreDimension, MdfCoreDimensionMember> dim_members) {
            return $@"<{ String.Join(",", dim_members.OrderBy(x => x.Key.Guid)
                        .Select(x => $@"{x.Key.CodeOrGuid}={x.Value?.DomainMember.CodeOrGuid}"))}>";
        }

        public static String CategoryMemberKeyGet(this MdfCoreCategoryMember cat_member) {
            var dict = new Dictionary<MdfCoreDimension, MdfCoreDimensionMember>();
            foreach (var field in cat_member.CategoryMemberFields) {
                dict[field.CategoryTypeField.Dimension] = field.DimensionMember;
            }
            return dict.CategoryMemberKeyGet();
        }

    }

}
