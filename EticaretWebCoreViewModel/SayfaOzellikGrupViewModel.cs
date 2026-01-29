using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class SayfaOzellikGrupViewModel : SayfaOzellikGruplari
    {
        public SayfaOzellikGruplari SayfaOzellikGrup { get; set; } = new SayfaOzellikGruplari();
    }
}
