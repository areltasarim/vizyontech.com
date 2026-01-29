using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class OdemeMetodlariController : Controller
    {
        OdemeMetodlariServis _odemeMetoduServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Ödeme Metodları";
        private readonly string entityAltBaslik = "Ödeme Metodu Ekle";
        public OdemeMetodlariController(AppDbContext _context)
        {
            this._context = _context;
            _odemeMetoduServis = new OdemeMetodlariServis(_context);
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _odemeMetoduServis.PageList();

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.OdemeMetodlari.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(OdemeMetoduViewModel Model, string submit)
        {
            var model = await _odemeMetoduServis.UpdatePage(Model, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                return RedirectToAction(model.Action, controllerValue, new { Id = model.SayfaId });
            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();

                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId });
            }
        }

        public async Task<IActionResult> Delete(OdemeMetoduViewModel Model)
        {

            var result = await _odemeMetoduServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = result.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _odemeMetoduServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
            ViewData["SiparisDurumlari"] = _context.SiparisDurumlari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.SiparisDurumlariTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").SiparisDurumu, Value = p.Id.ToString() });

        }

    }
}
