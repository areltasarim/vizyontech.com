using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    [Table("TBLBANKAHAR")]

    public class TBLBANKAHAR
    {
        public int? ID { get; set; }
        public int? BANKAID { get; set; }
        public int? KARTID { get; set; }
        public int? CARIID { get; set; }
        public int? PLASIYERID { get; set; }
        public int? SUBEID { get; set; }
        public DateTime? TARIH { get; set; }
        public DateTime? VADETARIH { get; set; }
        public DateTime? EFEKTIFTARIH { get; set; }
        public string BELGE_NO { get; set; }
        public string ACIKLAMA { get; set; }

        [Column(TypeName = "decimal(15, 8)")] public decimal? KOMISYONBORC { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? KOMISYONALACAK { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? BORC { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? ALACAK { get; set; }

        public int? DOVIZID { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? KUR { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? DOVIZKOMISYONBORC { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? DOVIZKOMISYONALACAK { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? DOVIZBORC { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? DOVIZALACAK { get; set; }

        public int? TIP { get; set; }
        public int? FISID { get; set; }
        public int? FATURAID { get; set; }
        public int? MCEKID { get; set; }
        public int? MSENETID { get; set; }
        public int? KCEKID { get; set; }
        public int? KSENETID { get; set; }
        public int? KASAID { get; set; }
        public int? MUHASEBEID { get; set; }
        public int? MASRAFID { get; set; }
        public int? PERSONELID { get; set; }
        public int? BANKAHARID { get; set; }

        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public string ACIKLAMA3 { get; set; }

        [Column(TypeName = "decimal(15, 2)")] public decimal? SACIKLAMA1 { get; set; }
        [Column(TypeName = "decimal(15, 2)")] public decimal? SACIKLAMA2 { get; set; }
        [Column(TypeName = "decimal(15, 2)")] public decimal? SACIKLAMA3 { get; set; }

        public string TEXTYEDEK1 { get; set; }
        public string TEXTYEDEK2 { get; set; }

        [Column(TypeName = "decimal(15, 2)")] public decimal? SAYISALYEDEK1 { get; set; }
        [Column(TypeName = "decimal(15, 2)")] public decimal? SAYISALYEDEK2 { get; set; }

        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }

        public int? DEVIRID { get; set; }
        public int? PROJEID { get; set; }
        public int? ALTHESAPID { get; set; }
        public int? MAHSUPID { get; set; }
        public int? SOZLESMEID { get; set; }

        public char? KDVDAHILMI { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? KDV { get; set; }
        [Column(TypeName = "decimal(15, 8)")] public decimal? KDVTUTAR { get; set; }

        public int? HESAPTIP { get; set; }
        public int? KAYITTIPI { get; set; }
        public int? ESKIID { get; set; }
        public int? DONEM { get; set; }

        public char? SANALPOS { get; set; }

        public int? HIZMETID { get; set; }
        public int? STOKID { get; set; }

        [Column(TypeName = "decimal(15, 8)")] public decimal? MIKTAR { get; set; }

        public string HIZMETKATEGORIID { get; set; }
        public int? ISLEMTIPI { get; set; }
        public string GUID { get; set; }

        public int? MUHASEBEHESAPID { get; set; }
        public int? BANKAFISID { get; set; }

        public string SAAT { get; set; }
        public bool? IBKB { get; set; }
        public int? KAYNAKTIPI { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string KAYNAKUUID { get; set; }
    }

}
