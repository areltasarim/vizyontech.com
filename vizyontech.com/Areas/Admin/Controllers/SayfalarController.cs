using vizyontech.com.Controllers;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Text;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class SayfalarController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IHtmlLocalizer<HomeController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        private readonly SayfalarServis _sayfaServis;
        private readonly string entityBaslik = "Sayfalar";
        private readonly string entityAltBaslik = "Sayfa Ekle";
        public SayfalarController(AppDbContext _context, SayfalarServis _sayfaServis, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            this._context = _context;
            this._sayfaServis = _sayfaServis;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
        }

        public async Task<IActionResult> Index(int ParentSayfaId, bool TumSayfaDurumu, SayfaTipleri SayfaTipi)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _sayfaServis.PageList(SayfaTipi);

            return View(model);
        }
        [HttpPost]
        public JsonResult SayfalariListele(IFormCollection form, int ParentSayfaId, bool TumSayfaDurumu, SayfaTipleri SayfaTipi)
        {

            var dataTableFilter = form.ToDataTableFilter();
            var data = _context.Sayfalar.Where(p => p.Id != 1).ToList().AsQueryable();
            if (ParentSayfaId != 0)
            {
                List<Sayfalar> altSayfalar = GetAltSayfalar(ParentSayfaId);
                List<int> altSayfaIdListesi = altSayfalar.Select(s => s.Id).ToList();
                data = _context.Sayfalar.Where(p => p.Id != 1 && (p.Id == ParentSayfaId || altSayfaIdListesi.Contains(p.Id))).ToList().AsQueryable();
            }
            //get total count of data in table
            dataTableFilter.totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(dataTableFilter.searchValue))
            {
                data = data.Where(x =>
                  string.Join(" > ", x.ToPageTree()).ToLower().ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.SayfaTipi.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.AdminSolMenu.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.SayfaResimleri.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.Hit.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.Durum.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.Vitrin.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.Sira.ToString().Contains(dataTableFilter.searchValue.ToLower())
                );
            }

            // get total count of records after search 
            dataTableFilter.filterRecord = data.Count();

            // Veriyi memory'ye al
            var dataList = data.ToList();
            
            // SeoUrl'leri önceden yükle (performans için) - EntityId ve EntityName ile eşleştir
            var sayfaIds = dataList.Select(x => x.Id).ToList();
            var seoUrlList = _context.SeoUrl
                .Where(s => sayfaIds.Contains(s.EntityId))
                .ToList();
            
            // İzin verilen SayfaTipleri listesi
            var izinVerilenTipler = new List<SayfaTipleri>
            {
                SayfaTipleri.Hakkimizda,
                SayfaTipleri.DinamikSayfa,
                SayfaTipleri.Url,
                SayfaTipleri.Hizmetlerimiz,
                SayfaTipleri.Cozumlerimiz,
                SayfaTipleri.Blog,
                SayfaTipleri.Video
            };
            
            // Her sayfa için SeoUrl'yi bul (sadece izin verilen tipler için)
            var seoUrlDict = new Dictionary<int, string>();
            foreach (var sayfa in dataList)
            {
                // Sadece izin verilen tipler için URL Git butonu göster
                if (!izinVerilenTipler.Contains(sayfa.SayfaTipi))
                {
                    continue;
                }
                
                // EntityName ile eşleşen SeoUrl'yi bul
                var seoUrl = seoUrlList.FirstOrDefault(s => s.EntityId == sayfa.Id && s.EntityName == sayfa.EntityName);
                
                // Eğer bulunamazsa, sadece EntityId ile eşleşen ilk SeoUrl'yi al
                if (seoUrl == null)
                {
                    seoUrl = seoUrlList.FirstOrDefault(s => s.EntityId == sayfa.Id);
                }
                
                if (seoUrl != null && !string.IsNullOrEmpty(seoUrl.Url))
                {
                    seoUrlDict[sayfa.Id] = seoUrl.Url;
                }
            }

            var tmpdata = dataList.Select(x =>
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
                
                var sayfaAdi = string.Join(" > ", x.ToPageTree());
                var sayfaAdiKisaltilmis = sayfaAdi.Length > 50 ? sayfaAdi.Substring(0, 50) + "..." : sayfaAdi;
                
                return new
                {
                    
                    Id = x.Id,
                    SayfaAdi = sayfaAdiKisaltilmis,
                    SayfaTipi = x.SayfaTipi.GetDisplayName(),
                    UrlGit = urlGitButonu,
                    AdminSolMenu = $"<div class='badge badge-soft-{(x.AdminSolMenu == AdminSolMenuDurumlari.Goster ? "info" : "danger")} font-size-12'>{(x.AdminSolMenu.GetDisplayName())}</div>",
                    SayfaResimleri = SayfaResimDil(x.Id, SayfaResimKategorileri.Galeri1),
                    SayfaResimleri2 = SayfaResimDil(x.Id, SayfaResimKategorileri.Galeri2),
                    Hit = x.Hit,
                    Durum = $"<div class='badge badge-soft-{(x.Durum == SayfaDurumlari.Aktif ? "success" : "danger")} font-size-12'>{(x.Durum == SayfaDurumlari.Aktif ? "Aktif" : "Pasif")}</div>",
                    Vitrin = $"<div class='badge badge-soft-{(x.Vitrin == SayfaDurumlari.Aktif ? "success" : "danger")} font-size-12'>{(x.Vitrin.GetDisplayName())}</div>",
                    Sira = x.Sira,
                    Buttons = $"<a  href='/Admin/Sayfalar/AddOrUpdate/{x.Id}?ParentSayfaId={ParentSayfaId}&SayfaTipi={x.SayfaTipi}&TumSayfaDurumu={TumSayfaDurumu}' class='mr-3 text-primary' data-toggle='tooltip' data-placement='top' title='Düzenle' data-original-title='Düzenle'><i class='mdi mdi-pencil font-size-18'></i></a>" +
                    $"<a style='display:{(x.SilmeYetkisi == SayfaDurumlari.Aktif ? "inline" : "none")}' href='/Admin/Sayfalar/Delete/{x.Id}?ParentSayfaId={ParentSayfaId}&SayfaTipi={SayfaTipi}&TumSayfaDurumu={TumSayfaDurumu}' data-name='{ViewData["ToPageTree"]}' data-subcategory='{x.AltSayfalar.Count}' asp-route-SayfaTipi='{x.SayfaTipi}' class='text-danger remove' data-bs-toggle='modal' data-bs-target='#staticBackdrop' data-placement='top' title='Sil' data-original-title='Sil'><i class='mdi mdi-trash-can font-size-18'></i></a>"
                };
            }).AsQueryable();


            //dataTableFilter.sortColumn = "MagazaAdi";
            //sort data
            if (!string.IsNullOrEmpty(dataTableFilter.sortColumn) && !string.IsNullOrEmpty(dataTableFilter.sortColumnDirection))
            {
                try
                {
                    tmpdata = tmpdata.OrderBy(dataTableFilter.sortColumn + " " + dataTableFilter.sortColumnDirection);
                }
                catch (Exception hata)
                {

                }
            }
            var empList = tmpdata.Skip(dataTableFilter.skip).Take(dataTableFilter.pageSize).ToList();
            var returnObj = new { draw = dataTableFilter.draw, recordsTotal = dataTableFilter.totalRecord, recordsFiltered = dataTableFilter.filterRecord, data = empList };


            return Json(returnObj);
        }

        private string SayfaResimDil(int sayfaId, SayfaResimKategorileri sayfaResimKategori)
        {
            var SayfaResim1lDilListesi = new StringBuilder();
            foreach (var dil in _context.Diller.ToList())
            {
                SayfaResim1lDilListesi.Append($"<a onclick='GaleriResimleri({sayfaId},{Convert.ToInt32(sayfaResimKategori)},{dil.Id})' href='javascript: void()' class='btn btn-sm btn-success'>{dil.KisaDilAdi} </a>");
            }

            return SayfaResim1lDilListesi.ToString();
        }
        private string SayfaOzellikGoster(int sayfaId)
        {
            var model = new StringBuilder();

            if (sayfaId == 2)
            {
                model.Append($"<a href='/Admin/Sayfalar/SayfaOzellikleri/{sayfaId}?OzellikGrupId=1' class='btn btn-sm btn-info'>Özellikler</a>");
            }
           
            return model.ToString();
        }
        private List<Sayfalar> GetAltSayfalar(int parentSayfaId)
        {
            List<Sayfalar> altSayfalar = new List<Sayfalar>();
            List<Sayfalar> altSayfaListesi = _context.Sayfalar.Where(p => p.ParentSayfaId == parentSayfaId).ToList();
            foreach (var altSayfa in altSayfaListesi)
            {
                altSayfalar.Add(altSayfa);
                List<Sayfalar> altAltSayfalar = GetAltSayfalar(altSayfa.Id);
                altSayfalar.AddRange(altAltSayfalar);
            }
            return altSayfalar;
        }

        public IActionResult SayfaAutoComplete(string term)
        {

            var result = _context.Sayfalar.ToList().Where(p => p.Id != 1).Where(x => x.SayfalarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).SayfaAdi.ToLower().Contains(term.ToLower())).Select(x => new { value = string.Join(" > ", x.ToPageTree()), id = x.Id.ToString() });

            return Json(result);
        }

        public IActionResult SayfaOzellikleri(int Id, int OzellikGrupId)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            ViewData["AltBaslik"] = "Sayfa Özellikleri";

            ViewData["SayfaOzellikleri"] = _context.SayfaOzellikleri.ToList().Where(x => x.SayfaOzellikGrupId == OzellikGrupId).AsQueryable().Select(p => new SelectListItem() { Text = p.SayfaOzellikleriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").OzellikAdi, Value = p.Id.ToString() });

            SayfaToOzellikViewModel model = new SayfaToOzellikViewModel();

            var SayfaToOzellik = _context.SayfaToOzellik.Where(x => x.SayfaId == Id && x.SayfaOzellikleri.SayfaOzellikGrupId == OzellikGrupId).ToList();
            foreach (var ozellik in SayfaToOzellik)
            {
                model.SayfaToOzellikListesi.Add(new SayfaToOzellik
                {
                    SayfaOzellikId = ozellik.SayfaOzellikId,
                    SayfaId = Id,
                    Aciklama = ozellik.Aciklama,
                    Resim = ozellik.Resim,
                    DilId = ozellik.DilId,
                });
            }
            model.SayfaId = Id;
            model.OzellikGrupId = OzellikGrupId;

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> SayfaOzellikEkle(SayfaToOzellikViewModel SayfaOzellik)
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            var diller = ViewBag.Diller;

            foreach (var dil in diller as IEnumerable<Diller>)
            {
                SayfaOzellik.SayfaToOzellikListesi.Add(new SayfaToOzellik
                {
                    SayfaId = SayfaOzellik.SayfaId,
                    DilId = dil.Id,
                });
            }

            ViewData["SayfaOzellikleri"] = _context.SayfaOzellikleri.ToList().Where(x => x.SayfaOzellikGrupId == SayfaOzellik.OzellikGrupId).AsQueryable().Select(p => new SelectListItem() { Text = p.SayfaOzellikleriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").OzellikAdi, Value = p.Id.ToString() });

            //SayfaSecenekDeger.SayfaSecenekDegerListesi.Add(new SayfaSecenekDegerleriTranslate());
            return PartialView("/Areas/Admin/Views/Sayfalar/EditorTemplates/_SayfaOzellikEkle.cshtml", SayfaOzellik);
        }


        [HttpPost]
        public async Task<IActionResult> SayfaOzellikKaydet(SayfaToOzellikViewModel Model, string submit)
        {

            var model = await _sayfaServis.SayfaOzellik(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("SayfaOzellikleri", controllerValue, new { Id = Model.SayfaId, OzellikGrupId = Model.OzellikGrupId });
        }


        public async Task<IActionResult> FormBasvurulari()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _sayfaServis.FormBasvulariListele();

            return View(model);
        }


        public async Task<IActionResult> FormCevaplari(int Id)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _sayfaServis.FormCevaplariListele();

            var basvuruDurumu = _context.FormBasvurulari.Where(p => p.Id == Id).FirstOrDefault();
            basvuruDurumu.BasvuruDurumu = BasvuruDurumlari.Incelendi;

            _context.Entry(basvuruDurumu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return View(model);
        }

        public IActionResult AddOrUpdate(SayfaTipleri SayfaTipi, int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            PopulateDropdown();


            if (SayfaTipi == SayfaTipleri.DinamikSayfa)
            {
                ViewData["Sayfalar"] = _context.Sayfalar.ToList().AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToPageTree()), Value = p.Id.ToString() });
            }
            else
            {
                var tmpSayfalar = _context.Sayfalar.ToList().Where(p => p.SayfaTipi == SayfaTipi | p.Id == 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToPageTree()), Value = p.Id.ToString() });
                ViewData["Sayfalar"] = tmpSayfalar;
            }


            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                var sayfa = _context.Sayfalar.Find(Id);
                foreach (var item in diller)
                {
                    var SayfalarTranslate = sayfa.SayfalarTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (SayfalarTranslate == null)
                    {
                        SayfalarTranslate = new SayfalarTranslate()
                        {
                            SayfaId = Id,
                            SayfaAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(SayfalarTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                SayfaViewModel model = new SayfaViewModel()
                {
                    Sayfa = _context.Sayfalar.Find(Id),
                };



                PopulateDropdown();
                return View(model);
            }
            else
            {
                PopulateDropdown();

                SayfaViewModel Model = new SayfaViewModel();
                Model.Sayfa.Durum = SayfaDurumlari.Aktif;
                foreach (var item in diller)
                {
                    var SayfalarTranslate = Model.SayfalarTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (SayfalarTranslate == null)
                    {
                        SayfalarTranslate = new SayfalarTranslate()
                        {
                            SayfaAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.Sayfa.SayfalarTranslate.Add(SayfalarTranslate);

                    }
                }
                return View(Model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(SayfaViewModel Model, int ParentSayfaId, SayfaTipleri SayfaTipi, bool TumSayfaDurumu, string submit)
        {
            var uyeId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            int parentSayfaId = 0;

            if (TumSayfaDurumu == false)
            {
                if (Model.Id == 0)
                {
                    parentSayfaId = Model.Sayfa.ParentSayfaId == 1 ? ParentSayfaId : Model.Sayfa.ParentSayfaId;

                }
                else
                {
                    parentSayfaId = Model.Sayfa.ParentSayfaId;
                }
            }
            else
            {
                parentSayfaId = Model.Sayfa.ParentSayfaId;
            }

            Model.Sayfa.ParentSayfaId = parentSayfaId;

            if (Model.Id == 0)
            {
                if (!User.IsInRole("Administrator"))
                {
                    Model.SilmeYetkisi = SayfaDurumlari.Aktif;
                    Model.SinirsizAltSayfaDurumu = SayfaDurumlari.Aktif;
                }
            }
            var model = await _sayfaServis.UpdatePage(Model, SayfaTipi, uyeId, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if(submit == "KaydetGuncelle")
            {
                SayfaTipi = Model.SayfaTipi;
            }
            else
            {
                if (TumSayfaDurumu == true)
                {
                    SayfaTipi = SayfaTipleri.DinamikSayfa;
                }

                if (User.IsInRole("Administrator"))
                {
                    if (TumSayfaDurumu == true)
                    {
                        SayfaTipi = SayfaTipleri.DinamikSayfa;
                    }
                    else
                    {
                        SayfaTipi = Model.SayfaTipi;
                    }
                }
            }


            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId, ParentSayfaId = ParentSayfaId, SayfaTipi = SayfaTipi, TumSayfaDurumu = TumSayfaDurumu });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                if (SayfaTipi == SayfaTipleri.DinamikSayfa)
                {
                    ViewData["Sayfalar"] = _context.Sayfalar.ToList().Where(p => p.Id == 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToPageTree()), Value = p.Id.ToString() });
                }
                else
                {
                    ViewData["Sayfalar"] = _context.Sayfalar.ToList().Where(p => p.SayfaTipi == SayfaTipi | p.Id == 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToPageTree()), Value = p.Id.ToString() });
                }

                PopulateDropdown();
                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId, ParentSayfaId =  ParentSayfaId, SayfaTipi = SayfaTipi, TumSayfaDurumu = TumSayfaDurumu });

            }
        }
        public async Task<IActionResult> Delete(SayfaViewModel Model, int ParentSayfaId, SayfaTipleri SayfaTipi, bool TumSayfaDurumu)
        {

            var model = await _sayfaServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new { ParentSayfaId = ParentSayfaId, SayfaTipi = SayfaTipi, TumSayfaDurumu = TumSayfaDurumu });
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes, int ParentSayfaId, SayfaTipleri SayfaTipi, bool TumSayfaDurumu)
        {
            var model = await _sayfaServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue, new { ParentSayfaId = ParentSayfaId, SayfaTipi = SayfaTipi, TumSayfaDurumu = TumSayfaDurumu });
        }
        public async Task<IActionResult> PageImages(int Id, int SayfaResimKategoriId, int? DilId)
        {

            var model = _context.Sayfalar.Where(x => x.Id == Id).FirstOrDefault();
            var resimadi = model.SayfalarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").SayfaAdi;

            if (Request.HasFormContentType && Request.Form != null && Request.Form.Count() > 0)
            {
                {
                    foreach (var formFile in Request.Form.Files)
                    {
                        string imageName = ImageHelper.ImageReplaceName(formFile,resimadi);

                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Sayfalar/" + imageName;

                        FileInfo serverfile = new FileInfo(Mappath);
                        if (!serverfile.Directory.Exists)
                        {
                            serverfile.Directory.Create();
                        }

                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }

                        var sayfaResim = new SayfaResimleri()
                        {
                            ResimAdi = Path.GetFileNameWithoutExtension(formFile.FileName),
                            Resim = Mappath.Remove(0, 7),
                            SayfaId = Id,
                            SayfaResimKategori = (SayfaResimKategorileri)SayfaResimKategoriId,
                            DilId = DilId,
                            Sira = 0
                        };

                        _context.SayfaResimleri.Add(sayfaResim);
                        await _context.SaveChangesAsync();

                    }

                }

                var resimListesi = _context.SayfaResimleri.Where(x => x.SayfaId == model.Id);
                int ResimIndex = 0;
                foreach (var item in resimListesi)
                {
                    ResimIndex++;
                    item.Sira = ResimIndex;
                    _context.SayfaResimleri.Update(item);
                }
                await _context.SaveChangesAsync();

            }

            return View(_context.SayfaResimleri.Where(x => x.SayfaId == Id && x.SayfaResimKategori == (SayfaResimKategorileri)SayfaResimKategoriId && x.DilId == DilId).ToList());
        }

        public async Task<IActionResult> PageImageSortOrder(string sira)
        {
            var model = await _sayfaServis.ImageSortOrder(sira);

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
            var model = await _sayfaServis.ImageDelete(id);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }

        #region Yorumlar
        [HttpPost]
        public JsonResult YorumlariListele(IFormCollection form, int SayfaId)
        {

            var dataTableFilter = form.ToDataTableFilter();
            var data = _context.Yorumlar.Where(p => p.SayfaId == SayfaId).ToList().AsQueryable();
         
            //get total count of data in table
            dataTableFilter.totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(dataTableFilter.searchValue))
            {
                data = data.Where(x =>
                  x.AdSoyad.ToString().ToLower().ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.YorumTarihi.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.YorumDurumu.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.Yorum.ToString().Contains(dataTableFilter.searchValue.ToLower())
                  || x.Sehir.ToString().Contains(dataTableFilter.searchValue.ToLower())
                );
            }

            // get total count of records after search 
            dataTableFilter.filterRecord = data.Count();

            var tmpdata = data.Select(x =>
            new
            {
                Id = x.Id,
                AdSoyad = x.AdSoyad.ToString(),
                YorumTarihi = x.YorumTarihi.ToShortDateString(),
                YorumDurumu = $"<div class='badge badge-soft-{(x.YorumDurumu == SayfaDurumlari.Aktif ? "success" : "danger")} font-size-12'>{(x.YorumDurumu == SayfaDurumlari.Aktif ? "Aktif" : "Pasif")}</div>",
                Yorum = x.Yorum.ToString(),
                Sehir = x.Sehir.ToString(),
                Buttons = $"<a  href='/Admin/Sayfalar/YorumOnayla/{x.Id}?SayfaId={SayfaId}' class='mr-3 text-primary' data-toggle='tooltip' data-placement='top' title='Düzenle' data-original-title='Düzenle'><i class='mdi mdi-pencil font-size-18'></i></a>" +
                $"<a  href='/Admin/Sayfalar/YorumSil/{x.Id}?SayfaId={SayfaId}' data-name='{x.Yorum}' class='text-danger remove' data-bs-toggle='modal' data-bs-target='#staticBackdrop' data-placement='top' title='Sil' data-original-title='Sil'><i class='mdi mdi-trash-can font-size-18'></i></a>"
            });
            //dataTableFilter.sortColumn = "MagazaAdi";
            //sort data
            if (!string.IsNullOrEmpty(dataTableFilter.sortColumn) && !string.IsNullOrEmpty(dataTableFilter.sortColumnDirection))
            {
                try
                {
                    tmpdata = tmpdata.OrderBy(dataTableFilter.sortColumn + " " + dataTableFilter.sortColumnDirection);
                }
                catch (Exception hata)
                {

                }
            }
            var empList = tmpdata.Skip(dataTableFilter.skip).Take(dataTableFilter.pageSize).ToList();
            var returnObj = new { draw = dataTableFilter.draw, recordsTotal = dataTableFilter.totalRecord, recordsFiltered = dataTableFilter.filterRecord, data = empList };


            return Json(returnObj);
        }

        public IActionResult Yorumlar()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            return View();
        }
        public IActionResult YorumOnayla(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.Yorumlar.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YorumOnayla(YorumViewModel Model, IList<IFormFile> Files, string submit)
        {
            var model = await _sayfaServis.YorumEkle(Model, Files, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction(model.Action, controllerValue, new {Id = model.SayfaId, SayfaId = model.SayfaUrl });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return RedirectToAction("Yorumlar", controllerValue, new { Id = model.SayfaId, SayfaId = model.SayfaUrl });
            }
        }

        public async Task<IActionResult> YorumSil(YorumViewModel Model, int SayfaId)
        {

            var model = await _sayfaServis.YorumDeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Yorumlar", controllerValue, new { SayfaId = SayfaId});
        }


        #endregion

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }
    }
}
