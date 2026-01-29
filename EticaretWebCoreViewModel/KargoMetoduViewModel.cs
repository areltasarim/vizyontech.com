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
    public class KargoMetoduViewModel : KargoMetodlari
    {
        public string[] KargoAdiCeviri { get; set; }
        public string[] AciklamaCeviri { get; set; }

    }
}
