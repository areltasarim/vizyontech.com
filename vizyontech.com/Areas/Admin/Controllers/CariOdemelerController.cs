using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreService.CariOdeme;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = "AdminAuth")]

    public class CariOdemelerController : Controller
    {
        private readonly CariOdemeServis _cariOdemeServis;
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Cari Ödemeler";

        public CariOdemelerController(AppDbContext _context, CariOdemeServis cariOdemeServis)
        {
            this._context = _context;
            _cariOdemeServis = cariOdemeServis;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;

            var model = await _cariOdemeServis.OdemeListesi();

            return View(model);
        }
     
    }
}
