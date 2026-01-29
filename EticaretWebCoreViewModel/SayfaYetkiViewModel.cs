using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class SayfaYetkiViewModel
    {
       public string RoleId { get; set; }
        public string UserId { get; set; }
        public IList<RoleClaimsViewModel> RoleClaims { get; set; }
        public IList<UserClaimsViewModel> UserClaims { get; set; }
    }

    public class RoleClaimsViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }

    public class UserClaimsViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
