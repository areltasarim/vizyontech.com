using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class MesajlarServis : IMesajlarServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Mesaj";

        public MesajlarServis(AppDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<MesajKonulari>> PageList(MesajTipleri MesajTipi,int UyeId)
        {

            var model = await _context.MesajKonulari.ToListAsync();
            switch (MesajTipi)
            {
                case MesajTipleri.GelenMesajlar:
                    model = await _context.MesajKonulari.Where(x=>x.GonderilenUyeId==UyeId).ToListAsync();
                    break;
                case MesajTipleri.GidenMesajlar:
                    model = await _context.MesajKonulari.Where(x=>x.UyeId== UyeId).ToListAsync();
                    break;
                case MesajTipleri.GelenOkunmamisMesajlar:
                    model = await _context.MesajKonulari.Where(x => x.GonderilenUyeId == UyeId).Where(p => p.Mesajlar.Any(x => x.OkunmaDurumu == MesajOkunmaDurumlari.Okunmadi)).ToListAsync();
                    break;
                case MesajTipleri.GelenSilinmisMesajlar:
                    model = await _context.MesajKonulari.Where(x => x.GonderilenUyeId == UyeId).Where(p => p.Mesajlar.Any(x => x.OkunmaDurumu == MesajOkunmaDurumlari.Silindi)).ToListAsync();
                    break;
                case MesajTipleri.GidenOkunmamisMesajlar:
                    model = await _context.MesajKonulari.Where(x => x.UyeId == UyeId).Where(p => p.Mesajlar.Any(x => x.OkunmaDurumu == MesajOkunmaDurumlari.Okunmadi)).ToListAsync();
                    break;
                case MesajTipleri.GidenSilinmisMesajlar:
                    model = await _context.MesajKonulari.Where(x => x.UyeId == UyeId).Where(p => p.Mesajlar.Any(x => x.OkunmaDurumu == MesajOkunmaDurumlari.Silindi)).ToListAsync();
                    break;
                default:
                    break;
            }
            return  model;
        }

        public async Task<ResultViewModel> UpdatePage(MesajViewModel Model, string submit)
        {

            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.MesajKonuId == 0)
                    {

                        #region Mesaj Konu Ekleme
                        var urun = _context.Urunler.Where(p => p.Id == Model.UrunId).FirstOrDefault();
                        var mesajKonuEkle = new MesajKonulari()
                        {
                            UyeId = Model.UyeId,
                            GonderilenUyeId = Model.GonderilenUyeId,
                            UrunId = Model.UrunId,
                            Konu = urun.UrunlerTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == "tr-TR").UrunAdi,
                        };

                        _context.Entry(mesajKonuEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Mesaj Ekleme
                        var mesajEkle = new Mesajlar()
                        {
                            MesajKonuId = mesajKonuEkle.Id,
                            UyeId = Model.UyeId,
                            GonderilenUyeId = Model.GonderilenUyeId,
                            Mesaj = Model.Mesaj,
                            MesajTarihi = DateTime.Now,
                            OkunmaDurumu = MesajOkunmaDurumlari.Okunmadi
                        };

                        _context.Entry(mesajEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion


                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = mesajEkle.Id;
                        }

                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme islemi başarıyla tamamlanmıştır.";

                    }

                    else
                    {

                        var mesajKonusu = _context.MesajKonulari.Where(p => p.Id == Model.MesajKonuId).FirstOrDefault();

                        #region Mesaj Ekleme
                        var mesajEkle = new Mesajlar()
                        {
                            MesajKonuId = Model.MesajKonuId,
                            UyeId = mesajKonusu.UyeId,
                            GonderilenUyeId = mesajKonusu.GonderilenUyeId,
                            Mesaj = Model.Mesaj,
                            MesajTarihi = DateTime.Now,
                            OkunmaDurumu = MesajOkunmaDurumlari.Okunmadi
                        };

                        _context.Entry(mesajEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = mesajEkle.Id;
                        }
                        #endregion

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Mesajınız başarıyla gönderilmiştir.";


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


        public async Task<ResultViewModel> DeletePage(MesajViewModel Model)
        {

            var result = new ResultViewModel();

            var model = _context.Mesajlar.ToList().Find(p => p.Id == Model.Id);

            _context.Entry(model).State = EntityState.Deleted;

            try
            {

                await _context.SaveChangesAsync();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = $"{entity} Başarıyla Silindi.";
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


            if (Deletes != null)
            {
                foreach (var item in Deletes)
                {
                    var model = _context.Mesajlar.ToList().Find(p => p.Id == item);

                    _context.Entry(model).State = EntityState.Deleted;
                }
            }

            try
            {
                await _context.SaveChangesAsync();

                result.Basarilimi = true;
                result.MesajDurumu = "success";
                result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
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
