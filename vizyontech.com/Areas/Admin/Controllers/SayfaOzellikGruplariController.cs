using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class SayfaOzellikGruplariController : Controller
    {
        private readonly SayfaOzellikGruplariServis _SayfaOzellikGrupServis;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Sayfa Özellik Grupları";
        private readonly string entityAltBaslik = "Grup Ekle";

        public SayfaOzellikGruplariController(AppDbContext _context, SayfaOzellikGruplariServis _SayfaOzellikGrupServis)
        {
            this._context = _context;
            this._SayfaOzellikGrupServis = _SayfaOzellikGrupServis;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _SayfaOzellikGrupServis.PageList();

            return View(model);
        }


        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            PopulateDropdown();

            var diller = _context.Diller.ToList() as IEnumerable<Diller>;
            if (Id > 0)
            {
                var Kategori = _context.SayfaOzellikGruplari.Find(Id);
                foreach (var item in diller)
                {
                    var SayfaOzellikGruplariTranslate = _context.SayfaOzellikGruplariTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (SayfaOzellikGruplariTranslate == null)
                    {
                        SayfaOzellikGruplariTranslate = new SayfaOzellikGruplariTranslate()
                        {
                            SayfaOzellikGrupId = Id,
                            GrupAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        _context.Entry(SayfaOzellikGruplariTranslate).State = EntityState.Added;
                    }
                }
                _context.SaveChanges();
                SayfaOzellikGrupViewModel model = new SayfaOzellikGrupViewModel()
                {
                    SayfaOzellikGrup = _context.SayfaOzellikGruplari.Find(Id),
                };
                PopulateDropdown();
                return View(model);
            }
            else
            {
                SayfaOzellikGrupViewModel Model = new SayfaOzellikGrupViewModel();

                foreach (var item in diller)
                {
                    var SayfaOzellikGruplariTranslate = Model.SayfaOzellikGrup.SayfaOzellikGruplariTranslate.FirstOrDefault(x => x.DilId == item.Id);
                    if (SayfaOzellikGruplariTranslate == null)
                    {
                        SayfaOzellikGruplariTranslate = new SayfaOzellikGruplariTranslate()
                        {
                            GrupAdi = "",
                            DilId = item.Id,
                            Diller = new Diller() { Id = item.Id, DilAdi = item.DilAdi }
                        };
                        Model.SayfaOzellikGrup.SayfaOzellikGruplariTranslate.Add(SayfaOzellikGruplariTranslate);
                    }
                }
                return View(Model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(SayfaOzellikGrupViewModel Model, string submit)
        {
            var model = await _SayfaOzellikGrupServis.UpdatePage(Model, submit);

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
                return RedirectToAction("AddOrUpdate", controllerValue, new { Id = model.SayfaId});

            }
        }
        public async Task<IActionResult> Delete(SayfaOzellikGrupViewModel Model)
        {

            var model = await _SayfaOzellikGrupServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _SayfaOzellikGrupServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }


        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }
    }
}
