using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class UyeSifreGuncelleViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string YeniSifre { get; set; }
    }
}
