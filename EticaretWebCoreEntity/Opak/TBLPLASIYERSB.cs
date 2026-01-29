using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreEntity.Opak
{
    public class TBLPLASIYERSB
    {
        public int ID { get; set; }
        [Required]
        public string ADI { get; set; }
        public string TELEFON { get; set; }
        public string ACIKLAMA1 { get; set; }
        public string ACIKLAMA2 { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? SACIKLAMA1 { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal? SACIKLAMA2 { get; set; }

        public char? AKTIF { get; set; }
        public string KOD { get; set; }
        public string GRUP { get; set; }
        public string EMAIL { get; set; }
        public string B2BAKTIF { get; set; }
        public string B2BSIFRE { get; set; }
        public string ADMIN { get; set; }
        public int? USTID { get; set; }
        public int? PERSONELID { get; set; }
    }
}
