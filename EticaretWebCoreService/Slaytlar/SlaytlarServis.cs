using EticaretWebCoreCaching.Abstraction;
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

    public partial class SlaytlarServis : ISlaytlarServis
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        private readonly string entity = "Slayt";

        public SlaytlarServis(AppDbContext _context, ICacheService cacheService)
        {
            this._context = _context;
            _cacheService = cacheService;
        }



        public async Task<List<Slaytlar>> PageList()
        {
            return (await _context.Slaytlar.ToListAsync());
        }


        public async Task<ResultViewModel> UpdatePage(SlaytViewModel Model, string submit)
        {

            var result = new ResultViewModel();
            int pageId= 0;
            try
            {
                
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string video = "";

                    List<string> ResimDosyaTipleri = new()
                    {
                        "image/jpeg",
                        "image/png",
                        "image/gif",
                        "image/webp",
                        "image/svg+xml"
                    };

                    List<string> videoTipleri = new()
                    {
                        "video/mp4",
                    };

                    if (Model.Id == 0)
                    {
                        #region Sayfa Ekleme
                        var sayfaEkle = new Slaytlar()
                        {
                            Sira = Model.Slayt.Sira,
                            BackgroundColor = Model.Slayt.BackgroundColor,
                            FontColor = Model.Slayt.FontColor,
                            Durum = Model.Slayt.Durum,
                            SlaytlarTranslate = new List<SlaytlarTranslate>(),
                        };
                        _context.Entry(sayfaEkle).State = EntityState.Added;


                        string resim = "";
                        string arkaplanresim = "";

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {
                            #region Resim
                            resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);

                            var dilkodu = diller[i].DilKodlari.DilKodu;

                            if (Model.SayfaResmiSecimListesi != null)
                            {
                                var secilenresimlist = Model.SayfaResmiSecimListesi.FirstOrDefault(x => x != null && x.StartsWith(dilkodu));
                                if (secilenresimlist != null)
                                {


                                    var rsm = Model.SayfaResmi.FirstOrDefault(x => secilenresimlist.Contains(x.FileName));
                                    if (rsm != null)
                                    {

                                        string imageName = ImageHelper.ImageReplaceName(rsm, "");
                                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;
                                        FileInfo serverfile = new FileInfo(Mappath);
                                        if (!serverfile.Directory.Exists)
                                        {
                                            serverfile.Directory.Create();
                                        }
                                        if (ResimDosyaTipleri.Contains(rsm.ContentType))
                                        {
                                            //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                            //File.WriteAllBytes(Mappath, ImageHelper.Resize(rsm.OpenReadStream()));

                                            using (var stream = new FileStream(Mappath, FileMode.Create))
                                            {
                                                rsm.CopyTo(stream);
                                            }
                                            resim = Mappath.Remove(0, 7);
                                        }

                                        else
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                            result.SayfaId = sayfaEkle.Id;

                                            return result;
                                        }

                                        if (rsm.Length > 5242880)
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                            result.SayfaId = sayfaEkle.Id;

                                            return result;
                                        }

                                    }
                                    else
                                    {
                                        resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                                    }
                                }
                                else
                                {
                                    resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                                }
                            }

                            #endregion
                            #region Arkaplan Resim
                            if (Model.ArkaplanSayfaResim != null)
                            {

                                string imageName = ImageHelper.ImageReplaceName(Model.ArkaplanSayfaResim, Model.ArkaplanResim);

                                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                                if (ResimDosyaTipleri.Contains(Model.ArkaplanSayfaResim.ContentType))
                                {
                                    //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                    //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.ArkaplanSayfaResim.OpenReadStream()));

                                    using (var stream = new FileStream(Mappath, FileMode.Create))
                                    {
                                        Model.ArkaplanSayfaResim.CopyTo(stream);
                                    }

                                    sayfaEkle.ArkaplanResim = Mappath.Remove(0, 7);
                                }

                                else
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                    result.SayfaId = sayfaEkle.Id;

                                    return result;
                                }

                                if (Model.ArkaplanSayfaResim.Length > 5242880)
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                    result.SayfaId = sayfaEkle.Id;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkle.ArkaplanResim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                            }
                            #endregion Arkaplan 
                            #region Resim 1
                            if (Model.SlaytResim1 != null)
                            {

                                string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim1, Model.Resim1);

                                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                                if (ResimDosyaTipleri.Contains(Model.SlaytResim1.ContentType))
                                {
                                    //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                    //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim1.OpenReadStream()));

                                    using (var stream = new FileStream(Mappath, FileMode.Create))
                                    {
                                        Model.SlaytResim1.CopyTo(stream);
                                    }

                                    sayfaEkle.Resim1 = Mappath.Remove(0, 7);
                                }

                                else
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                    result.SayfaId = sayfaEkle.Id;

                                    return result;
                                }

                                if (Model.SlaytResim1.Length > 5242880)
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                    result.SayfaId = sayfaEkle.Id;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkle.Resim1 = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            }
                            #endregion
                            #region Resim 2
                            if (Model.SlaytResim2 != null)
                            {

                                string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim2, Model.Resim2);

                                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                                if (ResimDosyaTipleri.Contains(Model.SlaytResim2.ContentType))
                                {
                                    //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                    //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim2.OpenReadStream()));

                                    using (var stream = new FileStream(Mappath, FileMode.Create))
                                    {
                                        Model.SlaytResim2.CopyTo(stream);
                                    }

                                    sayfaEkle.Resim2 = Mappath.Remove(0, 7);
                                }

                                else
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                    result.SayfaId = sayfaEkle.Id;

                                    return result;
                                }

                                if (Model.SlaytResim2.Length > 5242880)
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                                    result.SayfaId = sayfaEkle.Id;

                                    return result;
                                }
                            }

                            else
                            {
                                sayfaEkle.Resim2 = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            }
                            #endregion
                            //#region Resim 3
                            //if (Model.SlaytResim3 != null)
                            //{

                            //    string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim3, Model.Resim3);

                            //    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                            //    if (ResimDosyaTipleri.Contains(Model.SlaytResim3.ContentType))
                            //    {
                            //        //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                            //        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim3.OpenReadStream()));

                            //        using (var stream = new FileStream(Mappath, FileMode.Create))
                            //        {
                            //            Model.SlaytResim3.CopyTo(stream);
                            //        }

                            //        sayfaEkle.Resim3 = Mappath.Remove(0, 7);
                            //    }

                            //    else
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                            //        result.SayfaId = sayfaEkle.Id;

                            //        return result;
                            //    }

                            //    if (Model.SlaytResim3.Length > 5242880)
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                            //        result.SayfaId = sayfaEkle.Id;

                            //        return result;
                            //    }
                            //}

                            //else
                            //{
                            //    sayfaEkle.Resim3 = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            //}
                            //#endregion
                            //#region Resim 4
                            //if (Model.SlaytResim4 != null)
                            //{

                            //    string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim4, Model.Resim4);

                            //    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                            //    if (ResimDosyaTipleri.Contains(Model.SlaytResim4.ContentType))
                            //    {
                            //        //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                            //        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim4.OpenReadStream()));

                            //        using (var stream = new FileStream(Mappath, FileMode.Create))
                            //        {
                            //            Model.SlaytResim4.CopyTo(stream);
                            //        }

                            //        sayfaEkle.Resim4 = Mappath.Remove(0, 7);
                            //    }

                            //    else
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                            //        result.SayfaId = sayfaEkle.Id;

                            //        return result;
                            //    }

                            //    if (Model.SlaytResim4.Length > 5242880)
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                            //        result.SayfaId = sayfaEkle.Id;

                            //        return result;
                            //    }
                            //}

                            //else
                            //{
                            //    sayfaEkle.Resim4 = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            //}
                            //#endregion
                            //#region Resim 5
                            //if (Model.SlaytResim5 != null)
                            //{

                            //    string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim5, Model.Resim5);

                            //    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                            //    if (ResimDosyaTipleri.Contains(Model.SlaytResim5.ContentType))
                            //    {
                            //        //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                            //        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim5.OpenReadStream()));

                            //        using (var stream = new FileStream(Mappath, FileMode.Create))
                            //        {
                            //            Model.SlaytResim5.CopyTo(stream);
                            //        }

                            //        sayfaEkle.Resim5 = Mappath.Remove(0, 7);
                            //    }

                            //    else
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                            //        result.SayfaId = sayfaEkle.Id;

                            //        return result;
                            //    }

                            //    if (Model.SlaytResim5.Length > 5242880)
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                            //        result.SayfaId = sayfaEkle.Id;

                            //        return result;
                            //    }
                            //}

                            //else
                            //{
                            //    sayfaEkle.Resim5 = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                            //}
                            //#endregion

                            #region Video
                            video = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                            if(Model.VideoSecimListesi != null)
                            {
                                var secilenvideolistesi = Model.VideoSecimListesi.FirstOrDefault(x => x != null && x.StartsWith(dilkodu));
                                if (secilenvideolistesi != null)
                                {

                                    var videoDosya = Model.SayfaVideo.FirstOrDefault(x => secilenvideolistesi.Contains(x.FileName));
                                    if (videoDosya != null)
                                    {

                                        string videoName = ImageHelper.ImageReplaceName(videoDosya, "");
                                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + videoName;
                                        FileInfo serverfile = new FileInfo(Mappath);
                                        if (!serverfile.Directory.Exists)
                                        {
                                            serverfile.Directory.Create();
                                        }
                                        if (videoTipleri.Contains(videoDosya.ContentType))
                                        {
                                            using (var stream = new FileStream(Mappath, FileMode.Create))
                                            {
                                                videoDosya.CopyTo(stream);
                                            }
                                            video = Mappath.Remove(0, 7);
                                        }

                                        else
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Mp4 formatında dosya yükleyiniz.";

                                            return result;
                                        }

                                        if (videoDosya.Length > 26214400)
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Maksimum 25 Mb boyutunda dosya yükleyiniz.";

                                            return result;
                                        }

                                    }

                                }
                                else
                                {
                                    video = "#";
                                }
                            }
                           
                            #endregion

                            var sayfaEkleTranslate = new SlaytlarTranslate()
                            {
                                SlaytBaslik = Model.SlaytBaslikCeviri != null && i < Model.SlaytBaslikCeviri.Length ? Model.SlaytBaslikCeviri[i] : string.Empty,
                                SlaytBaslik2 = Model.SlaytBaslikCeviri2 != null && i < Model.SlaytBaslikCeviri2.Length ? Model.SlaytBaslikCeviri2[i] : string.Empty,
                                SlaytBaslik3 = Model.SlaytBaslikCeviri3 != null && i < Model.SlaytBaslikCeviri3.Length ? Model.SlaytBaslikCeviri3[i] : string.Empty,
                                SlaytBaslik4 = Model.SlaytBaslikCeviri4 != null && i < Model.SlaytBaslikCeviri4.Length ? Model.SlaytBaslikCeviri4[i] : string.Empty,
                                SlaytBaslik5 = Model.SlaytBaslikCeviri5 != null && i < Model.SlaytBaslikCeviri5.Length ? Model.SlaytBaslikCeviri5[i] : string.Empty,
                                Aciklama = Model.AciklamaCeviri != null && i < Model.AciklamaCeviri.Length ? Model.AciklamaCeviri[i] : string.Empty,
                                ButonAdi = Model.ButonAdiCeviri != null && i < Model.ButonAdiCeviri.Length ? Model.ButonAdiCeviri[i] : string.Empty,
                                Url = Model.UrlCeviri != null && i < Model.UrlCeviri.Length ? Model.UrlCeviri[i] : string.Empty,
                                ButonAdi2 = Model.ButonAdiCeviri2 != null && i < Model.ButonAdiCeviri2.Length ? Model.ButonAdiCeviri2[i] : string.Empty,
                                Url2 = Model.UrlCeviri2 != null && i < Model.UrlCeviri2.Length ? Model.UrlCeviri2[i] : string.Empty,
                                YoutubeVideo = Model.YoutubeVideoCeviri != null && i < Model.YoutubeVideoCeviri.Length ? Model.YoutubeVideoCeviri[i] : string.Empty,
                                Video = video,
                                Resim = resim,
                                DilId = diller[i].Id,
                            };
                            sayfaEkle.SlaytlarTranslate.Add(sayfaEkleTranslate);

                        }
                        _context.Entry(sayfaEkle).State = EntityState.Added;
                        await _context.SaveChangesAsync();

                        #endregion

                        #region Autocomplete
                        if (Model.SeciliUrunlerAutocomplete != null)
                        {
                            foreach (var item in Model.SeciliUrunlerAutocomplete)
                            {
                                var urunToSlaytEkle = new UrunToSlayt()
                                {
                                    SlaytId = sayfaEkle.Id,
                                    UrunId = item
                                };
                                _context.Entry(urunToSlaytEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
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

                        _cacheService.RemoveByPattern($"SlaytResim");

                        pageId = sayfaEkle.Id;
                        result.Basarilimi = true;
                        result.MesajDurumu = "success";
                        result.Mesaj = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";
                    }

                    else
                    {
                        #region Sayfa Güncelleme
                        var sayfaGuncelle = _context.Slaytlar.Where(p => p.Id == Model.Id).FirstOrDefault();
                        pageId = sayfaGuncelle.Id;

                        sayfaGuncelle.BackgroundColor = Model.Slayt.BackgroundColor;
                        sayfaGuncelle.FontColor = Model.Slayt.FontColor;
                        sayfaGuncelle.Sira = Model.Slayt.Sira;
                        sayfaGuncelle.Durum = Model.Slayt.Durum;


                        string resim = "";
                        string arkaplanresim = "";

                        var diller = _context.Diller.ToList();

                        for (int i = 0; i < diller.Count; i++)
                        {

                            #region Resim

                            resim = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);

                            var dilkodu = diller[i].DilKodlari.DilKodu;
                            if(Model.SayfaResmiSecimListesi != null)
                            {
                                var secilenresimlist = Model.SayfaResmiSecimListesi.FirstOrDefault(x => x != null && x.StartsWith(dilkodu));
                                if (secilenresimlist != null)
                                {

                                    var rsm = Model.SayfaResmi.FirstOrDefault(x => secilenresimlist.Contains(x.FileName));
                                    if (rsm != null)
                                    {
                                        string imageName = ImageHelper.ImageReplaceName(rsm, "");
                                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;
                                        FileInfo serverfile = new FileInfo(Mappath);
                                        if (!serverfile.Directory.Exists)
                                        {
                                            serverfile.Directory.Create();
                                        }
                                        if (ResimDosyaTipleri.Contains(rsm.ContentType))
                                        {
                                            //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                            //File.WriteAllBytes(Mappath, ImageHelper.Resize(rsm.OpenReadStream()));

                                            using (var stream = new FileStream(Mappath, FileMode.Create))
                                            {
                                                rsm.CopyTo(stream);
                                            }
                                            resim = Mappath.Remove(0, 7);
                                        }

                                        else
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                            result.SayfaId = sayfaGuncelle.Id;

                                            return result;
                                        }

                                        if (rsm.Length > 5242880)
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
                                        resim = ImageHelper.DosyaYok(DosyaYoluTipleri.Resim);
                                    }
                                }
                                else
                                {
                                    resim = new AppDbContext().Slaytlar.Find(Model.Id).SlaytlarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim;
                                }
                            }

                            #endregion Resim
                            #region Arkaplan Resim
                            if (Model.ArkaplanSayfaResim != null)
                            {

                                string imageName = ImageHelper.ImageReplaceName(Model.ArkaplanSayfaResim, Model.ArkaplanResim);

                                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                                if (ResimDosyaTipleri.Contains(Model.ArkaplanSayfaResim.ContentType))
                                {
                                    //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                    //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.ArkaplanSayfaResim.OpenReadStream()));

                                    using (var stream = new FileStream(Mappath, FileMode.Create))
                                    {
                                        Model.ArkaplanSayfaResim.CopyTo(stream);
                                    }

                                    sayfaGuncelle.ArkaplanResim = Mappath.Remove(0, 7);
                                }

                                else
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                    result.SayfaId = sayfaGuncelle.Id;

                                    return result;
                                }

                                if (Model.ArkaplanSayfaResim.Length > 5242880)
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
                                sayfaGuncelle.ArkaplanResim = new AppDbContext().Slaytlar.Find(sayfaGuncelle.Id).ArkaplanResim;
                            }
                            #endregion Arkaplan Resim
                            #region Resim 1
                            if (Model.SlaytResim1 != null)
                            {

                                string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim1, Model.Resim1);

                                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                                if (ResimDosyaTipleri.Contains(Model.SlaytResim1.ContentType))
                                {
                                    //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                    //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim1.OpenReadStream()));

                                    using (var stream = new FileStream(Mappath, FileMode.Create))
                                    {
                                        Model.SlaytResim1.CopyTo(stream);
                                    }

                                    sayfaGuncelle.Resim1 = Mappath.Remove(0, 7);
                                }

                                else
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                    result.SayfaId = sayfaGuncelle.Id;

                                    return result;
                                }

                                if (Model.SlaytResim1.Length > 5242880)
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
                                sayfaGuncelle.Resim1 = new AppDbContext().Slaytlar.Find(sayfaGuncelle.Id).Resim1;
                            }
                            #endregion Resim 1
                            #region Resim 2
                            if (Model.SlaytResim2 != null)
                            {

                                string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim2, Model.Resim2);

                                string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                                if (ResimDosyaTipleri.Contains(Model.SlaytResim2.ContentType))
                                {
                                    //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                                    //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim2.OpenReadStream()));

                                    using (var stream = new FileStream(Mappath, FileMode.Create))
                                    {
                                        Model.SlaytResim2.CopyTo(stream);
                                    }

                                    sayfaGuncelle.Resim2 = Mappath.Remove(0, 7);
                                }

                                else
                                {
                                    result.Basarilimi = false;
                                    result.MesajDurumu = "danger";
                                    result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                                    result.SayfaId = sayfaGuncelle.Id;

                                    return result;
                                }

                                if (Model.SlaytResim2.Length > 5242880)
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
                                sayfaGuncelle.Resim2 = new AppDbContext().Slaytlar.Find(sayfaGuncelle.Id).Resim2;
                            }
                            #endregion Resim 2
                            //#region Resim 3
                            //if (Model.SlaytResim3 != null)
                            //{

                            //    string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim3, Model.Resim3);

                            //    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                            //    if (ResimDosyaTipleri.Contains(Model.SlaytResim3.ContentType))
                            //    {
                            //        //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                            //        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim3.OpenReadStream()));

                            //        using (var stream = new FileStream(Mappath, FileMode.Create))
                            //        {
                            //            Model.SlaytResim3.CopyTo(stream);
                            //        }

                            //        sayfaGuncelle.Resim3 = Mappath.Remove(0, 7);
                            //    }

                            //    else
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                            //        result.SayfaId = sayfaGuncelle.Id;

                            //        return result;
                            //    }

                            //    if (Model.SlaytResim3.Length > 5242880)
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                            //        result.SayfaId = sayfaGuncelle.Id;

                            //        return result;
                            //    }
                            //}

                            //else
                            //{
                            //    sayfaGuncelle.Resim3 = new AppDbContext().Slaytlar.Find(sayfaGuncelle.Id).Resim3;
                            //}
                            //#endregion Resim 3
                            //#region Resim 4
                            //if (Model.SlaytResim4 != null)
                            //{

                            //    string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim4, Model.Resim4);

                            //    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                            //    if (ResimDosyaTipleri.Contains(Model.SlaytResim4.ContentType))
                            //    {
                            //        //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                            //        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim4.OpenReadStream()));

                            //        using (var stream = new FileStream(Mappath, FileMode.Create))
                            //        {
                            //            Model.SlaytResim4.CopyTo(stream);
                            //        }

                            //        sayfaGuncelle.Resim4 = Mappath.Remove(0, 7);
                            //    }

                            //    else
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                            //        result.SayfaId = sayfaGuncelle.Id;

                            //        return result;
                            //    }

                            //    if (Model.SlaytResim4.Length > 5242880)
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                            //        result.SayfaId = sayfaGuncelle.Id;

                            //        return result;
                            //    }
                            //}

                            //else
                            //{
                            //    sayfaGuncelle.Resim4 = new AppDbContext().Slaytlar.Find(sayfaGuncelle.Id).Resim4;
                            //}
                            //#endregion Resim 4
                            //#region Resim 5
                            //if (Model.SlaytResim5 != null)
                            //{

                            //    string imageName = ImageHelper.ImageReplaceName(Model.SlaytResim5, Model.Resim5);

                            //    string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + imageName;

                            //    if (ResimDosyaTipleri.Contains(Model.SlaytResim5.ContentType))
                            //    {
                            //        //Resmi Belirli Boyutta Kaydetmek icin (ImageHelper dan boyutlandirma ayarlaniyor)
                            //        //File.WriteAllBytes(Mappath, ImageHelper.Resize(Model.SlaytResim5.OpenReadStream()));

                            //        using (var stream = new FileStream(Mappath, FileMode.Create))
                            //        {
                            //            Model.SlaytResim5.CopyTo(stream);
                            //        }

                            //        sayfaGuncelle.Resim5 = Mappath.Remove(0, 7);
                            //    }

                            //    else
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Jpeg, Png, Gif, Svg veya WebP formatinda resim yükleyiniz.";
                            //        result.SayfaId = sayfaGuncelle.Id;

                            //        return result;
                            //    }

                            //    if (Model.SlaytResim5.Length > 5242880)
                            //    {
                            //        result.Basarilimi = false;
                            //        result.MesajDurumu = "danger";
                            //        result.Mesaj = "Maksimum 5 Mb boyutunda resim yükleyiniz.";
                            //        result.SayfaId = sayfaGuncelle.Id;

                            //        return result;
                            //    }
                            //}

                            //else
                            //{
                            //    sayfaGuncelle.Resim5 = new AppDbContext().Slaytlar.Find(sayfaGuncelle.Id).Resim5;
                            //}
                            //#endregion Resim 5

                            #region Video
                            video = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim);
                            if(Model.VideoSecimListesi != null)
                            {
                                var secilenvideolistesi = Model.VideoSecimListesi.FirstOrDefault(x => x != null && x.StartsWith(dilkodu));
                                if (secilenvideolistesi != null)
                                {

                                    var videoDosya = Model.SayfaVideo.FirstOrDefault(x => secilenvideolistesi.Contains(x.FileName));
                                    if (videoDosya != null)
                                    {
                                        string videoName = ImageHelper.ImageReplaceName(videoDosya, "");
                                        string Mappath = ImageHelper.DosyaYolu(DosyaYoluTipleri.Resim) + "Slaytlar/" + videoName;
                                        FileInfo serverfile = new FileInfo(Mappath);
                                        if (!serverfile.Directory.Exists)
                                        {
                                            serverfile.Directory.Create();
                                        }
                                        if (videoTipleri.Contains(videoDosya.ContentType))
                                        {
                                            using (var stream = new FileStream(Mappath, FileMode.Create))
                                            {
                                                videoDosya.CopyTo(stream);
                                            }
                                            video = Mappath.Remove(0, 7);
                                        }

                                        else
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Mp4 formatında dosya yükleyiniz.";
                                            result.SayfaId = sayfaGuncelle.Id;

                                            return result;
                                        }

                                        if (videoDosya.Length > 26214400)
                                        {
                                            result.Basarilimi = false;
                                            result.MesajDurumu = "danger";
                                            result.Mesaj = "Maksimum 25 Mb boyutunda dosya yükleyiniz.";
                                            result.SayfaId = sayfaGuncelle.Id;

                                            return result;
                                        }

                                    }

                                }
                                else
                                {
                                    video = new AppDbContext().Slaytlar.Find(Model.Id).SlaytlarTranslate.SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Video;
                                }
                            }
                           
                            #endregion

                            var sayfaGuncelleTranslate = new SlaytlarTranslate()
                            {
                                SlaytBaslik = Model.SlaytBaslikCeviri != null && i < Model.SlaytBaslikCeviri.Length ? Model.SlaytBaslikCeviri[i] : string.Empty,
                                SlaytBaslik2 = Model.SlaytBaslikCeviri2 != null && i < Model.SlaytBaslikCeviri2.Length ? Model.SlaytBaslikCeviri2[i] : string.Empty,
                                SlaytBaslik3 = Model.SlaytBaslikCeviri3 != null && i < Model.SlaytBaslikCeviri3.Length ? Model.SlaytBaslikCeviri3[i] : string.Empty,
                                SlaytBaslik4 = Model.SlaytBaslikCeviri4 != null && i < Model.SlaytBaslikCeviri4.Length ? Model.SlaytBaslikCeviri4[i] : string.Empty,
                                SlaytBaslik5 = Model.SlaytBaslikCeviri5 != null && i < Model.SlaytBaslikCeviri5.Length ? Model.SlaytBaslikCeviri5[i] : string.Empty,
                                Aciklama = Model.AciklamaCeviri != null && i < Model.AciklamaCeviri.Length ? Model.AciklamaCeviri[i] : string.Empty,
                                ButonAdi = Model.ButonAdiCeviri != null && i < Model.ButonAdiCeviri.Length ? Model.ButonAdiCeviri[i] : string.Empty,
                                Url = Model.UrlCeviri != null && i < Model.UrlCeviri.Length ? Model.UrlCeviri[i] : string.Empty,
                                ButonAdi2 = Model.ButonAdiCeviri2 != null && i < Model.ButonAdiCeviri2.Length ? Model.ButonAdiCeviri2[i] : string.Empty,
                                Url2 = Model.UrlCeviri2 != null && i < Model.UrlCeviri2.Length ? Model.UrlCeviri2[i] : string.Empty,
                                Video = video,
                                YoutubeVideo = Model.YoutubeVideoCeviri != null && i < Model.YoutubeVideoCeviri.Length ? Model.YoutubeVideoCeviri[i] : string.Empty,
                                Resim = resim,
                                DilId = diller[i].Id,
                                SlaytId = Model.Id
                            };
                            _context.Entry(sayfaGuncelleTranslate).State = EntityState.Added;
                        }

                        var db = new AppDbContext();
                        db.Slaytlar.Find(Model.Id).SlaytlarTranslate.ToList().ForEach(p => db.SlaytlarTranslate.Remove(p));
                        await db.SaveChangesAsync();


                        _context.Entry(sayfaGuncelle).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        #endregion

                        #region Autocomplete
                        if (Model.SeciliUrunlerAutocomplete != null)
                        {
                            db.UrunToSlayt.Where(p => p.SlaytId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunToSlayt.Remove(p));
                            db.SaveChanges();

                            foreach (var item in Model.SeciliUrunlerAutocomplete)
                            {
                                var urunToSlaytEkle = new UrunToSlayt()
                                {
                                    SlaytId = sayfaGuncelle.Id,
                                    UrunId = item
                                };
                                _context.Entry(urunToSlaytEkle).State = EntityState.Added;
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            db.UrunToSlayt.Where(p => p.SlaytId == sayfaGuncelle.Id).ToList().ForEach(p => db.UrunToSlayt.Remove(p));
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
                            result.SayfaId = sayfaGuncelle.Id;
                        }
                        #endregion

                        _cacheService.RemoveByPattern($"SlaytResim");

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
                result.SayfaId = pageId;
                result.MesajDurumu = "danger";
                result.Mesaj = "Hata Oluştu.";

            }

 

            return result;

        }

        public async Task<ResultViewModel> DeletePage(SlaytViewModel Model)
        {
            var result = new ResultViewModel();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var model = _context.Slaytlar.Find(Model.Id);
                    _context.Entry(model).State = EntityState.Deleted;
                    var diller = _context.Diller.ToList();
                    for (int i = 0; i < diller.Count; i++)
                    {

                        FileInfo file = new(@"wwwroot" + model.SlaytlarTranslate.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                    }
                    await _context.SaveChangesAsync();

                    _cacheService.RemoveByPattern($"SlaytResim");

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
                            var model = _context.Slaytlar.Find(item);

                            _context.Entry(_context.Slaytlar.Find(item)).State = EntityState.Deleted;

                            var diller = _context.Diller.ToList();
                            for (int i = 0; i < diller.Count; i++)
                            {
                                FileInfo file = new(@"wwwroot" + model.SlaytlarTranslate.Where(p => p.Resim != "/Content/Upload/Images/resimyok.png").SingleOrDefault(p => p.Diller.DilKodlari.DilKodu == diller[i].DilKodlari.DilKodu)?.Resim);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                            }
                        }

                        await _context.SaveChangesAsync();

                        _cacheService.RemoveByPattern($"SlaytResim");

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
