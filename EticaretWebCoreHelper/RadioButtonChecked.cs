using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EticaretWebCoreHelper
{
    [HtmlTargetElement("input", Attributes = "asp-is-checked")]
    public class RadioButtonCheckedTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-is-checked")]
        public bool IsChecked { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsChecked)
            {
                output.Attributes.Add("checked", "checked");
            }
        }
    }
}


