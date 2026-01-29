using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class SayfaViewModel : Sayfalar
    {
        public Sayfalar Sayfa { get; set; } = new Sayfalar();

        public SayfaToSayfalar SayfaToSayfa { get; set; }
        public SayfaOzellikleri SayfaOzellik { get; set; }

        public string[] MetaBaslikCeviri { get; set; }
        public string[] MetaAciklamaCeviri { get; set; }
        public string[] MetaAnahtarCeviri { get; set; }

        public IFormFile BreadcrumbImage { get; set; }

        public int[] SeciliSayfalar { get; set; }

        public string SayfaAutocomplete { get; set; }
        public int[] SeciliSayfalarAutocomplete { get; set; }

        //public int[] SayfaSecenekId { get; set; }
        //public int[] SayfaSecenekDegerId { get; set; }

        public List<SayfaViewModel> SayfaOzellikListesi { get; set; } = new List<SayfaViewModel>();

        public List<SayfaViewModel> SayfaSecenekSelectListe { get; set; } = new List<SayfaViewModel>();

        public List<SayfaToOzellik> SayfaToOzellikListesi { get; set; } = new List<SayfaToOzellik>();
        public List<MenuYerleri> MenuYerleri { get; set; }


    }


}
