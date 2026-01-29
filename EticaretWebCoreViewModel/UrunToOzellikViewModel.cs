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
    public class UrunToOzellikViewModel : UrunToOzellik
    {
        public int OzellikGrupId { get; set; }
        public List<UrunToOzellik> UrunToOzellikListesi { get; set; } = new List<UrunToOzellik>();
    }
}
