using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class OneCikanUrunViewModel : OneCikanUrunler
    {
        public OneCikanUrunler OneCikanUrun { get; set; } = new OneCikanUrunler();
        public ModulTipleri ModulTipi { get; set; }
        public IFormFile SayfaResmi { get; set; }
        public string ModulAdi { get; set; }
        public int Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }

        public string UrunAutocomplete { get; set; }
        public int[] SeciliUrunlerAutocomplete { get; set; }

        public string KategoriAutocomplete { get; set; }
        public int[] SeciliKategorilerAutocomplete { get; set; }
    }
}
