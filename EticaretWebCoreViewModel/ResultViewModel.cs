using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class ResultViewModel
    {
        public bool Basarilimi { get; set; }
        public string Mesaj { get; set; }
        public string MesajDurumu { get; set; }
        public string Sonuc { get; set; }


        public string Display { get; set; }
        public string Data { get; set; }
        public string SayfaUrl { get; set; }
        public int? SayfaId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }


        public bool NotfyAlert { get; set; }
        public bool BootBoxAlert { get; set; }

        public PaytrViewModel PaytrModel { get; set; }

    }
}
