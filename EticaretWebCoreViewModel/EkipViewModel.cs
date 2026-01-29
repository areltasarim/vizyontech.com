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
    public class EkipViewModel : Ekipler
    {
        public string[] GorevCeviri { get; set; }
        public string[] AciklamaCeviri { get; set; }
        public IFormFile EkipResim { get; set; }
        public IFormFile LogoResim { get; set; }

    }
}
