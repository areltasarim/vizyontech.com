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
    public class SiteAyariViewModel : SiteAyarlari
    {
        public string[] MetaBaslikCeviri { get; set; }
        public string[] MetaAciklamaCeviri { get; set; }
        public string[] MetaAnahtarCeviri { get; set; }
        public string[] HeaderAciklamaCeviri { get; set; }
        public string[] FooterAciklamaCeviri { get; set; }
        public string[] PopupCeviri { get; set; }

        public IFormFile UstLogoResim { get; set; }
        public IFormFile FooterLogoResim { get; set; }
        public IFormFile FaviconResim { get; set; }
        public IFormFile MobilLogoResim { get; set; }
        public IFormFile MailLogoResim { get; set; }


    }
}
