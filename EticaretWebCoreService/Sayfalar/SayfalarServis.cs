using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace EticaretWebCoreService
{

    public partial class SayfalarServis : ISayfalarServis
    {
        private readonly AppDbContext _context;

        private UnitOfWork _uow = null;


        private readonly string entity = "Sayfa";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager = null;
        private readonly SeoServis _seoServis;

        public SayfalarServis(AppDbContext _context, UnitOfWork uow, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, SeoServis _seoServis)
        {
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            _uow = uow;
            this._seoServis = _seoServis;
        }

        public async Task<List<Sayfalar>> PageList(SayfaTipleri SayfaTipi)
        {
            var model = await _context.Sayfalar.ToListAsync();
            if (SayfaTipi != SayfaTipleri.DinamikSayfa)
            {
                model = await _context.Sayfalar.Where(p => p.SayfaTipi == SayfaTipi).ToListAsync();
            }

            return (model);
        }
        public async Task<List<FormBasvurulari>> FormBasvulariListele()
        {
            return (await _context.FormBasvurulari.ToListAsync());
        }

        public async Task<List<FormCevaplari>> FormCevaplariListele()
        {
            return (await _context.FormCevaplari.ToListAsync());
        }
        public async Task<ResultViewModel> UpdatePage(SayfaViewModel Model, SayfaTipleri SayfaTipi, int uyeId, string submit)
        {

            var result = new ResultViewModel();
            int pageId = 0;

            var parentSayfaTipi = _context.Sayfalar.Where(p => p.Id == Model.Sayfa.ParentSayfaId).FirstOrDefault().SayfaTipi;

            if (parentSayfaTipi == 0)
            {
                parentSayfaTipi = Model.Sayfa.SayfaTipi;
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };


                    List<string> DosyaTipleri = new()
                    {
                        "application/pdf",
                        "application/msword",
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        "application/vnd.rar",
                        "application/x-rar-compressed",
                        "application/octet-stream",
                        "application/zip",
                        "application/octet-stream",
                        "application/x-zip-compressed",
                        "multipart/x-zip",
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };

                    var db = new AppDbContext();


                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        DateTime tarih = DateTime.Now;
                        if (Model.Tarih.Year == 1)
                        {
                            tarih = DateTime.Now;
                        }
                        else
                        {
                            tarih = Model.Sayfa.Tarih;
                        }





                        var sayfaEkle = new Sayfalar()
                        {
                            ParentSayfaId = Model.Sayfa.ParentSayfaId,
                            SayfaTipi = parentSayfaTipi,
                            SSS = Model.Sayfa.SSS,
                            EntityName = SeoUrlTipleri.DinamikSayfaDetay,
                            Tarih = tarih,
                            Hit = 0,
                            UyeId = uyeId,
                            Ikon = Model.Sayfa.Ikon,
                            Ikon2 = Model.Sayfa.Ikon2,
                            SinirsizAltSayfaDurumu = Model.Sayfa.SinirsizAltSayfaDurumu,
                            SilmeYetkisi = SayfaDurumlari.Pasif,
                            AdminSolMenu = Model.Sayfa.AdminSolMenu,
                            KisayolMenuAdi = Model.Sayfa.KisayolMenuAdi,
                            Sira = Model.Sayfa.Sira,
                            Durum = Model.Sayfa.Durum,
                            Vitrin = Model.Sayfa.Vitrin,
                            SayfalarTranslate = new List<SayfalarTranslate>(),
                        };


                        #region Breadcrumb Resim
                        if (Model.BreadcrumbImage != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.BreadcrumbImage, "Sayfalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                sayfaEkle.BreadcrumbResim = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.BreadcrumbResim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        await _context.SaveChangesAsync();

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.Sayfa.SayfalarTranslate)
                        {
                            var sayfaEkleTranslate = new SayfalarTranslate()
                            {
                                SayfaAdi = item.SayfaAdi,
                                SayfaAdiAltAciklama = item.SayfaAdiAltAciklama,
                                SayfaAdiAltAciklama2 = item.SayfaAdiAltAciklama2,
                                Aciklama = item.Aciklama,
                                KisaAciklama = item.KisaAciklama,
                                YoutubeVideoLink = item.YoutubeVideoLink,
                                BreadcrumbAdi = item.BreadcrumbAdi,
                                BreadcrumbAciklama = item.BreadcrumbAciklama,
                                MetaBaslik = item.MetaBaslik,
                                MetaAnahtar = item.MetaAnahtar,
                                MetaAciklama = item.MetaAciklama,
                                ButonAdi = item.ButonAdi,
                                ButonUrl = item.ButonUrl,
                                DilId = item.DilId,
                            };


                            #region Kapak Resmi
                            if (item.SayfaResmi != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.SayfaResmi, "Sayfalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.Resim = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.Resim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            }
                            #endregion

                            #region Ana Sayfa Resmi
                            if (item.SayfaResmi2 != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.SayfaResmi2, "Sayfalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.Resim2 = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.Resim2 = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            }
                            #endregion

                            #region Dosya
                            if (item.SayfaDosyasi != null)
                            {

                                var model = DosyaHelper.DosyaYukle(item.SayfaDosyasi, "", DosyaTipleri, 250000000, DosyaYoluTipleri.Dosya);

                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaEkleTranslate.Dosya = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkleTranslate.Dosya = ImageHelper.DosyaYok(DosyaYoluTipleri.Dosya);
                            }
                            #endregion


                            sayfaEkle.SayfalarTranslate.Add(sayfaEkleTranslate);
                        }

                        _context.SaveChanges();
                        #endregion


                        #region Autocomplete
                        if (Model.SeciliSayfalarAutocomplete != null)
                        {
                            foreach (var item in Model.SeciliSayfalarAutocomplete)
                            {
                                var sayfaToSayfaEkle = new SayfaToSayfalar()
                                {
                                    SayfaId = sayfaEkle.Id,
                                    SayfalarId = item
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        #endregion


                        #region Seo Url
                        foreach (var sayfa in Model.Sayfa.SayfalarTranslate)
                        {
                            var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: sayfa.SayfaAdi, sayfaId: sayfaEkle.Id, entityName: (SeoUrlTipleri)parentSayfaTipi, seoTipi: SeoTipleri.Sayfa, dilId: sayfa.DilId);

                            if (seoUrl.Basarilimi == false)
                            {
                                return seoUrl;
                            }

                            var menuEkle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: sayfa.SayfaAdi, sayfaId: sayfaEkle.Id, parentSayfaId: 1, seoTipi: (SeoUrlTipleri)parentSayfaTipi, MenuTipleri.DinamikSayfalar, dilId: sayfa.DilId);

                        }
                        #endregion


                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = sayfaEkle.Id;
                        }
                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";

                    }

                    else
                    {



                        #region Sayfa Guncelleme

                        DateTime tarih = DateTime.Now;

                        if (Model.Tarih.Year == 1)
                        {
                            tarih = new AppDbContext().Sayfalar.Find(Model.Id).Tarih;
                        }
                        else
                        {
                            tarih = Model.Tarih;
                        }

                        var sayfaGuncelle = _context.Sayfalar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.ParentSayfaId = Model.Sayfa.ParentSayfaId;
                        sayfaGuncelle.SayfaTipi = Model.Sayfa.SayfaTipi;
                        sayfaGuncelle.SSS = Model.Sayfa.SSS;
                        sayfaGuncelle.EntityName = Model.Sayfa.EntityName;
                        sayfaGuncelle.Tarih = tarih;
                        sayfaGuncelle.Hit = 0;
                        sayfaGuncelle.Ikon = Model.Sayfa.Ikon;
                        sayfaGuncelle.Ikon2 = Model.Sayfa.Ikon2;
                        sayfaGuncelle.SinirsizAltSayfaDurumu = Model.Sayfa.SinirsizAltSayfaDurumu;
                        sayfaGuncelle.SilmeYetkisi = Model.Sayfa.SilmeYetkisi;
                        sayfaGuncelle.AdminSolMenu = Model.Sayfa.AdminSolMenu;
                        sayfaGuncelle.KisayolMenuAdi = Model.Sayfa.KisayolMenuAdi;
                        sayfaGuncelle.SayfaTipi = Model.Sayfa.SayfaTipi;
                        sayfaGuncelle.Sira = Model.Sayfa.Sira;
                        sayfaGuncelle.Durum = Model.Sayfa.Durum;
                        sayfaGuncelle.Vitrin = Model.Sayfa.Vitrin;


                        #region Breadcrumb Resim
                        if (Model.BreadcrumbImage != null)
                        {

                            var model = DosyaHelper.DosyaYukle(Model.BreadcrumbImage, "Sayfalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);

                            if (model.Result.Basarilimi == true)
                            {
                                sayfaGuncelle.BreadcrumbResim = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;
                                result.SayfaId = pageId;
                                return result;
                            }
                        }
                        
                        else
                        {
                            sayfaGuncelle.BreadcrumbResim = new AppDbContext().Sayfalar.Find(Model.Id).BreadcrumbResim;
                        }
                        #endregion

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await db.SaveChangesAsync();



                        if (!UstKategoriAltKategoriAtanamaz(Model.Id, Model.Sayfa.ParentSayfaId))
                        {
                            // Hata durumunu ele al
                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Kategorinin kendisi ana kategori olarak seçilemez!";
                            result.SayfaId = sayfaGuncelle.Id;
                            return result;
                        }


             

                        var diller = _context.Diller.ToList();

                        foreach (var item in Model.Sayfa.SayfalarTranslate)
                        {

                            var sayfaGuncelleTranslate = _context.SayfalarTranslate.Find(item.Id);

                            sayfaGuncelleTranslate.SayfaAdi = item.SayfaAdi;
                            sayfaGuncelleTranslate.SayfaAdiAltAciklama = item.SayfaAdiAltAciklama;
                            sayfaGuncelleTranslate.SayfaAdiAltAciklama2 = item.SayfaAdiAltAciklama2;
                            sayfaGuncelleTranslate.Aciklama = item.Aciklama;
                            sayfaGuncelleTranslate.KisaAciklama = item.KisaAciklama;
                            sayfaGuncelleTranslate.YoutubeVideoLink = item.YoutubeVideoLink;
                            sayfaGuncelleTranslate.BreadcrumbAdi = item.BreadcrumbAdi;
                            sayfaGuncelleTranslate.BreadcrumbAciklama = item.BreadcrumbAciklama;
                            sayfaGuncelleTranslate.MetaBaslik = item.MetaBaslik;
                            sayfaGuncelleTranslate.MetaAnahtar = item.MetaAnahtar;
                            sayfaGuncelleTranslate.MetaAciklama = item.MetaAciklama;
                            sayfaGuncelleTranslate.ButonAdi = item.ButonAdi;
                            sayfaGuncelleTranslate.ButonUrl = item.ButonUrl;
                            sayfaGuncelleTranslate.DilId = item.DilId;


                            #region Kapak Resmi
                            if (item.SayfaResmi != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.SayfaResmi, "Sayfalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.Resim = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;
                                    result.SayfaId = pageId;
                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.Resim = new AppDbContext().Sayfalar.Find(Model.Id).SayfalarTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.Resim;
                            }
                            #endregion

                            #region Ana Sayfa Resmi
                            if (item.SayfaResmi2 != null)
                            {
                                var model = DosyaHelper.DosyaYukle(item.SayfaResmi2, "Sayfalar", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.Resim2 = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;
                                    result.SayfaId = pageId;
                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.Resim2 = new AppDbContext().Sayfalar.Find(Model.Id).SayfalarTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.Resim2;
                            }
                            #endregion

                            #region Dosya
                            if (item.SayfaDosyasi != null)
                            {

                                var model = DosyaHelper.DosyaYukle(item.SayfaDosyasi, "", DosyaTipleri, 5242880, DosyaYoluTipleri.Dosya);

                                if (model.Result.Basarilimi == true)
                                {
                                    sayfaGuncelleTranslate.Dosya = model.Result.Sonuc;
                                }
                                else
                                {
                                    result.Basarilimi = result.Basarilimi;
                                    result.MesajDurumu = result.MesajDurumu;
                                    result.Mesaj = result.Mesaj;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaGuncelleTranslate.Dosya = new AppDbContext().Sayfalar.Find(Model.Id).SayfalarTranslate.SingleOrDefault(p => p.DilId == item.DilId)?.Dosya;
                            }
                            #endregion

                            _uow.Repository<SayfalarTranslate>().Update(sayfaGuncelleTranslate);
                            await _uow.CompleteAsync();
                        }

                        #endregion


                        #region Seo Url
                        foreach (var sayfa in Model.Sayfa.SayfalarTranslate)
                        {
                            var seoUrl = await _seoServis.SeoLinkOlustur(sayfaAdi: sayfa.SayfaAdi, sayfaId: sayfaGuncelle.Id, entityName: (SeoUrlTipleri)parentSayfaTipi, seoTipi: SeoTipleri.Sayfa, dilId: sayfa.DilId);
                            if (seoUrl.Basarilimi == false)
                            {
                                return seoUrl;
                            }

                            var menuGuncelle = await MenuHelper.MenuKaydet(Model.MenuYerleri, sayfaAdi: sayfa.SayfaAdi, sayfaId: sayfaGuncelle.Id, parentSayfaId: 1, seoTipi: (SeoUrlTipleri)parentSayfaTipi, MenuTipleri.DinamikSayfalar, dilId: sayfa.DilId);

                            if (menuGuncelle.Basarilimi == false)
                            {
                                return menuGuncelle;
                            }
                            await _context.SaveChangesAsync();
                        }
                        #endregion

                        #region Autocomplete
                        if (Model.SeciliSayfalarAutocomplete != null)
                        {
                            db.SayfaToSayfalar.Where(p => p.SayfaId == sayfaGuncelle.Id).ToList().ForEach(p => db.SayfaToSayfalar.Remove(p));
                            db.SaveChanges();

                            foreach (var item in Model.SeciliSayfalarAutocomplete)
                            {
                                var sayfaToSayfaEkle = new SayfaToSayfalar()
                                {
                                    SayfaId = sayfaGuncelle.Id,
                                    SayfalarId = item
                                };
                                _context.Entry(sayfaToSayfaEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            db.SayfaToSayfalar.Where(p => p.SayfaId == sayfaGuncelle.Id).ToList().ForEach(p => db.SayfaToSayfalar.Remove(p));
                            db.SaveChanges();
                        }

                        #endregion



                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = pageId;
                        }
                        #endregion



                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";

                    }

                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message.ToString();
                result.SayfaId = pageId;

            }

            return result;

        }

        public async Task<ResultViewModel> SayfaOzellik(SayfaToOzellikViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            int pageId = 0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };

                    #region Sayfa Ekleme

                    AppDbContext db = new AppDbContext();
                    db.SayfaToOzellik.Where(p => p.SayfaId == Model.SayfaId && p.SayfaOzellikleri.SayfaOzellikGrupId == Model.OzellikGrupId).ToList().ForEach(p => db.SayfaToOzellik.Remove(p));
                    db.SaveChanges();
                    foreach (var item in Model.SayfaToOzellikListesi.Where(x => x.SilinmeDurum == false))
                    {
                        if (item.OzellikResim != null)
                        {
                            var model = DosyaHelper.DosyaYukle(item.OzellikResim, "SayfaOzellikleri", ResimDosyaTipleri, 5242880, DosyaYoluTipleri.Resim);
                            if (model.Result.Basarilimi == true)
                            {
                                item.Resim = model.Result.Sonuc;
                            }
                            else
                            {
                                result.Basarilimi = result.Basarilimi;
                                result.MesajDurumu = result.MesajDurumu;
                                result.Mesaj = result.Mesaj;
                                return result;
                            }
                        }

                        var sayfaOzellik = new SayfaToOzellik()
                        {
                            SayfaOzellikId = item.SayfaOzellikId,
                            SayfaId = item.SayfaId,
                            Aciklama = item.Aciklama,
                            Resim = item.Resim,
                            DilId = item.DilId
                        };
                        _context.Entry(sayfaOzellik).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }

                    #endregion

                    #region Sayfa Butonlari
                    if (submit == "Kaydet")
                    {
                        result.Action = "SayfaOzellikleri";
                        result.SayfaId = pageId;
                    }
                    if (submit == "KaydetGuncelle")
                    {
                        result.Action = "SayfaOzellikleri";
                        result.SayfaId = pageId;
                    }

                    #endregion

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";



                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;
                result.SayfaId = pageId;

            }

            return result;

        }

        public bool UstKategoriAltKategoriAtanamaz(int parentSayfaId, int yeniKategoriId)
        {
            // Kategori, kendi alt kategorisine atanamaz
            if (parentSayfaId == yeniKategoriId)
            {
                return false;
            }

            // Mevcut üst kategori kontrolü
            var mevcutKategori = _context.Sayfalar.FirstOrDefault(s => s.Id == parentSayfaId);
            if (mevcutKategori == null)
            {
                // Geçerli bir üst kategori bulunamadı, işlem geçersiz
                return false;
            }

            // Mevcut üst kategori, düzenlenen kategorinin gerçekten üst kategorisi mi kontrol edilir
            if (mevcutKategori.Id != null && mevcutKategori.Id == yeniKategoriId)
            {
                // Düzenlenen kategori, mevcut üst kategorinin alt kategorisi ise işlem geçersiz
                return false;
            }

            // Alt kategorileri kontrol et
            var altKategoriler = _context.Sayfalar.Where(s => s.ParentSayfaId == parentSayfaId).ToList();
            foreach (var altkategori in altKategoriler)
            {
                if (altkategori.Id == yeniKategoriId || !UstKategoriAltKategoriAtanamaz(altkategori.Id, yeniKategoriId))
                {
                    // Düzenlenen kategori, alt kategorisi olarak atanamaz
                    return false;
                }
            }

            // Geçerli bir üst kategori
            return true;
        }

        public async Task<ResultViewModel> DeletePage(SayfaViewModel Model)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Sayfalar.Find(Model.Id);

                    foreach (var item in model.AltSayfalar)
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                    }

                    foreach (var item in model.SayfaToSayfalar)
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                    }

                    _context.Entry(model).State = EntityState.Deleted;

                    var diller = _context.Diller.ToList();

                    for (int i = 0; i < diller.Count; i++)
                    {
                        FileInfo file = new(@"wwwroot" + model.SayfalarTranslate.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == Model.Id & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.SayfaTipi))?.Url;
                        _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));
                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();

                    var menuSil = await MenuHelper.MenuSil(sayfaId: Model.Id, seoTipi: SeoUrlTipleri.DinamikSayfaDetay);
                    if (menuSil.Basarilimi == false)
                    {
                        return menuSil;
                    }
                    _context.SaveChanges();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";
                    result.SayfaUrl = model.SayfaTipi.ToString();

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;
        }

        public async Task<ResultViewModel> DeleteAllPage(int[] Deletes)
        {
            var result = new ResultViewModel();

            var sayfaTipi = "";

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Deletes != null)
                    {
                        foreach (var item in Deletes)
                        {

                            var model = _context.Sayfalar.Find(item);

                            foreach (var item2 in model.AltSayfalar)
                            {
                                _context.Entry(item2).State = EntityState.Deleted;
                            }

                            foreach (var item2 in model.SayfaToSayfalar)
                            {
                                _context.Entry(item2).State = EntityState.Deleted;
                            }

                            sayfaTipi = model.SayfaTipi.ToString();


                            var diller = _context.Diller.ToList();
                            for (int i = 0; i < diller.Count; i++)
                            {

                                FileInfo file = new(@"wwwroot" + model.SayfalarTranslate.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                                List<SayfaResimleri> cokluresim = _context.SayfaResimleri.ToList();

                                foreach (var item2 in cokluresim.Where(p => p.SayfaId == item))
                                {
                                    FileInfo files = new(@"wwwroot" + item2.Resim);
                                    if (files.Exists)
                                    {
                                        files.Delete();
                                    }
                                }

                                var seoUrl = _context.SeoUrl.FirstOrDefault(p => p.EntityId == item & p.EntityName == (SeoUrlTipleri)Convert.ToInt32(model.SayfaTipi))?.Url;
                                _context.SeoUrl.Where(p => p.EntityId == model.Id & p.Url == seoUrl).ToList().ForEach(p => _context.SeoUrl.Remove(p));

                                _context.Entry(model).State = EntityState.Deleted;

                                _context.Menuler.Where(p => p.EntityId == item & p.SeoUrlTipi == SeoUrlTipleri.DinamikSayfaDetay).ToList().ForEach(p => _context.Menuler.Remove(p));


                            }

                        }
                        await _context.SaveChangesAsync();

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
                        result.SayfaUrl = sayfaTipi;
                    }

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }


            return result;
        }
        public async Task<ResultViewModel> ImageSortOrder(string sira)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string sira_ = sira.Replace("sira[]=", "");
                    string[] p = sira_.Split('&');
                    int ss = 0;
                    foreach (string item in p)
                    {
                        var model = _context.SayfaResimleri.Find(Convert.ToInt32(item));
                        model.Sira = ss;
                        _context.Entry(model).State = EntityState.Modified;
                        ss++;
                    }
                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Resimler Sıralandı.";

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }



            return result;
        }

        public async Task<ResultViewModel> ImageDelete(int id)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    var model = _context.SayfaResimleri.ToList().Find(p => p.Id == id);

                    _context.Entry(model).State = EntityState.Deleted;

                    FileInfo file = new(@"wwwroot" + model.Resim);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Resim silindi.";

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

            return result;
        }

        #region Yorumlar
        public async Task<List<Sayfalar>> YorumListesi(int SayfaId)
        {
            var model = await _context.Sayfalar.Where(p => p.Id == SayfaId).ToListAsync();

            return (model);
        }
        public async Task<ResultViewModel> YorumEkle(YorumViewModel Model, IList<IFormFile> Files, string submit)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Yorumlar()
                        {
                            SayfaId = Model.SayfaId,
                            AdSoyad = Model.AdSoyad,
                            Yorum = Model.Yorum,
                            Sehir = Model.Sehir,
                            YorumTarihi = DateTime.Now,
                            YorumDurumu = SayfaDurumlari.Pasif
                        };
                        #endregion

                        #region Kapak Resmi
                        if (Model.SayfaResmi != null)
                        {


                            string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                            string Mappath = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim) + "Yorumlar/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }
                            if (ResimDosyaTipleri.Contains(Model.SayfaResmi.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SayfaResmi.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.SayfaResmi.CopyTo(stream);
                                }

                                sayfaEkle.Resim = Mappath.Remove(0, 7);
                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }

                            if (Model.SayfaResmi.Length > 10485760)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 10 Mb boyutunda resim yükleyiniz.";
                                result.SayfaId = sayfaEkle.Id;

                                return result;
                            }
                        }

                        else
                        {
                            sayfaEkle.Resim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                        }
                        #endregion

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        #region Mail Gönderimi
                        var siteAyari = _context.SiteAyarlari.Where(x => x.Id == 1).FirstOrDefault();


                        List<string> gonderilecekMailler = new List<string>();
                        gonderilecekMailler.Add(siteAyari.GonderilecekMail);

                        List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();

                        //#region Çoklu Dosya Gönderme
                        //foreach (var file in Files)
                        //{
                        //    if (file.Length > 0)
                        //    {
                        //        using (var ms = new MemoryStream())
                        //        {
                        //            file.CopyTo(ms);
                        //            var fileBytes = ms.ToArray();
                        //            System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), file.FileName);
                        //            dosya.Add(att);
                        //        }
                        //    }
                        //}
                        //#endregion

                        if (Model.SayfaResmi != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                Model.SayfaResmi.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(new MemoryStream(fileBytes), Model.SayfaResmi.FileName);
                                dosya.Add(att);
                            }
                        }




                        string body;
                        string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                        using (StreamReader reader = new StreamReader(_hostingEnvironment.WebRootPath + @$"/Content/MailTemplates/Yorum.html"))
                        {

                            body = reader.ReadToEnd();
                        }


                        body = body.Replace("{AdSoyad}", sayfaEkle.AdSoyad);
                        body = body.Replace("{Sehir}", sayfaEkle.Sehir);
                        body = body.Replace("{Yorum}", sayfaEkle.Yorum);

                        MailHelper.HostMailGonder(
                                     siteAyari?.EmailAdresi ?? "",
                                     siteAyari?.EmailSifre ?? "",
                                     siteAyari?.EmailHost ?? "",
                                     siteAyari.EmailSSL,
                                     siteAyari.EmailPort,
                                     konu: "Yeni Yorum Var " + "#" + sayfaEkle.SayfaId,
                                     mailBaslik: "Yorum",
                                     body,
                                     dosya,
                                     gonderilecekMailler.ToList()
                                     );

                        #endregion




                        #region Sayfa Butonlari

                        result.Action = "YorumOnayla";
                        result.SayfaId = sayfaEkle.Id;

                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "alert alert-success";
                        result.Display = "block";
                        result.Mesaj = "Your Comment Has Been Submitted Successfully. It will be published after approval.";

                    }
                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Yorumlar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        sayfaGuncelle.YorumTarihi = new AppDbContext().Yorumlar.Find(sayfaGuncelle.Id).YorumTarihi;
                        sayfaGuncelle.AdSoyad = Model.AdSoyad;
                        sayfaGuncelle.Yorum = Model.Yorum;
                        sayfaGuncelle.Sehir = Model.Sehir;
                        sayfaGuncelle.YorumDurumu = Model.YorumDurumu;
                        #endregion


                        #region Kapak Resim
                        if (Model.SayfaResmi != null)
                        {

                            string imageName = ImageHelper.ImageReplaceName(Model.SayfaResmi, Model.Resim);

                            string Mappath = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim) + "Yorumlar/" + imageName;
                            FileInfo serverfile = new FileInfo(Mappath);
                            if (!serverfile.Directory.Exists)
                            {
                                serverfile.Directory.Create();
                            }

                            //string Mappath2 = ImageHelper.ImageMappath2() + "Yorumlar/" + imageName;

                            if (ResimDosyaTipleri.Contains(Model.SayfaResmi.ContentType))
                            {
                                //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SayfaResmi.OpenReadStream()));

                                using (var stream = new FileStream(Mappath, FileMode.Create))
                                {
                                    Model.SayfaResmi.CopyTo(stream);
                                }

                                sayfaGuncelle.Resim = Mappath.Remove(0, 7);

                            }

                            else
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                            if (Model.SayfaResmi.Length > 5242880)
                            {
                                result.Basarilimi = false;
                                result.MesajDurumu = "danger";
                                result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                result.SayfaId = sayfaGuncelle.Id;

                                return result;
                            }

                        }

                        else
                        {
                            sayfaGuncelle.Resim = new AppDbContext().Yorumlar.Find(sayfaGuncelle.Id).Resim;
                        }
                        #endregion

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Yorumlar";
                            result.SayfaId = sayfaGuncelle.Id;
                            result.SayfaUrl = sayfaGuncelle.SayfaId.ToString();
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "YorumOnayla";
                            result.SayfaId = sayfaGuncelle.Id;
                            result.SayfaUrl = sayfaGuncelle.SayfaId.ToString();
                        }

                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";


                    }

                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                if (Model.Id == 0)
                {
                    result.MesajDurumu = "alert alert-danger";
                }
                else
                {
                    result.MesajDurumu = "danger";
                }

                result.Basarilimi = false;
                result.Display = "block";
                result.Mesaj = "An error occurred." + hata.Message.ToString();

            }

            return result;


        }


        public async Task<ResultViewModel> YorumDeletePage(YorumViewModel Model)
        {

            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Yorumlar.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;

                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";

                    transaction.Complete();
                }
            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }


            return result;
        }

        #endregion
    }
}
