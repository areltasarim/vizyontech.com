using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EticaretWebCoreViewModel
{
    public class KuponViewModel : Kuponlar
    {
        public Kuponlar Kupon { get; set; } = new Kuponlar();

        public string KuponAutocomplete { get; set; }
        public int[] SeciliKuponAutocomplete { get; set; }
    }
}
