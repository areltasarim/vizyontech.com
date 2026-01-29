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
using EticaretWebCoreService;

namespace vizyontech.com
{

    public partial class ExcelImportServis : IExcelImportServis
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
        public ExcelImportServis(AppDbContext _context, SeoServis _seoServis, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IProgressReporterFactory progressReporterFactory, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
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


            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            var result = new ResultViewModel();

            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
             .AddJsonFile($"appsettings.Development.json", optional: true).AddEnvironmentVariables()
             .Build();

            var provider = configuration["Application:DatabaseProvider"];
            string baglanti = "";
            if (provider == "SqlServer")
            {
                baglanti = configuration.GetConnectionString("SqlServer");
            }
            else
            {
                baglanti = configuration.GetConnectionString("Mysql");
            }

            try
            {

                using (var connection = new MySqlConnection(baglanti))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            List<string> urunlistesi = new List<string>();

                            var uye = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                            var progressReporter = _progressReporterFactory.GetLoadingBarReporter();

                            IFormFile file = _httpContextAccessor.HttpContext.Request.Form.Files[0];

                            string folderName = "UploadExcel";
                            string webRootPath = _hostingEnvironment.WebRootPath;
                            string newPath = Path.Combine(webRootPath, folderName);
                            StringBuilder sb = new StringBuilder();
                            if (!Directory.Exists(newPath))
                            {
                                Directory.CreateDirectory(newPath);
                            }
                            if (file.Length > 0)
                            {

                                var link = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Content/Upload/Images/Urunler/";

                                string sFileExtension = Path.GetExtension(file.FileName).ToLower();

                                ClosedXML.Excel.IXLWorksheet WorkSheet;
                                ClosedXML.Excel.XLWorkbook XLWorkbook;
                                ClosedXML.Excel.IXLRow Baslik;
                                string fullPath = Path.Combine(newPath, file.FileName);


                                //Ürün Adı = 1
                                //Ürün Kodu = 2
                                //Fiyat = 3
                                //Stok = 4
                                //Açıklama = 5
                                //Ürün Ana Resim = 6
                                //Ürün Ek Resimler = 7
                                //Ürün Seçenek = 8
                                //Ürün Değer = 9
                                //Kategori = 10
                                //Alt Kategori = 11
                                //Kategori Resmi = 12
                                //Marka = 13
                                //Marka Resmi = 14
                                //Ürün Meta Başlık = 15
                                //Ürün Meta Açıklama = 16
                                //Ürün Meta Anahtar Kelime = 17


                                //new FileStream(fullPath, FileMode.Create) fullPath yerine xlsFilePath kullanıyoruz
                                //convert işlemi yapılmayacaksa direk fullPath değişkenini kullan
                                var xlsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcel", "Yeni.xls");

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                    stream.Position = 0;
                                    XLWorkbook = new ClosedXML.Excel.XLWorkbook(stream);
                                    WorkSheet = XLWorkbook.Worksheet(1);
                                    Baslik = WorkSheet.Row(1);
                                    var Satirlar = WorkSheet.RangeUsed().RowsUsed().Skip(1).ToArray();


                                    foreach (var item in Satirlar)
                                    {

                                        //urunlistesi.Add(item.Cell(2).CachedValue.ToString());

                                        var kategoriVarmi = _context.KategorilerTranslate.Where(x => x.KategoriAdi == item.Cell(10).CachedValue.ToString()).FirstOrDefault();
                                        int kategoriId = 0;

                                        string kategoriResim = "/Content/Upload/Images/resimyok.png";

                                        if (!string.IsNullOrEmpty(item.Cell(12).CachedValue.ToString()))
                                        {
                                            kategoriResim = "/Content/Upload/Images/Kategoriler/" + item.Cell(12).CachedValue.ToString();
                                        }

                                        if (kategoriVarmi == null)
                                        {
                                            #region Üst Kategori
                                            var kategoriEkle = new Kategoriler()
                                            {
                                                ParentKategoriId = 1,
                                                Resim = kategoriResim,
                                                Durum = SayfaDurumlari.Aktif,
                                                Vitrin = SayfaDurumlari.Pasif,
                                                KategorilerTranslate = new List<KategorilerTranslate>(),
                                            };
                                            _context.Entry(kategoriEkle).State = EntityState.Added;
                                            await _context.SaveChangesAsync();

                                            kategoriId = kategoriEkle.Id;

                                            var kategoriEkleTranslate = new KategorilerTranslate()
                                            {
                                                KategoriId = kategoriEkle.Id,
                                                KategoriAdi = item.Cell(10).CachedValue.ToString(),
                                                DilId = 1,
                                            };
                                            _context.Entry(kategoriEkleTranslate).State = EntityState.Added;
                                            _context.SaveChanges();


                                            await _seoServis.SeoLinkOlustur(sayfaAdi: Replace.UrlSeo(item.Cell(10).CachedValue.ToString()), sayfaId: kategoriId, entityName: SeoUrlTipleri.Kategori, seoTipi: SeoTipleri.Kategori, dilId: 1);

                                            #endregion Üst Kategori



                                        }
                                        else
                                        {
                                            kategoriId = kategoriVarmi.KategoriId;
                                        }

                                        #region Alt Kategori
                                        if (!string.IsNullOrEmpty(item.Cell(11).CachedValue.ToString()))
                                        {
                                            var altkategoriVarmi = _context.KategorilerTranslate.Where(x => x.Kategoriler.ParentKategoriId != 1 && x.KategoriAdi == item.Cell(11).CachedValue.ToString()).FirstOrDefault();

                                            if (altkategoriVarmi == null)
                                            {
                                                var altkategoriEkle = new Kategoriler()
                                                {
                                                    ParentKategoriId = kategoriId,
                                                    Resim = kategoriResim,
                                                    Durum = SayfaDurumlari.Aktif,
                                                    Vitrin = SayfaDurumlari.Pasif,
                                                    KategorilerTranslate = new List<KategorilerTranslate>(),
                                                };
                                                _context.Entry(altkategoriEkle).State = EntityState.Added;
                                                await _context.SaveChangesAsync();

                                                kategoriId = altkategoriEkle.Id;



                                                var altkategoriEkleTranslate = new KategorilerTranslate()
                                                {
                                                    KategoriId = altkategoriEkle.Id,
                                                    KategoriAdi = item.Cell(11).CachedValue.ToString(),
                                                    DilId = 1,
                                                };
                                                _context.Entry(altkategoriEkleTranslate).State = EntityState.Added;
                                                _context.SaveChanges();

                                                await _seoServis.SeoLinkOlustur(sayfaAdi: Replace.UrlSeo(item.Cell(11).CachedValue.ToString()), sayfaId: kategoriId, entityName: SeoUrlTipleri.Kategori, seoTipi: SeoTipleri.Kategori, dilId: 1);

                                            }
                                            else
                                            {
                                                kategoriId = altkategoriVarmi.KategoriId;
                                            }

                                        }
                                        #endregion Alt Kategori

                                        var markaVarmi = _context.Markalar.Where(x => x.MarkaAdi == item.Cell(13).CachedValue.ToString()).FirstOrDefault();
                                        int markaId = 0;

                                        string markaResim = "/Content/Upload/Images/resimyok.png";

                                        if (!string.IsNullOrEmpty(item.Cell(14).CachedValue.ToString()))
                                        {
                                            markaResim = "/Content/Upload/Images/Markalar/" + item.Cell(14).CachedValue.ToString();
                                        }

                                        if (markaVarmi == null)
                                        {
                                            var markaEkle = new Markalar()
                                            {
                                                MarkaAdi = item.Cell(13).CachedValue.ToString(),
                                                Resim = markaResim,
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


                                        //var url = item.Cell(8).CachedValue.ToString();
                                        //byte[] bytes = GetResimBytesFromUrl(url);
                                        //string tmpFileName = Guid.NewGuid().ToString() + ".jpg";
                                        //string savefile = Path.Combine(_hostingEnvironment.WebRootPath, "Content/Upload/Images/Urunler/", tmpFileName);
                                        //FileInfo serverfile = new FileInfo(savefile);
                                        //if (!serverfile.Directory.Exists)
                                        //{
                                        //    serverfile.Directory.Create();
                                        //}
                                        //ResizeandSave(bytes, savefile);





                                        var urunVarmi = _context.Urunler.Where(x => x.UrunKodu == item.Cell(2).CachedValue.ToString()).FirstOrDefault();
                                        int urunId = 0;
                                        int stok = Convert.ToInt32(item.Cell(4).CachedValue);
                                        decimal fiyat = Convert.ToDecimal(item.Cell(3).CachedValue.ToString().Replace(".", ","));
                                        if (urunVarmi == null)
                                        {
                                            var urunEkle = new Urunler()
                                            {
                                                UrunKodu = item.Cell(2).CachedValue.ToString(),
                                                MarkaId = markaId,
                                                StokTipi = StokTipleri.Adet,
                                                Stok = stok,
                                                ListeFiyat = fiyat,
                                                Sira = 1,
                                                Durum = SayfaDurumlari.Aktif,
                                                Vitrin = SayfaDurumlari.Pasif,
                                                UrunlerTranslate = new List<UrunlerTranslate>(),
                                                UrunToKategori = new List<UrunToKategori>(),
                                            };
                                            _context.Entry(urunEkle).State = EntityState.Added;
                                            await _context.SaveChangesAsync();
                                            urunId = urunEkle.Id;

                                            //Meta Başlık = 10
                                            //Meta Açıklama = 11
                                            //Meta Anahtar Kelime = 12

                                            string urunResim = "/Content/Upload/Images/resimyok.png";

                                            if (!string.IsNullOrEmpty(item.Cell(6).CachedValue.ToString()))
                                            {
                                                urunResim = "/Content/Upload/Images/Urunler/" + item.Cell(6).CachedValue.ToString();
                                            }
                                            var urunEkleTranslate = new UrunlerTranslate()
                                            {
                                                UrunId = urunEkle.Id,
                                                UrunAdi = item.Cell(1).CachedValue.ToString(),
                                                //KisaAciklama = item.Cell(3).CachedValue.ToString(),
                                                Aciklama = item.Cell(5).CachedValue.ToString(),
                                                MetaBaslik = item.Cell(15).CachedValue.ToString(),
                                                MetaAciklama = item.Cell(16).CachedValue.ToString(),
                                                MetaAnahtar = item.Cell(17).CachedValue.ToString(),
                                                //Resim = "/Content/Upload/Images/Urunler/" + tmpFileName,
                                                Resim = urunResim,
                                                DilId = 1,
                                            };

                                            _context.Entry(urunEkleTranslate).State = EntityState.Added;
                                            await _context.SaveChangesAsync();

                                            var urunToKategori = new UrunToKategori()
                                            {
                                                KategoriId = kategoriId,
                                                UrunId = urunId,
                                            };
                                            _context.Entry(urunToKategori).State = EntityState.Added;
                                            await _context.SaveChangesAsync();

                                            await _seoServis.SeoLinkOlustur(sayfaAdi: Replace.UrlSeo(item.Cell(1).CachedValue.ToString()), sayfaId: urunEkle.Id, entityName: SeoUrlTipleri.Urun, seoTipi: SeoTipleri.Urun, dilId: 1);

                                            #region Ürün Seçenekleri
                                            var urunSecenekVarmi = _context.UrunSecenekleriTranslate.Where(x => x.SecenekAdi == item.Cell(8).CachedValue.ToString()).FirstOrDefault();
                                            int urunSecenekId = 0;
                                            int urunToUrunSecenekId = 0;
                                            if (urunSecenekVarmi == null)
                                            {
                                                var urunSecenek = new UrunSecenekleri()
                                                {
                                                    SecenekTipi = UrunSecenekTipleri.Secenek,
                                                    Sira = 1,
                                                };
                                                _context.Entry(urunSecenek).State = EntityState.Added;
                                                await _context.SaveChangesAsync();
                                                urunSecenekId = urunSecenek.Id;

                                                var urunSecenekTranslate = new UrunSecenekleriTranslate()
                                                {
                                                    SecenekAdi = item.Cell(8).CachedValue.ToString(),
                                                    UrunSecenekId = urunSecenekId,
                                                    DilId = 1
                                                };
                                                _context.Entry(urunSecenekTranslate).State = EntityState.Added;
                                                await _context.SaveChangesAsync();


                                                var urunToUrunSecenek = new UrunToUrunSecenek()
                                                {
                                                    UrunSecenekId = urunSecenekId,
                                                    UrunId = urunId
                                                };
                                                _context.Entry(urunToUrunSecenek).State = EntityState.Added;
                                                await _context.SaveChangesAsync();
                                                urunToUrunSecenekId = urunToUrunSecenek.Id;

                                            }
                                            else
                                            {
                                                urunSecenekId = urunSecenekVarmi.Id;
                                            }
                                            #endregion

                                            #region Ürün Seçenek Değerleri
                                            var secenekDegerleri = item.Cell(9).CachedValue.ToString().Split(',');

                                            foreach (var deger in secenekDegerleri)
                                            {
                                                var trimmedDeger = deger.Trim();
                                                var urunSecenekDegerVarmi = _context.UrunSecenekDegerleriTranslate.Where(x => x.DegerAdi == trimmedDeger).FirstOrDefault();
                                                int urunSecenekDegerId = 0;

                                                if (urunSecenekDegerVarmi == null)
                                                {
                                                    var urunSecenekDeger = new UrunSecenekDegerleri()
                                                    {
                                                        UrunSecenekId = urunSecenekId,
                                                        Sira = 1,
                                                    };
                                                    _context.Entry(urunSecenekDeger).State = EntityState.Added;
                                                    await _context.SaveChangesAsync();
                                                    urunSecenekDegerId = urunSecenekDeger.Id;

                                                    var urunSecenekDegerTranslate = new UrunSecenekDegerleriTranslate()
                                                    {
                                                        DegerAdi = trimmedDeger,
                                                        UrunSecenekDegerId = urunSecenekDegerId,
                                                        DilId = 1
                                                    };
                                                    _context.Entry(urunSecenekDegerTranslate).State = EntityState.Added;
                                                    await _context.SaveChangesAsync();

                                                    var urunToUrunSecenekToUrunDeger = new UrunToUrunSecenekToUrunDeger()
                                                    {
                                                        UrunSecenekId = urunSecenekId,
                                                        UrunSecenekDegerId = urunSecenekDegerId,
                                                        UrunToUrunSecenekId = urunToUrunSecenekId,
                                                        UrunId = urunId
                                                    };
                                                    _context.Entry(urunToUrunSecenekToUrunDeger).State = EntityState.Added;
                                                    await _context.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    urunSecenekDegerId = urunSecenekDegerVarmi.Id;
                                                }
                                            }
                                            #endregion

                                        }
                                        else
                                        {
                                            urunId = urunVarmi.Id;
                                            urunVarmi.UrunKodu = item.Cell(2).CachedValue.ToString();
                                            urunVarmi.ListeFiyat = fiyat;
                                            urunVarmi.Stok = stok;
                                            _context.Entry(urunVarmi).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();


                                            var urunTranslateVarmi = _context.UrunlerTranslate.Where(x => x.UrunId == urunVarmi.Id).FirstOrDefault();
                                            urunTranslateVarmi.UrunAdi = item.Cell(1).CachedValue.ToString();
                                            urunTranslateVarmi.Aciklama = item.Cell(5).CachedValue.ToString();
                                            urunTranslateVarmi.Resim = item.Cell(6).CachedValue.ToString();
                                            urunTranslateVarmi.MetaBaslik = item.Cell(15).CachedValue.ToString();
                                            urunTranslateVarmi.MetaAnahtar = item.Cell(16).CachedValue.ToString();
                                            urunTranslateVarmi.MetaAciklama = item.Cell(17).CachedValue.ToString();
                                            _context.Entry(urunTranslateVarmi).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();

                                            await _seoServis.SeoLinkOlustur(sayfaAdi: Replace.UrlSeo(item.Cell(1).CachedValue.ToString()), sayfaId: urunId, entityName: SeoUrlTipleri.Urun, seoTipi: SeoTipleri.Urun, dilId: 1);





                                            #region Ürün Seçenekleri
                                            
                                            _context.UrunToUrunSecenek.Where(p => p.UrunId == urunId).ToList().ForEach(p => _context.UrunToUrunSecenek.Remove(p));

                                            var urunSecenekVarmi = _context.UrunSecenekleriTranslate.Where(x => x.SecenekAdi == item.Cell(8).CachedValue.ToString()).FirstOrDefault();
                                            int urunSecenekId = 0;
                                            int urunToUrunSecenekId = 0;
                                            if (urunSecenekVarmi == null)
                                            {
                                                var urunSecenek = new UrunSecenekleri()
                                                {
                                                    SecenekTipi = UrunSecenekTipleri.Secenek,
                                                    Sira = 1,
                                                };
                                                _context.Entry(urunSecenek).State = EntityState.Added;
                                                await _context.SaveChangesAsync();
                                                urunSecenekId = urunSecenek.Id;

                                                var urunSecenekTranslate = new UrunSecenekleriTranslate()
                                                {
                                                    SecenekAdi = item.Cell(8).CachedValue.ToString(),
                                                    UrunSecenekId = urunSecenekId,
                                                    DilId = 1
                                                };
                                                _context.Entry(urunSecenekTranslate).State = EntityState.Added;
                                                await _context.SaveChangesAsync();


                                                var urunToUrunSecenek = new UrunToUrunSecenek()
                                                {
                                                    UrunSecenekId = urunSecenekId,
                                                    UrunId = urunId
                                                };
                                                _context.Entry(urunToUrunSecenek).State = EntityState.Added;
                                                await _context.SaveChangesAsync();
                                                urunToUrunSecenekId = urunToUrunSecenek.Id;

                                            }
                                            else
                                            {
                                                urunSecenekId = urunSecenekVarmi.Id;
                                            }
                                            #endregion

                                            #region Ürün Seçenek Değerleri

                                            _context.UrunToUrunSecenekToUrunDeger.Where(p => p.UrunId == urunId).ToList().ForEach(p => _context.UrunToUrunSecenekToUrunDeger.Remove(p));

                                            var secenekDegerleri = item.Cell(9).CachedValue.ToString().Split(',');
                                            foreach (var deger in secenekDegerleri)
                                            {
                                                var trimmedDeger = deger.Trim();
                                                var urunSecenekDegerVarmi = _context.UrunSecenekDegerleriTranslate.Where(x => x.DegerAdi == trimmedDeger).FirstOrDefault();
                                                int urunSecenekDegerId = 0;

                                                if (urunSecenekDegerVarmi == null)
                                                {
                                                    var urunSecenekDeger = new UrunSecenekDegerleri()
                                                    {
                                                        UrunSecenekId = urunSecenekId,
                                                        Sira = 1,
                                                    };
                                                    _context.Entry(urunSecenekDeger).State = EntityState.Added;
                                                    await _context.SaveChangesAsync();
                                                    urunSecenekDegerId = urunSecenekDeger.Id;

                                                    var urunSecenekDegerTranslate = new UrunSecenekDegerleriTranslate()
                                                    {
                                                        DegerAdi = trimmedDeger,
                                                        UrunSecenekDegerId = urunSecenekDegerId,
                                                        DilId = 1
                                                    };
                                                    _context.Entry(urunSecenekDegerTranslate).State = EntityState.Added;
                                                    await _context.SaveChangesAsync();

                                                    var urunToUrunSecenekToUrunDeger = new UrunToUrunSecenekToUrunDeger()
                                                    {
                                                        UrunSecenekId = urunSecenekId,
                                                        UrunSecenekDegerId = urunSecenekDegerId,
                                                        UrunToUrunSecenekId = urunToUrunSecenekId,
                                                        UrunId = urunId
                                                    };
                                                    _context.Entry(urunToUrunSecenekToUrunDeger).State = EntityState.Added;
                                                    await _context.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    urunSecenekDegerId = urunSecenekDegerVarmi.Id;
                                                }
                                            }
                                            #endregion


                                        }


                                        progressReporter.Report(1 / (double)Satirlar.Length);

                                    }

                                }

                            }


                            result.Basarilimi = true;
                            result.MesajDurumu = "success";
                            result.Mesaj = "Excel Başarıyla Yüklendi";

                            //var db = new AppDbContext();
                            //db.Urunler.Where(p => !urunlistesi.Contains(p.UrunKodu)).ToList().ForEach(p => db.Urunler.Remove(p));
                            //db.SaveChanges();

                            transaction.Commit();
                        }
                        catch (Exception hata)
                        {
                            transaction.Rollback();

                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Hata Oluştu." + hata.Message;
                            // Hata durumunda gerekli işlemler yapılır
                        }
                    }
                }


                //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                //{

                //    var uye = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                //    var progressReporter = _progressReporterFactory.GetLoadingBarReporter();

                //    IFormFile file = _httpContextAccessor.HttpContext.Request.Form.Files[0];

                //    string folderName = "UploadExcel";
                //    string webRootPath = _hostingEnvironment.WebRootPath;
                //    string newPath = Path.Combine(webRootPath, folderName);
                //    StringBuilder sb = new StringBuilder();
                //    if (!Directory.Exists(newPath))
                //    {
                //        Directory.CreateDirectory(newPath);
                //    }
                //    if (file.Length > 0)
                //    {

                //        var link = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/Content/Upload/Images/Urunler/";

                //        string sFileExtension = Path.GetExtension(file.FileName).ToLower();

                //        ClosedXML.Excel.IXLWorksheet WorkSheet;
                //        ClosedXML.Excel.XLWorkbook XLWorkbook;
                //        ClosedXML.Excel.IXLRow Baslik;
                //        string fullPath = Path.Combine(newPath, file.FileName);
                //        using (var stream = new FileStream(fullPath, FileMode.Create))
                //        {
                //            file.CopyTo(stream);
                //            stream.Position = 0;
                //            XLWorkbook = new ClosedXML.Excel.XLWorkbook(stream);
                //            WorkSheet = XLWorkbook.Worksheet(1);
                //            Baslik = WorkSheet.Row(1);
                //            var Satirlar = WorkSheet.RangeUsed().RowsUsed().Skip(1).ToArray();


                //            foreach (var item in Satirlar)
                //            {

                //                var kategoriVarmi = _context.KategorilerTranslate.Where(x => x.KategoriAdi == item.Cell(5).CachedValue.ToString()).FirstOrDefault();
                //                int kategoriId = 0;

                //                if (kategoriVarmi == null)
                //                {
                //                    var kategoriEkle = new Kategoriler()
                //                    {
                //                        ParentKategoriId = 1,
                //                        Durum = SayfaDurumlari.Aktif,
                //                        KategorilerTranslate = new List<KategorilerTranslate>(),
                //                    };
                //                    _context.Entry(kategoriEkle).State = EntityState.Added;
                //                    _context.SaveChanges();

                //                    kategoriId = kategoriEkle.Id;

                //                    var kategoriEkleTranslate = new KategorilerTranslate()
                //                    {
                //                        KategoriId = kategoriEkle.Id,
                //                        KategoriAdi = item.Cell(5).CachedValue.ToString(),
                //                        DilId = 1,
                //                    };
                //                    kategoriEkle.KategorilerTranslate.Add(kategoriEkleTranslate);
                //                    _context.SaveChanges();
                //                }
                //                else
                //                {
                //                    kategoriId = kategoriVarmi.KategoriId;
                //                }

                //                var markaVarmi = _context.Markalar.Where(x => x.MarkaAdi == item.Cell(6).CachedValue.ToString()).FirstOrDefault();
                //                int markaId = 0;
                //                if (markaVarmi == null)
                //                {
                //                    var markaEkle = new Markalar()
                //                    {
                //                        MarkaAdi = item.Cell(6).CachedValue.ToString(),
                //                        Durum = SayfaDurumlari.Aktif
                //                    };
                //                    _context.Entry(markaEkle).State = EntityState.Added;
                //                    _context.SaveChanges();

                //                    markaId = markaEkle.Id;
                //                }
                //                else
                //                {
                //                    markaId = markaVarmi.Id;
                //                }


                //                //var url = item.Cell(8).CachedValue.ToString();
                //                //byte[] bytes = GetResimBytesFromUrl(url);
                //                //string tmpFileName = Guid.NewGuid().ToString() + ".jpg";
                //                //string savefile = Path.Combine(_hostingEnvironment.WebRootPath, "Content/Upload/Images/Urunler/", tmpFileName);
                //                //FileInfo serverfile = new FileInfo(savefile);
                //                //if (!serverfile.Directory.Exists)
                //                //{
                //                //    serverfile.Directory.Create();
                //                //}
                //                //ResizeandSave(bytes, savefile);





                //                var urunVarmi = _context.Urunler.Where(x => x.UrunKodu == item.Cell(1).CachedValue.ToString()).FirstOrDefault();
                //                int urunId = 0;

                //                decimal fiyat = Convert.ToDecimal(item.Cell(2).CachedValue.ToString().Replace(".", ","));
                //                if (urunVarmi == null)
                //                {
                //                    var urunEkle = new Urunler()
                //                    {
                //                        UrunKodu = item.Cell(1).CachedValue.ToString(),
                //                        MarkaId = markaId,
                //                        StokTipi = StokTipleri.Adet,
                //                        Stok = 1000,
                //                        Fiyat = fiyat,
                //                        SariEtiketliUrun = true,
                //                        Sira = 1,
                //                        Durum = SayfaDurumlari.Aktif,
                //                        Vitrin = SayfaDurumlari.Aktif,
                //                        UrunlerTranslate = new List<UrunlerTranslate>(),
                //                        UrunToKategori = new List<UrunToKategori>(),
                //                    };
                //                    _context.Entry(urunEkle).State = EntityState.Added;
                //                    _context.SaveChanges();
                //                    urunId = urunEkle.Id;

                //                    var urunEkleTranslate = new UrunlerTranslate()
                //                    {
                //                        UrunId = urunEkle.Id,
                //                        UrunAdi = item.Cell(7).CachedValue.ToString(),
                //                        Aciklama = item.Cell(3).CachedValue.ToString(),
                //                        //Resim = "/Content/Upload/Images/Urunler/" + tmpFileName,
                //                        Resim = "/Content/Upload/Images/Urunler/resimyok.png",
                //                        DilId = 1,
                //                    };
                //                    urunEkle.UrunlerTranslate.Add(urunEkleTranslate);
                //                    await _context.SaveChangesAsync();

                //                    var urunToKategori = new UrunToKategori()
                //                    {
                //                        KategoriId = kategoriId,
                //                        UrunId = urunId,
                //                    };
                //                    _context.Entry(urunToKategori).State = EntityState.Added;
                //                    await _context.SaveChangesAsync();

                //                    var seoUrl = new SeoUrl()
                //                    {
                //                        Url = Replace.UrlSeo(item.Cell(7).CachedValue.ToString()),
                //                        EntityName = SeoUrlTipleri.Urun,
                //                        SeoTipi = SeoTipleri.Urun,
                //                        EntityId = urunEkle.Id,
                //                        DilId = 1,
                //                    };

                //                    _context.Entry(seoUrl).State = EntityState.Added;
                //                    await _context.SaveChangesAsync();
                //                }
                //                else
                //                {
                //                    urunId = urunVarmi.Id;
                //                    urunVarmi.Fiyat = fiyat;
                //                    _context.Entry(urunVarmi).State = EntityState.Modified;
                //                    await _context.SaveChangesAsync();

                //                }


                //                progressReporter.Report(1 / (double)Satirlar.Length);

                //            }

                //        }

                //    }


                //    result.Basarilimi = true;
                //    result.MesajDurumu = "success";
                //    result.Mesaj = "Excel Başarıyla Yüklendi";


                //    transaction.Complete();

                //}
            }
            catch (Exception hata)
            {

                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu." + hata.Message;

            }

            return result;


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
