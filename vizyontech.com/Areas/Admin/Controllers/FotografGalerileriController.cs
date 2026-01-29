using ComponentAce.Compression.Libs.zlib;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace vizyontech.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Yonetici", AuthenticationSchemes = "AdminAuth")]

    public class FotografGalerileriController : Controller
    {
        FotografGalerileriServis _fotografGalerisiServis = null;
        private readonly AppDbContext _context;
        private readonly string entityBaslik = "Fotoğraf Galerileri";
        private readonly string entityAltBaslik = "Galeri Ekle";

        public FotografGalerileriController(AppDbContext _context)
        {
            this._context = _context;
            _fotografGalerisiServis = new FotografGalerileriServis(_context);
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = await _fotografGalerisiServis.PageList();

            return View(model);
        }

        public IActionResult AddOrUpdate(int Id = 0)
        {
            ViewData["Baslik"] = entityBaslik;
            ViewData["AltBaslik"] = entityAltBaslik;

            var model = _context.FotografGalerileri.Find(Id);

            PopulateDropdown();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(FotografGalerisiViewModel Model, GaleriTipleri GaleriTipi, string submit)
        {
            var model = await _fotografGalerisiServis.UpdatePage(Model, GaleriTipi, submit);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            if (model.Basarilimi == true)
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
                return Redirect("/Admin/" + controllerValue + "/" + model.Action + "/"+ Model.Id + HttpContext.Request.QueryString);

            }
            else
            {
                TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });

                PopulateDropdown();
                return Redirect("/Admin/" + controllerValue + "/AddOrUpdate" + "/" + Model.Id + HttpContext.Request.QueryString);
            }
        }

        public async Task<IActionResult> Delete(FotografGalerisiViewModel Model)
        {

            var result = await _fotografGalerisiServis.DeletePage(Model);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = result.MesajDurumu, Text = result.Mesaj });

            return RedirectToAction("Index", controllerValue, new { SayfaTipi = result.SayfaUrl });

        }

        public async Task<IActionResult> DeleteAll(int[] Deletes)
        {
            var model = await _fotografGalerisiServis.DeleteAllPage(Deletes);

            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = model.MesajDurumu, Text = model.Mesaj });
            return RedirectToAction("Index", controllerValue);
        }

       

        public async Task<IActionResult> PageImages(int Id, bool TopluResim, GaleriSayfaTipleri GaleriSayfaTipi)
        {

            var model = _context.FotografGalerileri.Where(x => x.Id == Id).FirstOrDefault();

            if (Request.HasFormContentType && Request.Form != null && Request.Form.Count() > 0)
            {
                {
                    foreach (var formFile in Request.Form.Files)
                    {
                        string imageName = ImageHelper.ImageReplaceName(formFile, "");

                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "FotografGalerileri/" + imageName;

                        FileInfo serverfile = new FileInfo(Mappath);
                        if (!serverfile.Directory.Exists)
                        {
                            serverfile.Directory.Create();
                        }

                        if (TopluResim == true)
                        {
                            switch (GaleriSayfaTipi)
                            {
                                case GaleriSayfaTipleri.Sayfa:
                                    Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Sayfalar/" + imageName;
                                    FileInfo sayfaserverfile = new FileInfo(Mappath);
                                    if (!sayfaserverfile.Directory.Exists)
                                    {
                                        sayfaserverfile.Directory.Create();
                                    }
                                    break;
                                case GaleriSayfaTipleri.Marka:
                                    Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Markalar/" + imageName;
                                    FileInfo markaserverfile = new FileInfo(Mappath);

                                    if (!markaserverfile.Directory.Exists)
                                    {
                                        markaserverfile.Directory.Create();
                                    }
                                    break;
                                case GaleriSayfaTipleri.Urun:
                                    Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Urunler/" + imageName;
                                    FileInfo urunserverfile = new FileInfo(Mappath);
                                    if (!urunserverfile.Directory.Exists)
                                    {
                                        urunserverfile.Directory.Create();
                                    }
                                    break;
                                case GaleriSayfaTipleri.Kategori:
                                    Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Kategoriler/" + imageName;
                                    FileInfo kategoriserverfile = new FileInfo(Mappath);
                                    if (!kategoriserverfile.Directory.Exists)
                                    {
                                        kategoriserverfile.Directory.Create();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }


                        using (var stream = new FileStream(Mappath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }

                        //using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(formFile.OpenReadStream()))
                        //{
                        //    JpegEncoder jp = new JpegEncoder() { Quality = 100 };
                        //    image.SaveAsJpeg(Mappath, jp);
                        //}


                        var sayfaResim = new FotografGaleriResimleri()
                        {
                            Resim = Mappath.Remove(0, 7),
                            FotografGaleriId = Id,
                            Sira = 0
                        };

                        _context.FotografGaleriResimleri.Add(sayfaResim);
                        await _context.SaveChangesAsync();

                    }

                }
                var resimListesi = _context.FotografGalerileri.Where(x => x.Id == model.Id);
                int ResimIndex = 0;
                foreach (var item in resimListesi)
                {
                    ResimIndex++;
                    item.Sira = ResimIndex;
                    _context.FotografGalerileri.Update(item);
                }
                await _context.SaveChangesAsync();

            }

            return View(_context.FotografGaleriResimleri.Where(x => x.FotografGaleriId == Id).ToList());
        }
        static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
        public async Task<IActionResult> PageImageSortOrder(string sira)
        {
            var model = await _fotografGalerisiServis.ImageSortOrder(sira);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = model.Basarilimi, Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }

        public async Task<IActionResult> PhotoGalleryImagesDelete(int id)
        {
            var model = await _fotografGalerisiServis.ImageDelete(id);

            if (model.Basarilimi == true)
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
            else
            {
                return Json(new ResultViewModel { Basarilimi = Convert.ToBoolean(model.Basarilimi), Mesaj = model.Mesaj, NotfyAlert = true, BootBoxAlert = false });
            }
        }

        private void PopulateDropdown()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }

        private void ControllerValue()
        {
            ViewBag.Diller = _context.Diller.ToList() as IEnumerable<Diller>;
        }
    }
}
