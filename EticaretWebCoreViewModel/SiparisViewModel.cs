using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class SiparisViewModel : Siparisler
    {
        public AdresViewModel AdresEkle { get; set; }
        public KasaViewModel KasaSiparis { get; set; }

        public Siparisler Siparis { get; set; }

        [Display(Name = "Ödeme Metodu")]
        [Required(ErrorMessage = "{0} boş bırakılamaz.")]
        public int SiparisOdemeMetodId { get; set; }
        public int SiparisKargoMetodId { get; set; }

        public int TeslimatAdresId { get; set; }

        public int FaturaAdresId { get; set; }


        //Projeye Özel Property
        public List<UrunSiparisViewModel> UrunListesi { get; set; } = new List<UrunSiparisViewModel>();
        //Projeye Özel Property

    }
}
