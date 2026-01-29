using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static EticaretWebCoreHelper.Replace;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]
    public class UrunlerController : Controller
    {
        private readonly UrunlerServis _urunServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Ürünler";
        private readonly string entityAltBaslik = "Ürün Ekle";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UrunlerController(AppDbContext _context, UrunlerServis _urunServis, IHttpContextAccessor httpContextAccessor)
        {
            this._context = _context;
            this._urunServis = _urunServis;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _urunServis.PageList();


            return View(model);
        }

        
        [HttpPost]
        public JsonResult UrunleriListele(IFormCollection form)
        {
            var dataTableFilter = form.ToDataTableFilter();
            var data = _context.Urunler.Include(u => u.UrunlerTranslate).ToList();

            // Arama değeri boş değilse, verilerde arama yapılır
            if (!string.IsNullOrEmpty(dataTableFilter.searchValue))
            {
                string searchValue = dataTableFilter.searchValue.ToLower(); // Arama değerini küçük harfe dönüştürüyoruz
                data = data.Where(x =>
                    (x.UrunlerTranslate
                         ?.SingleOrDefault(t => t.Diller?.DilKodlari?.DilKodu == "tr-TR")?.UrunAdi?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (x.UrunlerTranslate
                         ?.SingleOrDefault(t => t.Diller?.DilKodlari?.DilKodu == "tr-TR")?.Resim?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (x.UrunResimleri?.ToString()?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (x.UrunKodu?.ToString()?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (x.UrunToKategori?.ToString()?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (x.Sira.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0) // Sıra null olmayabilir, kontrol edilmeden bırakılabilir
                    || (x.Durum.ToString()?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (x.Vitrin.ToString()?.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            }


            // Toplam veri sayısını alın
            dataTableFilter.totalRecord = data.Count();

            // Sıralamayı uygula (sortBy ve sortDir değerlerini kullanarak sıralama yapın)
            if (!string.IsNullOrEmpty(dataTableFilter.sortColumn) && !string.IsNullOrEmpty(dataTableFilter.sortColumnDirection))
            {
                try
                {
                    data = ApplySorting(data.AsQueryable(), dataTableFilter.sortColumn, dataTableFilter.sortColumnDirection).ToList();
                }
                catch (Exception ex)
                {
                    // Sıralama hatası durumunda yapılacak işlemler
                }
            }

            // Filtrelenmiş veri sayısını alın
            dataTableFilter.filterRecord = data.Count();

            // Veriyi memory'ye al
            var dataList = data.Skip(dataTableFilter.skip).Take(dataTableFilter.pageSize).ToList();
            
            // SeoUrl'leri önceden yükle (performans için)
            var urunIds = dataList.Select(x => x.Id).ToList();
            var seoUrlList = _context.SeoUrl
                .Where(s => urunIds.Contains(s.EntityId) && s.EntityName == SeoUrlTipleri.Urun)
                .ToList();
            
            // Her ürün için SeoUrl'yi bul
            var seoUrlDict = new Dictionary<int, string>();
            foreach (var urun in dataList)
            {
                var seoUrl = seoUrlList.FirstOrDefault(s => s.EntityId == urun.Id);
                if (seoUrl != null && !string.IsNullOrEmpty(seoUrl.Url))
                {
                    seoUrlDict[urun.Id] = seoUrl.Url;
                }
            }

            var empList = dataList.Select(x =>
            {
                // SeoUrl'yi dictionary'den bul
                var seoUrl = seoUrlDict.ContainsKey(x.Id) ? seoUrlDict[x.Id] : null;
                var urlGitButonu = "";
                
                if (!string.IsNullOrEmpty(seoUrl))
                {
                    var urlPath = seoUrl.StartsWith("/") ? seoUrl : "/" + seoUrl;
                    urlGitButonu = $"<a href='{urlPath}' target='_blank' class='btn btn-sm btn-info' data-toggle='tooltip' data-placement='top' title='URL Git' data-original-title='URL Git'><i class='mdi mdi-open-in-new'></i> URL Git</a>";
                }
                else
                {
                    urlGitButonu = "<span class='text-muted'>-</span>";
                }
                
                // Ürün adını kısalt
                var urunAdi = x.UrunlerTranslate.FirstOrDefault(ut => ut.Diller.DilKodlari.DilKodu == "tr-TR")?.UrunAdi ?? "";
                var urunAdiKisaltilmis = urunAdi.Length > 50 ? urunAdi.Substring(0, 50) + "..." : urunAdi;
                
                // Stok durumuna göre badge oluştur
                string stokBadge = "";
                if (x.Stok > 10)
                {
                    stokBadge = $"<span class='badge badge-soft-success font-size-12'><i class='mdi mdi-check-circle me-1'></i>{x.Stok} Adet</span>";
                }
                else if (x.Stok > 0 && x.Stok <= 10)
                {
                    stokBadge = $"<span class='badge badge-soft-warning font-size-12'><i class='mdi mdi-alert-circle me-1'></i>{x.Stok} Adet</span>";
                }
                else if (x.Stok == 0)
                {
                    stokBadge = $"<span class='badge badge-soft-danger font-size-12'><i class='mdi mdi-close-circle me-1'></i>Tükendi</span>";
                }
                else
                {
                    stokBadge = $"<span class='badge badge-soft-secondary font-size-12'><i class='mdi mdi-help-circle me-1'></i>Stok Yok</span>";
                }
                
                return new
                {
                    Id = x.Id,
                    UrunAdi = urunAdiKisaltilmis,
                    UrunKodu = x.UrunKodu,
                    UrunToKategori = UrunKategoriListe(x.Id),
                    UrlGit = urlGitButonu,
                    Stok = stokBadge,
                    Ozelllik1 = $"<a href='/Admin/Urunler/UrunOzellikleri/{x.Id}?OzellikGrupId=1' class='btn btn-sm btn-info'>Ürün İkonları</a>",
                    Ozelllik2 = $"<a href='/Admin/Urunler/UrunOzellikleri/{x.Id}?OzellikGrupId=2' class='btn btn-sm btn-info'>Teknik Özellikleri</a>",
                    Ozelllik3 = $"<a href='/Admin/Urunler/UrunOzellikleri/{x.Id}?OzellikGrupId=3' class='btn btn-sm btn-info'>Ürün Tabları</a>",
                    Resimler = $"<a onclick='GaleriResimleri({x.Id}, {Convert.ToInt32(UrunResimKategorileri.UrunResim)})' href='javascript: void()' class='btn btn-sm btn-success'>Resimler</a>",
                    KullanimKilavuzu = $"<a onclick='GaleriResimleri({x.Id}, {Convert.ToInt32(UrunResimKategorileri.KullanimKulavuzu)})' href='javascript: void()' class='btn btn-sm btn-success'>Kullanım Kulavuzu</a>",
                    Sartname = $"<a onclick='GaleriResimleri({x.Id}, {Convert.ToInt32(UrunResimKategorileri.Sartname)})' href='javascript: void()' class='btn btn-sm btn-success'>Şartname</a>",
                    Belge = $"<a onclick='GaleriResimleri({x.Id}, {Convert.ToInt32(UrunResimKategorileri.Belge)})' href='javascript: void()' class='btn btn-sm btn-success'>Belge</a>",
                    Sira = x.Sira,
                    Durum = $"<div class='badge badge-soft-{(x.Durum == SayfaDurumlari.Aktif ? "success" : "danger")} font-size-12'>{(x.Durum == SayfaDurumlari.Aktif ? "Aktif" : "Pasif")}</div>",
                    Vitrin = $"<div class='badge badge-soft-{(x.Vitrin == SayfaDurumlari.Aktif ? "success" : "danger")} font-size-12'>{(x.Vitrin.GetDisplayName())}</div>",
                    Buttons = $"<a  href='/Admin/Urunler/AddOrUpdate/{x.Id}' class='mr-3 text-primary' data-toggle='tooltip' data-placement='top' title='Düzenle' data-original-title='Düzenle'><i class='mdi mdi-pencil font-size-18'></i></a>" +
                    $"<a  href='/Admin/Urunler/Delete/{x.Id}' data-name='{x.UrunlerTranslate.FirstOrDefault(ut => ut.Diller.DilKodlari.DilKodu == "tr-TR")?.UrunAdi}'  class='text-danger remove' data-bs-toggle='modal' data-bs-target='#staticBackdrop' data-placement='top' title='Sil' data-original-title='Sil'><i class='mdi mdi-trash-can font-size-18'></i></a>"
                };
            }).ToList();

            var returnObj = new
            {
                draw = dataTableFilter.draw,
                recordsTotal = dataTableFilter.totalRecord,
                recordsFiltered = dataTableFilter.filterRecord,
                data = empList
            };

            return Json(returnObj);
        }
        private string UrunKategoriListe(int urunId)
        {
            var kategoriListesi = new StringBuilder();
            kategoriListesi.Append("<ul>");

            var kategoriVerileri = _context.UrunToKategori
                .Where(x => x.UrunId == urunId)
                .Select(x => x.Kategoriler.KategorilerTranslate
                    .FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == "tr-TR").KategoriAdi)
                .ToList();

            foreach (var kategoriAdi in kategoriVerileri)
            {
                kategoriListesi.Append($"<li style=\"list-unstyled product-desc-list;\">{kategoriAdi}</li>");
            }

            kategoriListesi.Append("</ul>");

            return kategoriListesi.ToString();
        }
        private IQueryable<Urunler> ApplySorting(IQueryable<Urunler> data, string sortBy, string sortDir)
        {
            switch (sortBy.ToLower())
            {
                case "id":
                    return ApplyOrder(data, x => x.Id, sortDir);
                case "urunadi":
                    return ApplyOrder(data, x => x.UrunlerTranslate.FirstOrDefault(ut => ut.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi, sortDir);
                case "urunkodu":
                    return ApplyOrder(data, x => x.UrunKodu, sortDir);
                case "sira":
                    return ApplyOrder(data, x => x.Sira, sortDir);
                case "durum":
                    return ApplyOrder(data, x => x.Durum, sortDir);
                case "vitrin":
                    return ApplyOrder(data, x => x.Vitrin, sortDir);
                // Diğer sıralama kriterleri için case'ler eklenebilir
                default:
                    // Varsayılan olarak, belirtilen sütuna göre ID'ye göre sıralama yapılacak
                    return ApplyOrder(data, x => x.Id, sortDir);
            }
        }

        private IQueryable<T> ApplyOrder<T, TKey>(IQueryable<T> data, Expression<Func<T, TKey>> keySelector, string sortDir)
        {
            return sortDir.ToLower() == "asc" ? data.OrderBy(keySelector) : data.OrderByDescending(keySelector);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            PopulateDropdown();
            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                var Urun = _context.Urunler.Find(Id);
                foreach (var item in diller)
                {
                    var UrunlerTranslate = Urun.UrunlerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (UrunlerTranslate == null)
                    {
                        UrunlerTranslate = new UrunlerTranslate()
                        {
                            UrunId = Id,
                            UrunAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(UrunlerTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                UrunViewModel model = new UrunViewModel()
                {
                    Urun = _context.Urunler.Find(Id),
                };

                model.SeciliKategoriler = model.Urun.UrunToKategori.Select(p => p.KategoriId).ToArray();


                PopulateDropdown();
                return View(model);
            }
            else
            {
                PopulateDropdown();

                UrunViewModel Model = new UrunViewModel();
                Model.Urun.Durum = SayfaDurumlari.Aktif;

                foreach (var item in diller)
                {
                    var UrunlerTranslate = Model.UrunlerTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (UrunlerTranslate == null)
                    {
                        UrunlerTranslate = new UrunlerTranslate()
                        {
                            UrunAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.Urun.UrunlerTranslate.Add(UrunlerTranslate);

                    }
                }
                return View(Model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(UrunViewModel Model, string submit)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                   .Where(x => x.Value?.Errors.Count > 0)
                   .Select(x => new
                   {
                       Field = x.Key,
                       Messages = x.Value!.Errors.Select(e => e.ErrorMessage)
                   })
                   .ToList();


                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel()
                {
                    Type = "error",
                    Text = "Lütfen gerekli alanları eksiksiz doldurunuz."
                });

                PopulateDropdown();
                return View(Model); // Modeli tekrar form sayfasına gönderiyoruz
            }

            var model = await _urunServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel()
            {
                Type = model.MesajDurumu,
                Text = model.Mesaj
            });

            if (model.Basarilimi == true)
            {
                PopulateDropdown();
                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId });
            }
            else
            {
                PopulateDropdown();
                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId });
            }
        }



        public IActionResult UrunleriSirala(int Id = 0)
        {
            ViewData["Baslik"] = "Ürünleri Sırala";
            ViewData["AltBaslik"] = "Ürünleri Sırala";

            var urunler = _context.UrunlerTranslate
            .Where(x => x.DilId == 1)
            .OrderBy(x => x.Urunler.Sira)
            .ToList();

            var model = urunler.Select(item => new UrunSiralaViewModel
            {
                UrunId = item.UrunId,
                UrunKodu = item.Urunler.UrunKodu,
                UrunAdi = item.UrunAdi,
                KategoriYolu = GetCategoryPath(item.UrunId, 1),
                Sira = item.Urunler.Sira
            }).ToList();

            return View(model);
        }
        public string GetCategoryPath(int urunId, int dilId)
        {
            // Ürünün bağlı olduğu en alt kategoriyi al
            var kategori = _context.UrunToKategori
                .Where(x => x.UrunId == urunId)
                .Join(_context.KategorilerTranslate.Where(k => k.DilId == dilId),
                    utk => utk.KategoriId,
                    k => k.KategoriId,
                    (utk, k) => new { k.KategoriId, k.KategoriAdi, k.Kategoriler.ParentKategoriId })
                .FirstOrDefault();

            if (kategori == null)
                return "Kategori Bulunamadı";

            // Tüm kategori yolunu oluştur
            List<string> kategoriYolu = new List<string>();
            GetParent(kategori.KategoriId, kategoriYolu, dilId);

            kategoriYolu.Reverse(); // Root'tan başlayarak sıralamak için
            return string.Join(" > ", kategoriYolu);
        }

        private void GetParent(int kategoriId, List<string> kategoriYolu, int dilId)
        {
            var kategori = _context.KategorilerTranslate
                .Where(k => k.KategoriId == kategoriId && k.DilId == dilId)
                .Select(k => new { k.KategoriId, k.KategoriAdi, k.Kategoriler.ParentKategoriId })
                .FirstOrDefault();

            if (kategori != null)
            {
                kategoriYolu.Add(kategori.KategoriAdi);

                // Eğer root değilse (ParentKategoriId = 1 değilse), parent'ı da ekle
                if (kategori.ParentKategoriId != 1)
                {
                    GetParent(kategori.ParentKategoriId, kategoriYolu, dilId);
                }
            }
        }

        

        [HttpPost()]
        public IActionResult UpdateProductOrderByCategory([FromBody] List<UrunSiralamaModel> urunler)
        {
            ResultViewModel Sonuc = new ResultViewModel();

            if (urunler == null || urunler.Count == 0)
            {
                Sonuc.Basarilimi = false;
                Sonuc.MesajDurumu = "danger";
                Sonuc.Mesaj = "Geçerli bir sıralama verisi bulunamadı.";
            }

            // **1️⃣ Kullanıcının seçtiği kategori bilgisi JSON'dan alınıyor**
            // Kullanıcının seçtiği kategori yolunu al
            string selectedCategoryPath = urunler.FirstOrDefault()?.Category;

            if (string.IsNullOrEmpty(selectedCategoryPath))
            {
                Sonuc.Basarilimi = false;
                Sonuc.MesajDurumu = "danger";
                Sonuc.Mesaj = "Kategori bilgisi eksik.";
                return Json(Sonuc);
            }

            // Seçili kategorinin son öğesini al (örn: "Ses Sistemleri > Anfi" => "Anfi")
            string selectedCategory = selectedCategoryPath.Split(" > ").Last();

            // Veritabanındaki kategori ID'sini bul
            var selectedCategoryId = _context.KategorilerTranslate
                .Where(x => x.DilId == 1)
                .Where(k => k.KategoriAdi == selectedCategory)
                .Select(k => k.KategoriId)
                .FirstOrDefault();

            if (selectedCategoryId == 0)
            {
                Sonuc.Basarilimi = false;
                Sonuc.MesajDurumu = "danger";
                Sonuc.Mesaj = "Kategori bulunamadı.";
                return Json(Sonuc);
            }


            // **3️⃣ Seçili kategoriye ait ürünleri bul (UrunToKategori ile)**
            var categoryProductIds = _context.UrunToKategori
                .Where(utk => utk.KategoriId == selectedCategoryId)
                .Select(utk => utk.UrunId)
                .ToList();

            // **4️⃣ Bu ürünlerin sırasını güncelle**
            var categoryProducts = _context.Urunler
                .Where(u => categoryProductIds.Contains(u.Id))
                .OrderBy(u => u.Sira)
                .ToList();

            foreach (var urun in urunler)
            {
                var existingUrun = categoryProducts.FirstOrDefault(x => x.Id == urun.Id);
                if (existingUrun != null)
                {
                    existingUrun.Sira = urun.NewPosition;
                }
            }

            _context.SaveChanges();

            Sonuc.Basarilimi = true;
            Sonuc.Display = "block";
            Sonuc.MesajDurumu = "success";
            Sonuc.Mesaj = "Ürün sıralaması güncellendi!";

            return Json(Sonuc);
        }


     

        public IActionResult UrunSecenekAutoComplete(string searchTerm)
        {
            var result = _context.UrunSecenekleri.ToList().Where(x => x.UrunSecenekleriTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).SecenekAdi.ToLower().Contains(searchTerm.ToLower())).Select(x => new { value = x.UrunSecenekleriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").SecenekAdi, id = x.Id.ToString(), secenektipi = x.SecenekTipi });

            return Json(result);
        }

        public PartialViewResult UrunSecenekGetPartialView(UrunSecenekTipleri secenekTipi, int SecenekId)
        {
            List<UrunToUrunSecenekToUrunDeger> model = new List<UrunToUrunSecenekToUrunDeger>();
            if (secenekTipi == UrunSecenekTipleri.Secenek)
            {
                var UrunSecenek = _context.UrunSecenekleri.Find(SecenekId);
                if (UrunSecenek != null)
                {
                    var UrunSecenekDegerListesi = _context.UrunSecenekDegerleri.Where(x => x.UrunSecenekId == UrunSecenek.Id);
                    List<int> UrunSecenekDegerIdListesi = UrunSecenekDegerListesi.Select(x => x.Id).ToList(); ;
                    model = _context.UrunToUrunSecenekToUrunDeger.Where(x => UrunSecenekDegerIdListesi.Contains(x.UrunSecenekDegerId)).ToList();
                }
                List<UrunSecenek> UrunSecenekListesi = UrunSecenekListesiGetir();
                foreach (var UrunSecenekDeger in UrunSecenekListesi)
                {
                    int SenecekDegerId = Convert.ToInt32(UrunSecenekDeger.Secenek);
                    var TmpUrunSecenekTranslate = _context.UrunSecenekDegerleriTranslate.FirstOrDefault(x => x.Id == SenecekDegerId);
                    var TmpUrunSecenekDeger = _context.UrunSecenekDegerleri.FirstOrDefault(x => x.UrunSecenekId == TmpUrunSecenekTranslate.UrunSecenekDegerleri.UrunSecenekId);
                    model.Add(new UrunToUrunSecenekToUrunDeger()
                    {
                        Fiyat = 0,
                        Adet = Convert.ToInt32(UrunSecenekDeger.Value),
                        UrunSecenekDegerleri = TmpUrunSecenekDeger,
                    });
                }
                return PartialView("~/Areas/Admin/Views/Urunler/UrunSecenekleriPartial/_UrunSecenekSelectPartialView.cshtml", model);
            }
            else if (secenekTipi == UrunSecenekTipleri.Textbox)
            {
                return PartialView("~/Areas/Admin/Views/Urunler/UrunSecenekleriPartial/_UrunSecenekTextboxPartialView.cshtml", SecenekId);
            }
            else if (secenekTipi == UrunSecenekTipleri.Radio)
            {
                return PartialView("~/Areas/Admin/Views/Urunler/UrunSecenekleriPartial/_UrunSecenekRadioPartialView.cshtml", SecenekId);
            }
            else
            {
                return PartialView("Partial view not found");
            }

        }

        public IActionResult _UrunSecenekEkle(int SecenekId)
        {
            ViewData["UrunSecenekDegerleri"] = _context.UrunSecenekDegerleriTranslate.Where(x => x.Diller.DilKodlari.DilKodu == "tr-TR" && x.UrunSecenekDegerleri.UrunSecenekId == SecenekId).ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DegerAdi, Value = p.Id.ToString() });
            return PartialView(SecenekId);
        }


        [HttpPost]
        public IActionResult OzellikKaydet(IFormCollection Frm)
        {
            EticaretWebCoreService.ProcessResult<UrunSecenek> result = new ProcessResult<UrunSecenek>();
            try
            {
                var Secenek = Frm["Secenek"];
                var UrunSecenekTipleri = (UrunSecenekTipleri)Convert.ToInt32(Frm["UrunSecenekTipleri"]);
                var Value = Frm["Adet"];
                if (UrunSecenekTipleri != null && !string.IsNullOrEmpty(Secenek) && !string.IsNullOrEmpty(Value))
                {
                    List<UrunSecenek> UrunSecenekListesi = UrunSecenekListesiGetir();
                    var Nesne = new UrunSecenek()
                    {
                        SecenekTip = UrunSecenekTipleri,
                        Secenek = Secenek,
                        Value = Value
                    };
                    UrunSecenekListesi.Add(Nesne);
                    UrunSecenekListesiSessionKaydet(UrunSecenekListesi);
                    // Sessiona Atılacak İşlemler
                    result.Durum = true;
                    result.Mesaj = "ok";
                    result.Result = Nesne;
                }
                else
                {
                    // Sessiona Atılacak İşlemler

                    result.Durum = false;
                    result.Mesaj = "eksik parametre";
                    result.Result = new UrunSecenek()
                    {
                    };
                }
            }
            catch (Exception Hata)
            {
                result.Durum = false;
                result.Hata = Hata;
                result.Mesaj = "fail";
                result.Result = new UrunSecenek()
                {
                };
            }
            return Json(result);
        }

        public List<UrunSecenek> UrunSecenekListesiGetir()
        {
            List<UrunSecenek> UrunSecenekListesi = new List<UrunSecenek>();
            var UrunSecenekListesiTxt = _httpContextAccessor.HttpContext.Session.GetString("UrunSecenekListesi");
            if (!string.IsNullOrEmpty(UrunSecenekListesiTxt))
            {
                UrunSecenekListesi = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UrunSecenek>>(UrunSecenekListesiTxt);
            }
            if (UrunSecenekListesi == null)
            {
                UrunSecenekListesi = new List<UrunSecenek>();
            }
            return UrunSecenekListesi;
        }

        private void UrunSecenekListesiTemizle()
        {
            List<UrunSecenek> UrunSecenekListesi = new List<UrunSecenek>();
            var SaveUrunSecenekListesi = Newtonsoft.Json.JsonConvert.SerializeObject(UrunSecenekListesi);
            _httpContextAccessor.HttpContext.Session.SetString("UrunSecenekListesi", SaveUrunSecenekListesi);
        }

        private void UrunSecenekListesiSessionKaydet(List<UrunSecenek> urunSeceneks)
        {
            var SaveUrunSecenekListesi = Newtonsoft.Json.JsonConvert.SerializeObject(urunSeceneks);
            _httpContextAccessor.HttpContext.Session.SetString("UrunSecenekListesi", SaveUrunSecenekListesi);
        }

        [HttpPost]
        public IActionResult YeniUrunOzellikListesiEkle(int Index)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("UrunOzellikListesi[{0}]", Index);
            return PartialView("/Areas/Admin/Views/Urunler/EditorTemplates/UrunOzelliklerListesi.cshtml", new UrunViewModel());
        }

        public IActionResult UrunOzellikleri(int Id, int OzellikGrupId)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            ViewData["AltBaslik"] = "Ürün Özellikleri";

            ViewData["UrunOzellikleri"] = _context.UrunOzellikleri.ToList().Where(x => x.UrunOzellikGrupId == OzellikGrupId).AsQueryable().Select(p => new SelectListItem() { Text = p.UrunOzellikleriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").OzellikAdi, Value = p.Id.ToString() });

            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            var diller = ViewBag.Diller;


            var urunToOzellik = _context.UrunToOzellik.Where(x => x.UrunId == Id && x.UrunOzellikleri.UrunOzellikGrupId == OzellikGrupId).ToList().OrderBy(x => x.UrunOzellikId);

            foreach (var item in urunToOzellik)
            {
                foreach (var dil in diller)
                {
                    var urunOzellikTranslate = urunToOzellik.Where(x => x.DilId == dil.Id && x.UrunOzellikId == item.UrunOzellikId).FirstOrDefault();
                    if (urunOzellikTranslate == null)
                    {
                        urunOzellikTranslate = new UrunToOzellik()
                        {
                            UrunOzellikId = item.UrunOzellikId,
                            UrunId = Id,
                            DilId = dil.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = dil.DilAdi }
                        };
                        _context.Entry(urunOzellikTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
            }



            UrunToOzellikViewModel model = new UrunToOzellikViewModel();

            foreach (var ozellik in urunToOzellik)
            {
                model.UrunToOzellikListesi.Add(new UrunToOzellik
                {
                    UrunOzellikId = ozellik.UrunOzellikId,
                    UrunId = Id,
                    Aciklama = ozellik.Aciklama,
                    Resim = ozellik.Resim,
                    DilId = ozellik.DilId,
                });
            }
            model.UrunId = Id;
            model.OzellikGrupId = OzellikGrupId;

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> UrunOzellikEkle(UrunToOzellikViewModel urunOzellik)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            var diller = ViewBag.Diller;

            foreach (var dil in diller as IEnumerable<Diller>)
            {
                urunOzellik.UrunToOzellikListesi.Add(new UrunToOzellik
                {
                    UrunId = urunOzellik.UrunId,
                    DilId = dil.Id,
                    Aciklama = urunOzellik.Aciklama
                });
            }

            ViewData["UrunOzellikleri"] = _context.UrunOzellikleri.ToList().Where(x => x.UrunOzellikGrupId == urunOzellik.OzellikGrupId).AsQueryable().Select(p => new SelectListItem() { Text = p.UrunOzellikleriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").OzellikAdi, Value = p.Id.ToString() });

            //urunSecenekDeger.UrunSecenekDegerListesi.Add(new UrunSecenekDegerleriTranslate());
            return PartialView("/Areas/Admin/Views/Urunler/EditorTemplates/_UrunOzellikEkle.cshtml", urunOzellik);
        }


        [HttpPost]
        public async Task<IActionResult> UrunOzellikKaydet(UrunToOzellikViewModel Model, string submit)
        {

            var model = await _urunServis.UrunOzellik(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("UrunOzellikleri", controllerValue, new { Id = Model.UrunId, OzellikGrupId = Model.OzellikGrupId });
        }


        public IActionResult BenzerUrunAutoComplete(string term)
        {

            var result = _context.Urunler.ToList().Where(x => x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi.ToLower().Contains(term.ToLower())).Select(x => new { value = x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi, id = x.Id.ToString() });

            return Json(result);
        }
        public IActionResult TamamlayiciUrunAutoComplete(string term)
        {

            var result = _context.Urunler.ToList().Where(x => x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi.ToLower().Contains(term.ToLower())).Select(x => new { value = x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi, id = x.Id.ToString() });

            return Json(result);
        }
        public async Task<IActionResult> Delete(UrunViewModel Model)
        {

            var model = await _urunServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new { SayfaTipi = model.SayfaUrl });
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _urunServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        //DROPZONE RESIM YUKLEME, SILME VE SIRALAMA

        public async Task<IActionResult> PageImages(int Id, int UrunResimKategoriId)
        {

            var model = _context.Urunler.Where(x => x.Id == Id).FirstOrDefault();
            var resimadi = model.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi;

            string dosyaFormat = "";
            string dosyaBoyutUyari = "";
            float dosyaBoyut = 0;
            if (Request.HasFormContentType && Request.Form != null && Request.Form.Count() > 0)
            {


                List<string> ContentTypeListesi = new();

                if ((UrunResimKategorileri)UrunResimKategoriId == UrunResimKategorileri.UrunResim)
                {
                    ContentTypeListesi = new List<string> { "image/jpeg", "image/png", "image/gif", "image/webp" };
                    dosyaFormat = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                    dosyaBoyutUyari = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                    dosyaBoyut = 5242880;
                }

                else
                {
                    ContentTypeListesi = new List<string> { "image/jpeg", "image/png", "image/gif", "image/webp", "application/pdf", "application/msword" };
                    dosyaFormat = "Geçerli Bir Dosya Tipi Seçiniz";
                    dosyaBoyutUyari = "Maksimum 30 Mb boyutunda dosya yükleyiniz.";
                    dosyaBoyut = 31457280;
                }
                foreach (var formFile in Request.Form.Files)
                {
                    if (ContentTypeListesi.Contains(formFile.ContentType))
                    {
                        string imageName = ImageHelper.ImageReplaceName(formFile, "");

                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Urunler/" + imageName;

                        FileInfo serverfile = new FileInfo(Mappath);
                        if (!serverfile.Directory.Exists)
                        {
                            serverfile.Directory.Create();
                        }

                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }

                        var sayfaResim = new UrunResimleri()
                        {
                            ResimAdi = Path.GetFileNameWithoutExtension(formFile.FileName),
                            Resim = Mappath.Remove(0, 7),
                            UrunId = Id,
                            UrunResimKategori = (UrunResimKategorileri)UrunResimKategoriId,
                            Sira = 0
                        };

                        _context.UrunResimleri.Add(sayfaResim);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Json(new ResultViewModel { Basarilimi = false, Mesaj = dosyaFormat, NotfyAlert = true, BootBoxAlert = false });
                    }
                    if (formFile.Length > dosyaBoyut)
                    {
                        return Json(new ResultViewModel { Basarilimi = false, Mesaj = dosyaBoyutUyari, NotfyAlert = true, BootBoxAlert = false });

                    }
                }

                var resimListesi = _context.UrunResimleri.Where(x => x.UrunId == model.Id && x.UrunResimKategori == (UrunResimKategorileri)UrunResimKategoriId);
                int ResimIndex = 0;
                foreach (var item in resimListesi)
                {
                    ResimIndex++;
                    item.Sira = ResimIndex;
                    _context.UrunResimleri.Update(item);
                }
                await _context.SaveChangesAsync();

            }

            return View(_context.UrunResimleri.Where(x => x.UrunId == Id && x.UrunResimKategori == (UrunResimKategorileri)UrunResimKategoriId).ToList());
        }
        

        public async Task<IActionResult> PageImageSortOrder(string sira)
        {
            var model = await _urunServis.ImageSortOrder(sira);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }
        

        public async Task<IActionResult> PageImagesDelete(int id)
        {
            var model = await _urunServis.ImageDelete(id);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }
        //DROPZONE RESIM YUKLEME, SILME VE SIRALAMA

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            ViewData["Markalar"] = _context.Markalar.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.MarkaAdi, Value = p.Id.ToString() });
            ViewData["Kategoriler"] = _context.Kategoriler.ToList().Where(p => p.Id != 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToCategoryTree()), Value = p.Id.ToString() });
            ViewData["DataSheet"] = _context.Dosyalar.Where(x=> x.DosyaKategoriId == 1).ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DosyalarTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").DosyaAdi, Value = p.Id.ToString() });
        }
    }
}
