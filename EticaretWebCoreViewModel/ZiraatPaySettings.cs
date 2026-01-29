using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{

    public class ZiraatPayEnvironment
    {
        public string Merchant { get; set; }
        public string MerchantUser { get; set; }
        public string MerchantPassword { get; set; }
        public string ApiUrl { get; set; }
    }
    
    public class ZiraatPaySettings
    {
        public bool IsTest { get; set; }
        public ZiraatPayEnvironment Test { get; set; }
        public ZiraatPayEnvironment Live { get; set; }

        public ZiraatPayEnvironment Active => IsTest ? Test : Live;
    }

}
