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
    public class SayfaToOzellikViewModel : SayfaToOzellik
    {
        public int OzellikGrupId { get; set; }
        public List<SayfaToOzellik> SayfaToOzellikListesi { get; set; } = new List<SayfaToOzellik>();
    }
}
