using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class SifremiUnuttumViewModel
    {
        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz")]
        public string YeniSifre { get; set; }
    }
}
