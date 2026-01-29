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
    public class SlaytViewModel : Slaytlar
    {
        public Slaytlar Slayt { get; set; }

        public string[] SlaytBaslikCeviri { get; set; }
        public string[] SlaytBaslikCeviri2 { get; set; }
        public string[] SlaytBaslikCeviri3 { get; set; }
        public string[] SlaytBaslikCeviri4 { get; set; }
        public string[] SlaytBaslikCeviri5 { get; set; }
        public string[] AciklamaCeviri { get; set; }
        public string[] ButonAdiCeviri { get; set; }
        public string[] UrlCeviri { get; set; }
        public string[] ButonAdiCeviri2 { get; set; }
        public string[] UrlCeviri2 { get; set; }
        public string[] VideoCeviri { get; set; }
        public string[] YoutubeVideoCeviri { get; set; }

        public List<IFormFile> SayfaVideo { get; set; }
        public string[] VideoSecimListesi { get; set; }

        public List<IFormFile> SayfaResmi { get; set; }
        public string[] SayfaResmiSecimListesi { get; set; }

        public IFormFile ArkaplanSayfaResim { get; set; }
        public IFormFile SlaytResim1 { get; set; }
        public IFormFile SlaytResim2 { get; set; }
        public IFormFile SlaytResim3 { get; set; }
        public IFormFile SlaytResim4 { get; set; }
        public IFormFile SlaytResim5 { get; set; }

        public string UrunAutocomplete { get; set; }
        public int[] SeciliUrunlerAutocomplete { get; set; }
    }
}
