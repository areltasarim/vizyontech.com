using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class BayilerController : Controller
    {
        private readonly AppDbContext _context;

        public BayilerController(AppDbContext _context)
        {
            this._context = _context;
        }

        public IActionResult Index(string url)
        {
            //url de Bayiler yazınca null olarak geliyor bunun sebebi ise Bayiler adında bir Controller olduğundan karışıklık oluyor (ikisininde aynı isimde olmasından kaynaklanıyor)  bundan dolayı aşağıdaki if kısmı yazılmıştır (Yada cotroller ismi değiştirilebilir böylece if kısmına gerek kalmayacaktır.)
            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();
            var urlseo = "";
            if (controllerValue == "Bayiler")
            {
                urlseo = "bayiler";
            }
            else
            {
                urlseo = url;
            }


            return View();
        }

        [Route("detay")]
        public IActionResult Detay(int ilkodu)
        {

            var model = _context.Bayiler.Where(p => p.IlId == ilkodu);

            return View(model);
        }

    }
}
