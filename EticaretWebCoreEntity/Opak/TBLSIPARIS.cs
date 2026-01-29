using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    [Table("TBLSIPARIS")]
    public class TBLSIPARIS
    {
        public int ID { get; set; }
        public int? TIP { get; set; }
        public int? SUBEID { get; set; }
        public int? DEPOID { get; set; }
        public string BELGE_NO { get; set; }
        public int? CARIID { get; set; }
        public int? SEVKID { get; set; }
        public DateTime? TARIH { get; set; }
        public DateTime? TESLIM_TARIHI { get; set; }
        public DateTime? IST_TESLIM_TARIHI { get; set; }
        public DateTime? VADE_TARIHI { get; set; }
        public int? PLASIYERID { get; set; }
        public int? PROJEID { get; set; }
        public int? FIYATID { get; set; }
        public int? RAPORID1 { get; set; }
        public int? RAPORID2 { get; set; }
        public string KDV_DAHILMI { get; set; }
        public decimal? ISKDEGER1 { get; set; }
        public decimal? ISKDEGER2 { get; set; }
        public decimal? ISKDEGER3 { get; set; }
        public decimal? ALTMALIYET { get; set; }
        public decimal? KDV { get; set; }
        public decimal? ISKONTOTOPLAM { get; set; }
        public decimal? SATIRISKONTO { get; set; }
        public decimal? TOPLAM { get; set; }
        public decimal? ARATOPLAM { get; set; }
        public decimal? GENELTOPLAM { get; set; }
        public short? DURUM { get; set; }
        public string AKTIF { get; set; }
        public int? KOSULID { get; set; }
        public int? DEVIRID { get; set; }
        public string ACIKLAMA { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public string ACIKLAMA3 { get; set; }
        public string ACIKLAMA4 { get; set; }
        public decimal? SACIKLAMA1 { get; set; }
        public decimal? SACIKLAMA2 { get; set; }
        public decimal? SACIKLAMA3 { get; set; }
        public decimal? SACIKLAMA4 { get; set; }
        public string TEXTYEDEK1 { get; set; }
        public string TEXTYEDEK2 { get; set; }
        public decimal? SAYISALYEDEK1 { get; set; }
        public decimal? SAYISALYEDEK2 { get; set; }
        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }
        public string ONAY { get; set; }
        public string EVRAKNO { get; set; }
        public int? VADEGUNU { get; set; }
        public string KAPATILSIN { get; set; }
        public int? DOVIZID { get; set; }
        public decimal? KUR { get; set; }
        public int? KAYITTIPI { get; set; }
        public int? ESKIID { get; set; }
        public decimal? ALTHESAPID { get; set; }
        public decimal? NAKLIYEID { get; set; }
        public int? BASIMSAYISI { get; set; }
        public int? TUR { get; set; }
        public int? DONEM { get; set; }
        public string VARMITEVKIFAT { get; set; }
        public int? TEVKIFATPAY { get; set; }
        public int? TEVKIFATPAYDA { get; set; }
        public decimal? TEVKIFATTOPLAM { get; set; }
        public string YETKILI { get; set; }
        public string GOREVI { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string CEPTEL { get; set; }
        public string MAIL { get; set; }
        public string GONADI { get; set; }
        public string GONSOYADI { get; set; }
        public string GONTEL { get; set; }
        public string GONMAIL { get; set; }
        public int? ISLEMTIPI { get; set; }
        public string SEVKILCE { get; set; }
        public string SEVKIL { get; set; }
        public string SEVKADRES { get; set; }
        public string SEVKTEL { get; set; }
        public string SEVKCARIADI { get; set; }
        public string SEVKVERGIDAIRESI { get; set; }
        public string SEVKVERGINO { get; set; }
        public string AMBAR { get; set; }
        public string SEVKYAPILSIN { get; set; }
        public int? KULLANICIID { get; set; }
        public string SAAT { get; set; }
        public string SGUID { get; set; }
        public decimal? OTV { get; set; }
        public int? ODEMEYONTEMIID { get; set; }
        public int? SIPARISTIP { get; set; }
        public string KARGONO { get; set; }
        public string PARAMS { get; set; }
        public int? OPONAYDURUMU { get; set; }
        public string OPONAYACIKLAMA { get; set; }
        public string MUSSIPNO { get; set; }
        public string MUSSIPONAYLAYAN { get; set; }
        public string MUSSIPVEREN { get; set; }
        public string TERMINALNO { get; set; }
        public int? SEVKCARIID { get; set; }
        public int? SEVKCARIADRESID { get; set; }
        public int? SONSURECID { get; set; }
        public int? ODEMEPLANID { get; set; }
        public DateTime? CTARIH { get; set; }
        public DateTime? UTARIH { get; set; }
        public int? BELGETIPID { get; set; }
        public int? KAYNAKTIPI { get; set; }
        public string KAYNAKUUID { get; set; }
    }
}
