using EticaretWebCoreEntity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class GirisYapViewModel
    {
        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public RolTipleri RolTipi { get; set; }

    }
}
