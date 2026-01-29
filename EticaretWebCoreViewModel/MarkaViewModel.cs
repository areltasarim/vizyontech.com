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
    public class MarkaViewModel : Markalar
    {
        public IFormFile SayfaResmi { get; set; }
        public List<MenuYerleri> MenuYerleri { get; set; }


    }
}
