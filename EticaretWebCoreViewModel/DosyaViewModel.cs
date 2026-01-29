using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class DosyaViewModel : Dosyalar
    {
        public int SayfaId { get; set; }

        public string[] DosyaAdiCeviri { get; set; }

        public List<IFormFile> SayfaDosyasi { get; set; }

        public string[] SayfaDosyaSecimListesi { get; set; }

    }
}
