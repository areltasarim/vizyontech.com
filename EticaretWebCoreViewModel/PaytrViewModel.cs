using Azure.Core;
using EticaretWebCoreEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class PaytrViewModel : Paytr
    {
        public string IFrameSrc { get; set; }
        public bool Visible { get; set; }


        public string merchant_oid { get; set; }
        public string status { get; set; }
        public string total_amount { get; set; }
        public string hash { get; set; }



    }
}
