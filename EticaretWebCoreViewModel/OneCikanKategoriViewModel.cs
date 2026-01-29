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
    public class OneCikanKategoriViewModel : OneCikanKategoriler
    {
        public OneCikanKategoriler OneCikanKategori { get; set; } = new OneCikanKategoriler();
        public ModulTipleri ModulTipi { get; set; }
        public string ModulAdi { get; set; }
        public int Sira { get; set; }
        public SayfaDurumlari Durum { get; set; }
        public string KategoriAutocomplete { get; set; }
        public int[] SeciliKategorilerAutocomplete { get; set; }
    }
}
