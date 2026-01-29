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
    public class BannerResimTranslateModel
    {
        public int BannerResimId { get; set; }
        public string BannerAdi { get; set; } = "";
        public string Url { get; set; } = "";
        public int? EntityId { get; set; } // Nullable int
        public MenuTipleri UrlTipi { get; set; }
        public SeoUrlTipleri SeoUrlTipi { get; set; }

        public IFormFile? Resim { get; set; } // Nullable IFormFile
        public string ResimUrl { get; set; } = "";
        public int DilId { get; set; }
        public int Sira { get; set; }
        public int BannerRow { get; set; }
    }
}
