using DocumentFormat.OpenXml.Spreadsheet;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreService.Sepet;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class AlisverisListemServis : IAlisverisListemServis
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HelperServis _helperServis;

        private readonly string entity = "Alışveriş Listem";

        public AlisverisListemServis(AppDbContext _context, IHttpContextAccessor _httpContextAccessor, HelperServis helperServis = null)
        {
            this._context = _context;
            this._httpContextAccessor = _httpContextAccessor;
            _helperServis = helperServis;
        }
        public async Task<List<AlisverisListem>> PageList()
        {
            var uye = _helperServis.GetUye().Result;
            var model = await _context.AlisverisListem.Where(p => p.UyeId == uye.Id).ToListAsync();

            return model;
        }
        public class AlisverisListesiModel
        {
            public string SepetId { get; set; }
            public List<int> UrunIdList { get; set; } = new List<int>();

        }
        public async Task<ResultViewModel> AlisverisListesineEkle(int urunId)
        {

            ResultViewModel result = new ResultViewModel();

            var alisverisListeCookie = _httpContextAccessor.HttpContext.Request.Cookies["AlisverisCookie"];

            AlisverisListesiModel alisverisListe;
            try
            {


                if (urunId != 0)
                {

                    if (alisverisListeCookie == null)
                    {
                        alisverisListe = new AlisverisListesiModel
                        {
                            SepetId = Guid.NewGuid().ToString()
                        };
                    }
                    else
                    {
                        // Cookie varsa deserialize et
                        alisverisListe = JsonConvert.DeserializeObject<AlisverisListesiModel>(alisverisListeCookie);
                    }

                    var urunVarmi = alisverisListe.UrunIdList.Any(x => x == urunId);
                    var urun = _context.UrunlerTranslate.Where(p => p.UrunId == urunId).FirstOrDefault();

                    if (!urunVarmi)
                    {
                        alisverisListe.UrunIdList.Add(urunId);
                    }
                    else
                    {
                        result.Basarilimi = false;
                        result.Mesaj = "Ürün Alışveriş Listenizde Ekli";
                        result.Display = urun.UrunAdi;
                        result.MesajDurumu = "error";
                        return result;
                    }


                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7)
                    };

                    var alisverisListeJson = JsonConvert.SerializeObject(alisverisListe);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("AlisverisCookie", alisverisListeJson, options);



                    result.Basarilimi = true;
                    result.Mesaj = "Alışveriş Listesine Eklendi";
                    result.MesajDurumu = "success";
                    result.Display = urun.UrunAdi;

                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && _httpContextAccessor.HttpContext.User.IsInRole("Bayi"))
                    {
                        var uyeid = Convert.ToInt32(this._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                        var urunEklimi = _context.AlisverisListem.Where(x => x.UrunId == urunId && x.UyeId == uyeid).FirstOrDefault();
                        if (urunEklimi == null)
                        {
                            var alisverisListesi = new AlisverisListem()
                            {
                                UrunId = urunId,
                                UyeId = uyeid
                            };
                            _context.Entry(alisverisListesi).State = EntityState.Added;
                            await _context.SaveChangesAsync();
                        }
                    }

                }
                else
                {

                    result.Basarilimi = false;
                    result.Mesaj = "Hata Oluştu";
                    result.Display = "";
                    result.MesajDurumu = "error";
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.Mesaj = "Genel Bir Hata Oluştu : " + hata.Message;
                result.Display = "";
                result.MesajDurumu = "error";
            }



            return result;
        }



        public AlisverisListesiModel GetAlisverisListem()
        {
            var sepetIdCookie = _httpContextAccessor.HttpContext.Request.Cookies["AlisverisCookie"];
            if (sepetIdCookie != null)
            {
                return JsonConvert.DeserializeObject<AlisverisListesiModel>(sepetIdCookie);
            }
            return null;
        }

        public async Task<ResultViewModel> GetAlisverisListeUrunSil(int urunId)
        {
            ResultViewModel result = new ResultViewModel();

            try
            {

                var alisverisListeCookie = _httpContextAccessor.HttpContext.Request.Cookies["AlisverisCookie"];
                AlisverisListesiModel alisverisListe;

                if (alisverisListeCookie != null)
                {
                    alisverisListe = JsonConvert.DeserializeObject<AlisverisListesiModel>(alisverisListeCookie);

                    alisverisListe.UrunIdList.RemoveAll(x => x == urunId);

                    var alisverisListeJson = JsonConvert.SerializeObject(alisverisListe);
                    CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7)
                    };

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("AlisverisCookie", alisverisListeJson, options);

                    var urun = _helperServis.GetUrun(urunId);
                    var model = _context.AlisverisListem.Where(x => x.UyeId == _helperServis.GetUye().Result.Id && x.UrunId == urunId).FirstOrDefault();
                    _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.Mesaj = "Başaıyla Silindi";
                    result.Display = "";
                    result.MesajDurumu = "success";

                }
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.Mesaj = "Genel Bir Hata Oluştu : " + hata.Message;
                result.Display = "";
                result.MesajDurumu = "error";
            }
            return result;
        }

        public async Task<ResultViewModel> AlisverisListesiUyeKaydet()
        {
            var result = new ResultViewModel();

            try
            {

                var uyeid = Convert.ToInt32(this._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var uye = _context.Users.Where(x => x.Id == uyeid).FirstOrDefault();

                var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["AlisverisCookie"];

                var alisverisListe = JsonConvert.DeserializeObject<AlisverisListesiModel>(cookieValue);

                foreach (var item in alisverisListe.UrunIdList)
                {
                    var urunEklimi = _context.AlisverisListem.Where(x => x.UrunId == item && x.UyeId == uye.Id).FirstOrDefault();
                    if (urunEklimi == null)
                    {
                        var alisverisListesi = new AlisverisListem()
                        {
                            UrunId = item,
                            UyeId = uye.Id
                        };
                        _context.Entry(alisverisListesi).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }
                }

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = $"{entity} Başarıyla Eklendi.";
                result.Display = "";
            }
            catch (Exception)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                result.Display = "";
            }
          

            return result;
        }

        public async Task<ResultViewModel> DeletePage(MesajViewModel Model)
        {

            var result = new ResultViewModel();

            var model = _context.AlisverisListem.ToList().Find(p => p.Id == Model.Id);

            _context.Entry(model).State = EntityState.Deleted;

            try
            {

                await _context.SaveChangesAsync();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = $"{entity} Başarıyla Silindi.";
                result.Display = "";

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                result.Display = "";

            }

            return result;
        }

        public async Task<ResultViewModel> DeleteAllPage(int[] Deletes)
        {
            var result = new ResultViewModel();


            if (Deletes != null)
            {
                foreach (var item in Deletes)
                {
                    var model = _context.AlisverisListem.ToList().Find(p => p.Id == item);

                    _context.Entry(model).State = EntityState.Deleted;
                }
            }

            try
            {
                await _context.SaveChangesAsync();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                result.Display = "";

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                result.Display = "";

            }


            return result;
        }

    }
}
