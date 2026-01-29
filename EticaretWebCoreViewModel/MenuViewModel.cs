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
    public class MenuViewModel : Menuler
    {
        public string[] MenuAdiCeviri { get; set; }
        public string[] UrlCeviri { get; set; }
        public IFormFile PageImage { get; set; }

    }

    public class MenuKonumlari
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
