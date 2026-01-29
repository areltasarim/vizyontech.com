using AutoMapper;
using Devsense.PHP.Syntax;
using EticaretWebCoreCaching;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class MarkalarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly MarkalarServis _markaServis;
        private readonly IDataProtector _dataProtector;


        private readonly string entityBaslik = "Markalar";
        private readonly string entityAltBaslik = "Marka Ekle";

        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;


        public MarkalarController(AppDbContext _context, MarkalarServis _markaServis, ICacheService cacheService, IMapper mapper, IDataProtectionProvider dataProtectionProvider)
        {
            this._context = _context;
            this._markaServis = _markaServis;
            _cacheService = cacheService;
            _mapper = mapper;
            _dataProtector = dataProtectionProvider.CreateProtector("MarkalarController");
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _markaServis.PageList();
            model.Foreach(x =>
            {
                x.EncrypedId = _dataProtector.Protect(x.Id.ToString());
            });

            //int MusteriNo = 0;
            //string key = string.Format($"Anahtar-Marka-{MusteriNo}");
            //var result = await _cacheService.GetAsync<List<MarkaMapper>>(key, async () =>
            //{
            //    var markalar = await _markaServis.PageList();
            //    List<MarkaMapper> markaviewModel =  _mapper.Map<List<MarkaMapper>>(markalar);

            //    return markaviewModel.ToList();
            //});

            return View(model);
        }
        public async Task<IActionResult> AddOrUpdate(string Id)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            if (string.IsNullOrEmpty(Id))
            {
                // Yeni kayıt ekleme durumu
                var yeniModel = new Markalar(); // Marka senin entity sınıfının ismi
                return View(yeniModel);
            }

            try
            {
                var decryptedId = int.Parse(_dataProtector.Unprotect(Id));
                var model = await _context.Markalar.FindAsync(decryptedId);

                if (model == null)
                {
                    return NotFound(); // id varsa ama eşleşen veri yoksa
                }

                return View(model);
            }
            catch
            {
                return BadRequest("Geçersiz Id");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(string id, MarkaViewModel Model, string submit)
        {

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    Model.Id = int.Parse(_dataProtector.Unprotect(id));
                }
                catch
                {
                    return BadRequest("Geçersiz ID");
                }
            }

            var result = await _markaServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel()
            {
                Type = result.MesajDurumu,
                Text = result.Mesaj
            });

            return RedirectToAction(
                result.Basarilimi == true ? "Index" : result.Action,
                controllerValue,
                new { Id = result.SayfaId } // burada şifresiz gerçek ID'yi gönderiyoruz
            );
        }


        public async Task<IActionResult> Delete(MarkaViewModel Model)
        {
            var model = await _markaServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _markaServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> Index1()
        {
            int MusteriNo = 0;
            string key = string.Format($"Anahtar-Marka-{MusteriNo}");

            var result = await _cacheService.GetAsync<List<SelectListItem>>(key, async () =>
            {
                var tmpresult = await _markaServis.PageList();
                return tmpresult.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.MarkaAdi
                }).ToList();
            });
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
