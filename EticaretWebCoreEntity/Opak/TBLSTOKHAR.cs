using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    [Table("TBLSTOKHAR")]

    public class TBLSTOKHAR
    {
        public int ID { get; set; }
        public int? SIRA { get; set; }
        public int? FISID { get; set; }
        public int? SIPARISID { get; set; }
        public int? SIPARISKALEMID { get; set; }
        public int? FATURAID { get; set; }
        public int? KALEMID { get; set; }
        public int? SUBEID { get; set; }
        public int? DEPOID { get; set; }
        public int? GIDENSUBEID { get; set; }
        public int? GIDENDEPOID { get; set; }
        public int? GELENSUBEID { get; set; }
        public int? GELENDEPOID { get; set; }
        public string BELGE_NO { get; set; }
        public string FATURA_NO { get; set; }
        public string IRSALIYE_NO { get; set; }
        public int? CARIID { get; set; }
        public int? TIP { get; set; }
        public int? STOKID { get; set; }
        public char? GCKOD { get; set; }
        public int? PROJEID { get; set; }
        public int? RAPORID1 { get; set; }
        public int? RAPORID2 { get; set; }
        public int? PLASIYERID { get; set; }
        public DateTime? TARIH { get; set; }
        public int? KOSULID { get; set; }
        public int? DETAYKOSULID { get; set; }
        public int? FIYATLISTEID { get; set; }
        public int? FIYATID { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? MIKTAR { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? MALFAZLASI { get; set; }
        public int? BIRIMID { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? CEVRIM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? CEVRIM1 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? CEVRIM2 { get; set; }
        public int? OZELLIKID { get; set; }
        public int? DOVIZID { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? KUR { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? KDV { get; set; }
        public char? TEVKIFAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? TEVKIFATPAY { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? TEVKIFATPAYDA { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISK1 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISK2 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISK3 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? NETFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? BRUTFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISKONTODUSULMUSFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? KDVDAHILFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? NETTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISKONTOTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? TOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? KDVTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? BRUTTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_NETFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_BRUTFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_ISKONTODUSULMUSFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_KDVDAHILFIYAT { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_NETTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_ISKONTOTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_KDVTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_TOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? DOV_BRUTTOPLAM { get; set; }
        public short? DURUM { get; set; }
        public char? AKTIF { get; set; }
        public string STOK_ADI { get; set; }
        public string BARKOD { get; set; }
        public string EKACIKLAMA { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public string ACIKLAMA3 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? SACIKLAMA1 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? SACIKLAMA2 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? SACIKLAMA3 { get; set; }
        public string TEXTYEDEK1 { get; set; }
        public string TEXTYEDEK2 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? SAYISALYEDEK1 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? SAYISALYEDEK2 { get; set; }
        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }
        public int? DEVIRID { get; set; }
        public int? MAHSUPID { get; set; }
        public char? KDVDAHILMI { get; set; }
        public int? SEVKIYATID { get; set; }
        public int? HIZMETID { get; set; }
        public string LOTNO { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISK4 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISK5 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? ISK6 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? TEVKIFATTOPLAM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? OTVORAN { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? OTVTOPLAM { get; set; }
        public int? KAYITTIPI { get; set; }
        public int? MUHASEBEID { get; set; }
        public DateTime? GERCEKTARIH { get; set; }
        public int? DONEM { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? GIRIS { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? CIKIS { get; set; }
        public int? VARLIKID { get; set; }
        public char? NAKLIYEMI { get; set; }
        public char? WEBAKTARILDIMI { get; set; }
        public int? RAFID { get; set; }
        public string HIZMETKATEGORIID { get; set; }
        public int? ISLEMTIPI { get; set; }
        public int? PERSONELID { get; set; }
        public string OZELLIKACIKLAMA { get; set; }
        public int? VADEGUN { get; set; }
        public int? OZELLIKID1 { get; set; }
        public int? OZELLIKID2 { get; set; }
        public int? TEVKIFATKOD { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? BIRIMADET1 { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? BIRIMADET2 { get; set; }
        public string OTVKOD { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? EKMALIYET { get; set; }
        [Column(TypeName = "decimal(25,8)")] public decimal? MALIYETLIFIYAT { get; set; }
        public DateTime? CTARIH { get; set; }
        public string ESYAKAPCINSI { get; set; }
        public int? FIYATFARKISTOKID { get; set; }
        public string KONSINYEMI { get; set; }
        public int? ODEMEPLANID { get; set; }
        public int? IHRACATSBID { get; set; }
        public DateTime? UTARIH { get; set; }
        public int? BAGLANTIID { get; set; }
        [StringLength(100)] public string CKULLANICI { get; set; }
        [StringLength(100)] public string UKULLANICI { get; set; }
    }
}
