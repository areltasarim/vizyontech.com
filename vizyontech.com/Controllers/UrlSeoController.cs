using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using vizyontech.com;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EticaretWebCoreService;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class UrlSeoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly HelperServis _helperServis;

        IHttpContextAccessor httpContextAccessor;
        public UrlSeoController(IHttpContextAccessor accessor, AppDbContext _context, HelperServis _helperServis)
        {
            httpContextAccessor = accessor;

            this._context = _context;
            this._helperServis = _helperServis;
        }

        public IActionResult Index(string url = "")
        {

            var siteAyari = _context.SiteAyarlari.FirstOrDefault();

            RedirectModel Model = new()
            {
                Action = "",
                Controller = "",
                Area = "",
                Parameters = new { Id = 0 }
            };


            url = url.ToLower();

            var urlSeo = _context.SeoUrl.Where(x => x.Url.ToLower() == url).FirstOrDefault();

            if(urlSeo == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var dil = HttpContext.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>().RequestCulture.Culture.Name;

            var lang = urlSeo.Diller.DilKodlari.DilKodu;

            var kategori = _context.Kategoriler.FirstOrDefault(x => x.Id == urlSeo.EntityId);

            var sayfa = _context.Sayfalar.FirstOrDefault(x => x.Id == urlSeo.EntityId);


          


            if (urlSeo.SeoTipi == SeoTipleri.Sayfa)
            {


                if (sayfa.SayfaTipi == SayfaTipleri.DinamikSayfa)
                {
                    if (sayfa.AltSayfalar.Count > 0)
                    {
                        Model = new RedirectModel()
                        {
                            Action = "Index",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        return View(Model);
                    }

                    else
                    {
                        Model = new RedirectModel()
                        {
                            Action = "Detay",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        return View(Model);

                    }

                }

                if (sayfa.SayfaTipi == SayfaTipleri.Hakkimizda)
                {
                    Model = new RedirectModel()
                    {
                        Action = "Detay",
                        Controller = "Sayfalar",
                        Area = "",
                        Parameters = new { Id = urlSeo.EntityId, lang, url }
                    };
                    return View(Model);

                }


                if (sayfa.AltSayfalar.Count > 0 && sayfa.SinirsizAltSayfaDurumu == SayfaDurumlari.Aktif)
                {
                    Model = new RedirectModel()
                    {
                        Action = "Index",
                        Controller = "Sayfalar",
                        Area = "",
                        Parameters = new { Id = urlSeo.EntityId, lang, url }
                    };
                    return View(Model);
                }

                else
                {
                    Model = new RedirectModel()
                    {
                        Action = "Detay",
                        Controller = "Sayfalar",
                        Area = "",
                        Parameters = new { Id = urlSeo.EntityId, lang, url }
                    };
                    return View(Model);

                }
            }

            if (urlSeo.SeoTipi == SeoTipleri.BizeUlasin)
            {
                Model = new RedirectModel()
                {
                    Action = "BizeUlasin",
                    Controller = "Sayfalar",
                    Area = "",
                    Parameters = new { Id = urlSeo.EntityId, lang, url }
                };
                return View(Model);
            }


            if (urlSeo != null)
            {




                switch (urlSeo.EntityName)
                {

                    //PARENTKATEGORIID 1'E ESITSE VE ALT KATEGORISI VARSA KATEGORILERI LISTELE DEGILSE URUNLERI LISTELE

                    case SeoUrlTipleri.Kategori:
                        {
                            if (siteAyari.SinirsiKategoriDurum == SayfaDurumlari.Aktif)
                            {
                                if (kategori.AltKategoriler.Count > 0)
                                {
                                    Model = new RedirectModel()
                                    {
                                        Action = "Kategoriler",
                                        Controller = "Urunler",
                                        Area = "",
                                        Parameters = new { Id = urlSeo.EntityId, lang, url }
                                    };
                                }
                                else
                                {
                                    Model = new RedirectModel()
                                    {
                                        Action = "Index",
                                        Controller = "Urunler",
                                        Area = "",
                                        Parameters = new { Id = urlSeo.EntityId, lang, url }
                                    };
                                }
                            }
                            else
                            {
                                Model = new RedirectModel()
                                {
                                    Action = "Index",
                                    Controller = "Urunler",
                                    Area = "",
                                    Parameters = new { Id = urlSeo.EntityId, lang, url }
                                };
                            }

                            break;
                        }


                    //TUM KATEGORILERI LISTELE
                    case SeoUrlTipleri.Kategoriler:
                        {

                            Model = new RedirectModel()
                            {
                                Action = "TumKategoriler",
                                Controller = "Urunler",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };

                            break;
                        }

                    //TUM URUNLERI LISTELE
                    case SeoUrlTipleri.Urunler:
                        {

                            Model = new RedirectModel()
                            {
                                Action = "TumUrunler",
                                Controller = "Urunler",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                            break;
                        }

                    case SeoUrlTipleri.Urun:
                        {

                            Model = new RedirectModel()
                            {
                                Action = "Detay",
                                Controller = "Urunler",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                            break;
                        }
                    case SeoUrlTipleri.Hakkimizda:
                        Model = new RedirectModel()
                        {
                            Action = "Hakkimizda",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;


                    case SeoUrlTipleri.DinamikSayfaDetay:
                        //Sınırsız Sayfalamayı kullanmak istersen burayı aktif et
                        if (sayfa.AltSayfalar.Count > 0)
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Index",
                                Controller = "Sayfalar",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        else
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Detay",
                                Controller = "Sayfalar",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        break;





                    case SeoUrlTipleri.Projeler:
                        if (sayfa.AltSayfalar.Count > 0)
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Index",
                                Controller = "Projeler",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        else
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Detay",
                                Controller = "Projeler",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        break;


                    case SeoUrlTipleri.Blog:
                        if (sayfa.AltSayfalar.Count > 0)
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Index",
                                Controller = "Blog",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        else
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Detay",
                                Controller = "Blog",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        break;



                    case SeoUrlTipleri.Yorumlar:
                        Model = new RedirectModel()
                        {
                            Action = "Yorumlar",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;

                    case SeoUrlTipleri.Duyurular:
                        if (sayfa.AltSayfalar.Count > 0)
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Index",
                                Controller = "Duyurular",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        else
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Detay",
                                Controller = "Duyurular",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        break;



                    case SeoUrlTipleri.Hizmetlerimiz:
                        if (sayfa.AltSayfalar.Count > 0)
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Index",
                                Controller = "Hizmetlerimiz",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        else
                        {
                            Model = new RedirectModel()
                            {
                                Action = "Detay",
                                Controller = "Hizmetlerimiz",
                                Area = "",
                                Parameters = new { Id = urlSeo.EntityId, lang, url }
                            };
                        }
                        break;
                    case SeoUrlTipleri.Referanslar:
                        Model = new RedirectModel()
                        {
                            Action = "Referanslar",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;

              

                    case SeoUrlTipleri.GaleriKategoriSabitMenu:
                        Model = new RedirectModel()
                        {
                            Action = "Index",
                            Controller = "FotografGalerisi",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;


                    case SeoUrlTipleri.Galeri:
                        Model = new RedirectModel()
                        {
                            Action = "Detay",
                            Controller = "FotografGalerisi",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;

                    case SeoUrlTipleri.VideoGaleriSabitMenu:
                        Model = new RedirectModel()
                        {
                            Action = "VideoKategorileri",
                            Controller = "Videolar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;
                    case SeoUrlTipleri.VideoKategorileri:
                        Model = new RedirectModel()
                        {
                            Action = "Index",
                            Controller = "Videolar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;
                    case SeoUrlTipleri.Bayilerimiz:
                        Model = new RedirectModel()
                        {
                            Action = "Bayilerimiz",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;
                    case SeoUrlTipleri.InsanKaynaklariSabitMenu:
                        Model = new RedirectModel()
                        {
                            Action = "InsanKaynaklari",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;
        
                    case SeoUrlTipleri.EkibimizSabitMenu:
                        Model = new RedirectModel()
                        {
                            Action = "Ekibimiz",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;


                    //case SeoUrlTipleri.SSSSabitMenu:
                    //    Model = new RedirectModel()
                    //    {
                    //        Action = "SSS",
                    //        Controller = "Sayfalar",
                    //        Area = "",
                    //        Parameters = new { Id = urlSeo.EntityId, lang, url }
                    //    };
                    //    break;



                    case SeoUrlTipleri.EKatalog:
                        Model = new RedirectModel()
                        {
                            Action = "EKatalog",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;


                    case SeoUrlTipleri.BizeUlasinSabitMenu:
                        Model = new RedirectModel()
                        {
                            Action = "BizeUlasin",
                            Controller = "Sayfalar",
                            Area = "",
                            Parameters = new { Id = urlSeo.EntityId, lang, url }
                        };
                        break;
                    default:
                        break;
                }
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //private static RedirectModel GetLoginModel(string url, string lang)
        //{
        //    return new RedirectModel()
        //    {
        //        Action = "GirisYap",
        //        Controller = "Account",
        //        Area = "",
        //        Parameters = new { lang, url }
        //    };
        //}
    }
}
