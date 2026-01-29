using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class KuponlarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly KuponServis _KuponServis;

        private readonly string entityBaslik = "Kuponlar";
        private readonly string entityAltBaslik = "Kupon Ekle";


        public KuponlarController(AppDbContext _context, KuponServis _KuponServis)
        {
            this._context = _context;
            this._KuponServis = _KuponServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _KuponServis.PageList();
            return View(model);
        }
        public async Task<IActionResult> AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            KuponViewModel model = new KuponViewModel()
            {
                Kupon = _context.Kuponlar.Find(Id),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(KuponViewModel Model, string submit)
        {

            var model = await _KuponServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction(model.Basarilimi == true ? "Index" : model.Action, controllerValue, new { Id = model.SayfaId });

        }
        public IActionResult KuponAutoComplete(string term)
        {

            var result = _context.Urunler.ToList().Where(x => x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi.ToLower().Contains(term.ToLower())).Select(x => new { value = x.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu.ToLower() == "tr-TR".ToLower()).UrunAdi, id = x.Id.ToString() });

            return Json(result);
        }
        public async Task<IActionResult> Delete(KuponViewModel Model)
        {
            var model = await _KuponServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _KuponServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

       
    }
}
