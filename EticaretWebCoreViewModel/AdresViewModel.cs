using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class AdresViewModel : Adresler
    {
        public  Adresler AdresEkle { get; set; }

        public IFormFile VergiLevhasiDosya { get; set; }
        public int UlkeId { get; set; }
        public int IlId { get; set; }

        //Adresi Kayıt ederken üye ol sayfasındanmı yoksa modaldanmı kayıt işlemi gerçekleştiriliyor bunun ayrımını yapmak için
        //Bunun sebebi kayıt kısmını tek sayfadan yönetiyoruz. Üye olurken aynı anda adresler tablosunada kayıt ekliyoruz
        public AdresKayitTipleri AdresKayitTipi { get; set; }

    }
}
