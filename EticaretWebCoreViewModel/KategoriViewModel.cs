using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
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
    public class KategoriViewModel : Kategoriler
    {
        public Kategoriler Kategori { get; set; } = new Kategoriler();
        public IFormFile SayfaResmi { get; set; }
        public IFormFile BreadcrumbImage { get; set; }

        public List<MenuYerleri> MenuYerleri { get; set; }

    }
}
