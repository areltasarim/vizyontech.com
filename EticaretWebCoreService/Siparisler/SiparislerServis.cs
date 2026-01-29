using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class SiparislerServis : ISiparislerServis
    {
        private readonly AppDbContext _context;
        private readonly SiparislerServis _siparisServis;
        private readonly HelperServis _helperServis;

        private readonly string entity = "Sipariş";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public SiparislerServis(AppDbContext _context, HelperServis _helperServis, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            this._helperServis = _helperServis;
        }
        public async Task<List<Siparisler>> Listele()
        {
            return (await _context.Siparisler.Where(x=> x.SiparisDurumu != (int)SiparisDurumTipleri.EksikSiparis).ToListAsync());
        }
        public async Task<Siparisler> SiparisDetay(int Id)
        {
            return (await _context.Siparisler.FindAsync(Id));
        }
        public async Task<ResultViewModel> SiparisDuzenle(SiparisViewModel Model)
        {
            var result = new ResultViewModel();

            return result;
        }
        public async Task<ResultViewModel> SiparisDurumuGuncelle(SiparisGecmisViewModel Model, IFormFile[] Files)
        {
            var result = new ResultViewModel();
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

     
                    var siparisDurumGuncelle = _context.Siparisler.Find(Model.SiparisId);

                    #region Sipariş Geçmişi Ekleme
                    var siparisGecmisiEkle = new SiparisGecmisleri()
                    {
                        SiparisId = Model.SiparisId,
                        SiparisDurumId = Model.SiparisDurumId,
                        EklenmeTarihi = DateTime.Now
                    };
                    _context.Entry(siparisGecmisiEkle).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    #endregion

                    var siparisDurumu = _helperServis.GetSiparisDurumu(siparisGecmisiEkle.SiparisId).Result;

                    var model = _context.Siparisler.Where(x => x.Id == Model.SiparisId).FirstOrDefault();
                    if (Model.SiparisDurumId == (int)SiparisDurumTipleri.KargoyaVerildi)
                    {
                        model.KargoKodu = Model.KargoKodu;
                        model.KargoUrl = Model.KargoKodu;

                    }

                    model.SiparisDurumu = Model.SiparisDurumId;
                    _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();



                    #region Mail Gönderimi
                    try
                    {
                        var siteAyari = _context.SiteAyarlari.FirstOrDefault();

                        List<string> gonderilecekMailler = new List<string>();
                        gonderilecekMailler.Add(siparisDurumGuncelle.Email);


                        List<System.Net.Mail.Attachment> dosya = new List<System.Net.Mail.Attachment>();
                        if(Files != null)
                        {
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

                        }

                        string body;
                        string hostUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                        using (StreamReader reader = new StreamReader(_hostingEnvironment.WebRootPath + @$"/Content/MailTemplates/SiparisDurumu.html"))
                        {
                            body = reader.ReadToEnd();
                        }

                        body = body.Replace("{Ad_Soyad}", $"{siparisDurumGuncelle.TeslimatAd} {siparisDurumGuncelle.TeslimatSoyad}");

                        var siparisAciklama = $"#{siparisDurumGuncelle.Id} nolu siparişiniz güncellendi: {siparisDurumu}";

                        if (Model.SiparisDurumId == (int)SiparisDurumTipleri.KargoyaVerildi)
                        {
                            siparisAciklama = $"#{siparisDurumGuncelle.Id} nolu siparişiniz güncellendi: {siparisDurumu} Kargo Takibi için <a href='http://service.mngkargo.com.tr/iactive/popup/kargotakip.asp?k={Model.KargoKodu}' target='_blank'>Tıklayın</a> ";
                        }

                        body = body.Replace("{Siparis_Durum_Aciklama}", siparisAciklama);
                        body = body.Replace("{Logo}", hostUrl + siteAyari.MailLogo);

                        MailHelper.HostMailGonder(
                         siteAyari?.EmailAdresi ?? "",
                         siteAyari?.EmailSifre ?? "",
                         siteAyari?.EmailHost ?? "",
                         siteAyari.EmailSSL,
                         siteAyari.EmailPort,
                         konu: "Sipariş Durumu Güncellendi",
                         mailBaslik: "Sipariş Durumu Güncellendi",
                         body,
                         dosya,
                         gonderilecekMailler
                         );
                    }
                    catch (Exception hata)
                    {
                        result.Basarilimi = false;
                        result.MesajDurumu = "danger";
                        result.Mesaj = "Hata Oluştu." + hata.Message;
                    }
                    #endregion


                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} durumu başarıyla güncellenmiştir.";


                    transaction.Complete();
                }

            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu." + hata.Message;


            }

            return result;
        }

        //public async Task<ResultViewModel> KargoKoduGuncelle(int SiparisId, string KargoKodu)
        //{
        //    var result = new ResultViewModel();

        //    try
        //    {
        //        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        {

        //            var model = _context.Siparisler.Where(x => x.Id == SiparisId).FirstOrDefault();
        //            model.KargoKodu = KargoKodu;
        //            model.KargoUrl = KargoKodu;
        //            _context.Entry(model).State = EntityState.Modified;
        //            await _context.SaveChangesAsync();

        //            result.Basarilimi = true;
        //            result.MesajDurumu = "success";
        //            result.Mesaj = $"{entity} Başarıyla Eklendi.";

        //            transaction.Complete();
        //        }
        //    }
        //    catch
        //    {
        //        result.Basarilimi = false;
        //        result.MesajDurumu = "danger";
        //        result.Mesaj = "Hata Oluştu.";
        //    }

        //    return result;
        //}

        public async Task<ResultViewModel> DeletePage(SiparisViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var siparisGecmisListesi = _context.SiparisGecmisleri.Where(x => x.SiparisId == Model.Id).ToList();
                    _context.SiparisGecmisleri.RemoveRange(siparisGecmisListesi);
                    await _context.SaveChangesAsync();

                    var siparisUrunleriListesi = _context.SiparisUrunleri.Where(x => x.SiparisId == Model.Id).ToList();
                    _context.SiparisUrunleri.RemoveRange(siparisUrunleriListesi);
                    await _context.SaveChangesAsync();

                    var model = _context.Siparisler.Find(Model.Id); 
                    if (model != null)
                    {
                        _context.Siparisler.Remove(model);
                        await _context.SaveChangesAsync();
                    }

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = $"{entity} Başarıyla Silindi.";

                    transaction.Complete();
                }
            }
            catch (Exception hata)
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu : " + hata.Message;
            }
           
            return result;
        }

        public async Task<ResultViewModel> DeleteAllPage(int[] Deletes)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Deletes != null)
                    {
                        foreach (var item in Deletes)
                        {
                            var model = _context.Siparisler.ToList().Find(p => p.Id == item);

                            _context.Entry(model).State = EntityState.Deleted;
                        }
                    }


                    await _context.SaveChangesAsync();

                    result.Basarilimi = true;
                    result.MesajDurumu = "success";
                    result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";


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

    }
}
