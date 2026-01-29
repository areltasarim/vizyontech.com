using AutoMapper;
using EticaretWebCoreCaching;
using EticaretWebCoreCaching.Abstraction;
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

    public class PaytrController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PaytrServis _paytrServis;

        private readonly string entityBaslik = "Paytr";
        private readonly string entityAltBaslik = "Paytr Güncelle";

        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;


        public PaytrController(AppDbContext _context, PaytrServis _paytrServis, ICacheService cacheService, IMapper mapper)
        {
            this._context = _context;
            this._paytrServis = _paytrServis;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IActionResult> AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            ViewData["SiparisDurumlari"] = _context.SiparisDurumlari.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.SiparisDurumlariTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").SiparisDurumu, Value = p.Id.ToString() });
            ViewData["Diller"] = _context.Diller.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.DilAdi, Value = p.Id.ToString() });
            ViewData["ParaBirimleri"] = _context.ParaBirimleri.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.ParaBirimAdi, Value = p.Id.ToString() });

            var model = await _context.Paytr.FindAsync(Id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(PaytrViewModel Model, string submit)
        {

            var model = await _paytrServis.UpdatePage(Model, submit);


            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction(model.Basarilimi == true ? "Index" : model.Action, "OdemeMetodlari", new { Id =  model.SayfaId });

        }
       
    }
}
