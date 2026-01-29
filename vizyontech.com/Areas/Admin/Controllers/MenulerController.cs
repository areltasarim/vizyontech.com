using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class MenulerController : Controller
    {
        private readonly MenulerServis _menuServis;

        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Menüler";
        private readonly string entityAltBaslik = "Menü Ekle";
        public MenulerController(AppDbContext _context, MenulerServis menuServis)
        {
            this._context = _context;
            _menuServis = menuServis;
        }
        public IActionResult AddOrUpdate(int Id = 0, string MenuYeri = "1")
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.Menuler.Find(Id);

            if (Id > 0)
            {

                var dosya = _context.FotografGaleriResimleri.Find(model.EntityId);
                if(dosya != null)
                {
                    model.EntityId = dosya.Id;
                }
            }

            ViewData["Dosyalar"] = _context.FotografGaleriResimleri.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.Resim.Substring(42), Value = p.Id.ToString() });

            PopulateDropdown();

            SetMenuKonumlari();

            int Menukonumu;

            if (MenuYeri != null)
            {
                Menukonumu = Convert.ToInt32(MenuYeri);
            }
            else
            {
                Menukonumu = Convert.ToInt32(MenuYerleri.UstMenu);
            }

            ViewBag.AktifListe = Menukonumu;

            List<Menuler> menu = _context.Menuler.ToList().Where(p => p.Id != 1).Where(x => x.MenuYeri == (MenuYerleri)Enum.Parse(typeof(MenuYerleri), MenuYeri) && x.ParentMenuId == 1).OrderBy(x => x.Sira).ToList();

            ViewData["MenuListesi"] = menu;

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(MenuViewModel Model, MenuTipleri MenuTipi, MenuYerleri Menukonumu, int EntityMenuId)
        {

            var model = await _menuServis.UpdatePage(Model, MenuTipi, Menukonumu, EntityMenuId);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            PopulateDropdown();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("AddOrUpdate", controllerValue, new { MenuYeri = Convert.ToInt32(Menukonumu) });

        }



        public JsonResult DosyalariGetir(int id)
        {
            var model = _context.FotografGaleriResimleri.Where(p => p.FotografGaleriId == id).Select(x => new { Id = x.Id, Resim = x.Resim.Remove(0, 32) }).ToList();

            return Json(model);
        }


        public async Task<IActionResult> Delete(MenuViewModel Model)
        {

            var model = await _menuServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = model.SayfaUrl });

        }

        public JsonResult MenuOrder(string liste)
        {
            try
            {


                string durum = "";

                string gelen = liste.Replace("list[", "").Replace("]", "");
                string[] veriler = liste.TrimEnd('&').Split('&');
                foreach (string item in veriler)
                {
                    durum += item.Replace("list[", "").Replace("]", "").Replace("null", "1") + "|";
                }
                string deger = durum.TrimEnd('|');
                string[] sondeger = deger.Split('|');
                int i = 1;

                foreach (string gln in sondeger)
                {



                    string[] parcala = gln.Split('=');

                    var model = _context.Menuler.Find(Convert.ToInt32(parcala[0]));

                    string AnaID = parcala[0].ToString();
                    string UstID = parcala[1].ToString();



                    model.Sira = i;
                    model.ParentMenuId = Convert.ToInt32(UstID);
                    model.Id = Convert.ToInt32(AnaID);
                    _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();

                    //data.cmd("Update menu set ustID='" + UstID + "' where id=" + AnaID + "");
                    //data.cmd("Update menu set sira='" + i + "' where id=" + AnaID + "");
                    i++;
                }

                return Json(new ResultViewModel { Basarilimi = true, Mesaj = "Menü listesi güncellendi", NotfyAlert = true, BootBoxAlert = false });

            }
            catch (Exception ex)
            {

                return Json(new ResultViewModel { Basarilimi = false, Mesaj = ex.Message, NotfyAlert = false, BootBoxAlert = true });
            }


        }

        public JsonResult MenuDelete(string id)
        {
            int _id = Convert.ToInt32(id);

            var model = _context.Menuler.Find(_id);

            try
            {
                int menuVarmi = _context.Menuler.Where(x => x.ParentMenuId == _id).Count();

                bool menuKontrol = true;
                if (menuVarmi > 0)
                {
                    menuKontrol = true;
                }
                else
                {
                    menuKontrol = false;
                }


                if (menuKontrol == false)
                {
                    _context.Entry(model).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new ResultViewModel { Basarilimi = true, Mesaj = "Silme işlemi başarılı", NotfyAlert = true, BootBoxAlert = false });
                }
                else
                {
                    return Json(new ResultViewModel { Basarilimi = false, Mesaj = "Menu silmek icin önce alt menulerini silmelisiniz.", NotfyAlert = false, BootBoxAlert = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new ResultViewModel { Basarilimi = false, Mesaj = ex.StackTrace, NotfyAlert = false, BootBoxAlert = true });
            }


        }

        public void SetMenuKonumlari()
        {
            List<MenuKonumlari> Konum = Enum.GetValues(typeof(MenuYerleri))
                 .Cast<MenuYerleri>()
                 .Select(e => new MenuKonumlari
                 {
                     Id = (int)e,
                     Name = e.GetType()
                             .GetMember(e.ToString())[0]
                             .GetCustomAttributes(typeof(DisplayAttribute), false)
                             .OfType<DisplayAttribute>()
                             .FirstOrDefault()?.Name ?? e.ToString()
                 }).ToList();
            ViewBag.MenuKonumlari = Konum;
        }


        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;

            ViewData["DinamikSayfalar"] = _context.Sayfalar.ToList().Where(p => p.Id != 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToPageTree()), Value = p.Id.ToString() });
            ViewData["Kategoriler"] = _context.Kategoriler.ToList().Where(p => p.Id != 1).AsQueryable().Select(p => new SelectListItem() { Text = string.Join(" > ", p.ToCategoryTree()), Value = p.Id.ToString() });
            ViewData["Urunler"] = _context.Urunler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.UrunlerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi, Value = p.Id.ToString() });

            ViewData["SabitMenuler"] = _context.SabitMenuler.ToList().AsQueryable().Select(p => new SelectListItem() { Text = p.SabitMenulerTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").MenuAdi, Value = p.Id.ToString() });
            ViewData["FotografGalerileri"] = _context.FotografGalerileri.ToList().Where(p => p.GaleriTipi == GaleriTipleri.Galeri).AsQueryable().Select(p => new SelectListItem() { Text = p.FotografGalerileriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").GaleriAdi, Value = p.Id.ToString() });
            ViewData["EKatalog"] = _context.FotografGalerileri.ToList().Where(p => p.GaleriTipi == GaleriTipleri.EKatalog).AsQueryable().Select(p => new SelectListItem() { Text = p.FotografGalerileriTranslate.SingleOrDefault(x => x.Diller.DilKodlari.DilKodu == "tr-TR").GaleriAdi, Value = p.Id.ToString() });
        }

    }
}
