using EticaretWebCoreEntity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreService.Sepet
{
    public class ShoppingCartItem
    {
        public int UrunId { get; set; }
        public int UyeId { get; set; }
        public string SepetCookie { get; set; }
        public decimal Fiyat { get; set; }
        public int Adet { get; set; }
        public decimal Tutar => Fiyat * Adet;
        public string UrunSecenekleri { get; set; }

        public SepetAdetGuncellemeDurumlari SepetAdetGuncellemeDurum { get; set; }
    }
}
