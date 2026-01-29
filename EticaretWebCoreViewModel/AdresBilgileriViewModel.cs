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
    public class AdresBilgileriViewModel : AdresBilgileri
    {
        public string[] AdresBasligiCeviri { get; set; }
        public string[] TelefonCeviri { get; set; }
        public string[] FaksCeviri { get; set; }
        public string[] GsmCeviri { get; set; }
        public string[] WhatsappCeviri { get; set; }
        public string[] EmailCeviri { get; set; }
        public string[] AdresCeviri { get; set; }
        public string[] HaritaCeviri { get; set; }
        public string[] HaritaLinkCeviri { get; set; }
        public string[] CalismaSaatlariCeviri { get; set; }
        public IFormFile MagazaResim { get; set; }

    }
}
