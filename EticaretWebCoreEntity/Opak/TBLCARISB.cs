using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity
{
    [Table("TBLCARISB")]
    public class TBLCARISB
    {
        [Column("KOD")]
        [StringLength(50)]
        public string KOD { get; set; }

        [Column("ID")]
        public int ID { get; set; }

        [StringLength(200)] public string? ADI { get; set; }
        [StringLength(50)] public string? ILCE { get; set; }
        [StringLength(50)] public string? IL { get; set; }
        [StringLength(300)] public string? ADRES { get; set; }
        [StringLength(50)] public string? VERGI_DAIRESI { get; set; }
        [StringLength(50)] public string? VERGINO { get; set; }
        [StringLength(50)] public string? KIMLIKNO { get; set; }
        [StringLength(20)] public string? TIPI { get; set; }
        public int? MUHASEBEID { get; set; }
        public int? VADE_GUNU { get; set; }
        [StringLength(50)] public string? TELEFON { get; set; }
        [StringLength(50)] public string? FAX { get; set; }
        [StringLength(20)] public string? TUR { get; set; }
        [StringLength(50)] public string? GRUP_KODU { get; set; }
        [StringLength(100)] public string? EMAIL { get; set; }
        [StringLength(50)] public string? WEB { get; set; }
        public int? PLASIYERID { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? ISKONTO { get; set; }
        [StringLength(1)] public string? AKTIF { get; set; }

        public int? RAPORID1 { get; set; }
        public int? RAPORID2 { get; set; }
        public int? RAPORID3 { get; set; }
        public int? RAPORID4 { get; set; }
        public int? RAPORID5 { get; set; }

        [StringLength(100)] public string? ACIKLAMA1 { get; set; }
        [StringLength(100)] public string? ACIKLAMA2 { get; set; }
        [StringLength(100)] public string? ACIKLAMA3 { get; set; }
        [StringLength(100)] public string? ACIKLAMA4 { get; set; }
        [StringLength(100)] public string? ACIKLAMA5 { get; set; }

        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA1 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA2 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA3 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA4 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA5 { get; set; }

        [StringLength(1)] public string? TUMISLEMLERKILIT { get; set; }
        [StringLength(1)] public string? ALISYAPMA { get; set; }
        [StringLength(1)] public string? SATISYAPMA { get; set; }
        [StringLength(1)] public string? ODEMEYAPMA { get; set; }
        [StringLength(1)] public string? CEKVERME { get; set; }
        [StringLength(1)] public string? CEKALMA { get; set; }
        [StringLength(1)] public string? SENETVERME { get; set; }
        [StringLength(1)] public string? SENETALMA { get; set; }

        [StringLength(1)] public string? RISKVARMI { get; set; }
        [StringLength(1)] public string? SIPARISRISK { get; set; }
        public int? SIPARISRISKDURUM { get; set; }
        [StringLength(1)] public string? IRSALIYERISK { get; set; }
        public int? IRSALIYERISKDURUM { get; set; }
        [StringLength(1)] public string? SEVKRISK { get; set; }
        public int? SEVKRISKDURUM { get; set; }
        [StringLength(1)] public string? YUKLEMERISK { get; set; }
        public int? YUKLEMERISKDURUM { get; set; }
        public int? FATURARISKDURUM { get; set; }

        [Column(TypeName = "decimal(15,2)")] public decimal? CEKASILRISK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? CEKCIRORISK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SENETASILRISK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SENETCIRORISK { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? TEMINATRISK { get; set; }

        [StringLength(50)] public string? TEXTYEDEK1 { get; set; }
        [StringLength(50)] public string? TEXTYEDEK2 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SAYISALYEDEK1 { get; set; }
        [Column(TypeName = "decimal(15,8)")] public decimal? SAYISALYEDEK2 { get; set; }

        public DateTime? TARIHYEDEK1 { get; set; }
        public DateTime? TARIHYEDEK2 { get; set; }

        public int? FIYAT { get; set; }
        public int? KOSULID { get; set; }

        [StringLength(1)] public string? RENKVARMI { get; set; }
        public int? RENK { get; set; }
        public int? ULKEID { get; set; }
        public int? RAPORID6 { get; set; }

        [Column(TypeName = "decimal(15,8)")] public decimal? RISK { get; set; }

        [StringLength(1)] public string? KOSULVARMI { get; set; }
        [StringLength(1)] public string? KDVMUAF { get; set; }
        [StringLength(1)] public string? EFATURAMI { get; set; }

        [StringLength(150)] public string? EKEP { get; set; }
        [StringLength(1)] public string? B2B { get; set; }

        public int? DOVIZID { get; set; }

        [StringLength(50)] public string? CARIADI { get; set; }
        [StringLength(50)] public string? CARISOYADI { get; set; }

        public int? SATMUHASEBEID { get; set; }
        public int? FIYATLISTEID { get; set; }
        public int? FIYATLISTEIDALIS { get; set; }

        [StringLength(8000)] public string? ACIKLAMA { get; set; }

        [StringLength(50)] public string? CEPTEL1 { get; set; }
        [StringLength(50)] public string? CEPTEL2 { get; set; }

        [StringLength(1)] public string? KARGOTIP { get; set; }

        [StringLength(50)] public string? EFATDIZAYNADI { get; set; }
        [StringLength(50)] public string? EARSIVDIZAYNADI { get; set; }

        [StringLength(1)] public string? EIRSALIYEMI { get; set; }

        [StringLength(100)] public string? ACIKLAMA6 { get; set; }
        [StringLength(100)] public string? ACIKLAMA7 { get; set; }
        [StringLength(100)] public string? ACIKLAMA8 { get; set; }
        [StringLength(100)] public string? ACIKLAMA9 { get; set; }
        [StringLength(100)] public string? ACIKLAMA10 { get; set; }

        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA6 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA7 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA8 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA9 { get; set; }
        [Column(TypeName = "decimal(15,2)")] public decimal? SACIKLAMA10 { get; set; }

        [StringLength(150)] public string? EFATKEPVARSAYILAN { get; set; }
        [StringLength(150)] public string? EIRSKEPVARSAYILAN { get; set; }

        [StringLength(300)] public string? CADDE { get; set; }
        [StringLength(300)] public string? BINA { get; set; }
        [StringLength(300)] public string? KAPINO { get; set; }
        [StringLength(300)] public string? POSTAKODU { get; set; }

        [StringLength(1)] public string? OZELHESAPKAPATMAPASIF { get; set; }

        public int? CARIISLEMTIPI { get; set; }

        [StringLength(1)] public string? TEKLIFRISK { get; set; }
        public int? TEKLIFRISKDURUM { get; set; }

        [StringLength(50)] public string? EFATURATIPI { get; set; }
        public int? NAKLIYEID { get; set; }
        public int? TOPLAMABOLUMID { get; set; }

        [StringLength(50)] public string? B2BSIFRE { get; set; }

        public int? EIRSALIYENAKLIYEID { get; set; }

        [StringLength(1)] public string? HESAPKESIMBILGIMAILI { get; set; }
        public int? HESAPKESIMGUNU { get; set; }
        public int? SONODEMEGUNU { get; set; }

        [StringLength(1)] public string? SIPSEVKBIRLESTIRILMESIN { get; set; }
        [StringLength(1)] public string? KARTMUSTERI { get; set; }

        [StringLength(50)] public string? EIRSDIZAYNADI { get; set; }

        public int? ODEMEPLANID { get; set; }

        [StringLength(500)] public string? EKACIKLAMA { get; set; }

        [StringLength(1)] public string? KURDEGERLENDIRME { get; set; }
    }
}
