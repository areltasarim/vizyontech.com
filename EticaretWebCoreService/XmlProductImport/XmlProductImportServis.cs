using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System.Net;
using Irony.Parsing;
using Microsoft.Exchange.WebServices.Data;
using EticaretWebCoreEntity;
using EticaretWebCoreViewModel;
using EticaretWebCoreHelper;
using EticaretWebCoreEntity.Enums;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using System.Security.Policy;
using DocumentFormat.OpenXml.Bibliography;
using System.Text.RegularExpressions;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using NPOI.SS.Formula.Functions;
using System.Xml.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Ocsp;
using MathNet.Numerics;
using EticaretWebCoreService;

namespace vizyontech.com
{

    public partial class XmlProductImportServis : IXmlProductImportServis
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IProgressReporterFactory _progressReporterFactory;

        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        private readonly SeoServis _seoServis;

        private readonly string entity = "Excel Import";

        string xmlUrl = "https://perpaelektronik.com.tr/api/v2/1733699889-c7651e5d0ed3817bc339cfdb7de65d56";

        public XmlProductImportServis(AppDbContext _context, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IProgressReporterFactory progressReporterFactory, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, SeoServis _seoServis)
        {
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _progressReporterFactory = progressReporterFactory;

            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            this._seoServis = _seoServis;
        }

        public async Task<ResultViewModel> UrunEkleGuncelle(IFormCollection excelForm, string connectionId, string submit)
        {

            var result = new ResultViewModel();


            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var progressReporter = _progressReporterFactory.GetLoadingBarReporter();

                    var xmlData = await client.GetStringAsync(xmlUrl);

                    // XML verisini deserialize etme
                    XmlSerializer serializer = new XmlSerializer(typeof(Root), new XmlRootAttribute("root")
                    {
                        Namespace = "" // Namespace'i yok say
                    });

                    using (StringReader reader = new StringReader(xmlData))
                    {
                        var root = (Root)serializer.Deserialize(reader);

                        // Status değerini kontrol et
                        Console.WriteLine($"Status: {root.Status}");

                        if (root.Result != null && root.Result.Node != null)
                        {
                            var nodes = root.Result.Node; // Koleksiyon
                            int totalCount = nodes.Count(); // Eleman sayısını alıyoruz.

                            int currentCount = 0; // İlerleme durumunu takip etmek için bir sayaç.

                            foreach (var node in nodes)
                            {

                                #region Marka
                                var markaVarmi = _context.Markalar.Where(x => x.MarkaAdi == node.Marka).FirstOrDefault();
                                int markaId = 0;
                                if (markaVarmi == null)
                                {
                                    var markaEkle = new Markalar()
                                    {
                                        MarkaAdi = node.Marka,
                                        Resim = "/Content/Upload/Images/resimyok.png",
                                        Durum = SayfaDurumlari.Aktif
                                    };
                                    _context.Entry(markaEkle).State = EntityState.Added;
                                    await _context.SaveChangesAsync();

                                    markaId = markaEkle.Id;
                                }
                                else
                                {
                                    markaId = markaVarmi.Id;
                                }
                                #endregion


                                #region Kategoriler
                                int kategoriId = 1;
                                if (node.Kategoriler != null && node.Kategoriler.Any())
                                {
                                    // En üst kategori için başlangıçta null
                                    foreach (var kategori in node.Kategoriler)
                                    {
                                        // Kategori mevcut mu kontrol et
                                        var mevcutKategori = _context.KategorilerTranslate
                                            .FirstOrDefault(x => x.KategoriAdi == kategori &&
                                                                 (kategoriId == 1 || x.Kategoriler.ParentKategoriId == kategoriId));

                                        if (mevcutKategori == null)
                                        {
                                            // Yeni kategori ekle
                                            var yeniKategori = new Kategoriler()
                                            {
                                                ParentKategoriId = kategoriId, // Üst kategorinin ID'si
                                                Resim = "/Content/Upload/Images/resimyok.png",
                                                Durum = SayfaDurumlari.Aktif,
                                                Vitrin = SayfaDurumlari.Pasif,
                                                KategorilerTranslate = new List<KategorilerTranslate>(),
                                            };

                                            _context.Entry(yeniKategori).State = EntityState.Added;
                                            await _context.SaveChangesAsync(); // Yeni kategori kaydediliyor

                                            // Çeviri (Translate) kaydı ekle
                                            var yeniKategoriTranslate = new KategorilerTranslate()
                                            {
                                                KategoriId = yeniKategori.Id,
                                                KategoriAdi = kategori,
                                                DilId = 1,
                                            };

                                            _context.Entry(yeniKategoriTranslate).State = EntityState.Added;
                                            await _context.SaveChangesAsync();

                                            // SEO kaydı oluştur
                                            await _seoServis.SeoLinkOlustur(
                                                sayfaAdi: EticaretWebCoreHelper.Replace.UrlSeo(kategori),
                                                sayfaId: yeniKategori.Id,
                                                entityName: SeoUrlTipleri.Kategori,
                                                seoTipi: SeoTipleri.Kategori,
                                                dilId: 1);

                                            // Yeni eklenen kategoriyi parent olarak ayarla
                                            kategoriId = yeniKategori.Id;
                                        }
                                        else
                                        {
                                            // Mevcut kategoriyi parent olarak ayarla
                                            kategoriId = mevcutKategori.KategoriId;
                                        }
                                    }
                                }
                                #endregion


                                #region Ürünler
                                var urunVarmi = _context.Urunler.Where(x => x.UrunKodu == node.UrunKodu).FirstOrDefault();
                                int urunId = 0;
                                int stok = Convert.ToInt32(node.Stok);
                                decimal fiyat = Convert.ToDecimal(node.Fiyat);

                                if (urunVarmi == null)
                                {
                                    var urunEkle = new Urunler()
                                    {
                                        UrunKodu = node.UrunKodu,
                                        MarkaId = markaId,
                                        StokTipi = StokTipleri.Adet,
                                        Stok = stok,
                                        ListeFiyat = fiyat,
                                        Sira = 1,
                                        Durum = SayfaDurumlari.Aktif,
                                        Vitrin = SayfaDurumlari.Pasif,
                                        UrunlerTranslate = new List<UrunlerTranslate>
                                            {
                                                new UrunlerTranslate
                                                {
                                                    UrunAdi = node.Baslik,
                                                    Aciklama = node.Aciklama,
                                                    MetaBaslik = node.Baslik,
                                                    Resim = await ProcessAndSaveImage(node.Resimler.FirstOrDefault(), Path.Combine(_hostingEnvironment.WebRootPath, "Content/Upload/Images/Urunler")),
                                                    DilId = 1
                                                }
                                            },
                                        UrunToKategori = new List<UrunToKategori>
                                            {
                                                new UrunToKategori { KategoriId = kategoriId }
                                            }
                                    };

                                    _context.Urunler.Add(urunEkle);
                                    await _context.SaveChangesAsync();


                                    var urunToKategori = new UrunToKategori()
                                    {
                                        KategoriId = kategoriId,
                                        UrunId = urunEkle.Id,
                                    };
                                    _context.Entry(urunToKategori).State = EntityState.Added;
                                    await _context.SaveChangesAsync();


                                    urunId = urunEkle.Id;
                                    await _seoServis.SeoLinkOlustur(sayfaAdi: EticaretWebCoreHelper.Replace.UrlSeo(node.Baslik), sayfaId: urunId, entityName: SeoUrlTipleri.Urun, seoTipi: SeoTipleri.Urun, dilId: 1);
                                }
                                else
                                {
                                    urunVarmi.UrunKodu = node.UrunKodu;
                                    urunVarmi.ListeFiyat = fiyat;
                                    urunVarmi.Stok = stok;

                                    var urunTranslateVarmi = _context.UrunlerTranslate.FirstOrDefault(x => x.UrunId == urunVarmi.Id);
                                    if (urunTranslateVarmi != null)
                                    {
                                        urunTranslateVarmi.UrunAdi = node.Baslik;
                                        urunTranslateVarmi.Aciklama = node.Aciklama;
                                        urunTranslateVarmi.MetaBaslik = node.Baslik;
                                        urunTranslateVarmi.Resim = await ProcessAndSaveImage(node.Resimler.FirstOrDefault(), Path.Combine(_hostingEnvironment.WebRootPath, "Content/Upload/Images/Urunler"));
                                    }
                                    urunId = urunVarmi.Id;

                                    await _context.SaveChangesAsync();
                                    await _seoServis.SeoLinkOlustur(sayfaAdi: EticaretWebCoreHelper.Replace.UrlSeo(node.Baslik), sayfaId: urunVarmi.Id, entityName: SeoUrlTipleri.Urun, seoTipi: SeoTipleri.Urun, dilId: 1);
                                }

                                #endregion

                                #region Ürün Resimleri
                                var urunResimleri = new List<UrunResimleri>();
                                if (node.Resimler.Count > 1)
                                {
                                    var galeriResimleri = node.Resimler.Skip(1); 
                                    foreach (var urunresim in galeriResimleri)
                                    {
                                        string resimPath = await ProcessAndSaveImage(urunresim, Path.Combine(_hostingEnvironment.WebRootPath, "Content/Upload/Images/Urunler"));
                                        urunResimleri.Add(new UrunResimleri
                                        {
                                            Resim = resimPath,
                                            UrunResimKategori = UrunResimKategorileri.UrunResim,
                                            UrunId = urunId
                                        });
                                    }

                                    if (urunResimleri.Any())
                                    {
                                        _context.UrunResimleri.AddRange(urunResimleri);
                                        await _context.SaveChangesAsync();
                                    }
                                }

                                #endregion



                                currentCount++;
                                progressReporter.Report(1 / (double)totalCount);
                            }

                        }
                        else
                        {
                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Sonuçlar bulunamadı veya düğüm listesi boş.";
                        }
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = $"XML Deserialize Hatası: {httpEx.Message}";
                }
                catch (InvalidOperationException invOpEx)
                {
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = $"XML Deserialize Hatası: {invOpEx.Message}";
                }
                catch (Exception hata)
                {
                    result.Basarilimi = false;
                    result.MesajDurumu = "danger";
                    result.Mesaj = "Hata Oluştu." + hata.Message;
                }
            }


            return result;


        }
        private async Task<string> ProcessAndSaveImage(string url, string folderPath)
        {
            byte[] bytes = GetResimBytesFromUrl(url);
            string tmpFileName = Guid.NewGuid().ToString() + ".jpg";
            string savePath = Path.Combine(folderPath, tmpFileName);

            FileInfo serverfile = new FileInfo(savePath);
            if (!serverfile.Directory.Exists)
            {
                serverfile.Directory.Create();
            }

            ResizeandSave(bytes, savePath);
            return "/Content/Upload/Images/Urunler/" + tmpFileName; // Resim yolu
        }


        public byte[] GetResimBytesFromUrl(string Url)
        {
            byte[] result = new byte[0];
            using (var webClient = new WebClient())
            {
                result = webClient.DownloadData(Url);
            }
            return result;
        }
        public bool ResizeandSave(byte[] ImgBytes, string LocalFile)
        {
            bool result = false;
            try
            {


                SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load<Rgba32>(ImgBytes);
                image.Mutate(x => x.Resize(new ResizeOptions()
                {
                    Size = new SixLabors.ImageSharp.Size(1000, 1000),
                    Mode = ResizeMode.Pad,
                }));
                image.Mutate(imageContext =>
                {
                    var bgColor = new SixLabors.ImageSharp.PixelFormats.Rgba32(255, 255, 255);
                    imageContext.BackgroundColor(bgColor);
                    int x = (Convert.ToInt32(1000) - image.Width) / 2;
                    int y = (Convert.ToInt32(1000) - image.Height) / 2;
                    imageContext.DrawImage(image, new SixLabors.ImageSharp.Point(x, y), 1);
                });
                image.SaveAsJpeg(LocalFile);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
    }
}
