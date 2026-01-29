using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com
{
    public class RedirectModel
    {
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
        public string Area { get; set; } = "";
        public object Parameters { get; set; }
    }
}
