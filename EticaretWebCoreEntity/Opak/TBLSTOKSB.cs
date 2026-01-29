using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    [Table("TBLSTOKSB")]
    public class TBLSTOKSB
    {
        public int ID { get; set; }
        public string KOD { get; set; }
        public string ADI { get; set; }
        public string GRUP_KODU { get; set; }
        public int? MUHASEBEID { get; set; }
        public char? TIP { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SATIS_KDV { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? ALIS_KDV { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SATIS_OTV { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? ALIS_OTV { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BIRIM_AGIRLIK { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? EN { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BOY { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? YUKSEKLIK { get; set; }
        public char? OZELLIKVARMI { get; set; }
        public char? AKTIF { get; set; }
        public char? TARTIURUN { get; set; }
        public char? GIRISSERI { get; set; }
        public char? CIKISSERI { get; set; }
        public char? SERIBAKIYE { get; set; }
        public int? RAPORID1 { get; set; }
        public int? RAPORID2 { get; set; }
        public int? RAPORID3 { get; set; }
        public int? RAPORID4 { get; set; }
        public int? RAPORID5 { get; set; }
        public string URETICI_KODU { get; set; }
        public string URETICIBARKOD { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }
        public string ACIKLAMA3 { get; set; }
        public string ACIKLAMA4 { get; set; }
        public string ACIKLAMA5 { get; set; }
        public string ACIKLAMA6 { get; set; }
        public string ACIKLAMA7 { get; set; }
        public string ACIKLAMA8 { get; set; }
        public string ACIKLAMA9 { get; set; }
        public string ACIKLAMA10 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA1 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA2 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA3 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA4 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA5 { get; set; }
        public string TEXTYEDEK1 { get; set; }
        public string TEXTYEDEK2 { get; set; }
        public string TEXTYEDEK3 { get; set; }
        public string TEXTYEDEK4 { get; set; }
        public string TEXTYEDEK5 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SAYISALYEDEK1 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SAYISALYEDEK2 { get; set; }
        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }
        public char? DUZELTILDI { get; set; }
        public char? YENIKAYIT { get; set; }
        public int? SATDOVTIP { get; set; }
        public int? ALDOVTIP { get; set; }
        public string MARKA { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SFIYAT1 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SFIYAT2 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SFIYAT3 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SFIYAT4 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SFIYAT5 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? AFIYAT1 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? AFIYAT2 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? AFIYAT3 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? AFIYAT4 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? AFIYAT5 { get; set; }
        public int? OLCUBR1 { get; set; }
        public int? OLCUBR2 { get; set; }
        public int? OLCUBR3 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BIRIMADET1 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BIRIMADET2 { get; set; }

        public string BARKOD1 { get; set; }
        public string BARKOD2 { get; set; }
        public string BARKOD3 { get; set; }
        public int? BARKOD1BIRIMID { get; set; }
        public int? BARKOD2BIRIMID { get; set; }
        public int? BARKOD3BIRIMID { get; set; }
        public string KOSULGRUP_KODU { get; set; }
        public char? MARKETFIYATDEGISSIN { get; set; }
        public string KOSULALISGRUP_KODU { get; set; }
        public string ACIKLAMA { get; set; }
        public int? B2CDOVIZID { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? B2CFIYAT { get; set; }
        public int? N11KATEGORIID { get; set; }
        public char? N11YAYINLANSIN { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? N11FIYAT { get; set; }
        public string N11MARKA { get; set; }
        public int? SATISBIRIM { get; set; }
        public int? ALISBIRIM { get; set; }
        public string RAF { get; set; }
        public string BARKOD4 { get; set; }
        public string BARKOD5 { get; set; }
        public string BARKOD6 { get; set; }
        public int? BARKOD4BIRIMID { get; set; }
        public int? BARKOD5BIRIMID { get; set; }
        public int? BARKOD6BIRIMID { get; set; }
        public char? GITTIGIDIYOR { get; set; }
        public char? HEPSIBURADA { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal? BARKODCARPAN1 { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal? BARKODCARPAN2 { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal? BARKODCARPAN3 { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal? BARKODCARPAN4 { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal? BARKODCARPAN5 { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal? BARKODCARPAN6 { get; set; }
        public char? PTT { get; set; }
        public int? OZELLIKID1 { get; set; }
        public int? OZELLIKID2 { get; set; }
        public string ANAOZELLIK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SATISISK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? ALISISK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA6 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA7 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA8 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA9 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA10 { get; set; }
        public int? RAPORID6 { get; set; }
        public int? RAPORID7 { get; set; }
        public int? RAPORID8 { get; set; }
        public int? RAPORID9 { get; set; }
        public int? RAPORID10 { get; set; }
        public int? OLCUBR4 { get; set; }
        public int? OLCUBR5 { get; set; }
        public int? OLCUBR6 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BIRIMADET3 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BIRIMADET4 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? BIRIMADET5 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? BARKODISK1 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? BARKODISK2 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? BARKODISK3 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? BARKODISK4 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? BARKODISK5 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? BARKODISK6 { get; set; }
        public string MARKETFIYATDEGISMESIN { get; set; }
        public string MARKETISKONTODEGISMESIN { get; set; }
        public string GTIP { get; set; }
        public int? SATINALMATOLERANSID { get; set; }
        public string ANATEDARIKCIKOD { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? LISTEFIYAT { get; set; }
        public int? LISTEDOVIZID { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? B2BFIYAT { get; set; }
        public int? B2BDOVIZID { get; set; }
        public string MENSEI { get; set; }
        public string VARMITEVKIFAT { get; set; }
        public int? TEVKIFATPAY { get; set; }
        public int? TEVKIFATPAYDA { get; set; }
        public int? TEVKIFATKOD { get; set; }
        public string EKACIKLAMA { get; set; }
        public string DATAKODU { get; set; }
        public string GRUP_ADI { get; set; }
        public int? ODEMEPLANID { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal? MINSASMIKTAR { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal? MAXSASMIKTAR { get; set; }


    }
}
