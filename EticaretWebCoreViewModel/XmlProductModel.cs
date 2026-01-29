using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EticaretWebCoreViewModel
{
    public class Root
    {
        [XmlElement("status")]
        public int Status { get; set; }

        [XmlElement("result")]
        public Result Result { get; set; }

        [XmlElement("error")]
        public string Error { get; set; }
    }

    public class Result
    {
        [XmlElement("node")]
        public List<Node> Node { get; set; }
    }

    public class Node
    {
        [XmlElement("urun_id")]
        public int UrunId { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("baslik")]
        public string Baslik { get; set; }

        [XmlElement("durum")]
        public int Durum { get; set; }

        [XmlElement("vergi")]
        public decimal Vergi { get; set; }

        [XmlElement("desi")]
        public decimal Desi { get; set; }

        [XmlElement("aciklama")]
        public string Aciklama { get; set; }

        [XmlElement("urun_kodu")]
        public string UrunKodu { get; set; }

        [XmlElement("entegrasyon_kodu")]
        public string EntegrasyonKodu { get; set; }

        [XmlElement("barkod")]
        public string Barkod { get; set; }

        [XmlElement("marka")]
        public string Marka { get; set; }

        [XmlElement("stok")]
        public int Stok { get; set; }

        [XmlElement("indirim")]
        public decimal Indirim { get; set; }

        [XmlElement("para_birimi")]
        public string ParaBirimi { get; set; }

        [XmlElement("seo")]
        public string Seo { get; set; }

        [XmlElement("fiyat")]
        public decimal Fiyat { get; set; }

        [XmlElement("indirimli_fiyat")]
        public decimal IndirimliFiyat { get; set; }

        [XmlArray("kategoriler")]
        [XmlArrayItem("kategori")]
        public List<string> Kategoriler { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlArray("resimler")]
        [XmlArrayItem("resim")]
        public List<string> Resimler { get; set; }
    }

}
