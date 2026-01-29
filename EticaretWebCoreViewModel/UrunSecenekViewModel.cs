using EticaretWebCoreEntity;
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
    public class UrunSecenekViewModel : UrunSecenekleri
    {
        public UrunSecenekleri UrunSecenek { get; set; } = new UrunSecenekleri();

        public List<UrunSecenekDegerleriTranslate> UrunSecenekDegerListesi { get; set; } = new List<UrunSecenekDegerleriTranslate>();

    }
}
