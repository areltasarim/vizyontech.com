using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EticaretWebCoreHelper
{
    [HtmlTargetElement("input", Attributes = "asp-is-readonly")]
    public class ReadonlyInputTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-is-readonly")]
        public bool IsReadonly { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (IsReadonly)
            {
                output.Attributes.SetAttribute("readonly", "readonly");
            }
            base.Process(context, output);
        }
    }
}


