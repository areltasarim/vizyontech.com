using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public class Replace
    {
        public static string UrlSeo(string Metin)
        {
            string deger = "";
            //if (!string.IsNullOrWhiteSpace(Metin.ToString()))
            //{
            deger = Metin;
            deger = deger.Replace("'", "");
            deger = deger.Replace("’", "");
            deger = deger.Replace(" ", "-");
            deger = deger.Replace("-", "-");
            deger = deger.Replace("<", "");
            deger = deger.Replace(">", "");
            deger = deger.Replace("&", "ve");
            deger = deger.Replace("[", "");
            deger = deger.Replace("]", "");
            deger = deger.Replace("ı", "i");
            deger = deger.Replace("ö", "o");
            deger = deger.Replace("ü", "u");
            deger = deger.Replace("ş", "s");
            deger = deger.Replace("ç", "c");
            deger = deger.Replace("ğ", "g");
            deger = deger.Replace("İ", "i");
            deger = deger.Replace("I", "i");
            deger = deger.Replace("Ö", "o");
            deger = deger.Replace("Ü", "u");
            deger = deger.Replace("Ş", "s");
            deger = deger.Replace("Ç", "c");
            deger = deger.Replace("Ğ", "g");
            deger = deger.Replace("A", "a");
            deger = deger.Replace("B", "b");
            deger = deger.Replace("C", "c");
            deger = deger.Replace("D", "d");
            deger = deger.Replace("E", "e");
            deger = deger.Replace("F", "f");
            deger = deger.Replace("G", "g");
            deger = deger.Replace("H", "h");
            deger = deger.Replace("J", "j");
            deger = deger.Replace("K", "k");
            deger = deger.Replace("L", "l");
            deger = deger.Replace("M", "m");
            deger = deger.Replace("N", "n");
            deger = deger.Replace("O", "o");
            deger = deger.Replace("P", "p");
            deger = deger.Replace("R", "r");
            deger = deger.Replace("S", "s");
            deger = deger.Replace("T", "t");
            deger = deger.Replace("U", "u");
            deger = deger.Replace("V", "v");
            deger = deger.Replace("Y", "y");
            deger = deger.Replace("Z", "z");
            deger = deger.Replace("?", "-");
            deger = deger.Replace("!", "-");
            deger = deger.Replace("*", "-");
            deger = deger.Replace("+", "-");
            deger = deger.Replace("$", "-");
            deger = deger.Replace("#", "-");
            deger = deger.Replace("=", "-");
            deger = deger.Replace(".", "-");
            deger = deger.Replace(",", "-");
            deger = deger.Replace(";", "-");
            deger = deger.Replace("~", "-");
            deger = deger.Replace(":", "-");
            deger = deger.Replace("|", "-");
            deger = deger.Replace("€", "-");
            deger = deger.Replace("%", "-");
            deger = deger.Replace("^", "-");
            deger = deger.Replace("(", "");
            deger = deger.Replace(")", "");
            deger = deger.Replace("/", "-");
            deger = deger.Replace("\"", "-");



            //}
            return deger;
        }

        public static string FriendlyUrl(UrlHelper helper, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            url = url.ToLower();
            url = url.Trim();
            if (url.Length > 100)
            {
                url = url.Substring(0, 100);
            }
            url = url.Replace("İ", "I");
            url = url.Replace("ı", "i");
            url = url.Replace("ğ", "g");
            url = url.Replace("Ğ", "G");
            url = url.Replace("ç", "c");
            url = url.Replace("Ç", "C");
            url = url.Replace("ö", "o");
            url = url.Replace("Ö", "O");
            url = url.Replace("ş", "s");
            url = url.Replace("Ş", "S");
            url = url.Replace("ü", "u");
            url = url.Replace("Ü", "U");
            url = url.Replace("'", "");
            url = url.Replace("\"", "");
            char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            for (int i = 0; i < replacerList.Length; i++)
            {
                string strChr = replacerList[i].ToString();
                if (url.Contains(strChr))
                {
                    url = url.Replace(strChr, string.Empty);
                }
            }
            Regex r = new Regex("[^a-zA-Z0-9_-]");
            url = r.Replace(url, "-");
            while (url.IndexOf("--") > -1)
            {
                url = url.Replace("--", "-");
            }

            return url;
        }

        public string FriendlyUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            url = url.ToLower();
            url = url.Trim();
            if (url.Length > 100)
            {
                url = url.Substring(0, 100);
            }
            url = url.Replace("İ", "I");
            url = url.Replace("ı", "i");
            url = url.Replace("ğ", "g");
            url = url.Replace("Ğ", "G");
            url = url.Replace("ç", "c");
            url = url.Replace("Ç", "C");
            url = url.Replace("ö", "o");
            url = url.Replace("Ö", "O");
            url = url.Replace("ş", "s");
            url = url.Replace("Ş", "S");
            url = url.Replace("ü", "u");
            url = url.Replace("Ü", "U");
            url = url.Replace("'", "");
            url = url.Replace("\"", "");
            url = url.Replace("/", "");
            char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            for (int i = 0; i < replacerList.Length; i++)
            {
                string strChr = replacerList[i].ToString();
                if (url.Contains(strChr))
                {
                    url = url.Replace(strChr, string.Empty);
                }
            }
            Regex r = new Regex("[^a-zA-Z0-9_-]");
            url = r.Replace(url, "-");
            while (url.IndexOf("--") > -1)
            {
                url = url.Replace("--", "-");
            }

            return url;
        }

    }
}
