using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    [Table("TBLSIPARISKALEM")]

    public class TBLSIPARISKALEM
    {
        public int ID { get; set; }
        public int? SIRA { get; set; }
        public int? SIPARISID { get; set; }
        public int? KALEMID { get; set; }
        public int? SUBEID { get; set; }
        public int? DEPOID { get; set; }
        public string BELGE_NO { get; set; }
        public int? CARIID { get; set; }
        public int? TIP { get; set; }
        public int? STOKID { get; set; }
        public string GCKOD { get; set; }
        public int? PROJEID { get; set; }
        public int? RAPORID1 { get; set; }
        public int? RAPORID2 { get; set; }
        public int? PLASIYERID { get; set; }
        public DateTime? TARIH { get; set; }
        public int? KOSULID { get; set; }
        public int? DETAYKOSULID { get; set; }
        public int? FIYATLISTEID { get; set; }
        public int? FIYATID { get; set; }
        public decimal? MIKTAR { get; set; }
        public decimal? MALFAZLASI { get; set; }
        public decimal? TESLIM_MIKTAR { get; set; }
        public decimal? KALAN_MIKTAR { get; set; }
        public decimal? SEVKEDILEBILIR_MIKTAR { get; set; }
        public decimal? SEVKEMRIVERILEN_MIKTAR { get; set; }
        public int? BIRIMID { get; set; }
        public decimal? CEVRIM { get; set; }
        public decimal? CEVRIM1 { get; set; }
        public decimal? CEVRIM2 { get; set; }
        public int? OZELLIKID { get; set; }
        public int? DOVIZID { get; set; }
        public decimal? KUR { get; set; }
        public decimal? KDV { get; set; }
        public string TEVKIFAT { get; set; }
        public decimal? TEVKIFATPAY { get; set; }
        public decimal? TEVKIFATPAYDA { get; set; }
        public decimal? ISK1 { get; set; }
        public decimal? ISK2 { get; set; }
        public decimal? ISK3 { get; set; }
        public decimal? NETFIYAT { get; set; }
        public decimal? BRUTFIYAT { get; set; }
        public decimal? ISKONTODUSULMUSFIYAT { get; set; }
        public decimal? KDVDAHILFIYAT { get; set; }
        public decimal? NETTOPLAM { get; set; }
        public decimal? ISKONTOTOPLAM { get; set; }
        public decimal? TOPLAM { get; set; }
        public decimal? KDVTOPLAM { get; set; }
        public decimal? BRUTTOPLAM { get; set; }
        public decimal? DOV_NETFIYAT { get; set; }
        public decimal? DOV_BRUTFIYAT { get; set; }
        public decimal? DOV_ISKONTODUSULMUSFIYAT { get; set; }
        public decimal? DOV_KDVDAHILFIYAT { get; set; }
        public decimal? DOV_NETTOPLAM { get; set; }
        public decimal? DOV_ISKONTOTOPLAM { get; set; }
        public decimal? DOV_KDVTOPLAM { get; set; }
        public decimal? DOV_TOPLAM { get; set; }
        public decimal? DOV_BRUTTOPLAM { get; set; }
        public short? DURUM { get; set; }
        public string AKTIF { get; set; }
        public string STOK_ADI { get; set; }
        public string BARKOD { get; set; }
        public string EKACIKLAMA { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public string ACIKLAMA3 { get; set; }
        public decimal? SACIKLAMA1 { get; set; }
        public decimal? SACIKLAMA2 { get; set; }
        public decimal? SACIKLAMA3 { get; set; }
        public string TEXTYEDEK1 { get; set; }
        public string TEXTYEDEK2 { get; set; }
        public decimal? SAYISALYEDEK1 { get; set; }
        public decimal? SAYISALYEDEK2 { get; set; }
        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }
        public int? DEVIRID { get; set; }
        public string ONAY { get; set; }
        public string REZERVE { get; set; }
        public string KAPATILSIN { get; set; }
        public int? HIZMETID { get; set; }
        public DateTime? TESLIM_TARIHI { get; set; }
        public decimal? ISK4 { get; set; }
        public decimal? ISK5 { get; set; }
        public decimal? ISK6 { get; set; }
        public int? KAYITTIPI { get; set; }
        public int? TALEPHARID { get; set; }
        public int? DONEM { get; set; }
        public int? TEKLIFHARID { get; set; }
        public int? RAFID { get; set; }
        public int? VADEGUN { get; set; }
        public string SEVKYAPILSIN { get; set; }
        public int? OZELLIKID1 { get; set; }
        public int? OZELLIKID2 { get; set; }
        public decimal? OTVORAN { get; set; }
        public decimal? OTVTOPLAM { get; set; }
        public string LOTNO { get; set; }
        public string OZELLIKACIKLAMA { get; set; }
        public int? KAPATMARAPORID1 { get; set; }
        public int? KAPATMARAPORID2 { get; set; }
        public int? PALETID { get; set; }
        public int? TEVKIFATKOD { get; set; }
        public decimal? TEVKIFATTOPLAM { get; set; }
        public string KAPATMASEBEP { get; set; }
        public string ESYAKAPCINSI { get; set; }
        public int? ODEMEPLANID { get; set; }
        public DateTime? CTARIH { get; set; }
        public DateTime? UTARIH { get; set; }
    }
}
