using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vizyontech.com.Controllers;
using System.Security.Claims;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreService;


namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class XmlProductImportController : Controller
    {
        private readonly AppDbContext _context;
        XmlProductImportServis _xmlproductimportServis = null;

        [Obsolete]
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        private IProgressReporterFactory _progressReporterFactory;


        private readonly string entityBaslik = "Xml Yükle";
        private readonly string entityAltBaslik = "Xml Yükle";


        public XmlProductImportController(AppDbContext _context, XmlProductImportServis _xmlproductimportServis)
        {
            this._context = _context;
            this._xmlproductimportServis = _xmlproductimportServis;
        }
        public IActionResult Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            return View();
        }

        public async Task<ResultViewModel> XmlImport(IFormCollection excelForm, string connectionId, string submit)
        {

            var result = new ResultViewModel();

            var model = await _xmlproductimportServis.UrunEkleGuncelle(excelForm, connectionId, submit);

            return model;

        }
    }
}
