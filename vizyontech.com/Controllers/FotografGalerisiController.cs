using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class FotografGalerisiController : Controller
    {
        private readonly AppDbContext _context;

        public FotografGalerisiController(AppDbContext _context)
        {
            this._context = _context;
        }

        public IActionResult Index()
        {
            var model = _context.FotografGalerileri.Where(p=> p.Durum == SayfaDurumlari.Aktif).OrderBy(p=> p.Sira).ToList();

            return View(model);
        }

        public IActionResult Detay(string url)
        {
            var entityId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.FotografGaleriResimleri.ToList().Where(p => p.FotografGaleriId == entityId);

            return View(model);
        }
    }
}
