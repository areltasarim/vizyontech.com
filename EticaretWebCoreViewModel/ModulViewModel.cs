using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class ModulViewModel : Moduller
    {
        public Moduller Modul { get; set; } = new Moduller();

       
    }

    public class ModulListesi
    {
        public int Id { get; set; }
        public string ModulAdi { get; set; }
        public ModulTipleri ModulTipi { get; set; }
        public string Contoller { get; set; }
        public string Action { get; set; }
    }
}
