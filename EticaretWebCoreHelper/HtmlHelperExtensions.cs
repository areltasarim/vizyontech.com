using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumSelectListWithDefaultValue<TEnum>(this IHtmlHelper htmlHelper, TEnum defaultValue)
            where TEnum : struct, Enum
        {
            var selectList = htmlHelper.GetEnumSelectList<TEnum>().ToList();
            var enumValue = Enum.Parse(typeof(TEnum), defaultValue.ToString());
            selectList.Single(x => x.Value == ((int)enumValue).ToString()).Selected = true;
            return selectList;
        }

    }
}
