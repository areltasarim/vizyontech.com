using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EticaretWebCoreService
{

    public partial class FormlarServis : IFormlarServis
    {
        private readonly AppDbContext _context;

        private readonly string entity = "Form";

        public FormlarServis(AppDbContext _context)
        {
            this._context = _context;
        }

        public async Task<List<Formlar>> PageList(int FormBaslikId)
        {
            return (await _context.Formlar.Where(p=> p.FormBaslikId == FormBaslikId).ToListAsync());
        }

        public async Task<ResultViewModel> UpdatePage(FormViewModel Model, string submit)
        {

            var result = new ResultViewModel();

            int pageId = 0;
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Formlar()
                        {
                            FormBaslikId = Model.FormBaslikId,
                            FormTuru = Model.Form.FormTuru,
                            TexboxTipi = Model.Form.TexboxTipi,
                            Genislik = Model.Form.Genislik,
                            Zorunlumu = Model.Form.Zorunlumu,
                            Sira = Model.Form.Sira,
                            Durum = Model.Form.Durum,
                            FormlarTranslate = new List<FormlarTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaEkleTranslate = new FormlarTranslate()
                            {
                                FormAdi = Model.FormAdiCeviri[i],
                                PlaceHolder = Model.PlaceHolderCeviri[i],
                                HataMesaji = Model.HataMesajiCeviri[i],
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.FormlarTranslate.Add(sayfaEkleTranslate);

                        }

                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();



                        #region Form Degerleri
                        int formdegerId = 0;
                        List<int> formDegerIdListesi = new List<int>();
                        if (Model.DegerAdiCeviri != null)
                        {
                            for (int i2 = 0; i2 < Model.FormDegerListesi.Length; i2++)
                            {
                                var formDegerEkle = new FormDegerleri()
                                {
                                    FormId = sayfaEkle.Id,
                                    Sira = Model.FormDeger.Sira,
                                    FormDegerleriTranslate = new List<FormDegerleriTranslate>(),
                                };
                                _context.Entry(formDegerEkle).State = EntityState.Added;
                                await _context.SaveChangesAsync();

                                formdegerId = formDegerEkle.Id;
                                for (int i = 0; i < diller.Count; i++)
                                {
                                    formDegerIdListesi.Add(formdegerId);
                                }
                            }


                            for (int i = 0; i < Model.DegerAdiCeviri.Length; i++)
                            {
                                var formDegerEkleTranslate = new FormDegerleriTranslate()
                                {
                                    FormId = sayfaEkle.Id,
                                    FormDegerId = formDegerIdListesi[i],
                                    DegerAdi = Model.DegerAdiCeviri[i],
                                    DilId = Model.DilIds[i],
                                };
                                _context.Entry(formDegerEkleTranslate).State = EntityState.Added;
                                await _context.SaveChangesAsync();
                            }
                        }
                        #endregion


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
                        var db = new AppDbContext();
                        db.Formlar.Find(Model.Id).FormlarTranslate.ToList().ForEach(p => db.FormlarTranslate.Remove(p));
                        db.SaveChanges();

                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Formlar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.FormBaslikId = Model.FormBaslikId;
                        sayfaGuncelle.FormTuru = Model.FormTuru;
                        sayfaGuncelle.TexboxTipi = Model.TexboxTipi;
                        sayfaGuncelle.Genislik = Model.Genislik;
                        sayfaGuncelle.Zorunlumu = Model.Zorunlumu;
                        sayfaGuncelle.Sira = Model.Sira;
                        sayfaGuncelle.Durum = Model.Durum;


                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            var sayfaGuncelleTranslate = new FormlarTranslate()
                            {
                                FormAdi = Model.FormAdiCeviri[i],
                                PlaceHolder = Model.PlaceHolderCeviri[i],
                                HataMesaji = Model.HataMesajiCeviri[i],
                                DilId = diller[i].Id,
                                FormId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        #endregion

                        #region Form Değerleri
                        if (Model.FormDegerGuncelleListesi != null)
                        {
                            foreach (var item in Model.FormDegerGuncelleListesi)
                            {
                                db.Entry(db.FormDegerleri.Find(item)).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                                db.SaveChanges();
                            }
                        }


                        if (Model.FormDegerListesi != null)
                        {

                            int formdegerId = 0;
                            List<int> formDegerIdListesi = new List<int>();

                            for (int i2 = 0; i2 < Model.FormDegerListesi.Length; i2++)
                            {
                                var formDegerEkle = new FormDegerleri()
                                {
                                    FormId = Model.Id,
                                    Sira = Model.FormDeger.Sira,
                                    FormDegerleriTranslate = new List<FormDegerleriTranslate>(),
                                };
                                _context.Entry(formDegerEkle).State = EntityState.Added;
                                await _context.SaveChangesAsync();

                                formdegerId = formDegerEkle.Id;
                                for (int i = 0; i < diller.Count; i++)
                                {
                                    formDegerIdListesi.Add(formdegerId);
                                }
                            }


                            for (int i = 0; i < Model.DegerAdiCeviri.Length; i++)
                            {
                                var formDegerEkleTranslate = new FormDegerleriTranslate()
                                {
                                    FormId = Model.Id,
                                    FormDegerId = formDegerIdListesi[i],
                                    DegerAdi = Model.DegerAdiCeviri[i],
                                    DilId = Model.DilIds[i],
                                };
                                _context.Entry(formDegerEkleTranslate).State = EntityState.Added;
                            }

                            await _context.SaveChangesAsync();

                        }
                        #endregion

                        await _context.SaveChangesAsync();

                        #region Sayfa Butonlari
                        if (submit == "Kaydet")
                        {
                            result.Action = "Index";
                        }
                        if (submit == "KaydetGuncelle")
                        {
                            result.Action = "AddOrUpdate";
                            result.SayfaId = sayfaGuncelle.Id;
                        }
                        #endregion


                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";

                    }

                    transaction.Complete();
                }

            }
            catch
            {
                result.Basarilimi = false;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";
                result.SayfaId = pageId;
            }

            return result;
        }

        public async Task<ResultViewModel> DeletePage(FormViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in _context.FormDegerleri.Where(p => p.FormId == Model.Id).ToList())
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                        await _context.SaveChangesAsync();
                    }

                    var model = _context.Formlar.Find(Model.Id);
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
                            var model = _context.Formlar.Find(item);
                            _context.Entry(model).State = EntityState.Deleted;
                        }

                        await _context.SaveChangesAsync();

                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = "Seçili Kayıtlar Başarıyla Silindi.";
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

    }
}
