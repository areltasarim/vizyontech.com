using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace vizyontech.com.Controllers
{
    [AllowAnonymous]

    public class FormlarController : Controller
    {
        private readonly IHtmlLocalizer<FormlarController> _localizer;

        private readonly AppDbContext _context;

        public FormlarController(AppDbContext _context, IHtmlLocalizer<FormlarController> localizer)
        {
            this._context = _context;
            _localizer = localizer;
        }

        public IActionResult Index(string url)
        {
            //url de Haberler yazınca null olarak geliyor bunun sebebi ise Hizmetlerimiz adında bir Controller olduğundan karışıklık oluyor (ikisininde aynı isimde olmasından kaynaklanıyor)  bundan dolayı aşağıdaki if kısmı yazılmıştır (Yada cotroller ismi değiştirilebilir böylece if kısmına gerek kalmayacaktır.)
            var controllerValue = HttpContext.Request.RouteValues["Controller"].ToString();

            var sayfaId = _context.SeoUrl.FirstOrDefault(p => p.Url == url)?.EntityId;
            var model = _context.Sayfalar.ToList().Where(p => p.ParentSayfaId == sayfaId).OrderBy(p => p.Sira);

            return View(model);
        }

        public IActionResult Detay(int Id)
        {
            var model = _context.Sayfalar.Find(Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Detay(string[] Form, string[] FormTuru, IFormCollection Frm, IList<IFormFile> Files)
        {
            ResultViewModel sonuc = new ResultViewModel();

            try
            {
                using (System.Transactions.TransactionScope Transaction = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, TimeSpan.FromMinutes(30)))
                {

                    var formBasvuru = new FormBasvurulari()
                    {
                        BasvuruDurumu = BasvuruDurumlari.Incelenmedi,
                        SayfaId = Convert.ToInt32(Frm["SayfaId"]),
                        BasvuruTarihi = DateTime.Now
                    };
                    _context.Add(formBasvuru);
                    _context.SaveChanges();

                    List<string> formcevap = new List<string>();

                    for (int i = 0; i < FormTuru.Length; i++)
                    {


                        if (FormTuru[i].ToString().Contains(FormTurleri.DropDownList.ToString()))
                        {
                            string Anahtar = $"DropDownListCevap-{Form[i]}";
                            string Deger = Frm[Anahtar];
                            formcevap.Add(Deger);
                        }

                        if (FormTuru[i].ToString().Contains(FormTurleri.RadioButon.ToString()))
                        {
                            string Anahtar = $"RadioButonCevap-{Form[i]}";
                            string Deger = Frm[Anahtar];
                            formcevap.Add(Deger);
                        }

                        if (FormTuru[i].ToString().Contains(FormTurleri.CheckBox.ToString()))
                        {

                            string Anahtar = $"CheckboxCevap-{Form[i]}";
                            string Deger = Frm[Anahtar];
                            formcevap.Add(Deger);
                        }

                        if (FormTuru[i].ToString().Contains(FormTurleri.TexBox.ToString()))
                        {
                            string Anahtar = $"TexboxCevap-{Form[i]}";
                            string Deger = Frm[Anahtar];
                            formcevap.Add(Deger);
                        }
                        if (FormTuru[i].ToString().Contains(FormTurleri.TexArea.ToString()))
                        {
                            string Anahtar = $"TexAreaCevap-{Form[i]}";
                            string Deger = Frm[Anahtar];
                            formcevap.Add(Deger);
                        }
                        if (FormTuru[i].ToString().Contains(FormTurleri.Dosya.ToString()))
                        {
                            string Anahtar = $"FormDosyasi-{Form[i]}";
                            string Deger = "";
                            var DosyaListesi = Frm.Files.Where(x => x.Name == Anahtar);
                            foreach (var FormDosyasi in DosyaListesi)
                            {
                                #region Dosya
                                if (FormDosyasi != null)
                                {

                                    List<string> ContentTypeListesi = new()
                                    {
                                        "image/jpeg",
                                        "image/png",
                                        "application/pdf",
                                        "application/msword",
                                    };

                                    string dosyaName = ImageHelper.ImageReplaceName(FormDosyasi,"");

                                    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Dosya) + dosyaName;

                                    if (ContentTypeListesi.Contains(FormDosyasi.ContentType))
                                    {
                                        //Resmi Belirli Boyutta Kaydetmek için (ImageHelper dan boyutlandırma ayarlanıyor)
                                        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.DilResim.OpenReadStream()));
                                        using (var stream = new FileStream(Mappath, FileMode.Create))
                                        {
                                            FormDosyasi.CopyTo(stream);
                                        }
                                        if (Deger == "")
                                        {
                                            Deger += Mappath.Remove(0, 7);
                                        }
                                        else
                                        {
                                            Deger += "," + Mappath.Remove(0, 7);
                                        }
                                    }
                                }
                                else
                                {

                                }
                                #endregion
                            }
                            formcevap.Add(Deger);
                        }
                    }



                    for (int i = 0; i < Form.Length; i++)
                    {


                        var formCevaplari = new FormCevaplari()
                        {
                            SayfaId = Convert.ToInt32(Frm["SayfaId"]),
                            FormId = Convert.ToInt32(Form[i]),
                            FormBasvuruId = formBasvuru.Id,
                            Cevap = formcevap[i],
                        };
                        _context.Entry(formCevaplari).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        _context.SaveChanges();

                    }

                    sonuc.Basarilimi = true;
                    sonuc.Mesaj = "Form Başarıyla Gönderildi";
                    sonuc.Display = "block";

                    sonuc.MesajDurumu = "alert-success";

                    ViewBag.Alert = "block";


                    var siteAyari = _context.SiteAyarlariTranslate.ToList().Where(p => p.SiteAyarId == 1).FirstOrDefault();

                    string body;
                    body = "Form Başvuru detayını aşağıdaki linke tıklayarak inceleyebilirsiniz<br /><br />";
                    body += $"{Request.Scheme}://{Request.Host}" + "/Admin/Sayfalar/FormBasvurulari?SayfaId="+ Frm["SayfaId"];


                    List<string> gonderilecekMailler = new List<string>();
                    gonderilecekMailler.Add(siteAyari?.SiteAyarlari.GonderilecekMail ?? "");

                    List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();
                    foreach (var file in Files)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), file.FileName);
                                dosya.Add(att);
                            }
                        }
                    }

                    MailHelper.HostMailGonder(
                    siteAyari?.SiteAyarlari.EmailAdresi ?? "",
                    siteAyari?.SiteAyarlari.EmailSifre ?? "",
                    siteAyari?.SiteAyarlari.EmailHost ?? "",
                    siteAyari.SiteAyarlari.EmailSSL,
                    siteAyari.SiteAyarlari.EmailPort,
                    konu: "Form Başvurusu",
                    mailBaslik: "Form Başvurusu",
                    body,
                    dosya,
                    gonderilecekMailler);


                    Transaction.Complete();
                    return Json(sonuc);

                    //TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "success", Text = "Form Başarıyla Gönderildi" });

                    //return Redirect("FormBasarili");
                }
            }
            catch (Exception Hata)
            {

                sonuc.Basarilimi = false;
                sonuc.Mesaj = "Hata Oluştu.";
                sonuc.Display = "block";
                sonuc.MesajDurumu = "alert-danger";
                return Json(sonuc);
                // Hata Oluşması Durumunda Uyarı Ver...

                //TempDataExtensions.Put(TempData, "BilgiMesaji", new PageMessageModel() { Type = "danger", Text = "Hata Oluştu." });

                //return Redirect("FormBasarili");
            }

        }

    }
}
