using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class SitemapController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SitemapController(AppDbContext _context, IMemoryCache cache, IHostingEnvironment hostingEnvironment)
        {
            this._context = _context;
            _cache = cache;
            _hostingEnvironment = hostingEnvironment;

        }

        [Route("/sitemap.xml")]
        public IActionResult Index()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            string contentType = "application/xml";

            string cacheKey = "sitemap.xml";

            // For showing in browser (Without download)
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = cacheKey,
                Inline = true,
            };

            Response.Headers.Append("Content-Disposition", cd.ToString());

            // Cache
            var bytes = _cache.Get<byte[]>(cacheKey);
            if (bytes != null)
                return File(bytes, contentType);

            var sitemap = _context.SeoUrl.ToList().Where(p => p.EntityName != SeoUrlTipleri.DinamikSayfaDetay);

            var sb = new StringBuilder();
            sb.AppendLine($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine($"<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");
            sb.AppendLine($"xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            sb.AppendLine($"xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">");

            sb.AppendLine($"<url>");
            sb.AppendLine($"<loc>{baseUrl}</loc>");
            //sb.AppendLine($"<lastmod>{lastmod}</lastmod>");
            sb.AppendLine($"<changefreq>daily</changefreq>");
            sb.AppendLine($"<priority>0.8</priority>");
            sb.AppendLine($"</url>");

            foreach (var item in sitemap)
            {
                //var dt = m.LastModified;
                //string lastmod = $"{dt.Year}-{dt.Month.ToString("00")}-{dt.Day.ToString("00")}";

                sb.AppendLine($"<url>");
                sb.AppendLine($"<loc>{baseUrl}/{item.Url}</loc>");
                //sb.AppendLine($"<lastmod>{lastmod}</lastmod>");
                sb.AppendLine($"<changefreq>daily</changefreq>");
                sb.AppendLine($"<priority>0.8</priority>");
                sb.AppendLine($"</url>");
            }

            sb.AppendLine($"</urlset>");

            bytes = Encoding.UTF8.GetBytes(sb.ToString());

            //_cache.Set(cacheKey, bytes, TimeSpan.FromHours(24));



            string sitemapFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Seo");
            if (!System.IO.Directory.Exists(sitemapFolderPath))
            {
                System.IO.Directory.CreateDirectory(sitemapFolderPath);
            }
            string sitemapFilePath = Path.Combine(sitemapFolderPath, "sitemap.xml");
            System.IO.File.WriteAllText(sitemapFilePath, sb.ToString());

            return File(bytes, contentType);
        }

        [Route("/sitemap-images.xml")]
        public IActionResult ImageSitemap()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            string contentType = "application/xml";

            string cacheKey = "sitemap-images.xml";

            // For showing in browser (Without download)
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = cacheKey,
                Inline = true,
            };

            Response.Headers.Append("Content-Disposition", cd.ToString());

            // Cache
            var bytes = _cache.Get<byte[]>(cacheKey);
            if (bytes != null)
                return File(bytes, contentType);

            var sitemap = _context.SeoUrl.ToList().Where(p => p.EntityName != SeoUrlTipleri.DinamikSayfaDetay);

            var sb = new StringBuilder();
            sb.AppendLine($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine($"<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");
            sb.AppendLine($"xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\"");
            sb.AppendLine($"xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            sb.AppendLine($"xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">");

            // Ana sayfa için (eğer ana sayfada resim varsa buraya eklenebilir)
            sb.AppendLine($"<url>");
            sb.AppendLine($"<loc>{baseUrl}</loc>");
            sb.AppendLine($"</url>");

            foreach (var item in sitemap)
            {
                var url = $"{baseUrl}/{item.Url}";
                var hasImages = false;
                var imageList = new List<(string imageUrl, string title, string caption)>();

                // Ürün resimleri
                if (item.EntityName == SeoUrlTipleri.Urun)
                {
                    var urunResimleri = _context.UrunResimleri
                        .Where(r => r.UrunId == item.EntityId && !string.IsNullOrEmpty(r.Resim))
                        .OrderBy(r => r.Sira)
                        .ToList();

                    foreach (var resim in urunResimleri)
                    {
                        var imageUrl = resim.Resim.StartsWith("http") ? resim.Resim : $"{baseUrl}/{resim.Resim.TrimStart('/')}";
                        var title = !string.IsNullOrEmpty(resim.ResimAdi) ? resim.ResimAdi : "";
                        imageList.Add((imageUrl, title, ""));
                        hasImages = true;
                    }
                }
                // Fotoğraf galeri resimleri
                else if (item.EntityName == SeoUrlTipleri.Galeri || item.EntityName == SeoUrlTipleri.EKatalog)
                {
                    var galeriResimleri = _context.FotografGaleriResimleri
                        .Where(r => r.FotografGaleriId == item.EntityId && !string.IsNullOrEmpty(r.Resim))
                        .OrderBy(r => r.Sira)
                        .ToList();

                    foreach (var resim in galeriResimleri)
                    {
                        var imageUrl = resim.Resim.StartsWith("http") ? resim.Resim : $"{baseUrl}/{resim.Resim.TrimStart('/')}";
                        imageList.Add((imageUrl, "", ""));
                        hasImages = true;
                    }
                }
                // Sayfa resimleri
                else if (item.EntityName == SeoUrlTipleri.Hakkimizda ||
                         item.EntityName == SeoUrlTipleri.Referanslar ||
                         item.EntityName == SeoUrlTipleri.Projeler ||
                         item.EntityName == SeoUrlTipleri.Hizmetlerimiz ||
                         item.EntityName == SeoUrlTipleri.Cozumlerimiz ||
                         item.EntityName == SeoUrlTipleri.Blog ||
                         item.EntityName == SeoUrlTipleri.Haberler)
                {
                    var sayfaResimleri = _context.SayfaResimleri
                        .Where(r => r.SayfaId == item.EntityId && !string.IsNullOrEmpty(r.Resim))
                        .OrderBy(r => r.Sira)
                        .ToList();

                    foreach (var resim in sayfaResimleri)
                    {
                        var imageUrl = resim.Resim.StartsWith("http") ? resim.Resim : $"{baseUrl}/{resim.Resim.TrimStart('/')}";
                        var title = !string.IsNullOrEmpty(resim.ResimAdi) ? resim.ResimAdi : "";
                        imageList.Add((imageUrl, title, ""));
                        hasImages = true;
                    }
                }

                // Eğer resim varsa URL'i ekle
                if (hasImages && imageList.Any())
                {
                    sb.AppendLine($"<url>");
                    sb.AppendLine($"<loc>{url}</loc>");

                    foreach (var image in imageList)
                    {
                        sb.AppendLine($"<image:image>");
                        sb.AppendLine($"<image:loc>{WebUtility.HtmlEncode(image.imageUrl)}</image:loc>");
                        if (!string.IsNullOrEmpty(image.title))
                        {
                            sb.AppendLine($"<image:title>{WebUtility.HtmlEncode(image.title)}</image:title>");
                        }
                        if (!string.IsNullOrEmpty(image.caption))
                        {
                            sb.AppendLine($"<image:caption>{WebUtility.HtmlEncode(image.caption)}</image:caption>");
                        }
                        sb.AppendLine($"</image:image>");
                    }

                    sb.AppendLine($"</url>");
                }
            }

            sb.AppendLine($"</urlset>");

            bytes = Encoding.UTF8.GetBytes(sb.ToString());

            // Cache'e kaydet (isteğe bağlı)
            //_cache.Set(cacheKey, bytes, TimeSpan.FromHours(24));

            // Dosyaya kaydet
            string sitemapFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Seo");
            if (!System.IO.Directory.Exists(sitemapFolderPath))
            {
                System.IO.Directory.CreateDirectory(sitemapFolderPath);
            }
            string sitemapFilePath = Path.Combine(sitemapFolderPath, "sitemap-images.xml");
            System.IO.File.WriteAllText(sitemapFilePath, sb.ToString());

            return File(bytes, contentType);
        }

    }
}
