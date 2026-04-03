using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using Isopoh.Cryptography.Blake2b;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Quartz;
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
    public class JobIslemUyeler : IJob
    {
        private readonly string _connectionString;

        public JobIslemUyeler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OpakSqlServer");
        }

        // :contentReference[oaicite:0]{index=0}

        public virtual async Task Execute(IJobExecutionContext context)
        {
            AppDbContext _context = new AppDbContext();
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM TBLCARISB WHERE GRUP_KODU = '001'", connection);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var yeniUyeler = new List<AppUser>();

                        var mevcutUserNames = _context.Users
                            .AsNoTracking()
                            .Select(u => u.UserName.ToLower())
                            .ToHashSet();

                        var mevcutKullanicilar = _context.Users
                            .AsNoTracking()
                            .Select(u => new { UserName = u.UserName.ToLower(), u.CariKodu })
                            .ToList();

                        var yeniUserNames = new HashSet<string>();

                        while (dr.Read())
                        {
                            var kod = dr["KOD"]?.ToString().Trim();
                            var cariAdi = dr["CARIADI"]?.ToString().Trim();
                            var cariSoyadi = dr["CARISOYADI"]?.ToString().Trim();
                            var adi = dr["ADI"]?.ToString().Trim();
                            var adres = dr["ADRES"]?.ToString().Trim();
                            var email = dr["EMAIL"]?.ToString().Trim();
                            var vergino = dr["VERGINO"]?.ToString().Trim();
                            int plasiyerid = Convert.ToInt32(dr["PLASIYERID"] ?? 0);
                            int cariid = Convert.ToInt32(dr["ID"]);
                            decimal iskonto = 0;
                            if (dr["ISKONTO"] != null && !string.IsNullOrWhiteSpace(dr["ISKONTO"].ToString()))
                                decimal.TryParse(dr["ISKONTO"].ToString().Trim(), out iskonto);

                            decimal risk = 0;
                            if (dr["RISK"] != null && !string.IsNullOrWhiteSpace(dr["RISK"].ToString()))
                                decimal.TryParse(dr["RISK"].ToString().Trim(), out risk);

                            var telefon = dr["TELEFON"]?.ToString().Trim();
                            var b2bSifre = dr["B2BSIFRE"]?.ToString().Trim();

                            if (string.IsNullOrEmpty(kod)) continue;

                            AppUser mevcutUye = null;

                            // 🔥 SIRALI EŞLEŞME (kritik fix)
                            if (!string.IsNullOrEmpty(vergino))
                                mevcutUye = _context.Users.FirstOrDefault(u => u.VergiNumarasi == vergino);

                            if (mevcutUye == null && !string.IsNullOrEmpty(kod))
                                mevcutUye = _context.Users.FirstOrDefault(u => u.CariKodu == kod);

                            if (mevcutUye == null && !string.IsNullOrEmpty(email))
                                mevcutUye = _context.Users.FirstOrDefault(u => u.Email == email);

                            if (mevcutUye != null)
                            {
                                mevcutUye.OpakCariId = cariid;
                                mevcutUye.PlasiyerId = plasiyerid == 0 ? null : plasiyerid;

                                if (!string.IsNullOrWhiteSpace(kod))
                                    mevcutUye.CariKodu = kod;

                                if (!string.IsNullOrWhiteSpace(cariAdi))
                                    mevcutUye.Ad = cariAdi;

                                if (!string.IsNullOrWhiteSpace(cariSoyadi))
                                    mevcutUye.Soyad = cariSoyadi;

                                if (!string.IsNullOrWhiteSpace(adi))
                                    mevcutUye.FirmaAdi = adi;

                                if (!string.IsNullOrWhiteSpace(email))
                                    mevcutUye.Email = email;

                                if (!string.IsNullOrWhiteSpace(adres))
                                    mevcutUye.Adres = adres;

                                if (iskonto > 0)
                                    mevcutUye.IskontoOrani = iskonto;

                                mevcutUye.CariLimit = risk;

                                if (!string.IsNullOrWhiteSpace(telefon))
                                    mevcutUye.PhoneNumber = telefon;

                                if (!string.IsNullOrWhiteSpace(b2bSifre))
                                {
                                    var hasher = new PasswordHasher<AppUser>();
                                    mevcutUye.PasswordHash = hasher.HashPassword(null, b2bSifre);
                                }

            
                                mevcutUye.UyeDurumu = UyeDurumlari.Onaylandi;

                                mevcutUye.Ad = cariAdi;
                                mevcutUye.Soyad = cariSoyadi;
                                mevcutUye.FirmaAdi = adi;
                                mevcutUye.Adres = adres;
                                mevcutUye.VergiNumarasi = vergino;
                                mevcutUye.Gsm = telefon;
                                mevcutUye.IskontoOrani = iskonto;
                                mevcutUye.CariLimit = risk;

                            }
                            else
                            {
                                var userNameBase = NormalizeUserName(kod);
                                var userName = MakeUserNameUnique(userNameBase, mevcutUserNames, yeniUserNames);

                                if (yeniUyeler.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)))
                                {
                                    Console.WriteLine($"⛔ Aynı UserName zaten yeniUyeler listesinde: {userName} – ATLANDI");
                                    continue;
                                }

                                Console.WriteLine($"🟢 Yeni kullanıcı eklenecek: {userName} (CariKodu: {kod})");

                                var hasher = new PasswordHasher<AppUser>();
                                var yeniUye = new AppUser
                                {
                                    UyeKayitTipi = UyeKayitTipi.Opak,
                                    Tarih = DateTime.Now,
                                    UserName = userName,
                                    NormalizedUserName = userName.ToUpper(),
                                    Ad = cariAdi,
                                    Soyad = cariSoyadi,
                                    FirmaAdi = adi,
                                    CariKodu = kod,
                                    Email = email,
                                    Adres = adres,
                                    IskontoOrani = iskonto,
                                    CariLimit = risk,
                                    PhoneNumber = telefon,
                                    NormalizedEmail = email?.ToUpper(),
                                    PasswordHash = hasher.HashPassword(null, b2bSifre),
                                    EmailConfirmed = true,
                                    LockoutEnabled = false,
                                    UyeDurumu = UyeDurumlari.Onaylandi,
                                    SecurityStamp = Guid.NewGuid().ToString("D"),
                                    OpakCariId = cariid,
                                    PlasiyerId = plasiyerid == 0 ? null : plasiyerid
                                };

                                yeniUyeler.Add(yeniUye);
                            }
                        }

                        if (yeniUyeler.Any())
                        {
                            AddUsersWithRoles(_context, yeniUyeler);
                        }

                        await _context.SaveChangesAsync();
                    }

                    Console.WriteLine("✅ İşlem başarılı bir şekilde tamamlandı.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Hatası: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                    Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                }
            }
        }

        private void AddUsersWithRoles(AppDbContext context, List<AppUser> users)
        {
            foreach (var user in users)
            {
                try
                {
                    context.Users.Add(user);
                    context.SaveChanges();

                    context.UserRoles.Add(new AppUserRole
                    {
                        UserId = user.Id,
                        RoleId = (int)RolTipleri.Bayi
                    });
                    context.SaveChanges();

                    Console.WriteLine($"✅ Kullanıcı eklendi: {user.UserName}");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"❌ Kullanıcı EKLENEMEDİ: {user.UserName}");
                    Console.WriteLine("Hata: " + ex.InnerException?.Message);
                    context.Entry(user).State = EntityState.Detached; // diğer kayıtları engellemesin
                }
            }
        }



        private string MakeUserNameUnique(string userName, HashSet<string> mevcutUserNames, HashSet<string> yeniUserNames)
        {
            var originalUserName = userName;
            int counter = 1;

            while (mevcutUserNames.Contains(userName.ToLower()) || yeniUserNames.Contains(userName.ToLower()))
            {
                Console.WriteLine($"⚠️ Çakışma: {userName.ToLower()} – yeni deneniyor...");
                userName = $"{originalUserName}_{counter}";
                counter++;
            }

            var unique = userName.ToLower();
            mevcutUserNames.Add(unique);
            yeniUserNames.Add(unique);

            Console.WriteLine($"✅ Eşsiz kullanıcı adı: {userName}");
            return userName;
        }




        private string NormalizeUserName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.Replace("-", "_");

            var invalidChars = new[] { ' ', '_', '@', '!', '#', '$', '%', '^', '&', '*', '(', ')', '=', '+', '[', ']', '{', '}', '\\', '|', ';', ':', '"', '\'', '<', '>', ',', '.', '?', '/' };
            foreach (var ch in invalidChars)
            {
                input = input.Replace(ch.ToString(), "");
            }

            return input.Replace("ç", "c")
                        .Replace("Ç", "C")
                        .Replace("ğ", "g")
                        .Replace("Ğ", "G")
                        .Replace("ı", "i")
                        .Replace("İ", "I")
                        .Replace("ö", "o")
                        .Replace("Ö", "O")
                        .Replace("ş", "s")
                        .Replace("Ş", "S")
                        .Replace("ü", "u")
                        .Replace("Ü", "U")
                        .ToLower();
        }


    }
}
