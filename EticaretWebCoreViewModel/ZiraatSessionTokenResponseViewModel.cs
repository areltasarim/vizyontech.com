using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EticaretWebCoreViewModel
{
    public class ZiraatSessionTokenResponseViewModel
    {
        [JsonPropertyName("sessionToken")]
        public string SessionToken { get; set; }

        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("responseMsg")]
        public string ResponseMsg { get; set; }
    }
}
