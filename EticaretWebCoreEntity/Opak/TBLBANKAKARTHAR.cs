using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    public class TBLBANKAKARTHAR
    {
        [Column("ID")]
        public int ID { get; set; }
        public int? BANKAHARID { get; set; }

        public DateTime? TARIH { get; set; }

        public int? SUBEID { get; set; }

        public int? FATURAID { get; set; }

        public int? AY { get; set; }

        [Column(TypeName = "decimal(15,8)")]
        public decimal? TUTAR { get; set; }

        public int? DOVIZID { get; set; }

        [Column(TypeName = "decimal(15,8)")]
        public decimal? KUR { get; set; }

        [Column(TypeName = "decimal(15,8)")]
        public decimal? DOVIZTUTAR { get; set; }

        public int? KAYITTIPI { get; set; }

        public int? ESKIID { get; set; }
    }
}
