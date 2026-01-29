using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace EticaretWebCoreService
{

    public partial class UyelerServis : IUyelerServis
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string entity = "Üye";
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        private readonly AdresServis _adresServis;
        private readonly AlisverisListemServis _alisverisListemServis;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly B2BSifreService _sifreService;

        public UyelerServis(AppDbContext _context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, AlisverisListemServis alisverisListemServis, AdresServis _adresServis, ICaptchaValidator _captchaValidator, B2BSifreService sifreService)
        {
            this._context = _context;

            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            _alisverisListemServis = alisverisListemServis;
            this._adresServis = _adresServis;
            this._captchaValidator = _captchaValidator;
            _sifreService = sifreService;
        }


        public async Task<List<AppUser>> UyeListele()
        {
            return (await _context.Users.ToListAsync());
        }

        public async Task<ResultViewModel> UyeOl(BayiOlViewModel Model, IList<IFormFile> Files)
        {
            List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml",
                        "application/pdf",
                        "application/msword"
                    };

            var sonuc = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    bool CapcheKontrol = await _captchaValidator.IsCaptchaPassedAsync(Model.Captcha);

                    if (CapcheKontrol)
                    {
                        var uyeVarmi = _context.Users.Where(x => x.Email == Model.Email).FirstOrDefault();
                        var ipAdresi = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;


                        var b2bsifre = _sifreService.Encrypt(Model.Password);
                   
                        if (uyeVarmi == null)
                        {
                            AppUser uye = new();
                            uye.UyeKayitTipi = UyeKayitTipi.Web;
                            uye.Tarih = DateTime.Now;
                            uye.Ad = Model.Ad;
                            uye.Soyad = Model.Soyad;
                            uye.FirmaAdi = Model.FirmaAdi;
                            uye.UserName = Replace.UrlSeo(Model.Ad.Replace(" ", ""));
                            uye.Email = Model.Email.Trim();
                            uye.B2bSifre = b2bsifre;
                            uye.PhoneNumber = Model.PhoneNumber;
                            uye.Gsm = Model.Gsm;
                            uye.IlceId = Model.AdresEkle.IlceId;
                            uye.Adres = Model.Adres;
                            uye.VergiDairesi = Model.VergiDairesi;
                            uye.VergiNumarasi = Model.VergiNumarasi;
                            uye.EmailConfirmed = false;
                            uye.UyeKayitTipi = UyeKayitTipi.Web;
                            uye.UyeDurumu = UyeDurumlari.OnayBekliyor;
                            uye.IpAdres = ipAdresi.ToString();
                            var userNameVarmi = _context.Users.Where(x => x.UserName == uye.UserName).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (userNameVarmi != null)
                            {
                                var sonUye = _context.Users.Max(x => x.Id);
                                uye.UserName = uye.UserName + sonUye;
                            }


                            //#region Resim
                            //if (Model.VergiLevhasiDosya != null)
                            //{

                            //    var model = DosyaHelper.DosyaYukle(Model.VergiLevhasiDosya, "VergiLevhasi", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            //    if (model.Result.Basarilimi == true)
                            //    {
                            //        uye.VergiLevhasi = model.Result.Sonuc;
                            //    }
                            //    else
                            //    {
                            //        sonuc.Basarilimi = sonuc.Basarilimi;
                            //        sonuc.MesajDurumu = sonuc.MesajDurumu;
                            //        sonuc.Mesaj = sonuc.Mesaj;

                            //        return sonuc;
                            //    }
                            //}

                            //else
                            //{
                            //    uye.VergiLevhasi = "#";
                            //}
                            //#endregion

                            IdentityResult result = await _userManager.CreateAsync(uye, Model.Password);
                            if (result.Succeeded)
                            {
                                if (uye.EmailConfirmed == false)
                                {
                                    string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(uye);
                                    var siteAyari = _context.SiteAyarlari.FirstOrDefault();

                                    List<string> gonderilecekMailler = new List<string>();

                                    // Kullanıcı e-posta adresini ekle (boş değilse)
                                    if (!string.IsNullOrEmpty(uye?.Email))
                                        gonderilecekMailler.Add(uye.Email);

                                    // Site ayarlarından gelen ekstra e-postaları virgüle göre ayır ve ekle
                                    if (!string.IsNullOrEmpty(siteAyari?.GonderilecekMail))
                                    {
                                        var ekstraMailler = siteAyari.GonderilecekMail
                                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(x => x.Trim());

                                        gonderilecekMailler.AddRange(ekstraMailler);
                                    }

                                    // Aynı mail tekrar yazılmasın
                                    gonderilecekMailler = gonderilecekMailler.Distinct().ToList();

                                    // Dosyaları hazırla
                                    List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();

                                    if (Files != null)
                                    {
                                        foreach (var file in Files)
                                        {
                                            if (file.Length > 0)
                                            {
                                                using (var ms = new MemoryStream())
                                                {
                                                    file.CopyTo(ms);
                                                    var fileBytes = ms.ToArray();
                                                    var att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), file.FileName);
                                                    dosya.Add(att);
                                                }
                                            }
                                        }
                                    }

                                    

                                    string body;
                                    string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                                    using (StreamReader reader = new StreamReader(_hostingEnvironment.WebRootPath + @$"/Content/MailTemplates/UyeKayit.html"))
                                    {

                                        body = reader.ReadToEnd();
                                    }

                                    body = body.Replace("{EmailDogrulama}", $"{hostUrl}/Account/EmailDogrulama?userId={uye.Id}&token={HttpUtility.UrlEncode(confirmationToken)}");



                                    MailHelper.HostMailGonder(
                                                 siteAyari?.EmailAdresi ?? "",
                                                 siteAyari?.EmailSifre ?? "",
                                                 siteAyari?.EmailHost ?? "",
                                                 siteAyari.EmailSSL,
                                                 siteAyari.EmailPort,
                                                 konu: "Bayi Kayıt",
                                                 mailBaslik: "Yeni Bayi Kayıt Oldu",
                                                 body,
                                                 dosya,
                                                 gonderilecekMailler.ToList()
                                                 );
                                }


                                await _userManager.AddToRoleAsync(uye, RolTipleri.Bayi.ToString());

                                //await _adresServis.EkleGuncelle(Model.AdresEkle, uye.Id);

                                sonuc.Basarilimi = true;
                                sonuc.MesajDurumu = "alert alert-success";
                                sonuc.Mesaj = "Bayi Kaydınız Başarıyla Tamamlandı. Gerekli bilgiler incelendikten sonra uygun görülmeniz durumunda bayiliğiniz onaylanarak tarafınıza mail ile bilgilendirme yapılacaktır.";
                                sonuc.SayfaUrl = "/girisyap?ReturnUrl=" + Model.ReturnUrl;

                                sonuc.SayfaId = uye.Id;
                            }
                            else
                            {
                                sonuc.Basarilimi = false;
                                sonuc.MesajDurumu = "alert alert-danger";

                                // Hataları birleştir
                                string hataMesaji = string.Join(" | ", result.Errors.Select(e => e.Description));

                                sonuc.Mesaj = hataMesaji;
                            }
                        }
                        else
                        {
                            sonuc.Basarilimi = false;
                            sonuc.MesajDurumu = "alert alert-danger";
                            sonuc.Mesaj = "Bu Mail adresiyle kayıtlı bir üye var";
                        }
                        transaction.Complete();
                    }

                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.Mesaj = "Captcha Doğrulaması Başarısız Oldu";
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Display = "block";
                    }
                }

            }
            catch (Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "alert alert-danger";
                sonuc.Mesaj = "Hata Oluştu : " + hata.Message;

            }

            return sonuc;

        }
        public async Task<ResultViewModel> HesabimiGuncelle(BayiOlViewModel Model, int Id, IList<IFormFile> Files)
        {
            List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml",
                        "application/pdf",
                        "application/msword"
                    };

            var sonuc = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var resultmodel = new ResultViewModel();

                    //ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
                    AppUser userName = await _userManager.FindByIdAsync(Id.ToString());

                    var b2bsifre = _sifreService.Encrypt(Model.Password);


                    var uye = _userManager.FindByNameAsync(userName.UserName).Result;
                    if (uye != null)
                    {
                        var sonUye = _context.Users.Max(x => x.Id);
                        uye.UserName = uye.UserName + sonUye;
                    }
                    else
                    {
                        uye.UserName = Replace.UrlSeo(Model.Ad.Replace(" ", "") + Model.Soyad.Replace(" ", ""));
                    }
                    uye.Ad = Model.Ad;
                    uye.Soyad = Model.Soyad;
                    uye.FirmaAdi = Model.FirmaAdi;
                    uye.UserName = Replace.UrlSeo(Model.Ad.Replace(" ", "") + Model.Soyad.Replace(" ", ""));
                    uye.IlceId = Model.IlceId;
                    uye.Adres = Model.Adres;
                    //uye.PhoneNumber = Model.PhoneNumber;
                    //uye.Gsm = Model.Gsm;
                    //uye.Email = Model.Email.Trim();
                    //uye.UyeDurumu = Model.UyeDurumu;
                    //uye.ParentId = Model.ParentId;

                    #region Kapak Resim
                    if (Model.UyeResim != null)
                    {
                        List<string> ContentTypeListesi = new()
                        {
                            "image/jpeg",
                            "image/png",
                            "image/gif",
                            "image/WebP"

                        };

                        string imageName = ImageHelper.ImageReplaceName(Model.UyeResim, Model.Ad);

                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Uyeler/" + imageName;


                        //string Mappath2 = ImageHelper.ImageMappath2() + "Diller/" + imageName;

                        if (ContentTypeListesi.Contains(Model.UyeResim.ContentType))
                        {
                            //Resmi Belirli Boyutta Kaydetmek için (ImageHelper dan boyutlandırma ayarlanıyor)
                            //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.UyeResim.OpenReadStream()));

                            using (var stream = new FileStream(Mappath, FileMode.Create))
                            {
                                Model.UyeResim.CopyTo(stream);
                            }

                            uye.ProfilResmi = Mappath.Remove(0, 7);

                        }

                        else
                        {
                            resultmodel.Basarilimi = false;
                            resultmodel.MesajDurumu = "danger";
                            resultmodel.Mesaj = "Jpeg, Png, Gif veya WebP formatında resim yükleyiniz.";
                        }

                        if (Model.UyeResim.Length > 5242880)
                        {
                            resultmodel.Basarilimi = false;
                            resultmodel.MesajDurumu = "danger";
                            resultmodel.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";

                        }

                    }

                    else
                    {
                        uye.ProfilResmi = new AppDbContext().Users.Find(uye.Id).ProfilResmi;
                    }
                    #endregion

                    IdentityResult result = await _userManager.UpdateAsync(uye);

                    #region Üye Şifre Güncelleme

                    if (Model.Password != null)
                    {
                        uye.B2bSifre = b2bsifre;
                        AppUser user = await _userManager.FindByIdAsync(uye.Id.ToString());

                        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                        await _userManager.ResetPasswordAsync(user, token, Model.Password);

                        await _userManager.UpdateSecurityStampAsync(user);
                    }



                    #endregion

                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(uye);

                        //await _signInManager.SignOutAsync();
                        //await _signInManager.SignInAsync(user, true);

                        sonuc.Basarilimi = true;
                        sonuc.MesajDurumu = "alert alert-success";
                        sonuc.Mesaj = "Üye Bilgileri Başarıyla Güncellendi.";
                    }

                    else
                    {


                        sonuc.Basarilimi = false;
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Mesaj = result.ToString();

                    }



                    IQueryable<AppRole> roles = _roleManager.Roles;

                    List<RoleAssignViewModel> roleAssignViewModel = new();

                    foreach (var role in roles)
                    {
                        RoleAssignViewModel r = new();
                        r.RoleId = role.Id.ToString();
                        r.RoleName = role.Name;

                        roleAssignViewModel.Add(r);

                    }

                    //ViewData["Roller"] = roleAssignViewModel;

                    transaction.Complete();
                }

            }
            catch(Exception hata)
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "alert alert-danger";
                sonuc.Mesaj = "Hata Oluştu : " + hata.Message;

            }
           
            return sonuc;

        }
        public async Task<ResultViewModel> EmailDogrulama(string userId, string token)
        {

            var sonuc = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var user = await _userManager.FindByIdAsync(userId);

                    IdentityResult result = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
                    if (result.Succeeded)
                    {
                        sonuc.Basarilimi = true;
                        sonuc.MesajDurumu = "alert alert-success";
                        sonuc.Mesaj = "Email Adresiniz Onaylanmıştır, Giriş Yapabilirsiniz";
                        sonuc.SayfaUrl = "/girisyap";
                    }
                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Mesaj = "Bir Hata Meydana Geldi.";
                    }

                    transaction.Complete();
                }

            }
            catch
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "alert alert-danger";
                sonuc.Mesaj = "Hata Oluştu.";

            }

            return sonuc;

        }

        public async Task<ResultViewModel> GirisYap(GirisYapViewModel Model, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager)
        {

            var sonuc = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    AppUser user = await _userManager.Users
                    .Where(u => u.Email == Model.Email || u.CariKodu == Model.Email)
                    .FirstOrDefaultAsync();

                    if (user != null)
                    {

                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            sonuc.Basarilimi = false;
                            sonuc.MesajDurumu = "alert alert-danger";
                            sonuc.Mesaj = "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz";
                            return sonuc;

                        };

                        if (_userManager.IsEmailConfirmedAsync(user).Result == false)
                        {
                            sonuc.Basarilimi = false;
                            sonuc.MesajDurumu = "alert alert-danger";
                            sonuc.Mesaj = "Bayilik başvurrunuz inceleme aşamasındadır. Onaylandıktan sonra giriş yapabilirsiniz.";
                            return sonuc;
                        }


                        await _signInManager.SignOutAsync();

                        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, Model.Password, Model.RememberMe, false);


                        if (result.Succeeded)
                        {

                            var roles = await _userManager.GetRolesAsync(user);
                            await _userManager.ResetAccessFailedCountAsync(user);

                            var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, user.UserName),
                                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                                };

                            var url = "/";
                            var identities = new List<ClaimsIdentity>();
                            foreach (var role in roles)
                            {
                                switch (role)
                                {
                                    case "Administrator":
                                        var adminClaims = new List<Claim>(claims)
                                            {
                                                new Claim(ClaimTypes.Role, "Administrator")
                                            };
                                        var adminIdentity = new ClaimsIdentity(adminClaims, "AdminAuth");
                                        identities.Add(adminIdentity);

                                        var adminPrincipal = new ClaimsPrincipal(adminIdentity);
                                        await _httpContextAccessor.HttpContext.SignInAsync("AdminAuth", adminPrincipal);

                                        url = "/Admin/";
                                        break;

                                    case "Bayi":
                                        var uyeClaims = new List<Claim>(claims)
                                        {
                                            new Claim(ClaimTypes.Role, "Bayi")
                                        };
                                        var uyeIdentity = new ClaimsIdentity(uyeClaims, "BayiAuth");
                                        identities.Add(uyeIdentity);

                                        var uyePrincipal = new ClaimsPrincipal(uyeIdentity);
                                        await _httpContextAccessor.HttpContext.SignInAsync("BayiAuth", uyePrincipal);
                                        url = "/";
                                        break;
                                }
                            }

                            var alisveriscookieValue = _httpContextAccessor.HttpContext.Request.Cookies["AlisverisCookie"];
                            if(alisveriscookieValue != null)
                            {
                                await _alisverisListemServis.AlisverisListesiUyeKaydet();
                            }


                            sonuc.Basarilimi = true;
                            sonuc.MesajDurumu = "alert alert-success";
                            sonuc.Mesaj = "Giriş Başarılı";
                            sonuc.SayfaUrl = url;

                        }
                        else
                        {
                            await _userManager.AccessFailedAsync(user);

                            int fail = await _userManager.GetAccessFailedCountAsync(user);

                            if (fail == 3)
                            {
                                await _userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));
                                sonuc.Basarilimi = false;
                                sonuc.MesajDurumu = "alert alert-danger";
                                sonuc.Mesaj = "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir.";
                                return sonuc;
                            }
                            else
                            {
                                sonuc.Basarilimi = false;
                                sonuc.MesajDurumu = "alert alert-danger";
                                sonuc.Mesaj = "Geçersiz Kullanıcı Adı veya Şifre";
                                return sonuc;

                            }
                        }
                    }

                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Mesaj = "Bu email adresine kayıtlı kullanıcı bulunamamıştır.";
                    }
                    transaction.Complete();
                }

            }
            catch
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "alert alert-danger";
                sonuc.Mesaj = "Hata Oluştu.";

            }

            return sonuc;

        }
        public async Task<ResultViewModel> SifremiUnuttum(SifremiUnuttumViewModel Model, IList<IFormFile> Files)
        {

            var sonuc = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    AppUser uye = _userManager.FindByEmailAsync(Model.Email).Result;

                    if (uye != null)
                    {
                        string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(uye).Result;

                        var siteAyari = _context.SiteAyarlari.FirstOrDefault();

                        List<string> gonderilecekMailler = new List<string>();
                        gonderilecekMailler.Add(Model.Email);


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
                        string body;
                        string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                        using (StreamReader reader = new StreamReader(_hostingEnvironment.WebRootPath + @$"/Content/MailTemplates/SifreSifirlama.html"))
                        {

                            body = reader.ReadToEnd();
                        }

                        body = body.Replace("{SifreSifirlamaLinki}", $"{hostUrl}/sifresifirlamayenisifreOlustur?userId={uye.Id}&token={HttpUtility.UrlEncode(passwordResetToken)}");
                        body = body.Replace("{Logo}", hostUrl + siteAyari.MailLogo);



                        MailHelper.HostMailGonder(
                         siteAyari?.EmailAdresi ?? "",
                         siteAyari?.EmailSifre ?? "",
                         siteAyari?.EmailHost ?? "",
                         siteAyari.EmailSSL,
                         siteAyari.EmailPort,
                         konu: "Şifre Sıfırlama",
                         mailBaslik: "Şifre Sıfırlama",
                         body,
                         dosya,
                         gonderilecekMailler
                         );


                        sonuc.Basarilimi = true;
                        sonuc.MesajDurumu = "alert alert-success";
                        sonuc.Mesaj = "Yeni Şifreniz Mailinize Gönderilmiştir.";
                    }
                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Mesaj = "Sistemde Kayıtlı Bir Email Bulunamadı.";

                    }
                    transaction.Complete();
                }

            }
            catch
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "alert alert-danger";
                sonuc.Mesaj = "Hata Oluştu.";

            }

            return sonuc;

        }

        public async Task<ResultViewModel> SifreSifirlamaYeniSifreOlustur(SifremiUnuttumViewModel Model, string userId, string token)
        {

            var sonuc = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    AppUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), Model.YeniSifre);

                        if (result.Succeeded)
                        {
                            await _userManager.UpdateSecurityStampAsync(user);

                            sonuc.Basarilimi = true;
                            sonuc.MesajDurumu = "alert alert-success";
                            sonuc.Mesaj = "Şifreniz Başarıyla Değiştirilmiştir.";
                            sonuc.SayfaUrl = "/girisyap";
                        }

                        else
                        {
                            sonuc.Basarilimi = false;
                            sonuc.MesajDurumu = "alert alert-danger";
                            sonuc.Mesaj = result.ToString();
                        }


                    }

                    else
                    {
                        sonuc.Basarilimi = false;
                        sonuc.MesajDurumu = "alert alert-danger";
                        sonuc.Mesaj = "Bir Hata Meydana Gelmiştir. Lütfen Daha Sonra Tekrar Deneyiniz. ";

                    }


                    transaction.Complete();
                }

            }
            catch
            {
                sonuc.Basarilimi = false;
                sonuc.MesajDurumu = "alert alert-danger";
                sonuc.Mesaj = "Hata Oluştu.";

            }

            return sonuc;

        }

    }
}
