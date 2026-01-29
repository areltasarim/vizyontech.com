using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public  class UserRoleName : TagHelper
    {
        public UserManager<AppUser> UserManager { get; set; }

        public UserRoleName(UserManager<AppUser> userManager)
        {
            this.UserManager = userManager;
        }
        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser user = await UserManager.FindByIdAsync(UserId);

            IList<string> roller =  await UserManager.GetRolesAsync(user);

            string html = string.Empty;

            roller.ToList().ForEach(x =>
            {
                html += $"<span class='badge badge-soft-info'> {x} </span></br>";
            });

            output.Content.SetHtmlContent(html);
        }
    }
}
