using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EticaretWebCoreHelper
{
    public class PageMessageModel
    {

        public string Type { get; set; }
        public string Text { get; set; }

    }
}