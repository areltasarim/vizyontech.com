using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EticaretWebCoreViewModel
{
   public class RoleViewModel
    {
        [Required(ErrorMessage = "Rol Adi Boş Bırakılamaz."), MaxLength(250, ErrorMessage = "Rol Adi 250 Karakterden Fazla Olmamaz.")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}
