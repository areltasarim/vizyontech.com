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
    public class KategoriBannerViewModel : KategoriBanner
    {

        public List<KategoriBanner> KategoriBannerListe { get; set; } = new List<KategoriBanner>();

        public IFormFile SayfaResmi { get; set; }

        
    }
}
