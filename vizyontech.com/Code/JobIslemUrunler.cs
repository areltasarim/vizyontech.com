using DocumentFormat.OpenXml.Office2010.Excel;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
using EticaretWebCoreViewModel;
using Humanizer;
using Isopoh.Cryptography.Blake2b;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NPOI.SS.Formula.Functions;
using Quartz;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace vizyontech.com.Code
{
    public class JobIslemUrunler : IJob
    {
        private readonly string _connectionString;
        private readonly SeoServis _seoServis;
        private readonly ICacheService _cacheService;

        public JobIslemUrunler(IConfiguration configuration, SeoServis seoServis, ICacheService cacheService)
        {
            _connectionString = configuration.GetConnectionString("OpakSqlServer");
            _seoServis = seoServis;
            _cacheService = cacheService;
        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            AppDbContext _context = new AppDbContext();
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(@"
            SELECT TSB.KOD AS TSB_KOD, TSB.ADI AS TSB_ADI, 
                   TSB.LISTEFIYAT AS TSB_LISTEFIYAT, TSB.B2BFIYAT AS TSB_B2BFIYAT, 
                   TSB.MARKA AS TSB_MARKA,TSB.ACIKLAMA10 AS TSB_ACIKLAMA10, TWB.STOKID AS TWB_STOKID, 
                   TWB.B2C AS TWB_B2C 
            FROM TBLSTOKSB TSB 
            INNER JOIN TBLSTOKWEBSB TWB ON TSB.ID = TWB.STOKID 
            WHERE TWB.B2C = 'E'", connection);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var yeniUrunler = new List<Urunler>();

                        while (dr.Read())
                        {
                            var kod = dr["TSB_KOD"]?.ToString().Trim();
                            var urunAdi = dr["TSB_ADI"]?.ToString().Trim();
                            var marka = dr["TSB_MARKA"]?.ToString().Trim();

                            decimal listeFiyatı = 0;
                            decimal b2bFiyat = 0;
                            int sizeozelfiyatstoksarti = 0; // Varsayılan değer

                            if (dr["TSB_LISTEFIYAT"] != null && !string.IsNullOrWhiteSpace(dr["TSB_LISTEFIYAT"].ToString()))
                            {
                                decimal.TryParse(dr["TSB_LISTEFIYAT"].ToString().Trim(), out listeFiyatı);
                            }

                            if (dr["TSB_B2BFIYAT"] != null && !string.IsNullOrWhiteSpace(dr["TSB_B2BFIYAT"].ToString()))
                            {
                                decimal.TryParse(dr["TSB_B2BFIYAT"].ToString().Trim(), out b2bFiyat);
                            }

                            if (dr["TSB_ACIKLAMA10"] != null && !string.IsNullOrWhiteSpace(dr["TSB_ACIKLAMA10"].ToString()))
                            {
                                int.TryParse(dr["TSB_ACIKLAMA10"].ToString().Trim(), out sizeozelfiyatstoksarti);
                            }

                            if (string.IsNullOrEmpty(kod))
                            {
                                Console.WriteLine("Kod alanı boş, bu kayıt atlanacak.");
                                continue;
                            }

                            // Mevcut ürün kontrolü
                            var mevcutUrun = _context.Urunler.FirstOrDefault(u => u.UrunKodu == kod);

                            int? markaId = null; // Nullable MarkaId tanımlandı
                            if (!string.IsNullOrWhiteSpace(marka))
                            {
                                var markaNormalized = marka.Trim().ToLower();

                                var markaVarmi = _context.Markalar
                                    .AsEnumerable() // veritabanında case-insensitive karşılaştırma yoksa
                                    .FirstOrDefault(u => u.MarkaAdi?.Trim().ToLower() == markaNormalized);
                                if (markaVarmi == null)
                                {
                                    var yeniMarka = new Markalar()
                                    {
                                        MarkaAdi = marka,
                                        Durum = SayfaDurumlari.Aktif,
                                    };
                                    _context.Markalar.Add(yeniMarka);
                                    _context.SaveChanges();
                                    markaId = yeniMarka.Id;
                                }
                                else
                                {
                                    markaId = markaVarmi.Id;
                                }
                            }

                            int stok = GetStockValue(kod, _connectionString);

                            if (mevcutUrun != null)
                            {
                                // Mevcut ürünü güncelle
                                mevcutUrun.ListeFiyat = listeFiyatı;
                                mevcutUrun.SizeOzelFiyat = b2bFiyat;
                                mevcutUrun.OzelFiyatStokSarti = sizeozelfiyatstoksarti;
                                mevcutUrun.Stok = stok;
                                mevcutUrun.MarkaId = markaId ?? mevcutUrun.MarkaId;

                                if (mevcutUrun.UrunlerTranslate != null && mevcutUrun.UrunlerTranslate.Any())
                                {
                                    var urunTranslate = mevcutUrun.UrunlerTranslate.FirstOrDefault();
                                    if (urunTranslate != null)
                                    {
                                        urunTranslate.UrunAdi = urunAdi ?? urunTranslate.UrunAdi;
                                    }


                                    foreach (var urun in yeniUrunler)
                                    {
                                        var seoUrl = await _seoServis.SeoLinkOlustur(
                                            sayfaAdi: urunTranslate.UrunAdi,
                                            sayfaId: mevcutUrun.Id,
                                            entityName: SeoUrlTipleri.Urun,
                                            seoTipi: SeoTipleri.Urun,
                                            dilId: 1
                                        );
                                    }
                                }

                                _context.Urunler.Update(mevcutUrun);
                               
                            }
                            else
                            {
                                // Yeni ürün oluştur
                                var yeniUrun = new Urunler()
                                {
                                    UrunKodu = kod,
                                    ListeFiyat = listeFiyatı,
                                    SizeOzelFiyat = b2bFiyat,
                                    OzelFiyatStokSarti = sizeozelfiyatstoksarti,
                                    MarkaId = markaId,
                                    KdvId = 1,
                                    Stok = stok,
                                    Durum = SayfaDurumlari.Aktif,
                                    UrunlerTranslate = new List<UrunlerTranslate>()
                        {
                            new UrunlerTranslate
                            {
                                UrunAdi = urunAdi,
                                Resim = "/Content/Upload/Images/resimyok.png",
                                DilId = 1
                            }
                        }
                                };
                                yeniUrunler.Add(yeniUrun);
                            }
                        }

                        if (yeniUrunler.Any())
                        {
                            // Yeni ürünler toplu olarak kaydedilir
                            _context.Urunler.AddRange(yeniUrunler);
                            _context.SaveChanges();
                            // SEO URL'ler oluşturulur
                            foreach (var urun in yeniUrunler)
                            {
                                var urunAdi = urun.UrunlerTranslate.First().UrunAdi; // İlk çeviri üzerinden ürün adı alınır
                                var seoUrl = await _seoServis.SeoLinkOlustur(
                                    sayfaAdi: urunAdi,
                                    sayfaId: urun.Id, // SaveChanges sonrası ID kullanılır
                                    entityName: SeoUrlTipleri.Urun,
                                    seoTipi: SeoTipleri.Urun,
                                    dilId: 1
                                );
                            }
                        }

                        // Değişiklikleri kaydet
                        _context.SaveChanges();
                    }

                    _cacheService.RemoveByPattern("vizyontech.com");

                    Console.WriteLine("İşlem başarılı bir şekilde tamamlandı.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Hatası: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                }
            }
        }
        private int GetStockValue(string kod, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
            SELECT 
                COALESCE(
                    (SELECT SUM(MIKTAR) FROM TBLSTOKHAR T1 
                     LEFT JOIN TBLSTOKSB T2 ON(T1.STOKID=T2.ID) 
                     WHERE T2.KOD=@kod AND GCKOD='G' AND KAYITTIPI=0 AND ISLEMTIPI IN (0,1)) 
                , 0) - 
                COALESCE(
                    (SELECT SUM(MIKTAR) FROM TBLSTOKHAR T1 
                     LEFT JOIN TBLSTOKSB T2 ON(T1.STOKID=T2.ID) 
                     WHERE T2.KOD=@kod AND GCKOD='C' AND KAYITTIPI=0 AND ISLEMTIPI IN (0,1))
                , 0)", connection))
                {
                    command.Parameters.AddWithValue("@kod", kod);

                    var result = command.ExecuteScalar();
                    return (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                }
            }
        }

        
    }
}
