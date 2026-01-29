using EticaretWebCoreEntity.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreService.Sepet
{
    public sealed class PriceResult
    {
        public decimal Net { get; }                    // Hesaplanmış net tutar (decimal)
        public ParaBirimi CurrencyForDisplay { get; }  // Ekranda hangi PB ile gösterileceği (TRY veya orijinal)

        public PriceResult(decimal net, ParaBirimi displayCurrency)
        {
            Net = net;
            CurrencyForDisplay = displayCurrency;
        }

        public string Format(bool showSymbol)
        {
            if (!showSymbol)
                return Net.ToString("N2", CultureInfo.InvariantCulture);

            var culture = new CultureInfo(ParaBirimiToCulture(CurrencyForDisplay));
            return Net.ToString("C2", culture);
        }

        // İstersen public static yapıp ortak yerden kullan
        private static string ParaBirimiToCulture(ParaBirimi pb) => pb switch
        {
            ParaBirimi.TRY => "tr-TR",
            ParaBirimi.USD => "en-US",
            ParaBirimi.EUR => "de-DE",
            _ => CultureInfo.InvariantCulture.Name
        };
    }

}
