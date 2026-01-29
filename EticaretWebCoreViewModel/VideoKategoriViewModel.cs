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
    public class VideoKategoriViewModel : VideoKategorileri
    {

        public string[] KategoriAdiCeviri { get; set; }

        public string[] KisaAciklamaCeviri { get; set; }
        public string[] MetaBaslikCeviri { get; set; }
        public string[] MetaAciklamaCeviri { get; set; }
        public string[] MetaAnahtarCeviri { get; set; }
        public IFormFile SayfaResmi { get; set; }
    }
}
