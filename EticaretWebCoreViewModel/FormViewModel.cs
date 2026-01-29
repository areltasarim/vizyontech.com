using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class FormViewModel : Formlar
    {
        public Formlar Form { get; set; }
        public FormDegerleri FormDeger { get; set; }


        public string[] FormAdiCeviri { get; set; }
        public string[] PlaceHolderCeviri { get; set; }
        public string[] HataMesajiCeviri { get; set; }


        //Form Degerleri
        public int[] DilIds { get; set; }
        public int[] FormIds { get; set; }
        public int[] FormDegerListesi { get; set; }
        public int[] FormDegerGuncelleListesi { get; set; }
        public string[] DegerAdiCeviri { get; set; }

        public string[] SayfaDosyaSecimListesi { get; set; }


    }
}
