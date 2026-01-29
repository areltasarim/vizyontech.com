using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService.ZiraatPay;
using EticaretWebCoreViewModel;
using Google.Apis.Analytics.v3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;
using AnalyticsService = EticaretWebCoreHelper.AnalyticsService;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly AnalyticsService _analyticsService;
        private readonly ZiraatPayServis _ziraatPayService;

        public HomeController(AppDbContext _context, ILogger<HomeController> logger, EticaretWebCoreHelper.AnalyticsService analyticsService, ZiraatPayServis ziraatPayService)
        {
            _logger = logger;
            this._context = _context;
            _analyticsService = analyticsService;
            _ziraatPayService = ziraatPayService;
        }

        //[Authorize(Policy = "Permissions.Uyeler.Roller.View")]
        //NOT SAYFANIN ÜSTÜNDEKİ AuthenticationSchemes = "AdminAuth" BU OLUNCA ÇALIŞMIYOR VE ACCESS DENİED SAYFASI GELİYOR


        public async Task<ActionResult> Index()
        {
            //var data = await _analyticsService.GetActiveUsersByCountryAsync();
            //var data = await _analyticsService.GetActiveUsersByCityAsync();
            //var data = await _analyticsService.GetRealtimeUsersByCityAsync();
            
            //var ziraat = await _ziraatPayService.StartPaymentAsync();
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetLiveCityData()
        {
            var cityData = await _analyticsService.GetRealtimeUsersByCityAsync();
            int total = cityData.Values.Sum();

            return Json(new
            {
                total,
                cities = cityData
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetLiveUsers()
        {
            var count = await _analyticsService.GetRealtimeActiveUsersAsync();
            return Json(new { activeUsers = count });
        }
        [HttpGet]
        public async Task<IActionResult> GetLivePageData()
        {
            var result = await _analyticsService.GetTopPagesAsync(); // <-- await EKLENDİ
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLiveDeviceData()
        {
            var result = await _analyticsService.GetRealtimeUsersByDeviceAsync();
            return Json(result);
        }




    }
}
