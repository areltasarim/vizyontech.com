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
    public class VideoViewModel : Videolar
    {
        public string[] VideoAdiCeviri { get; set; }
        public string[] VideoLinkiCeviri { get; set; }
        public string[] KisaAciklamaCeviri { get; set; }
        public string[] MetaBaslikCeviri { get; set; }
        public string[] MetaAciklamaCeviri { get; set; }
        public string[] MetaAnahtarCeviri { get; set; }
        public IFormFile SayfaResim { get; set; }


    }
}
