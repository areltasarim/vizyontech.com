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
    public class SabitMenuViewModel : SabitMenuler
    {
        public string[] MenuAdiCeviri { get; set; }
        public string[] AciklamaCeviri { get; set; }
        public string[] BreadcrumbSayfaAdiCeviri { get; set; }
        public string[] BreadcrumbAciklamaCeviri { get; set; }
        public string[] UrlCeviri { get; set; }
        public string[] MetaBaslikCeviri { get; set; }
        public string[] MetaAciklamaCeviri { get; set; }
        public string[] MetaAnahtarCeviri { get; set; }

        public IFormFile BreadcrumbImage { get; set; }

    }
}
