using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class SiparisGecmisViewModel : SiparisGecmisleri
    {
        public string KargoKodu { get; set; }
        public int KargoSiparisId { get; set; }

    }
}
