using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class ProjelerController : Controller
    {
        private readonly AppDbContext _context;

        public ProjelerController(AppDbContext _context)
        {
            this._context = _context;
        }

        public IActionResult Index(string url)
        {
            var kategoriId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.Sayfalar.ToList().Where(p => p.ParentSayfaId == kategoriId || kategoriId == null).Where(p => p.ParentSayfaId != 1).OrderBy(p => p.Sira);

            return View(model);
        }

        public IActionResult Detay(int Id)
        {
            var model = _context.Sayfalar.Find(Id);
            return View(model);
        }


    }
}
