using EticaretWebCoreEntity;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Exchange.WebServices.Data;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EticaretWebCoreEntity.Enums;
using System.Globalization;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EticaretWebCoreHelper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EticaretWebCoreEntity.Opak;
using EticaretWebCoreViewModel.Opak;
using Microsoft.Data.SqlClient;

namespace EticaretWebCoreService.OpakOdeme
{

    public partial class OpakServis : IOpakServis
    {
        private readonly AppDbContext _context;
        private readonly OpakDbContext _opakDbContext;
        private readonly HelperServis _helperServis;
        private readonly HttpClient _httpClient;
        private static IHttpContextAccessor _httpContextAccessor;

        public OpakServis(AppDbContext _context, IHttpContextAccessor httpContextAccessor, HttpClient httpClient, HelperServis helperServis, OpakDbContext opakDbContext)
        {
            _httpClient = httpClient;
            _helperServis = helperServis;
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _opakDbContext = opakDbContext;
        }

        public async Task<ResultViewModel> TblCariSbKayitEtAsync(int UyeId)
        {
            var result = new ResultViewModel();


            var uye = _context.Users.Find(UyeId);

            bool cariKaydiVarMi = await _opakDbContext.TBLCARISB.AnyAsync(c => c.KOD == uye.CariKodu);
            if (!cariKaydiVarMi)
            {
                // Son ID'yi al
                int sonId = await _opakDbContext.TBLCARISB.MaxAsync(c => (int?)c.ID) ?? 0;
                int yeniId = sonId + 1;

                string yeniKod = $"CR{yeniId}";

                var YENICARI = new TBLCARISB
                {
                    KOD = yeniKod,
                    ADI = $"{uye.Ad} {uye.Soyad}",
                    ILCE = uye.Ilceler.IlceAdi,
                    IL = uye.Ilceler.Iller.IlAdi,
                    ADRES = uye.Adres,
                    VERGI_DAIRESI = uye.VergiDairesi,
                    VERGINO = uye.VergiNumarasi.ToString(),
                    KIMLIKNO = "11111111111",
                    TIPI = "Alıcı",
                    MUHASEBEID = 0,
                    VADE_GUNU = 0,
                    TELEFON = "",
                    FAX = "",
                    TUR = "Kurumsal",
                    GRUP_KODU = "001",
                    EMAIL = uye.Email,
                    WEB = "",
                    PLASIYERID = uye.PlasiyerId,
                    ISKONTO = uye.IskontoOrani,
                    AKTIF = "E",
                    RAPORID1 = 0,
                    RAPORID2 = 0,
                    RAPORID3 = 0,
                    RAPORID4 = 0,
                    RAPORID5 = 0,

                    ACIKLAMA1 = "",
                    ACIKLAMA2 = "",
                    ACIKLAMA3 = "",
                    ACIKLAMA4 = "",
                    ACIKLAMA5 = "",

                    SACIKLAMA1 = 0,
                    SACIKLAMA2 = 0,
                    SACIKLAMA3 = 0,
                    SACIKLAMA4 = 0,
                    SACIKLAMA5 = 0,

                    TUMISLEMLERKILIT = "H",
                    ALISYAPMA = "E",
                    SATISYAPMA = "E",
                    ODEMEYAPMA = "E",
                    CEKVERME = "H",
                    CEKALMA = "E",
                    SENETVERME = "H",
                    SENETALMA = "H",
                    RISKVARMI = "E",

                    SIPARISRISK = "E",
                    SIPARISRISKDURUM = 0,
                    IRSALIYERISK = "E",
                    IRSALIYERISKDURUM = 0,
                    SEVKRISK = "E",
                    SEVKRISKDURUM = 0,
                    YUKLEMERISK = "E",
                    YUKLEMERISKDURUM = 0,
                    FATURARISKDURUM = 0,

                    CEKASILRISK = 0m,
                    CEKCIRORISK = 0m,
                    SENETASILRISK = 0m,
                    SENETCIRORISK = 0m,
                    TEMINATRISK = 0m,

                    TEXTYEDEK1 = "",
                    TEXTYEDEK2 = "",
                    SAYISALYEDEK1 = 0,
                    SAYISALYEDEK2 = 0,
                    TARIHYEDEK1 = DateTime.Now,
                    TARIHYEDEK2 = DateTime.Now,

                    FIYAT = 1,
                    KOSULID = 0,
                    RENKVARMI = "E",
                    RENK = 0,
                    ULKEID = 1,
                    RAPORID6 = 0,
                    RISK = 0,
                    KOSULVARMI = "H",
                    KDVMUAF = "H",
                    EFATURAMI = "E",
                    EKEP = "",
                    B2B = "H",
                    DOVIZID = 1,
                    CARIADI = uye.Ad,
                    CARISOYADI = uye.Soyad,
                    SATMUHASEBEID = 0,
                    FIYATLISTEID = 0,
                    FIYATLISTEIDALIS = 0,
                    ACIKLAMA = "",
                    CEPTEL1 = uye.Gsm,
                    CEPTEL2 = "",
                    KARGOTIP = "H",
                    EFATDIZAYNADI = "",
                    EARSIVDIZAYNADI = "",
                    EIRSALIYEMI = "E",
                    ACIKLAMA6 = "",
                    ACIKLAMA7 = "",
                    ACIKLAMA8 = "",
                    ACIKLAMA9 = "",
                    ACIKLAMA10 = "",
                    SACIKLAMA6 = 0,
                    SACIKLAMA7 = 0,
                    SACIKLAMA8 = 0,
                    SACIKLAMA9 = 0,
                    SACIKLAMA10 = 0,
                    EFATKEPVARSAYILAN = "",
                    EIRSKEPVARSAYILAN = "",
                    CADDE = "",
                    BINA = "",
                    KAPINO = "",
                    POSTAKODU = "",
                    OZELHESAPKAPATMAPASIF = "H",
                    CARIISLEMTIPI = 0,
                    TEKLIFRISK = "E",
                    TEKLIFRISKDURUM = 0,
                    EFATURATIPI = "Temel",
                    NAKLIYEID = 0,
                    TOPLAMABOLUMID = 0,
                    B2BSIFRE = "123456",
                    EIRSALIYENAKLIYEID = 0,
                    HESAPKESIMBILGIMAILI = "E",
                    HESAPKESIMGUNU = 0,
                    SONODEMEGUNU = 0,
                    SIPSEVKBIRLESTIRILMESIN = "H",
                    KARTMUSTERI = "H",
                    EIRSDIZAYNADI = "",
                    ODEMEPLANID = 0,
                    EKACIKLAMA = "",
                    KURDEGERLENDIRME = "H"
                };


                await _opakDbContext.TBLCARISB.AddAsync(YENICARI);
                await _opakDbContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<ResultViewModel> TblCariHaraketKayitAsync(CariHaraketKayitViewModel Model)
        {
            var result = new ResultViewModel();

            using var transaction = await _opakDbContext.Database.BeginTransactionAsync();

            try
            {
                var siparis = _context.Siparisler.Find(Model.SiparisId);
                var siparisUrunleri = _context.SiparisUrunleri.Where(x => x.SiparisId == Model.SiparisId).ToList();
                var uye = _context.Users.FirstOrDefault(x => x.Id == siparis.UyeId);

                int sira = 1;

                var siparisbelgeNo = await _opakDbContext.TBLSIPARIS
                    .OrderByDescending(x => x.ID)
                    .Select(x => x.ID)
                    .FirstOrDefaultAsync();

                int siparisyenibelgeNo = Convert.ToInt32(siparisbelgeNo) + 1;

                var yeniSiparis = new TBLSIPARIS
                {
                    TIP = 1,
                    SUBEID = 1,
                    DEPOID = 1,
                    BELGE_NO = $"B2B-{siparisyenibelgeNo}",
                    CARIID = uye.OpakCariId,
                    SEVKID = 0,
                    TARIH = DateTime.Now,
                    TESLIM_TARIHI = DateTime.Now,
                    IST_TESLIM_TARIHI = DateTime.Now,
                    VADE_TARIHI = DateTime.Now,
                    PLASIYERID = 0,
                    PROJEID = 0,
                    FIYATID = 0,
                    RAPORID1 = 0,
                    RAPORID2 = 0,
                    KDV_DAHILMI = "E",
                    ISKDEGER1 = 0.0m,
                    ISKDEGER2 = 0.0m,
                    ISKDEGER3 = 0,
                    ALTMALIYET = 0,

                    KDV = 0.00m,
                    TOPLAM = 0.00m,
                    ARATOPLAM = 0.00m,
                    GENELTOPLAM = 0.00m,

                    ISKONTOTOPLAM = 0m,
                    SATIRISKONTO = 0m,
                    DURUM = 0,
                    AKTIF = "E",
                    KOSULID = 0,
                    DEVIRID = 0,
                    ACIKLAMA = Model.Aciklama,
                    ACIKLAMA1 = "",
                    ACIKLAMA2 = "",
                    ACIKLAMA3 = "",
                    ACIKLAMA4 = "",
                    SACIKLAMA1 = 0.00m,
                    SACIKLAMA2 = 0.00m,
                    SACIKLAMA3 = 0.00m,
                    SACIKLAMA4 = 0.00m,
                    TEXTYEDEK1 = "",
                    TEXTYEDEK2 = "",
                    SAYISALYEDEK1 = 0m,
                    SAYISALYEDEK2 = 0m,
                    TARIHYEDEK1 = DateTime.Now,
                    TARIHYEDEK2 = DateTime.Now,
                    ONAY = "H",
                    EVRAKNO = "",
                    VADEGUNU = 0,
                    KAPATILSIN = "H",
                    DOVIZID = 0,
                    KUR = 1m,
                    KAYITTIPI = 0,
                    ESKIID = 0,
                    ALTHESAPID = 1,
                    NAKLIYEID = 1,
                    BASIMSAYISI = 0,
                    TUR = 0,
                    DONEM = 2024,
                    VARMITEVKIFAT = "H",
                    TEVKIFATPAY = null,
                    TEVKIFATPAYDA = null,
                    TEVKIFATTOPLAM = 0m,
                    YETKILI = "",
                    GOREVI = "",
                    TEL = "",
                    FAX = "",
                    CEPTEL = "",
                    MAIL = "",
                    GONADI = "",
                    GONSOYADI = "",
                    GONTEL = "",
                    GONMAIL = "",
                    ISLEMTIPI = 0,
                    SEVKILCE = siparis.TeslimatIlce,
                    SEVKIL = siparis.TeslimatIl,
                    SEVKADRES = siparis.TeslimatAdres,
                    SEVKTEL = siparis.Telefon,
                    SEVKCARIADI = "",
                    SEVKVERGIDAIRESI = siparis.VergiDairesi,
                    SEVKVERGINO = siparis.VergiNumarasi.ToString(),
                    AMBAR = "",
                    SEVKYAPILSIN = "E",
                    KULLANICIID = 0,
                    SAAT = DateTime.Now.ToString("HH:mm"),
                    SGUID = Guid.NewGuid().ToString(),
                    OTV = 0.00m,
                    ODEMEYONTEMIID = 0,
                    SIPARISTIP = 0,
                    KARGONO = "",
                    PARAMS = "",
                    OPONAYDURUMU = 0,
                    OPONAYACIKLAMA = "",
                    MUSSIPNO = "",
                    MUSSIPONAYLAYAN = "",
                    MUSSIPVEREN = "",
                    TERMINALNO = "",
                    SEVKCARIID = 0,
                    SEVKCARIADRESID = 0,
                    SONSURECID = 0,
                    ODEMEPLANID = 0,
                    CTARIH = DateTime.Now,
                    UTARIH = DateTime.Now,
                    BELGETIPID = 0,
                    KAYNAKTIPI = 0,
                    KAYNAKUUID = ""
                };

                await _opakDbContext.TBLSIPARIS.AddAsync(yeniSiparis);
                await _opakDbContext.SaveChangesAsync();

                decimal toplamNet = 0;
                decimal toplamBrut = 0;
                decimal toplamKdv = 0;

                foreach (var item in siparisUrunleri)
                {
                    var stok = await _opakDbContext.TBLSTOKSB
                        .Where(x => x.KOD.Trim() == item.UrunKodu)
                        .FirstOrDefaultAsync();

                    if(stok == null)
                    {
                        result.Basarilimi = false;
                        result.MesajDurumu = "danger";
                        result.Mesaj = "Sistemde eşleşen ürün bulunamadı.";

                        return result;
                    }

                    var kur = siparis.Kur;
                    var kdv = stok.SATIS_KDV ?? 0;
                    decimal kdvCarpan = (100 + kdv) / 100m;

                    decimal fiyatTL = item.Fiyat * kur;
                    decimal netFiyat = fiyatTL / kdvCarpan;
                    var miktar = item.Adet;

                    decimal netToplam = netFiyat * miktar;
                    decimal brutToplam = fiyatTL * miktar;
                    decimal kdvToplam = brutToplam - netToplam;

                    toplamNet += netToplam;
                    toplamBrut += brutToplam;
                    toplamKdv += kdvToplam;

                    var siparisKalem = new TBLSIPARISKALEM
                    {
                        SIRA = sira,
                        SIPARISID = yeniSiparis.ID,
                        KALEMID = 0,
                        SUBEID = 1,
                        DEPOID = 1,
                        BELGE_NO = $"B2B-{siparisyenibelgeNo}",
                        CARIID = uye.OpakCariId,
                        TIP = 1,
                        STOKID = stok.ID,
                        GCKOD = "C",
                        PROJEID = 0,
                        RAPORID1 = 0,
                        RAPORID2 = 0,
                        PLASIYERID = uye.PlasiyerId,
                        TARIH = DateTime.Now,
                        KOSULID = 0,
                        DETAYKOSULID = 0,
                        FIYATLISTEID = 0,
                        FIYATID = 0,
                        MIKTAR = item.Adet,
                        MALFAZLASI = 0m,
                        TESLIM_MIKTAR = 0m,
                        KALAN_MIKTAR = item.Adet,
                        SEVKEDILEBILIR_MIKTAR = 0m,
                        SEVKEMRIVERILEN_MIKTAR = 0m,
                        BIRIMID = 1,
                        CEVRIM = 1m,
                        CEVRIM1 = 1m,
                        CEVRIM2 = 1m,
                        OZELLIKID = 0,
                        DOVIZID = 1,
                        KUR = 1m,
                        KDV = kdv,
                        TEVKIFAT = "H",
                        TEVKIFATPAY = 1m,
                        TEVKIFATPAYDA = 1m,
                        ISK1 = 0m,
                        ISK2 = 0m,
                        ISK3 = 0m,
                        NETFIYAT = netFiyat,
                        BRUTFIYAT = fiyatTL,
                        ISKONTODUSULMUSFIYAT = fiyatTL,
                        KDVDAHILFIYAT = fiyatTL,
                        NETTOPLAM = netToplam,
                        ISKONTOTOPLAM = 0m,
                        TOPLAM = netToplam,
                        KDVTOPLAM = kdvToplam,
                        BRUTTOPLAM = brutToplam,
                        DOV_NETFIYAT = 0m,
                        DOV_BRUTFIYAT = 0m,
                        DOV_ISKONTODUSULMUSFIYAT = 0m,
                        DOV_KDVDAHILFIYAT = 0m,
                        DOV_NETTOPLAM = 0m,
                        DOV_ISKONTOTOPLAM = 0m,
                        DOV_KDVTOPLAM = 0m,
                        DOV_TOPLAM = 0m,
                        DOV_BRUTTOPLAM = 0m,
                        DURUM = 0,
                        AKTIF = "E",
                        STOK_ADI = stok.ADI,
                        BARKOD = "",
                        EKACIKLAMA = "",
                        ACIKLAMA1 = "",
                        ACIKLAMA2 = "",
                        ACIKLAMA3 = "",
                        SACIKLAMA1 = 0m,
                        SACIKLAMA2 = 0m,
                        SACIKLAMA3 = 0m,
                        TEXTYEDEK1 = "",
                        TEXTYEDEK2 = "",
                        SAYISALYEDEK1 = 0m,
                        SAYISALYEDEK2 = 0m,
                        TARIHYEDEK1 = DateTime.Now,
                        TARIHYEDEK2 = DateTime.Now,
                        DEVIRID = 0,
                        ONAY = "H",
                        REZERVE = "H",
                        KAPATILSIN = "H",
                        HIZMETID = 0,
                        TESLIM_TARIHI = DateTime.Now,
                        ISK4 = 0m,
                        ISK5 = 0m,
                        ISK6 = 0m,
                        KAYITTIPI = 0,
                        TALEPHARID = 0,
                        DONEM = 2024,
                        TEKLIFHARID = 0,
                        RAFID = 0,
                        VADEGUN = 0,
                        SEVKYAPILSIN = "E",
                        OZELLIKID1 = 0,
                        OZELLIKID2 = 0,
                        OTVORAN = 0m,
                        OTVTOPLAM = 0m,
                        LOTNO = "",
                        OZELLIKACIKLAMA = "",
                        KAPATMARAPORID1 = 0,
                        KAPATMARAPORID2 = 0,
                        PALETID = 0,
                        TEVKIFATKOD = 0,
                        TEVKIFATTOPLAM = 0m,
                        KAPATMASEBEP = "",
                        ESYAKAPCINSI = "",
                        ODEMEPLANID = 0,
                        CTARIH = DateTime.Now,
                        UTARIH = DateTime.Now
                    };

                    await _opakDbContext.TBLSIPARISKALEM.AddAsync(siparisKalem);
                    sira++;
                }

                // Kalem toplamlarını siparişe ata
                yeniSiparis.TOPLAM = toplamNet;
                yeniSiparis.KDV = toplamKdv;
                yeniSiparis.ARATOPLAM = toplamNet;
                yeniSiparis.GENELTOPLAM = toplamBrut;

                _opakDbContext.TBLSIPARIS.Update(yeniSiparis);
                await _opakDbContext.SaveChangesAsync();

                var cariuye = _opakDbContext.TBLCARISB.FirstOrDefault(x => x.KOD == siparis.Uyeler.CariKodu);

                if (cariuye == null)
                {
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = "Cari bulunamadı.";

                    return result;
                }

                var mevcutLimit = cariuye.RISK;
                var odenenTutar = toplamBrut;
                cariuye.RISK = mevcutLimit - odenenTutar;
                _opakDbContext.Entry(cariuye).State = EntityState.Modified;
                await _opakDbContext.SaveChangesAsync();


                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = "Başarılı";

                await transaction.CommitAsync();
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata: " + hata.Message;
                await transaction.RollbackAsync();
                throw new Exception("Kayıt işlemi başarısız oldu. Tüm değişiklikler geri alındı.");
            }

            return result;
        }

        public async Task<ResultViewModel> TblBankaHaraketKayitAsync(CariBankaHaraketViewModel Model)
        {
            var result = new ResultViewModel();
            using var transaction = await _opakDbContext.Database.BeginTransactionAsync();

            try
            {

                var uye = _context.Users.Find(Model.UyeId);

                var sonBelgeNo = await _opakDbContext.TBLBANKAHAR
                    .OrderByDescending(x => x.ID)
                    .Select(x => x.ID)
                    .FirstOrDefaultAsync();

                int yeniBelgeNo = Convert.ToInt32(sonBelgeNo) + 1;

                var bankaHarEkle = new TBLBANKAHAR
                {
                    BANKAID = 13,
                    KARTID = 0,
                    CARIID = uye.OpakCariId,
                    PLASIYERID = uye.PlasiyerId,
                    SUBEID = 1,
                    TARIH = DateTime.Now,
                    VADETARIH = DateTime.Now,
                    EFEKTIFTARIH = DateTime.Now,
                    BELGE_NO = "B2B_"+yeniBelgeNo.ToString(), 
                    ACIKLAMA = $"{uye.FirmaAdi}  K.Kartı Tahsilat",
                    KOMISYONBORC = 0m,
                    KOMISYONALACAK = 0m,
                    BORC = Model.OdenenTutar,
                    ALACAK = 0m,
                    DOVIZID = 1,
                    KUR = 1.00000000m,
                    DOVIZKOMISYONBORC = 0m,
                    DOVIZKOMISYONALACAK = 0m,
                    DOVIZBORC = 0m,
                    DOVIZALACAK = 0m,
                    TIP = 1,
                    FISID = 0,
                    FATURAID = 0,
                    MCEKID = 0,
                    MSENETID = 0,
                    KCEKID = 0,
                    KSENETID = 0,
                    KASAID = 0,
                    MUHASEBEID = 0,
                    MASRAFID = 0,
                    PERSONELID = 0,
                    BANKAHARID = 0,
                    ACIKLAMA1 = "",
                    ACIKLAMA2 = "",
                    ACIKLAMA3 = "",
                    SACIKLAMA1 = 0m,
                    SACIKLAMA2 = 0m,
                    SACIKLAMA3 = 0m,
                    TEXTYEDEK1 = "",
                    TEXTYEDEK2 = "",
                    SAYISALYEDEK1 = 0m,
                    SAYISALYEDEK2 = 0m,
                    TARIHYEDEK1 = DateTime.Now,
                    TARIHYEDEK2 = DateTime.Now,
                    DEVIRID = 0,
                    PROJEID = 0,
                    ALTHESAPID = 1,
                    MAHSUPID = 0,
                    SOZLESMEID = 3,
                    KDVDAHILMI = 'E',
                    KDV = 0m,
                    KDVTUTAR = 0m,
                    HESAPTIP = 1,
                    KAYITTIPI = 0,
                    ESKIID = 0,
                    DONEM = 2024,
                    SANALPOS = 'H',
                    HIZMETID = 0,
                    STOKID = 0,
                    MIKTAR = 0m,
                    HIZMETKATEGORIID = "",
                    ISLEMTIPI = 0,
                    GUID = Guid.NewGuid().ToString(),
                    MUHASEBEHESAPID = 0,
                    BANKAFISID = 0,
                    SAAT = DateTime.Now.ToString("HH:mm"),
                    IBKB = false,
                    KAYNAKTIPI = 0,
                    KAYNAKUUID = "",
                };
                await _opakDbContext.TBLBANKAHAR.AddAsync(bankaHarEkle);
                await _opakDbContext.SaveChangesAsync();


                var bankaKartHarEkle = new TBLBANKAKARTHAR
                {
                    BANKAHARID = bankaHarEkle.ID,
                    TARIH = DateTime.Now,
                    SUBEID = 1,
                    FATURAID = 0,
                    AY = 0,
                    TUTAR = Model.OdenenTutar,
                    DOVIZID = 1,
                    KUR = 1,
                    DOVIZTUTAR = 0,
                    KAYITTIPI = 0,
                    ESKIID = 0
                };
                await _opakDbContext.TBLBANKAKARTHAR.AddAsync(bankaKartHarEkle);
                await _opakDbContext.SaveChangesAsync();

                var cariHarEkle = new TBLCARIHAR
                {
                    CARIID = uye.OpakCariId,
                    PLASIYERID = uye.PlasiyerId,
                    SUBEID = 1,
                    TARIH = DateTime.Now,
                    VADETARIHI = DateTime.Now,
                    BELGE_NO = "B2B_"+yeniBelgeNo.ToString(),
                    ACIKLAMA = "ZİRAAT PAY K.Kartı Tahsilat",
                    BORC = 0m,
                    ALACAK = Model.OdenenTutar,
                    DOVIZID = 1,
                    KUR = 1.00000000m,
                    DOVIZBORC = 0m,
                    DOVIZALACAK = 0m,
                    TIP = 4,
                    FISID = 0,
                    FATURAID = 0,
                    MCEKID = 0,
                    MSENETID = 0,
                    KCEKID = 0,
                    KSENETID = 0,
                    KASAID = 0,
                    BANKAID = bankaHarEkle.ID,
                    MUHASEBEID = 0,
                    ACIKLAMA1 = "",
                    ACIKLAMA2 = "",
                    ACIKLAMA3 = "",
                    SACIKLAMA1 = 0m,
                    SACIKLAMA2 = 0m,
                    SACIKLAMA3 = 0m,
                    TEXTYEDEK1 = "",
                    TEXTYEDEK2 = "",
                    SAYISALYEDEK1 = 0m,
                    SAYISALYEDEK2 = 0m,
                    TARIHYEDEK1 = DateTime.Now,
                    TARIHYEDEK2 = DateTime.Now,
                    DEVIRID = 0,
                    PROJEID = 0,
                    ALTHESAPID = 1,
                    MAHSUPID = 0,
                    KAYITTIPI = 0,
                    ESKIID = 0,
                    DONEM = 2024,
                    ISLEMDOVIZID = 1,
                    ISLEMDOVIZBORC = 0m,
                    ISLEMDOVIZALACAK = Model.OdenenTutar,
                    ISLEMDOVIZKUR = 1.00000000m,
                    WEBAKTARILDIMI = 'H',
                    STOKID = 0,
                    MIKTAR = 0,
                    ISLEMTIPI = 0,
                    GUID = Guid.NewGuid().ToString(),
                    KAPATILMAYANBORC = 0m,
                    GECIKENGUN = 0,
                    KAPATILANBORC = 0m,
                    KAPATILANALACAK = 0m,
                    KARTID = 0,
                    SAAT = DateTime.Now.ToString("HH:mm"),
                    CTARIH = DateTime.Now,
                    UTARIH = DateTime.Now,
                    ODEMEPLANID = 0
                };
                await _opakDbContext.TBLCARIHAR.AddAsync(cariHarEkle);
                await _opakDbContext.SaveChangesAsync();


                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception("Kayıt işlemi başarısız oldu. Tüm değişiklikler geri alındı.");
            }



            return result;
        }
    }
}



