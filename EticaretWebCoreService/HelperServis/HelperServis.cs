using Castle.Components.DictionaryAdapter.Xml;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService.Sepet;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Xml;
using X.PagedList;
using Formatting = Newtonsoft.Json.Formatting;

namespace EticaretWebCoreService
{
    public partial class HelperServis : IHelperServis
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService _cacheService;

        private readonly UnitOfWork _uow;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;

        public HelperServis(AppDbContext _context, UnitOfWork _uow, IHttpContextAccessor _httpContextAccessor, ICacheService cacheService, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this._context = _context;
            this._uow = _uow;
            _cacheService = cacheService;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
        }
        public string GetCurrentCulture()
        {
            return _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
        }



        public async Task<List<Diller>> GetDiller()
        {
            var model = await _uow.Repository<Diller>().GetAll().Result.ToListAsync();
            return model;
        }

        public async Task<Diller> GetAktifDil()
        {
            var dilKodu = GetCurrentCulture();
            var dil = await _uow.Repository<Diller>().GetAll().Result.SingleOrDefaultAsync(x => x.DilKodlari.DilKodu == dilKodu);

            return dil;
        }



        public async Task<SiteAyarlariTranslate> GetSiteAyari()
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SiteAyarlariTranslate>().GetAll().Result.SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }

        public async Task<List<AdresBilgileriTranslate>> GetAdresBilgileri()
        {
            var dil = GetCurrentCulture();

            var model = await _uow.Repository<AdresBilgileriTranslate>().GetAll().Result.Where(x => x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.AdresBilgileri.Sira).ToListAsync();
            return model;
        }

        public async Task<List<AdresBilgileriTelefonlar>> GetAdresBilgileriTelefonlar()
        {
            var model = await _uow.Repository<AdresBilgileriTelefonlar>().GetAll().Result.ToListAsync();
            return model;
        }

        public async Task<AdresBilgileriTelefonlarTranslate> GetEkTelefon(int adresBilgiId)
        {
            var dil = GetCurrentCulture();
            var siteAyari = await _uow.Repository<SiteAyarlariTranslate>().GetAll().Result.FirstOrDefaultAsync();
            var model = await _uow.Repository<AdresBilgileriTelefonlarTranslate>().GetAll().Result.Where(x => x.AdresBilgileriTelefonId == adresBilgiId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var aktiDilmodel = await _uow.Repository<AdresBilgileriTelefonlarTranslate>().GetAll().Result.Where(x => x.AdresBilgileriTelefonId == adresBilgiId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            if (string.IsNullOrEmpty(model.Telefon))
            {
                model.Telefon = aktiDilmodel.Telefon;
            }
            return model;
        }

        public async Task<AdresBilgileriTranslate> GetAdresBilgi()
        {
            var dil = GetCurrentCulture();
            var siteAyari = await _uow.Repository<SiteAyarlariTranslate>().GetAll().Result.FirstOrDefaultAsync();
            var model = await _uow.Repository<AdresBilgileriTranslate>().GetAll().Result.Where(x => x.AdresBilgileri.SiteAyarId == siteAyari.SiteAyarId && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.AdresBilgiId).FirstOrDefaultAsync();

            if (model == null)
            {
                var aktiDilmodel = await _uow.Repository<AdresBilgileriTranslate>().GetAll().Result.Where(x => x.AdresBilgileri.SiteAyarId == siteAyari.SiteAyarId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
                return aktiDilmodel;
            }
            else
            {
                return model;
            }

        }

        //public IEnumerable<Menuler> GetMenu(MenuYerleri MenuYeri = MenuYerleri.FooterMenuSol)
        //{
        //    var dil = GetCurrentCulture();

        //    var model = _uow.Repository<Menuler>().GetAll().Result.Where(x => x.Id != 1 && x.ParentMenuId == 1 && x.MenuYeri == MenuYeri && x.Durum == SayfaDurumlari.Aktif).OrderBy(x => x.Sira).ToList();
        //    return model;
        //}

        public class MenulerTranslateDTO
        {
            public string MenuAdi { get; set; }
            public string Url { get; set; }
            public string DilKodu { get; set; }
        }

        public class MenulerDTO
        {
            public int Id { get; set; }
            public MenulerDTO ParentMenu { get; set; } // recursive ilişki
            public int ParentMenuId { get; set; }
            public int EntityId { get; set; }
            public MenuSekmeleri SekmeDurumu { get; set; }
            public MenuTipleri MenuTipi { get; set; }
            public MenuYerleri MenuYeri { get; set; }
            public SeoUrlTipleri SeoUrlTipi { get; set; }
            public int MenuKolon { get; set; }
            public string Link { get; set; }
            public int Sira { get; set; }

            public List<MenulerTranslateDTO> MenulerTranslate { get; set; } = new(); // ✅ null olmaz
            public List<MenulerDTO> AltMenuler { get; set; } = new(); // ✅ null olmaz

        }
        private List<MenulerDTO> BuildMenuTree(
    List<Menuler> allMenus,
    List<MenulerTranslate> allTranslates,
    int parentId = 1)
        {
            return allMenus
                .Where(m => m.ParentMenuId == parentId)
                .OrderBy(m => m.Sira)
                .Select(menu =>
                {
                    var dto = new MenulerDTO
                    {
                        Id = menu.Id,
                        ParentMenuId = menu.ParentMenuId,
                        EntityId = menu.EntityId ?? 0,
                        SekmeDurumu = menu.SekmeDurumu,
                        MenuTipi = menu.MenuTipi,
                        MenuYeri = menu.MenuYeri,
                        SeoUrlTipi = menu.SeoUrlTipi,
                        MenuKolon = menu.MenuKolon,
                        Sira = menu.Sira,
                        MenulerTranslate = allTranslates
                            .Where(t => t.MenuId == menu.Id)
                            .Select(t => new MenulerTranslateDTO
                            {
                                MenuAdi = t.MenuAdi,
                                Url = t.Url,
                                DilKodu = t.Diller.DilKodlari.DilKodu
                            }).ToList()
                    };

                    // 🔁 Recursive çağrı
                    dto.AltMenuler = BuildMenuTree(allMenus, allTranslates, menu.Id);

                    return dto;
                }).ToList();
        }

        public async Task<List<MenulerDTO>> GetMenu(MenuYerleri menuYeri = MenuYerleri.FooterMenuSol)
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"Menuler-{menuYeri}-{dil}";

            var model = await _cacheService.GetAsync<List<MenulerDTO>>(cacheKey);
            if (model != null)
                return model;

            var allMenus = await _uow.Repository<Menuler>()
                .GetAll().Result
                .Where(x => x.Durum == SayfaDurumlari.Aktif && x.MenuYeri == menuYeri)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var allTranslates = await _uow.Repository<MenulerTranslate>()
                .GetAll().Result
                .Where(x => x.Diller.DilKodlari.DilKodu == dil)
                .ToListAsync();

            model = BuildMenuTree(allMenus, allTranslates); // ⬅️ Recursive yapı burada çağrılır

            await _cacheService.SetAsync(cacheKey, model, 360000);

            return model;
        }



        public IEnumerable<MenulerTranslate> GetMenuTranslate(MenuYerleri MenuYeri = MenuYerleri.FooterMenuSol)
        {
            var dil = GetCurrentCulture();

            var model = _uow.Repository<MenulerTranslate>().GetAll().Result.Where(x => x.Diller.DilKodlari.DilKodu == dil && x.Id != 1 && x.Menuler.ParentMenuId == 1 && x.Menuler.MenuYeri == MenuYeri && x.Menuler.Durum == SayfaDurumlari.Aktif).OrderBy(x => x.Menuler.Sira).ToList();
            return model;
        }

        public IEnumerable<SlaytlarTranslate> GetSlayt()
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<SlaytlarTranslate>().GetAll().Result.Where(x => x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Slaytlar.Sira).ToList();
            return model;
        }

        //public async Task<string> GetSlaytResim(int slaytId)
        //{
        //    var dil = GetCurrentCulture();
        //    var model = await _uow.Repository<SlaytlarTranslate>().GetAll().Result.Where(x => x.SlaytId == slaytId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
        //    var trDilmodel = await _uow.Repository<SlaytlarTranslate>().GetAll().Result.Where(x => x.SlaytId == slaytId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
        //    string sayfaResim = model?.Resim;
        //    if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
        //    {
        //        sayfaResim = trDilmodel.Resim;
        //    }
        //    return sayfaResim;
        //}
        public async Task<string> GetSlaytResim(int slaytId)
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"SlaytResim-{dil}-{slaytId}";

            // 1. Önce Cache'ten çek
            var sayfaResim = await _cacheService.GetAsync<string>(cacheKey);

            // 2. Cache'te yoksa veritabanından çek
            if (string.IsNullOrEmpty(sayfaResim))
            {
                var model = await _uow.Repository<SlaytlarTranslate>()
                    .GetAll().Result
                    .Where(x => x.SlaytId == slaytId)
                    .SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);

                var trDilmodel = await _uow.Repository<SlaytlarTranslate>()
                    .GetAll().Result
                    .Where(x => x.SlaytId == slaytId)
                    .SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");

                var resim = model?.Resim;

                if (string.IsNullOrEmpty(resim) || resim == "/Content/Upload/Images/resimyok.png")
                {
                    resim = trDilmodel?.Resim;
                }

                sayfaResim = resim;

                // 3. Cache'e yaz (örneğin 3600 saniye = 1 saat)
                await _cacheService.SetAsync(cacheKey, sayfaResim, 36000000);
            }

            return sayfaResim;
        }




        public IEnumerable<Banner> GetBanner(int bannerId)
        {
            var model = _uow.Repository<Banner>().GetAll().Result.Where(x => x.Id == bannerId && x.Durum == SayfaDurumlari.Aktif).OrderBy(x => x.Sira).ToList();
            return model;
        }

        public IEnumerable<BannerResimTranslate> GetBannerResim(int banneResimId)
        {
            var dil = GetCurrentCulture();

            var model = _uow.Repository<BannerResimTranslate>().GetAll().Result.Where(x => x.BannerResim.BannerId == banneResimId && x.Diller.DilKodlari.DilKodu == dil && x.BannerResim.Banner.Durum == SayfaDurumlari.Aktif).OrderBy(x => x.Sira).ToList();
            return model;
        }


        public IEnumerable<SayfalarTranslate> GetSayfalar(SayfaTipleri sayfaTipi, int sayfa, int sayfaSayisi)
        {
            var dil = GetCurrentCulture();

            var model = _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId != 1 && x.Sayfalar.Durum == SayfaDurumlari.Aktif && x.Sayfalar.SayfaTipi == sayfaTipi && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Sayfalar.Sira).ToList().ToPagedList(sayfa, sayfaSayisi);
            return model;
        }

        public IEnumerable<SayfalarTranslate> GetAltSayfalar(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == sayfaId && x.Sayfalar.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Sayfalar.Sira).ToList();
            return model;
        }


        public async Task<SayfaOzellikGruplariTranslate> GetSayfaOzellikGrup(int grupId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SayfaOzellikGruplariTranslate>().GetAll().Result.Where(x => x.SayfaOzellikGrupId == grupId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }
        public IEnumerable<SayfaToOzellik> GetSayfaOzellikleri(int sayfaId)
        {
            var dil = GetCurrentCulture();

            var model = _uow.Repository<SayfaToOzellik>().GetAll().Result.Where(x => x.SayfaId == sayfaId && x.Diller.DilKodlari.DilKodu == dil).ToList().OrderBy(x => x.SayfaOzellikleri.Sira);
            return model;
        }

        public async Task<string> GetSayfaOzellikResim(int sayfaId, int sayfaOzellikId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Sayfalar/").Result;
            var model = await _uow.Repository<SayfaToOzellik>().GetAll().Result.Where(x => x.SayfaId == sayfaId && x.SayfaOzellikId == sayfaOzellikId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<SayfaToOzellik>().GetAll().Result.Where(x => x.SayfaId == sayfaId && x.SayfaOzellikId == sayfaOzellikId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaOzellikResim = model?.Resim;
            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                sayfaOzellikResim = trDilmodel.Resim;
            }
            return sayfaOzellikResim;
        }

        public async Task<SayfaToOzellik> GetSayfaToOzellik(int sayfaId)
        {
            var dil = GetCurrentCulture();

            var model = await _uow.Repository<SayfaToOzellik>().GetAll().Result.Where(x => x.SayfaId == sayfaId && x.Diller.DilKodlari.DilKodu == dil).FirstOrDefaultAsync();
            return model;
        }

        public async Task<SayfalarTranslate> GetSayfa(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }


        public IEnumerable<SayfalarTranslate> GetParentSayfalar(int parentSayfaId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == parentSayfaId && x.Sayfalar.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Sayfalar.Sira).ToList();
            return model;

        }

        public async Task<SayfalarTranslate> GetParentSayfa(SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == 1 && x.Sayfalar.SayfaTipi == sayfaTipi && x.Diller.DilKodlari.DilKodu == dil).FirstOrDefaultAsync();
            return model;
        }


        public async Task<SayfalarTranslate> GetParentSayfa2(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId && x.Diller.DilKodlari.DilKodu == dil).FirstOrDefaultAsync();
            return model;
        }

        public async Task<string> GetParentSayfaResim(SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            //var resimDizini = GetResimDizini("Sayfalar/").Result;
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == 1 && x.Sayfalar.SayfaTipi == sayfaTipi).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == 1 && x.Sayfalar.SayfaTipi == sayfaTipi).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaResim = model?.Resim;
            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                sayfaResim = trDilmodel.Resim;
            }
            return sayfaResim;
        }
        public async Task<string> GetParentSayfaResim2(SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == 1 && x.Sayfalar.SayfaTipi == sayfaTipi).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.Sayfalar.ParentSayfaId == 1 && x.Sayfalar.SayfaTipi == sayfaTipi).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaResim = model?.Resim2;
            if (model?.Resim2 == "/Content/Upload/Images/resimyok.png" || model?.Resim2 == null)
            {
                sayfaResim = trDilmodel.Resim2;
            }
            return sayfaResim;
        }
        public async Task<string> GetSayfaResim(int sayfaId)
        {
            var dil = GetCurrentCulture();
            //var resimDizini = GetResimDizini("Sayfalar/").Result;
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaResim = model?.Resim;
            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                sayfaResim = trDilmodel.Resim;
            }
            return sayfaResim;
        }

        public async Task<string> GetAnaSayfaResim(int sayfaId)
        {
            var dil = GetCurrentCulture();
            //var resimDizini = GetResimDizini("Sayfalar/").Result;

            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaResim = model?.Resim2;
            if (model?.Resim2 == "/Content/Upload/Images/resimyok.png" || model?.Resim2 == null)
            {
                sayfaResim = trDilmodel.Resim2;
            }
            return sayfaResim;
        }



        public IEnumerable<SayfaResimleri> GetSayfaResimleri(int sayfaId, SayfaResimKategorileri sayfaResimKategori, int? DilId)
        {
            var model = _uow.Repository<SayfaResimleri>().GetAll().Result.Where(x => x.SayfaId == sayfaId && x.SayfaResimKategori == sayfaResimKategori && x.DilId == DilId).OrderBy(x => x.Sira);
            return model;
        }


        public async Task<string> GetSayfaDosya(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<SayfalarTranslate>().GetAll().Result.Where(x => x.SayfaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaDosya = model?.Dosya;
            if (model?.Dosya == "#" || model?.Dosya == null)
            {
                sayfaDosya = trDilmodel.Dosya;
            }
            return sayfaDosya;
        }
        public async Task<SayfalarTranslate> GetOncekiSayfa(int sayfaId, SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var sayfa = await _uow.Repository<SayfalarTranslate>().GetAll();

            var parentId = sayfa.Where(x => x.SayfaId == sayfaId).FirstOrDefault().Sayfalar.ParentSayfaId;

            var oncekiSayfa = sayfa.Where(x => x.SayfaId < sayfaId && x.SayfaId != parentId && x.Sayfalar.ParentSayfaId != 1 && x.Sayfalar.SayfaTipi == sayfaTipi).OrderByDescending(p => p.SayfaId).FirstOrDefault();
            var oncekiSayfaId = oncekiSayfa?.Id ?? -1;

            var model = await sayfa.SingleOrDefaultAsync(x => x.Id == oncekiSayfaId && x.Diller.DilKodlari.DilKodu == dil);

            return model;
        }

        public async Task<SayfalarTranslate> GetSonrakiSayfa(int sayfaId, SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var sayfa = await _uow.Repository<SayfalarTranslate>().GetAll();

            var sonrakiSayfa = sayfa.Where(x => x.SayfaId > sayfaId && x.Sayfalar.SayfaTipi == sayfaTipi).OrderBy(p => p.SayfaId).FirstOrDefault();
            var sonrakiSayfaId = sonrakiSayfa?.Id ?? -1;

            var model = await sayfa.SingleOrDefaultAsync(x => x.Id == sonrakiSayfaId && x.Diller.DilKodlari.DilKodu == dil);

            return model;
        }

        public async Task<string> GetOncekiSayfaResim(int sayfaId, SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Sayfalar/").Result;
            var sayfa = await _uow.Repository<SayfalarTranslate>().GetAll();

            var parentId = sayfa.Where(x => x.SayfaId == sayfaId).FirstOrDefault().Sayfalar.ParentSayfaId;

            var oncekiSayfa = sayfa.Where(x => x.SayfaId < sayfaId && x.SayfaId != parentId && x.Sayfalar.ParentSayfaId != 1 && x.Sayfalar.SayfaTipi == sayfaTipi).OrderByDescending(p => p.SayfaId).FirstOrDefault();
            var oncekiSayfaId = oncekiSayfa?.Id ?? -1;

            var model = await sayfa.SingleOrDefaultAsync(x => x.Id == oncekiSayfaId && x.Diller.DilKodlari.DilKodu == dil);

            var trDilmodel = sayfa.SingleOrDefaultAsync(x => x.Id == oncekiSayfaId && x.Diller.DilKodlari.DilKodu == "tr-TR").Result;
            string sayfaResim = resimDizini + model?.Resim;

            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                sayfaResim = resimDizini + trDilmodel.Resim;
            }
            return sayfaResim;
        }
        public async Task<string> GetSonrakiSayfaResim(int sayfaId, SayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Sayfalar/").Result;
            var sayfa = await _uow.Repository<SayfalarTranslate>().GetAll();

            var sonrakiSayfa = sayfa.Where(x => x.SayfaId > sayfaId && x.Sayfalar.SayfaTipi == sayfaTipi && x.Diller.DilKodlari.DilKodu == dil).OrderBy(p => p.SayfaId).FirstOrDefault();
            var sonrakiSayfaId = sonrakiSayfa?.Id ?? -1;

            var model = await sayfa.SingleOrDefaultAsync(x => x.Id == sonrakiSayfaId && x.Diller.DilKodlari.DilKodu == dil);

            var trDilmodel = sayfa.SingleOrDefaultAsync(x => x.Id == sonrakiSayfaId && x.Diller.DilKodlari.DilKodu == "tr-TR").Result;
            string sayfaResim = resimDizini + model?.Resim;

            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                sayfaResim = resimDizini + trDilmodel.Resim;
            }
            return sayfaResim;
        }

        public async Task<SeoUrl> GetOncekiSayfaUrl(int sayfaId, SeoUrlTipleri seoUrlTipi)
        {
            var dil = GetCurrentCulture();
            var seoUrls = await _uow.Repository<SeoUrl>().GetAll();

            var oncekiSayfa = seoUrls.Where(x => x.EntityName == seoUrlTipi && x.EntityId < sayfaId).OrderByDescending(p => p.EntityId).FirstOrDefault();
            var oncekiSayfaId = oncekiSayfa?.Id ?? -1;

            var model = await seoUrls.SingleOrDefaultAsync(x => x.Id == oncekiSayfaId && x.EntityName == seoUrlTipi && x.Diller.DilKodlari.DilKodu == dil);

            return model;
        }

        public async Task<SeoUrl> GetSonrakiSayfaUrl(int sayfaId, SeoUrlTipleri seoUrlTipi)
        {
            var dil = GetCurrentCulture();
            var seoUrls = await _uow.Repository<SeoUrl>().GetAll();

            var sonrakiSayfa = seoUrls.Where(x => x.EntityName == seoUrlTipi && x.EntityId > sayfaId).OrderBy(p => p.EntityId).FirstOrDefault();
            var sonrakiSayfaId = sonrakiSayfa?.Id ?? -1;

            var model = await seoUrls.SingleOrDefaultAsync(x => x.Id == sonrakiSayfaId && x.EntityName == seoUrlTipi && x.Diller.DilKodlari.DilKodu == dil);

            return model;
        }

        public async Task<string> GetDosya(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Dosyalar/").Result;
            var model = await _uow.Repository<DosyalarTranslate>().GetAll().Result.Where(x => x.DosyaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<DosyalarTranslate>().GetAll().Result.Where(x => x.DosyaId == sayfaId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string sayfaDosya = resimDizini + model?.Dosya;
            if (model?.Dosya == "#")
            {
                sayfaDosya = resimDizini + trDilmodel.Dosya;
            }
            return sayfaDosya;
        }

        public string GetDosyaAdi(string filePath)
        {
            string pathToRemove = "/Content/Upload/Dosyalar/Dosyalar/";
            string cleanedPath = filePath.Replace(pathToRemove, "");
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cleanedPath);
            return fileNameWithoutExtension;
        }

        public async Task<string> GetFileExtension(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            string extension = Path.GetExtension(filePath);

            // Noktalı uzantı (.jpg gibi) döner, istersen sadece jpg kısmını da alabilirsin
            return extension?.ToLower(); // .JPG yerine .jpg olarak döndürür
        }

        public IEnumerable<EkiplerTranslate> GetEkipler()
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<EkiplerTranslate>().GetAll().Result.Where(x => x.Ekipler.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Ekipler.Sira).ToList();
            return model;
        }

        public EkiplerTranslate GetEkipDetay(int EkipId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<EkiplerTranslate>().GetAll().Result.Where(x => x.EkipId == EkipId && x.Diller.DilKodlari.DilKodu == dil).FirstOrDefault();
            return model;
        }

        public IEnumerable<FotografGalerileriTranslate> GetGaleriKategori()
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<FotografGalerileriTranslate>().GetAll().Result.Where(x => x.FotografGalerileri.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.FotografGalerileri.Sira).ToList();
            return model;
        }
        public IEnumerable<FotografGaleriResimleri> GetGaleri(int galeriId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<FotografGaleriResimleri>().GetAll().Result.Where(x => x.FotografGaleriId == galeriId).OrderBy(x => x.Sira).ToList();
            return model;
        }
        public async Task<UrunlerTranslate> GetUrun(int urunId)
        {
            var dil = GetCurrentCulture();
            var urun = await _uow.Repository<Urunler>().GetById(urunId);
            var model = urun.UrunlerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }

        public object GetUrunFiyat(int urunId, bool formatted = false)
        {
            var dil = GetCurrentCulture();
            var urun = _uow.Repository<Urunler>().GetById(urunId).Result;

            decimal fiyat = Convert.ToDecimal(urun.SizeOzelFiyat > 0 ? urun.SizeOzelFiyat : urun.ListeFiyat);

            if (formatted)
            {
                var siteAyari = GetSiteAyari().Result;

                if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.USD)
                {
                    return GetKurTLCeviri(fiyat, FiyatTipleri.BayiFiyat,  ParaBirimi.USD).Result.ToString("C2");
                }
                else if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.EUR)
                {
                    return FormatCurrency(fiyat, FiyatTipleri.ListeFiyat, ParaBirimi.EUR.ToString());
                }
                else
                {
                    return FormatCurrency(fiyat, FiyatTipleri.ListeFiyat, ParaBirimi.TRY.ToString());
                }
            }

            return fiyat;
        }
        public object GetUrunAraFiyat(int urunId, int adet, bool formatted = false, FiyatTipleri fiyatTipi = FiyatTipleri.ListeFiyat)
        {
            var urun = _uow.Repository<Urunler>().GetById(urunId).Result;
            var bayi = GetUye().Result;

            // 1) Birim fiyatı fiyatTipi'ne göre belirle
            decimal birimFiyat;

            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                if ((urun.SizeOzelFiyat) > 0m)
                {
                    birimFiyat = urun.SizeOzelFiyat;
                }
                else
                {
                    decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
                    decimal faktor = 1m - iskontoOrani;
                    birimFiyat = (urun.ListeFiyat) * faktor;
                }
            }
            else if (fiyatTipi == FiyatTipleri.SizeOzelFiyat)
            {
                birimFiyat = (urun.SizeOzelFiyat) > 0m
                    ? (urun.SizeOzelFiyat)
                    : urun.ListeFiyat;
            }
            else // FiyatTipleri.ListeFiyat
            {
                birimFiyat = urun.ListeFiyat;
            }

            // 2) Ara fiyat = birim fiyat * adet
            decimal fiyat = birimFiyat * adet;

            if (!formatted)
                return fiyat; // düz decimal

            // 3) Formatlı çıktı (site para birimi USD/EUR olsa bile ekranda TL sembolü)
            var siteAyari = GetSiteAyari().Result;

            if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.USD)
            {
                var tlTutar = GetKurTLCeviri(fiyat, fiyatTipi, ParaBirimi.USD).Result;  // TL döner
                return FormatCurrency(tlTutar, fiyatTipi, ParaBirimi.TRY.ToString(), false);
            }
            else if (siteAyari.SiteAyarlari.ParaBirimId == (int)ParaBirimi.EUR)
            {
                var tlTutar = GetKurTLCeviri(fiyat, fiyatTipi, ParaBirimi.EUR).Result;  // TL döner
                return FormatCurrency(tlTutar, fiyatTipi, ParaBirimi.TRY.ToString(), false);
            }
            else
            {
                // Zaten TL
                return FormatCurrency(fiyat, fiyatTipi, ParaBirimi.TRY.ToString(), false);
            }
        }


        public IEnumerable<UrunlerTranslate> GetUrunler()
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.Urunler.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Urunler.Sira).ToList();
            return model;
        }

        public IEnumerable<UrunlerTranslate> GetDigerUrunler(int urunId)
        {
            var dil = GetCurrentCulture();
            var urunToKategorileri = _uow.Repository<UrunToKategori>().GetAll().Result.Where(x => x.UrunId == urunId).Select(x => x.KategoriId);
            var urunToUrunler = _uow.Repository<UrunToKategori>().GetAll().Result.Where(x => urunToKategorileri.Contains(x.KategoriId) && x.UrunId != urunId).Select(x => x.UrunId);

            var model = _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => urunToUrunler.Contains(x.UrunId) && x.Urunler.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Urunler.Sira).ToList();
            return model;
        }
        public IEnumerable<UrunResimleri> GetUrunResimleri(int urunId, UrunResimKategorileri urunResimKategori)
        {
            var allUrunResimleri = _uow.Repository<UrunResimleri>().GetAll().Result.AsQueryable();

            var query = allUrunResimleri
                            .Where(x => x.UrunId == urunId && x.UrunResimKategori == urunResimKategori)
                            .OrderBy(x => x.Sira);

            if (urunResimKategori == UrunResimKategorileri.UrunResim)
            {
                query = (IOrderedQueryable<UrunResimleri>)query.Skip(1);
            }

            return query.ToList();
        }


        public IEnumerable<DosyaGaleri> GetDosyalar(int dosyaId)
        {
            var dosyaListesi = _uow.Repository<DosyaGaleri>().GetAll().Result.AsQueryable();

            var query = dosyaListesi
                            .Where(x => x.DosyaId == dosyaId)
                            .OrderBy(x => x.Sira);
            return query.ToList();
        }


        public async Task<string> GetUrunResim(int urunId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Urunler/").Result;
            var model = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string urunResim = model?.Resim;
            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                urunResim = trDilmodel.Resim;
            }
            return urunResim;
        }
        public UrunResimleri GetUrunResimFirst(int urunId, UrunResimKategorileri urunResimKategori)
        {
            var model = _uow.Repository<UrunResimleri>().GetAll().Result
                        .Where(x => x.UrunId == urunId && x.UrunResimKategori == urunResimKategori)
                        .OrderBy(x => x.Sira) 
                        .FirstOrDefault();

            if (model == null)
            {
                model = new UrunResimleri
                {
                    Resim = "/Content/Upload/Images/resimyok.png"
                };
            }

            return model;
        }



        public IEnumerable<UrunSecenekleriTranslate> GetUrunSecenekleri(int urunId)
        {
            var dil = GetCurrentCulture();

            var urunToUrunSecenek = _uow.Repository<UrunToUrunSecenek>().GetAll().Result.Where(x => x.UrunId == urunId).Select(x => x.UrunSecenekId).Distinct();
            var urunSecenekleri = _uow.Repository<UrunSecenekleriTranslate>().GetAll().Result.Where(x => urunToUrunSecenek.Contains(x.Id)).ToList();

            return urunSecenekleri;
        }

        public IEnumerable<UrunToUrunSecenekToUrunDeger> GetUrunToUrunSecenekToUrunDeger(int urunId)
        {
            var dil = GetCurrentCulture();

            var urunToUrunSecenek = _uow.Repository<UrunToUrunSecenek>().GetAll().Result.Where(x => x.UrunId == urunId).Select(x => x.UrunSecenekId).Distinct();
            var urunSecenekDegerleri = _uow.Repository<UrunToUrunSecenekToUrunDeger>().GetAll().Result.Where(x => urunToUrunSecenek.Contains(x.UrunSecenekId)).ToList();

            return urunSecenekDegerleri;
        }


        public UrunSecenekDegerleriTranslate GetUrunSecenekDeger(int urunSecenekDegerId)
        {
            var dil = GetCurrentCulture();

            var model = _uow.Repository<UrunSecenekDegerleriTranslate>().GetAll().Result.Where(x => x.UrunSecenekDegerId == urunSecenekDegerId).FirstOrDefault();

            return model;
        }

        public IEnumerable<UrunlerTranslate> GetBenzerUrunler(int urunId)
        {
            var dil = GetCurrentCulture();

            var urunToBenzerUrun = _uow.Repository<UrunToBenzerUrun>().GetAll().Result.Where(x => x.UrunId == urunId).Select(x => x.BenzerUrunId).Distinct();
            var benzerUrunler = _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => urunToBenzerUrun.Contains(x.Id)).ToList();

            return benzerUrunler;
        }

        public IEnumerable<UrunlerTranslate> GetTamamlayiciUrunler(int urunId)
        {
            var dil = GetCurrentCulture();

            var urunToTamamlayiciUrun = _uow.Repository<UrunToTamamlayiciUrun>().GetAll().Result.Where(x => x.UrunId == urunId).Select(x => x.TamamlayiciUrunId).Distinct();
            var tamamlayiciUrunler = _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => urunToTamamlayiciUrun.Contains(x.Id)).ToList();

            return tamamlayiciUrunler;
        }

        public async Task<string> GetUrunYoutubeKapakResim(int urunId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Urunler/").Result;
            var model = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string youtubeKapakResim = model?.YoutubeResim;
            if (model?.YoutubeResim == "/Content/Upload/Images/resimyok.png" || model?.YoutubeResim == null)
            {
                youtubeKapakResim = trDilmodel.YoutubeResim;
            }
            return youtubeKapakResim;
        }
        public async Task<string> GetUrunDosya(int urunId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Dosyalar/").Result;
            var model = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string urunDosya = model?.Dosya;
            if (string.IsNullOrEmpty(model?.Dosya))
            {
                urunDosya = trDilmodel.Dosya;
            }
            return urunDosya;
        }
        public async Task<string> GetUrunDosya2(int urunId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Dosyalar/").Result;
            var model = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<UrunlerTranslate>().GetAll().Result.Where(x => x.UrunId == urunId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string urunDosya = model?.Dosya2;
            if (string.IsNullOrEmpty(model?.Dosya2))
            {
                urunDosya = trDilmodel.Dosya2;
            }
            return urunDosya;
        }
        public async Task<UrunlerTranslate> GetOncekiUrun(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var sayfa = await _uow.Repository<UrunlerTranslate>().GetAll();

            var oncekiSayfa = sayfa.Where(x => x.UrunId < sayfaId).OrderByDescending(p => p.UrunId).FirstOrDefault();
            var oncekiSayfaId = oncekiSayfa?.Id ?? -1;

            var model = await sayfa.SingleOrDefaultAsync(x => x.Id == oncekiSayfaId && x.Diller.DilKodlari.DilKodu == dil);

            return model;
        }

        public async Task<UrunlerTranslate> GetSonrakiUrun(int sayfaId)
        {
            var dil = GetCurrentCulture();
            var sayfa = await _uow.Repository<UrunlerTranslate>().GetAll();

            var sonrakiSayfa = sayfa.Where(x => x.UrunId > sayfaId).OrderBy(p => p.UrunId).FirstOrDefault();
            var sonrakiSayfaId = sonrakiSayfa?.Id ?? -1;

            var model = await sayfa.SingleOrDefaultAsync(x => x.Id == sonrakiSayfaId && x.Diller.DilKodlari.DilKodu == dil);

            return model;
        }

        public async Task<List<Markalar>> GetMarkalar()
        {
            var model = await _uow.Repository<Markalar>().GetAll().Result.Where(x => x.Durum == SayfaDurumlari.Aktif).OrderBy(x => x.Sira).ToListAsync();
            return model;
        }

        //Tıklanan Kategoriye Ait Olan Markaları Listeleme
        public async Task<List<Markalar>> GetKategoriMarkalari(int kategoriId)
        {
            var kategori = _uow.Repository<UrunToKategori>().GetAll().Result.Where(x => x.KategoriId == kategoriId).Select(x => x.UrunId);
            var marka = _uow.Repository<Urunler>().GetAll().Result.Where(x => x.MarkaId != null && kategori.Contains(x.Id)).Select(x => x.MarkaId);

            var model = await _uow.Repository<Markalar>().GetAll().Result.Where(x => x.Durum == SayfaDurumlari.Aktif && marka.Contains(x.Id)).OrderBy(x => x.Sira).ToListAsync();
            return model;
        }
        //public async Task<KategorilerTranslate> GetKategori(int kategoriId)
        //{
        //    var dil = GetCurrentCulture();
        //    var kategori = await _uow.Repository<Kategoriler>().GetById(kategoriId);
        //    var model = kategori.KategorilerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == dil);
        //    return model;
        //}


        public async Task<KategorilerTranslateDTO> GetKategori(int kategoriId)
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"KategoriTranslate-{kategoriId}-{dil}";

            var cached = await _cacheService.GetAsync<KategorilerTranslateDTO>(cacheKey);
            if (cached != null)
                return cached;

            var kategori = await _uow.Repository<Kategoriler>()
                .GetAll().Result
                .Where(k => k.Id == kategoriId)
                .Select(k => new
                {
                    Translate = k.KategorilerTranslate.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == dil),
                    k.Resim,
                    k.Ikon,
                    AltKategoriler = k.AltKategoriler
                        .Where(a => a.Durum == SayfaDurumlari.Aktif)
                        .Select(alt => new
                        {
                            Translate = alt.KategorilerTranslate.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == dil),
                            alt.Id,
                            alt.Resim,
                            alt.Ikon
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (kategori?.Translate == null)
                return null;

            var dto = new KategorilerTranslateDTO
            {
                KategoriId = kategori.Translate.KategoriId,
                KategoriAdi = kategori.Translate.KategoriAdi,
                Aciklama = kategori.Translate.Aciklama,
                ResimYolu = kategori.Resim,
                Ikon = kategori.Ikon,
                AltKategoriler = kategori.AltKategoriler
                    .Where(a => a.Translate != null)
                    .Select(a => new KategorilerTranslateDTO
                    {
                        KategoriId = a.Translate.KategoriId,
                        KategoriAdi = a.Translate.KategoriAdi,
                        Aciklama = a.Translate.Aciklama,
                        ResimYolu = a.Resim,
                        Ikon = a.Ikon
                    }).ToList()
            };

            await _cacheService.SetAsync(cacheKey, dto, 360000); // 1 saat

            return dto;
        }


        private KategorilerTranslateDTO MapKategoriRecursive(Kategoriler kategori, string dil)
        {
            var translate = kategori.KategorilerTranslate.FirstOrDefault(t => t.Diller.DilKodlari.DilKodu == dil);

            if (translate == null) return null;

            return new KategorilerTranslateDTO
            {
                KategoriId = translate.KategoriId,
                KategoriAdi = translate.KategoriAdi,
                Aciklama = translate.Aciklama,
                ResimYolu = kategori.Resim,
                Ikon = kategori.Ikon,
                AltKategoriler = kategori.AltKategoriler?
                    .Where(k => k.Durum == SayfaDurumlari.Aktif)
                    .Select(k => MapKategoriRecursive(k, dil))
                    .Where(x => x != null)
                    .ToList()
            };
        }



        public async Task<List<KategorilerTranslateDTO>> GetKategoriler()
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"KategorilerListesiDTO-{dil}";

            // 1. Cache'ten oku
            var cached = await _cacheService.GetAsync<List<KategorilerTranslateDTO>>(cacheKey);
            if (cached != null)
                return cached;

            // 2. Veritabanından çek (yalnızca gerekli alanları map'le)
            var model = await _uow.Repository<KategorilerTranslate>()
                .GetAll().Result
                .Where(x => x.Kategoriler.Durum == SayfaDurumlari.Aktif &&
                            x.KategoriId != 1 &&
                            x.Diller.DilKodlari.DilKodu == dil)
                .OrderBy(x => x.Kategoriler.Sira)
                .Select(x => new KategorilerTranslateDTO
                {
                    KategoriId = x.KategoriId,
                    KategoriAdi = x.KategoriAdi,
                    Aciklama = x.Aciklama,
                    ResimYolu = x.Kategoriler.Resim,
                    Ikon = x.Kategoriler.Ikon
                })
                .ToListAsync();

            // 3. Cache'e yaz
            await _cacheService.SetAsync(cacheKey, model, 360000); // 1 saat

            return model;
        }



        public IEnumerable<KategorilerTranslate> GetAltKategoriler(int kategoriId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<KategorilerTranslate>().GetAll().Result.Where(x => x.Kategoriler.ParentKategoriId == kategoriId && x.Kategoriler.Durum == SayfaDurumlari.Aktif && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Kategoriler.Sira).ToList();
            return model;
        }

        public async Task<List<KategorilerTranslate>> GetUrunKategori(int urunId)
        {
            var model = _uow.Repository<UrunToKategori>().GetAll().Result.Where(x => x.UrunId == urunId).Select(x => x.KategoriId);
            var kategori = await _uow.Repository<KategorilerTranslate>().GetAll().Result.Where(x => model.Contains(x.KategoriId)).ToListAsync();

            return kategori;
        }


        public IEnumerable<KategoriBanner> GetKategoriBanner(int kategoriId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<KategoriBanner>().GetAll().Result.Where(x => x.KategoriId == kategoriId && x.Diller.DilKodlari.DilKodu == dil).OrderBy(x => x.Sira).ToList();
            return model;
        }


        public int GetKategoriSayisi(int kategoriId)
        {
            var kategoriSayisi = _uow.Repository<UrunToKategori>()
             .GetAll().Result
             .Where(x => x.KategoriId == kategoriId && x.Kategoriler.Durum == SayfaDurumlari.Aktif)
             .Select(x => x.KategoriId)
             .Distinct()
             .Count();
            return kategoriSayisi;
        }
        public IEnumerable<UrunToOzellik> GetUrunOzellikleri(int urunId)
        {
            var dil = GetCurrentCulture();

            var model = _uow.Repository<UrunToOzellik>().GetAll().Result.Where(x => x.UrunId == urunId && x.Diller.DilKodlari.DilKodu == dil).ToList().OrderBy(x => x.UrunOzellikleri.Sira);
            return model;
        }

        public async Task<string> GetUrunOzellikResim(int urunId, int urunOzellikId)
        {
            var dil = GetCurrentCulture();
            var resimDizini = GetResimDizini("Urunler/").Result;
            var model = await _uow.Repository<UrunToOzellik>().GetAll().Result.Where(x => x.UrunId == urunId && x.UrunOzellikId == urunOzellikId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            var trDilmodel = await _uow.Repository<UrunToOzellik>().GetAll().Result.Where(x => x.UrunId == urunId && x.UrunOzellikId == urunOzellikId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == "tr-TR");
            string urunOzellikResim = model?.Resim;
            if (model?.Resim == "/Content/Upload/Images/resimyok.png" || model?.Resim == null)
            {
                urunOzellikResim = trDilmodel.Resim;
            }
            return urunOzellikResim;
        }

        public async Task<KategorilerTranslate> GetUrunKategoriBilgi(int kategoriId)
        {
            var dil = GetCurrentCulture();

            var model = await _uow.Repository<KategorilerTranslate>().GetAll().Result.SingleOrDefaultAsync(x => x.KategoriId == kategoriId && x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }
        public int GetUrunSayisi(int kategoriId)
        {
            var urunSayisi = _uow.Repository<UrunToKategori>()
             .GetAll().Result
             .Where(x => x.KategoriId == kategoriId && x.Kategoriler.Durum == SayfaDurumlari.Aktif)
             .Select(x => x.UrunId)
             .Distinct()
             .Count();
            return urunSayisi;
        }
        public List<KategorilerTranslate> GetBreadcrumbKategori(int kategoriId)
        {
            var dil = GetCurrentCulture();

            // Tüm kategoriler alınır ve filtrelenir.
            var kategoriler = _uow.Repository<KategorilerTranslate>().GetAll().Result
                .Where(x => x.KategoriId != 1 && x.Diller.DilKodlari.DilKodu == dil).ToList();

            // Breadcrumb kategorileri için bir liste oluşturulur.
            List<KategorilerTranslate> breadcrumbCategories = new List<KategorilerTranslate>();

            // Mevcut kategori bulunur.
            KategorilerTranslate currentCategory = kategoriler.FirstOrDefault(k => k.KategoriId == kategoriId);
            if (currentCategory != null)
            {
                breadcrumbCategories.Add(currentCategory);

                // Üst kategorilere doğru ilerlenir.
                while (currentCategory.Kategoriler.ParentKategoriId != 1)
                {
                    currentCategory = kategoriler.FirstOrDefault(k => k.KategoriId == currentCategory.Kategoriler.ParentKategoriId);
                    if (currentCategory != null)
                    {
                        breadcrumbCategories.Insert(0, currentCategory); // Baştan eklenir.
                    }
                }
            }

            return breadcrumbCategories;
        }

        // Sadece alt kategorileri almak için bir metod eklenir.
        public List<KategorilerTranslate> GetChildCategories(int kategoriId)
        {
            var dil = GetCurrentCulture();

            // Tüm kategoriler alınır ve filtrelenir.
            var kategoriler = _uow.Repository<KategorilerTranslate>().GetAll().Result
                .Where(x => x.KategoriId != 1 && x.Diller.DilKodlari.DilKodu == dil).ToList();

            // Sadece doğrudan alt kategoriler filtrelenir.
            return kategoriler.Where(x => x.Kategoriler.ParentKategoriId == kategoriId).ToList();
        }






        //public async Task<List<ModullerTranslate>> GetModuller(ModulTipleri modulTipi)
        //{
        //    var dil = GetCurrentCulture();
        //    var model = await _uow.Repository<ModullerTranslate>()
        //                          .GetAll()
        //                          .Result.Where(x => x.Moduller.ModulTipi == modulTipi && x.Diller.DilKodlari.DilKodu == dil && x.Moduller.Durum == SayfaDurumlari.Aktif)
        //                          .OrderBy(x => x.Moduller.Sira)
        //                          .ToListAsync();

        //    return model;
        //}


        public async Task<List<ModullerTranslateDTO>> GetModuller(ModulTipleri modulTipi)
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"ModullerListesi-{modulTipi}-{dil}";

            var model = await _cacheService.GetAsync<List<ModullerTranslateDTO>>(cacheKey);

            if (model == null)
            {
                var rawData = await _uow.Repository<ModullerTranslate>()
                                        .GetAll()
                                        .Result
                                        .Where(x => x.Moduller.ModulTipi == modulTipi &&
                                                    x.Diller.DilKodlari.DilKodu == dil &&
                                                    x.Moduller.Durum == SayfaDurumlari.Aktif)
                                        .OrderBy(x => x.Moduller.Sira)
                                        .ToListAsync();

                model = rawData.Select(x => new ModullerTranslateDTO
                {
                    ModulAdi = x.ModulAdi,
                    EntityId = x.Moduller.EntityId,
                    ModulId = x.ModulId,
                    DilId = x.DilId
                }).ToList();

                await _cacheService.SetAsync(cacheKey, model, 360000);
            }

            return model;
        }


        public class ModullerTranslateDTO
        {
            public string ModulAdi { get; set; }
            public int EntityId { get; set; }
            public int ModulId { get; set; }
            public int DilId { get; set; }
        }

      





        public async Task<ModullerTranslate> GetModul(int modulId)
        {
            var dil = GetCurrentCulture();

            var model = await _uow.Repository<ModullerTranslate>().GetAll().Result.Where(x => x.ModulId == modulId).SingleOrDefaultAsync(x => x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }

        public async Task<IEnumerable<KategorilerTranslate>> GetOneCikanKategoriler(int oneCikanKategoriId)
        {
            var dil = GetCurrentCulture();

            var oneCikanKategori = await _uow.Repository<OneCikanKategoriToKategoriler>()
                .GetAll().Result
                .Where(x => x.OneCikanKategoriId == oneCikanKategoriId)
                .OrderBy(x => x.Sira)
                .Select(x => x.KategoriId)
                .ToListAsync();

            var model = await _uow.Repository<KategorilerTranslate>()
                .GetAll().Result
                .Where(x => x.Kategoriler.Durum == SayfaDurumlari.Aktif &&
                            x.Diller.DilKodlari.DilKodu == dil &&
                            oneCikanKategori.Contains(x.KategoriId))
                .ToListAsync();

            return model.OrderBy(x => oneCikanKategori.IndexOf(x.KategoriId));
        }



        public async Task<OneCikanUrunler> GetOneCikanUrunModul(int oneCikanUrunModulId)
        {
            var dil = GetCurrentCulture();

            var model = await _uow.Repository<OneCikanUrunler>().GetAll().Result.Where(x => x.Id == oneCikanUrunModulId).FirstOrDefaultAsync();
            return model;
        }

        //public async Task<List<UrunlerTranslate>> GetOneCikanUrunler(int oneCikanUrunId)
        //{
        //    var dil = GetCurrentCulture();

        //    var oneCikanUrun = await _uow.Repository<OneCikanUrunToUrunler>()
        //        .GetAll().Result
        //        .Where(x => x.OneCikanUrunId == oneCikanUrunId)
        //        .OrderBy(x => x.Sira)
        //        .Select(x => x.UrunId)
        //        .ToListAsync();

        //    var model = await _uow.Repository<UrunlerTranslate>()
        //        .GetAll().Result
        //        .Where(x => x.Urunler.Durum == SayfaDurumlari.Aktif &&
        //                    oneCikanUrun.Contains(x.UrunId) &&
        //                    x.Diller.DilKodlari.DilKodu == dil)
        //        .ToListAsync();

        //    model = model.OrderBy(x => oneCikanUrun.IndexOf(x.UrunId)).ToList();

        //    return model;
        //}

        public class UrunlerTranslateDTO
        {
            public int UrunId { get; set; }
            public string UrunAdi { get; set; }
            public string UrunKodu { get; set; }
            public string Aciklama { get; set; }
            public string MarkaAdi { get; set; }
            public string Ozellik { get; set; }
            public int Stok { get; set; }
            public decimal Fiyat { get; set; }
            public string Resim { get; set; }

        }

        public async Task<List<UrunlerTranslateDTO>> GetOneCikanUrunler(int oneCikanUrunId)
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"OneCikanUrunler-{oneCikanUrunId}-{dil}";

            var model = await _cacheService.GetAsync<List<UrunlerTranslateDTO>>(cacheKey);
            if (model != null)
                return model;

            var oneCikanUrun = await _uow.Repository<OneCikanUrunToUrunler>()
                .GetAll().Result
                .Where(x => x.OneCikanUrunId == oneCikanUrunId)
                .OrderBy(x => x.Sira)
                .Select(x => x.UrunId)
                .ToListAsync();

            var urunler = await _uow.Repository<UrunlerTranslate>()
                .GetAll().Result
                .Where(x => x.Urunler.Durum == SayfaDurumlari.Aktif &&
                            oneCikanUrun.Contains(x.UrunId) &&
                            x.Diller.DilKodlari.DilKodu == dil)
                .ToListAsync();

            // Doğru sıralama
            urunler = urunler.OrderBy(x => oneCikanUrun.IndexOf(x.UrunId)).ToList();

            // DTO'ya map et
            model = urunler.Select(x => new UrunlerTranslateDTO
            {
                UrunId = x.UrunId,
                UrunAdi = x.UrunAdi,
                UrunKodu = x.Urunler.UrunKodu,
                MarkaAdi = x.Urunler?.Markalar?.MarkaAdi,
                Aciklama = x.Aciklama,
                Stok = x.Urunler.Stok,
                Fiyat = x.Urunler.ListeFiyat,
                Ozellik = x.Ozellik,
                Resim = x.Resim
            }).ToList();

            await _cacheService.SetAsync(cacheKey, model, 360000); // 1 saat cache

            return model;
        }


        //public async Task<List<KategorilerTranslate>> GetOneCikanUrunToKategori(int oneCikanUrunId)
        //{
        //    var dil = GetCurrentCulture();

        //    var oneCikanUrun = await _uow.Repository<OneCikanUrunToKategoriler>()
        //        .GetAll().Result
        //        .Where(x => x.OneCikanUrunId == oneCikanUrunId)
        //        .OrderBy(x => x.Sira)
        //        .Select(x => x.KategoriId)
        //        .ToListAsync();

        //    var model = await _uow.Repository<KategorilerTranslate>()
        //        .GetAll().Result
        //        .Where(x => x.Kategoriler.Durum == SayfaDurumlari.Aktif &&
        //                    oneCikanUrun.Contains(x.KategoriId) &&
        //                    x.Diller.DilKodlari.DilKodu == dil)
        //        .ToListAsync();

        //    model = model.OrderBy(x => oneCikanUrun.IndexOf(x.KategoriId)).ToList();

        //    return model;
        //}

        public class KategorilerTranslateDTO
        {
            public int KategoriId { get; set; }
            public string KategoriAdi { get; set; }
            public string Aciklama { get; set; }
            public string ResimYolu { get; set; }
            public string Ikon { get; set; }

            public List<KategorilerTranslateDTO> AltKategoriler { get; set; } // 👈 recursive yapı için süper

        }

        public async Task<List<KategorilerTranslateDTO>> GetOneCikanUrunToKategori(int oneCikanUrunId)
        {
            var dil = GetCurrentCulture();
            string cacheKey = $"OneCikanUrunToKategori-{oneCikanUrunId}-{dil}";

            var model = await _cacheService.GetAsync<List<KategorilerTranslateDTO>>(cacheKey);
            if (model != null)
                return model;

            var oneCikanUrun = await _uow.Repository<OneCikanUrunToKategoriler>()
                .GetAll().Result
                .Where(x => x.OneCikanUrunId == oneCikanUrunId)
                .OrderBy(x => x.Sira)
                .Select(x => x.KategoriId)
                .ToListAsync();

            var kategoriler = await _uow.Repository<KategorilerTranslate>()
                .GetAll().Result
                .Where(x => x.Kategoriler.Durum == SayfaDurumlari.Aktif &&
                            oneCikanUrun.Contains(x.KategoriId) &&
                            x.Diller.DilKodlari.DilKodu == dil)
                .ToListAsync();

            kategoriler = kategoriler.OrderBy(x => oneCikanUrun.IndexOf(x.KategoriId)).ToList();

            model = kategoriler.Select(x => new KategorilerTranslateDTO
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                Aciklama = x.Aciklama,
                ResimYolu = x.Kategoriler.BreadcrumbResim,
                Ikon = x.Kategoriler.Ikon
            }).ToList();

            await _cacheService.SetAsync(cacheKey, model, 360000); // 1 saat cache

            return model;
        }




        //public IEnumerable<OneCikanUrunResimleri> GetOneCikanUrunResimleri(int oneCikanUrunId)
        //{
        //    var model = _uow.Repository<OneCikanUrunResimleri>().GetAll().Result.Where(x => x.OneCikanUrunId == oneCikanUrunId).OrderBy(x => x.Sira).ToList();
        //    return model;
        //}

        public class OneCikanUrunResimleriDTO
        {
            public int Id { get; set; }
            public int OneCikanUrunId { get; set; }
            public string ResimYolu { get; set; }
            public int Sira { get; set; }
        }
        public async Task<List<OneCikanUrunResimleriDTO>> GetOneCikanUrunResimleri(int oneCikanUrunId)
        {
            string cacheKey = $"OneCikanUrunResimleri-{oneCikanUrunId}";

            var model = await _cacheService.GetAsync<List<OneCikanUrunResimleriDTO>>(cacheKey);
            if (model != null)
                return model;

            var rawData = await _uow.Repository<OneCikanUrunResimleri>()
                .GetAll().Result
                .Where(x => x.OneCikanUrunId == oneCikanUrunId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            model = rawData.Select(x => new OneCikanUrunResimleriDTO
            {
                Id = x.Id,
                OneCikanUrunId = x.OneCikanUrunId,
                ResimYolu = x.Resim,
                Sira = x.Sira
            }).ToList();

            await _cacheService.SetAsync(cacheKey, model, 360000); // 1 saat cache

            return model;
        }



        public async Task<int?> GetAktifKategori(int urunId)
        {
            var model = await _uow.Repository<UrunToKategori>().GetAll();
            var kategori = model.FirstOrDefault(x => x.UrunId == urunId)?.KategoriId;
            return kategori;
        }


        public int GetMarkaSayisi(int markaId)
        {
            var markaSayisi = _uow.Repository<Urunler>()
                 .GetAll().Result
                 .Where(x => x.MarkaId == markaId && x.Durum == SayfaDurumlari.Aktif)
                 .Count();
            return markaSayisi;
        }

        public async Task<SabitMenulerTranslate> GetSabitMenu(SabitSayfaTipleri sayfaTipi)
        {
            var dil = GetCurrentCulture();
            var sabitMenu = _uow.Repository<SabitMenuler>().GetAll().Result.Where(x => x.SayfaTipi == sayfaTipi).FirstOrDefault();
            var model = sabitMenu.SabitMenulerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == dil);
            return model;
        }

        public async Task<SeoUrl> GetSeoUrl(int entityId, SeoUrlTipleri seoUrlTipi)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SeoUrl>().GetAll().Result.SingleOrDefaultAsync(x => x.EntityId == entityId && x.EntityName == seoUrlTipi && x.Diller.DilKodlari.DilKodu == dil);
            if (seoUrlTipi == SeoUrlTipleri.Marka || seoUrlTipi == SeoUrlTipleri.Markalar)
            {
                model = await _uow.Repository<SeoUrl>().GetAll().Result.SingleOrDefaultAsync(x => x.EntityId == entityId && x.EntityName == seoUrlTipi);
            }
            return model;
        }

        public async Task<AppUser> GetUye()
        {
            var userId = Convert.ToInt32(this._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = await _context.Users.Where(p => p.Id == userId).FirstOrDefaultAsync();
            return model;
        }


        public async Task<List<RolTipleri>> GetUyeRolleri(int uyeId)
        {
            var user = await _userManager.FindByIdAsync(uyeId.ToString());
            var roles = await _userManager.GetRolesAsync(user);

            List<RolTipleri> rolTipleriListesi = new List<RolTipleri>();

            foreach (var role in roles)
            {
                if (Enum.TryParse(role, out RolTipleri rolTipi))
                {
                    rolTipleriListesi.Add(rolTipi);
                }
            }

            return rolTipleriListesi;
        }

        public async Task<(AppUser User, IList<string> Roles)> GetUyeLoginMi()
        {
            var uye = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated &&
            (_httpContextAccessor.HttpContext.User.IsInRole("Bayi") || _httpContextAccessor.HttpContext.User.IsInRole("Administrator"));
            if (!uye)
            {
                return (null, null);
            }

            var uyeBilgi = await GetUye();
            var uyeRolleri = await _userManager.GetRolesAsync(uyeBilgi);
            return (uyeBilgi, uyeRolleri);
        }




        public async Task<List<SiparisGecmisleri>> GetSiparisGecmisi(int siparisId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SiparisGecmisleri>()
                                 .GetAll().Result
                                 .Where(x => x.SiparisId == siparisId)
                                 .OrderByDescending(x => x.Id)
                                 .ToListAsync();
            return model;
        }

        public async Task<string> GetSiparisDurumu(int siparisId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<SiparisGecmisleri>().GetAll().Result.Where(x => x.SiparisId == siparisId).OrderBy(x => x.Id).LastOrDefaultAsync();

            var siparisDurumu = model.SiparisDurumlari.SiparisDurumlariTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == dil)?.SiparisDurumu;

            return siparisDurumu;
        }


        public async Task<KuponToSiparis> GetKuponIndirimTutari(int siparisId)
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<KuponToSiparis>().GetAll().Result.Where(x => x.SiparisId == siparisId).FirstOrDefaultAsync().Result;

            return model;
        }

        public IEnumerable<KargoMetodlariTranslate> GetKargoMetodlari()
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<KargoMetodlariTranslate>().GetAll().Result.Where(x => x.KargoMetodlari.Durum == SayfaDurumlari.Aktif).ToList().OrderBy(x => x.KargoMetodlari.Sira);
            return model;
        }
        public KargoMetodlariTranslate GetAktifKargo(decimal siparisToplam)
        {
            var dil = GetCurrentCulture();

            var sartliOdeme = _uow.Repository<EticaretWebCoreEntity.KargoMetodlari>().GetAll().Result.FirstOrDefault(x => x.Id == (int)EticaretWebCoreEntity.Enums.KargoMetodlari.Ucretsiz);

            var model = _uow.Repository<KargoMetodlariTranslate>().GetAll().Result;
            KargoMetodlariTranslate result = null;

            if (siparisToplam > sartliOdeme.Fiyat)
            {
                result = model.FirstOrDefault(x => x.KargoMetodId == (int)EticaretWebCoreEntity.Enums.KargoMetodlari.Ucretsiz);
            }
            else
            {
                result = model.FirstOrDefault(x => x.KargoMetodId == (int)EticaretWebCoreEntity.Enums.KargoMetodlari.SartliOdeme);
            }

            return result;
        }

        public string GetKargoFiyat(decimal siparisToplam)
        {
            var dil = GetCurrentCulture();

            var sartliOdeme = _uow.Repository<EticaretWebCoreEntity.KargoMetodlari>().GetAll().Result.FirstOrDefault(x => x.Id == (int)EticaretWebCoreEntity.Enums.KargoMetodlari.Ucretsiz);

            var model = _uow.Repository<KargoMetodlariTranslate>().GetAll().Result;
            var result = "";

            if (siparisToplam > sartliOdeme.Fiyat)
            {
                result = "Ücretsiz";
            }
            else
            {
                result = model.FirstOrDefault(x => x.KargoMetodId == (int)EticaretWebCoreEntity.Enums.KargoMetodlari.SartliOdeme).KargoMetodlari.Fiyat.ToString("C2");
            }

            return result;
        }

        public IEnumerable<OdemeMetodlariTranslate> GetOdemeMetodlari()
        {
            var dil = GetCurrentCulture();
            var model = _uow.Repository<OdemeMetodlariTranslate>().GetAll().Result.Where(x => x.OdemeMetodlari.Durum == SayfaDurumlari.Aktif).ToList().OrderBy(x => x.OdemeMetodlari.Sira);
            return model;
        }


        public async Task<PaytrIframeTransaction> GetPaytrIframeTransaction(int siparisId)
        {
            var dil = GetCurrentCulture();
            var model = await _uow.Repository<PaytrIframeTransaction>().GetAll().Result.Where(x => x.SiparisId == siparisId).FirstOrDefaultAsync();
            return model;
        }

        public async Task<IEnumerable<Adresler>> GetUyeAdresler()
        {
            var uye = await GetUye();

            var model = await _uow.Repository<Adresler>().GetAll().Result.Where(x => x.UyeId == uye.Id).ToListAsync();
            return model;
        }



        public async Task<string> GetResimDizini(string resimKlasoru)
        {
            var model = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim).Remove(0, 7) + resimKlasoru;

            return model;
        }
        public async Task<int> GetColumnSayisi(Device cihaz, ColumnSayisi column)
        {
            int gosterimSayisi = 0;
            switch (column)
            {
                case ColumnSayisi.Bir:
                    gosterimSayisi = 12;
                    break;
                case ColumnSayisi.iki:
                    gosterimSayisi = 6;
                    break;
                case ColumnSayisi.Uc:
                    gosterimSayisi = 4;

                    break;
                case ColumnSayisi.Dort:
                    gosterimSayisi = 3;

                    break;
                case ColumnSayisi.Alti:
                    gosterimSayisi = 2;

                    break;
                default:
                    break;
            }
            return gosterimSayisi;
        }


        //YENİ FONKSİYONLAR

        public async Task<decimal> ComputeFiyatAsync(
           decimal fiyat,
           FiyatTipleri fiyatTipi,
           ParaBirimi paraBirimi,
           bool dovizDurum,
           int? urunId = null)                 // <-- yeni parametre
        {
            // 0) Fiyat tipi SizeOzel ise: üründen oku
            if (fiyatTipi == FiyatTipleri.SizeOzelFiyat)
            {
                if (urunId == null)
                    throw new ArgumentException("SizeOzelFiyat için UrunId gereklidir.", nameof(urunId));

                var urun = await _uow.Repository<Urunler>().GetById(urunId.Value);
                if (urun == null)
                    throw new InvalidOperationException("Ürün bulunamadı.");

                var sizeOzel = urun.SizeOzelFiyat;
                if (sizeOzel > 0m)
                    fiyat = sizeOzel;
                else
                    fiyat = urun.ListeFiyat;   // SizeÖzel yoksa ListeFiyat'a düş
            }

            // 1) Bayi iskontosu (sadece BayiFiyat)
            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                var bayi = await GetUye();
                decimal iskonto = (bayi?.IskontoOrani ?? 0m) / 100m;
                if (iskonto > 0m)
                    fiyat -= fiyat * iskonto;

                // NOT: İstersen burada "üründe SizeOzelFiyat > 0 ise BayiFiyat'ta da onu baz al" kuralını da ekleyebilirim.
                // Mevcut isteğine göre BayiFiyat sadece iskonto uygular.
            }

            // 2) Kur çevirisi: hedef her zaman TRY
            if (dovizDurum && paraBirimi != ParaBirimi.TRY)
            {
                var kur = await GetKur((int)paraBirimi);
                if (kur <= 0m)
                    throw new InvalidOperationException("Kur değeri geçersiz.");

                fiyat *= kur;
            }

            // 3) 2 ondalık
            return Math.Round(fiyat, 2, MidpointRounding.AwayFromZero);
        }

        public async Task<string> FormatFiyatAsync(
            decimal fiyat,
            FiyatTipleri fiyatTipi,
            ParaBirimi paraBirimi,
            bool dovizDurum,
            bool paraBirimiGosterimi, int? urunId = null)
        {
            // Hesaplanmış net değeri al (indirim + gerekirse kur)
            var net = await ComputeFiyatAsync(fiyat, fiyatTipi, paraBirimi, dovizDurum, urunId);

            // Hangi para birimi ile gösterilecek?
            var gosterimParaBirimi = dovizDurum ? ParaBirimi.TRY : paraBirimi;

            if (!paraBirimiGosterimi)
            {
                // Sadece sayı (sembol yok)
                return net.ToString("N2", CultureInfo.InvariantCulture);
            }

            // Sembollü gösterim — culture eşleme
            var cultureName = ParaBirimiToCulture(gosterimParaBirimi);
            var culture = new CultureInfo(cultureName);

            return net.ToString("C2", culture);
        }

        // önce: private static string ParaBirimiToCulture(ParaBirimi pb)
        public static string ParaBirimiToCulture(ParaBirimi pb)
        {
            return pb switch
            {
                ParaBirimi.TRY => "tr-TR",
                ParaBirimi.USD => "en-US",
                ParaBirimi.EUR => "de-DE",
                _ => CultureInfo.InvariantCulture.Name
            };
        }


        //YENİ FONKSİYONLAR


        public async Task<decimal> GetKur(int ParaBirimId)
        {
            if (ParaBirimId == (int)ParaBirimi.TRY)
                return 1m;

            using (var context = new AppDbContext()) // kendi DbContext ismini yaz
            {
                var kur = await context.Kur
                    .Where(k => k.ParaBirimId == ParaBirimId)
                    .Select(k => k.TLKur)
                    .FirstOrDefaultAsync();

                return kur;
            }
        }





        public async Task<decimal> GetKurTLCeviri(decimal dovizFiyat, FiyatTipleri fiyatTipi, ParaBirimi paraBirimi)
        {
            // 1) Gerekirse bayi iskontosu uygula
            decimal fiyatNet = dovizFiyat;
            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                var bayi = await GetUye();
                decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
                if (iskontoOrani > 0m)
                    fiyatNet -= fiyatNet * iskontoOrani;
            }

            // 2) Para birimi TRY ise direkt dön
            if (paraBirimi == ParaBirimi.TRY)
                return fiyatNet;

            // 3) Para birimi kaydı
            var parabirimDeger = await _context.ParaBirimleri
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Kodu == paraBirimi.ToString());
            if (parabirimDeger == null)
                throw new InvalidOperationException("Para birimi bulunamadı.");

            // 4) Kur bilgisi
            var kur = await _uow.Repository<Kur>().GetAll().Result
                .Where(x => x.ParaBirimId == parabirimDeger.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (kur == null)
                throw new InvalidOperationException("Kur bilgisi bulunamadı.");

            // 5) Kura göre TL çevirisi (her zaman TL döner)
            var tlTutar = fiyatNet * kur.TLKur;
            return tlTutar;
        }


        public string FormatCurrency(decimal fiyat, FiyatTipleri fiyatTipi, string paraBirimi, bool useInvariant = true)
        {
            var bayi = GetUye().Result;
            var paraBirimleri = _context.ParaBirimleri.ToList();
            var paraBirimiCultures = new Dictionary<string, string>();

            foreach (var item in paraBirimleri)
            {
                if (!string.IsNullOrEmpty(item.DilKodu?.DilKodu))
                {
                    paraBirimiCultures[item.Kodu] = item.DilKodu.DilKodu;
                }
            }

            // İskonto kontrolü
            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;

                // İskonto oranı > 0 ise uygula, değilse fiyatı aynen bırak
                if (iskontoOrani > 0m)
                    fiyat -= fiyat * iskontoOrani;
            }
            // ListeFiyat ve SizeOzelFiyat’ta herhangi bir indirim yapılmıyor

            // Culture eşleşmesi kontrolü
            if (paraBirimleri.Any() && paraBirimiCultures.TryGetValue(paraBirimi, out string cultureName))
            {
                CultureInfo culture = new CultureInfo(cultureName);

                if (useInvariant)
                {
                    // Sadece sayı formatı
                    return fiyat.ToString("N2", CultureInfo.InvariantCulture);
                }
                else
                {
                    // Para birimi formatı
                    return fiyat.ToString("C2", culture);
                }
            }

            // Varsayılan dönüş
            return useInvariant
                ? fiyat.ToString("N2", CultureInfo.InvariantCulture) // sadece sayı
                : fiyat.ToString("C2", CultureInfo.InvariantCulture); // kodlu/formatlı
        }


        public string GetHostUrl()
        {
            var hosturl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            return hosturl;
        }

        public string GetMenuUrl(int menuId, MenuTipleri menuTipi, MenuYerleri menuYeri)
        {
            var menuUrl = _context.MenulerTranslate.Where(p => p.MenuId == menuId && p.Menuler.MenuTipi == menuTipi && p.Menuler.MenuYeri == menuYeri && p.Diller.DilKodlari.DilKodu == GetAktifDil().Result.DilKodlari.DilKodu).FirstOrDefault();
            string hostUrl = "/";
            if (menuTipi == MenuTipleri.Dosyalar && menuYeri == menuUrl.Menuler.MenuYeri)
            {
                hostUrl = GetHostUrl() + menuUrl.Url;
            }
            else
            {
                hostUrl = menuUrl.Url;
            }
            return hostUrl;
        }

        public int GetUrl(string url)
        {
            
            var entityId = _uow.Repository<SeoUrl>().GetAll().Result.Where(x => x.Url == url).FirstOrDefault().EntityId;
            return entityId;
        }
        public async Task<int> GetSiparisSayisi()
        {
            var uye = await GetUye();
            var siparisler = await _uow.Repository<Siparisler>().GetAll();
            var model = siparisler.Where(x => x.UyeId == uye.Id).Count();
            return model;
        }


        public async Task<PriceResult> GetPriceAsync(
    decimal fiyat,
    FiyatTipleri fiyatTipi,
    ParaBirimi paraBirimi,
    bool dovizDurum,
    int? urunId = null)
        {
            // Size Özel ise üründen oku (varsa), yoksa Liste
            if (fiyatTipi == FiyatTipleri.SizeOzelFiyat)
            {
                if (urunId == null)
                    throw new ArgumentException("SizeOzelFiyat için UrunId gereklidir.", nameof(urunId));

                var urun = await _uow.Repository<Urunler>().GetById(urunId.Value);
                if (urun == null) throw new InvalidOperationException("Ürün bulunamadı.");

                var sizeOzel = urun.SizeOzelFiyat;
                fiyat = sizeOzel > 0m ? sizeOzel : urun.ListeFiyat;
            }

            // BayiFiyat ise iskonto uygula
            if (fiyatTipi == FiyatTipleri.BayiFiyat)
            {
                var bayi = await GetUye();
                var isk = (bayi?.IskontoOrani ?? 0m) / 100m;
                if (isk > 0m) fiyat -= fiyat * isk;
            }

            // Kur çevir: hedef TRY, gösterim para birimi de TRY olsun
            var displayPB = paraBirimi;
            if (dovizDurum && paraBirimi != ParaBirimi.TRY)
            {
                var kur = await GetKur((int)paraBirimi);
                if (kur <= 0m) throw new InvalidOperationException("Kur değeri geçersiz.");
                fiyat *= kur;
                displayPB = ParaBirimi.TRY;
            }

            var net = Math.Round(fiyat, 2, MidpointRounding.AwayFromZero);
            return new PriceResult(net, displayPB);
        }

        // PriceResult: public record PriceResult(decimal Net, ParaBirimi ParaBirimi);

        // PriceResult: public record PriceResult(decimal Net, ParaBirimi ParaBirimi);

        public async Task<PriceResult> GetLineSubtotalAsync(
            decimal fiyat,
            FiyatTipleri fiyatTipi,
            ParaBirimi paraBirimi,
            bool dovizDurum,
            int adet,
            int? urunId = null,
            bool roundPerUnit = false) // true: birimi yuvarlayıp çarp, false: toplamı yuvarla
        {
            // Gösterim para birimi mantığı GetPriceAsync ile aynı
            var displayPB = (dovizDurum && paraBirimi != ParaBirimi.TRY)
                ? ParaBirimi.TRY
                : paraBirimi;

            if (adet <= 0)
                return new PriceResult(0m, displayPB);

            // Birim fiyatı hesapla
            var unit = await GetPriceAsync(fiyat, fiyatTipi, paraBirimi, dovizDurum, urunId);

            decimal line;
            if (roundPerUnit)
            {
                var unitRounded = Math.Round(unit.Net, 2, MidpointRounding.AwayFromZero);
                line = unitRounded * adet;
            }
            else
            {
                line = unit.Net * adet;
                line = Math.Round(line, 2, MidpointRounding.AwayFromZero);
            }

            return new PriceResult(line, displayPB);
        }

        public async Task<object> KdvToplamByUrunAsync(
    int urunId,
    FiyatTipleri fiyatTipi,
    bool dovizDurum = false,
    bool formatted = false,
    bool paraBirimiGosterimi = true)
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["SepetCookie"];
            if (string.IsNullOrEmpty(sepetIdCookie))
                return formatted
                    ? await FormatFiyatAsync(0m, fiyatTipi, ParaBirimi.TRY, false, paraBirimiGosterimi)
                    : 0m;

            // Bayi iskonto faktörü
            var bayi = await GetUye();
            decimal iskontoOrani = (bayi?.IskontoOrani ?? 0m) / 100m;
            decimal faktor = 1m - iskontoOrani;

            bool isBayi = fiyatTipi == FiyatTipleri.BayiFiyat;
            bool isSizeOzel = fiyatTipi == FiyatTipleri.SizeOzelFiyat; // nadiren kullanacaksan da kalsın

            // Sadece bu ürüne ait KDV toplamı
            decimal kdvToplamRaw = await _context.Sepet
                .Where(c => c.CookieId == sepetIdCookie && c.UrunId == urunId) // <-- ihtiyaca göre p.Urunler.Id == urunId
                .Select(p => (decimal?)(

                    // ► Birim fiyat seçimi (ürün bazında)
                    (
                        isBayi
                            ? (
                                // Bayi: SizeOzel uygunsa onu, değilse iskontolu ListeFiyat
                                ((p.Urunler.SizeOzelFiyat) > 0m) &&
                                (p.Urunler.OzelFiyatStokSarti > 0) &&
                                (p.Adet >= p.Urunler.OzelFiyatStokSarti)
                                    ? (p.Urunler.SizeOzelFiyat)
                                    : ((p.Urunler.ListeFiyat) * faktor)
                              )
                            : (
                                // Diğer tipler: isSizeOzel ise varsa SizeOzel, yoksa Liste; aksi halde Liste
                                isSizeOzel
                                    ? (((p.Urunler.SizeOzelFiyat) > 0m)
                                        ? (p.Urunler.SizeOzelFiyat)
                                        : (p.Urunler.ListeFiyat))
                                    : (p.Urunler.ListeFiyat)
                              )
                    )
                    * (decimal)p.Adet
                    *
                    // ► KDV oranı
                    (((p.Urunler.Kdv != null ? (decimal?)p.Urunler.Kdv.KdvOrani : (decimal?)0m) ?? 0m) / 100m)

                ))
                .SumAsync() ?? 0m;

            // Kur/gösterim
            var siteAyari = await GetSiteAyari();
            var sitePB = (ParaBirimi)siteAyari.SiteAyarlari.ParaBirimId;

            // Not: KDV tutarına iskonto tekrar uygulanmaz; sadece kur çevirisi yapılır
            decimal kdvToplam = await ComputeFiyatAsync(
                kdvToplamRaw, FiyatTipleri.ListeFiyat, sitePB, dovizDurum);

            if (!formatted)
                return kdvToplam;

            var gosterimPB = dovizDurum ? ParaBirimi.TRY : sitePB;
            return paraBirimiGosterimi
                ? kdvToplam.ToString("C2", new CultureInfo(ParaBirimiToCulture(gosterimPB)))
                : kdvToplam.ToString("N2", CultureInfo.InvariantCulture);
        }


    }
}
