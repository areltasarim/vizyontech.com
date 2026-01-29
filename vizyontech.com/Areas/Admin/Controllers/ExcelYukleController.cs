using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vizyontech.com.Controllers;
using System.Security.Claims;
using EticaretWebCoreService;


namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class ExcelYukleController : Controller
    {
        private readonly AppDbContext _context;
        ExcelImportServis _excelimportServis = null;

        [Obsolete]
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        private IProgressReporterFactory _progressReporterFactory;


        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        private readonly SeoServis _seoServis;

        private readonly string entityBaslik = "Excel Yükle";
        private readonly string entityAltBaslik = "Excel Yükle";

        [Obsolete]
        public ExcelYukleController(AppDbContext _context, SeoServis _seoServis, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IProgressReporterFactory progressReporterFactory, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {
            this._context = _context;
            this._seoServis = _seoServis;


            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;

            _excelimportServis = new ExcelImportServis(_context, _seoServis, hostingEnvironment, httpContextAccessor, progressReporterFactory, _userManager, _signInManager, _roleManager);

        }

        public IActionResult Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            #region Excel Convert Dışarı aktarılan excelllerde açarken hata veriyon ondan dolayı yeniden convert işlemi yapılıyor
            //var bozukEzcel = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcel", "sariEtiketler.csv.xls");
            //var duzeltilmisExcel = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcel", "sariEtiketlerOnarildi.xls");
            //ExcelConverter.ConvertToExcel(bozukEzcel, duzeltilmisExcel);
            #endregion

            return View();
        }

        public async Task<ResultViewModel> ExelImport(IFormCollection excelForm, string connectionId, string submit)
        {


            var result = new ResultViewModel();

            var uyeId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var uye = _context.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(uyeId));

            var model = await _excelimportServis.UrunEkleGuncelle(excelForm, connectionId, submit);

            return model;

        }
    }
}
