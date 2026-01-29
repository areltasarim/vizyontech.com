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
    public class BannerViewModel : Banner
    {
        public Banner Banner { get; set; } = new Banner();
        public Dictionary<int, List<BannerResimTranslateModel>> BannerResimListesi { get; set; } = new Dictionary<int, List<BannerResimTranslateModel>>();
    }
}
