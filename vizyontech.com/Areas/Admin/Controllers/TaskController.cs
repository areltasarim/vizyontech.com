using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        private readonly string entityBaslik = "Task";
        private readonly string entityAltBaslik = "Task";
        private readonly ISchedulerFactory _schedulerFactory;

        public TaskController(AppDbContext _context, ISchedulerFactory schedulerFactory)
        {
            this._context = _context;
            _schedulerFactory = schedulerFactory;

        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            return View();
        }


        public class JobRequest
        {
            public string JobName { get; set; }
        }

        [IgnoreAntiforgeryToken]

        [HttpPost]
        public async Task<IActionResult> TaskCalistir([FromBody] JobRequest request)
        {
            if (string.IsNullOrEmpty(request?.JobName))
            {
                return BadRequest("Job name cannot be null or empty.");
            }

            string mesaj = "";
            if(request.JobName == "JobIslemUrunler")
            {
                mesaj = "Opaktan Ürün Çekim İşlemi Başlamıştır. Tahmini 5 Dk İçerisinde Ürünler Çekilmiş Olacaktır.";
            }
            if (request.JobName == "JobIslemUyeler")
            {
                mesaj = "Opaktan Üye Çekim İşlemi Başlamıştır. Tahmini 5 Dk İçerisinde Üyeler Çekilmiş Olacaktır.";
            }
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(request.JobName);

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.TriggerJob(jobKey);
                return Ok($"{mesaj}");
            }
            else
            {
                return NotFound($"Job {request.JobName} bulunamadı.");
            }
        }


    }
}
