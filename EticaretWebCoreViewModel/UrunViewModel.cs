using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class UrunViewModel : Urunler
    {

        public Urunler Urun { get; set; } = new Urunler();

        public int DilId { get; set; } = 0;

        public string[] MetaBaslikCeviri { get; set; }
        public string[] MetaAciklamaCeviri { get; set; }
        public string[] MetaAnahtarCeviri { get; set; }


        public UrunOzellikleri UrunOzellik { get; set; }
        public bool YoutubeResimSilDurum { get; set; } = false;
        public string SecenekAutocomplete { get; set; }

        public IFormFile BreadcrumbImage { get; set; }

        public string UrunOzellikPath { get; set; } = "";
        public bool UrunOzellikSilDurum { get; set; } = false;
        public int[] UrunSecenekId { get; set; }
        public int[] UrunSecenekDegerId { get; set; }


        public string BenzerUrunAutocomplete { get; set; }
        public int[] SeciliBenzerUrunAutocomplete { get; set; }

        public string TamamlayiciUrunAutocomplete { get; set; }
        public int[] SeciliTamamlayiciUrunAutocomplete { get; set; }

        public List<UrunViewModel> UrunOzellikListesi { get; set; } = new List<UrunViewModel>();

        public List<UrunViewModel> UrunSecenekSelectListe { get; set; } = new List<UrunViewModel>();

        public List<UrunToOzellik> UrunToOzellikListesi { get; set; } = new List<UrunToOzellik>();
        public List<MenuYerleri> MenuYerleri { get; set; }

    }


    public class UrunSiralaViewModel
    {
        public int UrunId { get; set; }
        public string UrunKodu { get; set; }
        public string UrunAdi { get; set; }
        public string KategoriYolu { get; set; }
        public int Sira { get; set; }
    }


    public class UrunSiralamaModel
    {
        public int Id { get; set; } // Ürün ID
        public int NewPosition { get; set; } // Yeni sıra numarası
        public string Category { get; set; } // Seçili kategori adı
    }


}
