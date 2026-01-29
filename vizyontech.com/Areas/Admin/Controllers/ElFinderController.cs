using EticaretWebCoreHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]
    public class ElFinderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FileManager()
        {
            return View();
        }

        public async Task<IActionResult> Connector()
        {
            var connector = ElFinderHelper.GetConnector(Request);


            var result = await connector.ProcessAsync(Request);
            if (result is JsonResult)
            {
                var json = result as JsonResult;
                return Content(JsonSerializer.Serialize(json.Value), json.ContentType);
            }
            else
            {
                return result;
            }

        }

        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = ElFinderHelper.GetConnector(Request);
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }


        [HttpPost]
        public IActionResult UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null;

            var fileName = upload.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/CkEditorElfinder/Assets/Images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);
            }

            var url = $"/Admin/CkEditorElfinder/Assets/Images/{fileName}";
            return Json(new { uploaded = true, url });
        }
    }
}
