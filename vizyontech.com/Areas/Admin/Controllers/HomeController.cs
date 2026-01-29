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
            return View();
        }

        /// <summary>
        /// Şehir bazlı canlı kullanıcı verileri
        /// Cache: 5 dakika, Rate Limit: 1 concurrent request
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLiveCityData()
        {
            try
            {
                var cityData = await _analyticsService.GetRealtimeUsersByCityAsync();
                int total = cityData?.Values.Sum() ?? 0;

                return Json(new
                {
                    success = true,
                    total,
                    cities = cityData ?? new Dictionary<string, int>()
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Canlı aktif kullanıcı sayısı
        /// Cache: 5 dakika, Rate Limit: 1 concurrent request
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLiveUsers()
        {
            try
            {
                var count = await _analyticsService.GetRealtimeActiveUsersAsync();
                return Json(new { success = true, activeUsers = count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// En çok ziyaret edilen sayfalar (günlük)
        /// Cache: 5 dakika, Rate Limit: 1 concurrent request
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLivePageData()
        {
            try
            {
                var result = await _analyticsService.GetTopPagesAsync();
                return Json(new { success = true, data = result ?? new Dictionary<string, int>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Cihaz bazlı canlı kullanıcı verileri
        /// Cache: 5 dakika, Rate Limit: 1 concurrent request
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLiveDeviceData()
        {
            try
            {
                var result = await _analyticsService.GetRealtimeUsersByDeviceAsync();
                return Json(new { success = true, data = result ?? new Dictionary<string, int>() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        /// <summary>
        /// Analytics cache'ini temizler (test/debug için)
        /// </summary>
        [HttpGet]
        public IActionResult ClearAnalyticsCache()
        {
            try
            {
                var cacheService = HttpContext.RequestServices.GetRequiredService<EticaretWebCoreCaching.Abstraction.ICacheService>();
                
                // Tüm analytics cache'lerini temizle
                cacheService.Remove("analytics:active_users");
                cacheService.Remove("analytics:users_by_city");
                cacheService.Remove("analytics:top_pages");
                cacheService.Remove("analytics:users_by_device");
                
                return Json(new { success = true, message = "Analytics cache temizlendi. Lütfen sayfayı yenileyin." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }




    }
}
