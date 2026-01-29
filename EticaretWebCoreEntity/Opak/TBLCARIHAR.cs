using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    [Table("TBLCARIHAR")]

    public class TBLCARIHAR
    {
        public int ID { get; set; }
        public int? CARIID { get; set; }
        public int? PLASIYERID { get; set; }
        public int? SUBEID { get; set; }
        public DateTime? TARIH { get; set; }
        public DateTime? VADETARIHI { get; set; }
        public string BELGE_NO { get; set; }
        public string ACIKLAMA { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BORC { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? ALACAK { get; set; }
        public int? DOVIZID { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? KUR { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? DOVIZBORC { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? DOVIZALACAK { get; set; }
        public int? TIP { get; set; }
        public int? FISID { get; set; }
        public int? FATURAID { get; set; }
        public int? MCEKID { get; set; }
        public int? MSENETID { get; set; }
        public int? KCEKID { get; set; }
        public int? KSENETID { get; set; }
        public int? KASAID { get; set; }
        public int? BANKAID { get; set; }
        public int? MUHASEBEID { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public string ACIKLAMA3 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA1 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA2 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA3 { get; set; }
        public string TEXTYEDEK1 { get; set; }
        public string TEXTYEDEK2 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SAYISALYEDEK1 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SAYISALYEDEK2 { get; set; }
        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }
        public int? DEVIRID { get; set; }
        public int? PROJEID { get; set; }
        public int? ALTHESAPID { get; set; }
        public int? MAHSUPID { get; set; }
        public int? KAYITTIPI { get; set; }
        public int? ESKIID { get; set; }
        public int? DONEM { get; set; }
        public int? ISLEMDOVIZID { get; set; }
        [Column(TypeName = "decimal(22,8)")] public decimal? ISLEMDOVIZBORC { get; set; }
        [Column(TypeName = "decimal(22,8)")] public decimal? ISLEMDOVIZALACAK { get; set; }
        [Column(TypeName = "decimal(22,8)")] public decimal? ISLEMDOVIZKUR { get; set; }
        public char? WEBAKTARILDIMI { get; set; }
        public int? STOKID { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? MIKTAR { get; set; }
        public int? ISLEMTIPI { get; set; }
        public string GUID { get; set; }
        [Column(TypeName = "decimal(22,8)")] public decimal? KAPATILMAYANBORC { get; set; }
        public int? GECIKENGUN { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? KAPATILANBORC { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? KAPATILANALACAK { get; set; }
        public int? KARTID { get; set; }
        public string SAAT { get; set; }
        public DateTime? CTARIH { get; set; }
        public DateTime? UTARIH { get; set; }
        public int? ODEMEPLANID { get; set; }
    }
}
