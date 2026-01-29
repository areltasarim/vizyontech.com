using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using static EticaretWebCoreEntity.DilCeviri;

namespace EticaretWebCoreEntity
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public AppDbContext()
        {

        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<AppRole>().HasData(
                new AppRole()
                {
                    Id = 1,
                    Name = RolTipleri.Administrator.ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    NormalizedName = RolTipleri.Administrator.ToString().ToUpper().Replace("İ", "I").Replace("ı", "i").Replace("Ş", "S").Replace("ş", "s").Replace("Ğ", "G").Replace("ğ", "g").Replace("Ü", "U").Replace("ü", "u").Replace("Ö", "O").Replace("ö", "o").Replace("Ç", "C").Replace("ç", "c"),
                },
                 new AppRole()
                 {
                     Id = 2,
                     Name = RolTipleri.Yonetici.ToString(),
                     ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                     NormalizedName = RolTipleri.Yonetici.ToString().ToUpper().Replace("İ", "I").Replace("ı", "i").Replace("Ş", "S").Replace("ş", "s").Replace("Ğ", "G").Replace("ğ", "g").Replace("Ü", "U").Replace("ü", "u").Replace("Ö", "O").Replace("ö", "o").Replace("Ç", "C").Replace("ç", "c"),
                 },
                 new AppRole()
                 {
                     Id = 3,
                     Name = RolTipleri.Bayi.ToString(),
                     ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                     NormalizedName = RolTipleri.Bayi.ToString().ToUpper().Replace("İ", "I").Replace("ı", "i").Replace("Ş", "S").Replace("ş", "s").Replace("Ğ", "G").Replace("ğ", "g").Replace("Ü", "U").Replace("ü", "u").Replace("Ö", "O").Replace("ö", "o").Replace("Ç", "C").Replace("ç", "c"),
                 }
                );
        }
        private void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<AppUser>();

            AppUser user = new AppUser()
            {
                Id = 1,
                UserName = "areltasarim",
                NormalizedUserName = "ARELTASARIM",
                Ad = "Eyyup",
                Soyad = "Balta",
                Email = "info@areltasarim.com",
                NormalizedEmail = "INFO@ARELTASARIM.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Arel3429*"),
                LockoutEnabled = false,
                PhoneNumber = "11111",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            builder.Entity<AppUser>().HasData(user);
        }
        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>() { RoleId = 1, UserId = 1 }
                );
        }
        private void SeedDilKodlari(ModelBuilder builder)
        {
            builder.Entity<DilKodlari>().HasData(
               new DilKodlari()
               {
                   Id = 1,
                   DilKodu = "tr-TR"
               },
               new DilKodlari()
               {
                   Id = 2,
                   DilKodu = "en-US",
               },
               new DilKodlari()
               {
                   Id = 3,
                   DilKodu = "de-DE",
               }
               );
        }
        private void SeedDiller(ModelBuilder builder)
        {
            builder.Entity<Diller>().HasData(
                new Diller() { Id = 1, DilAdi = "Türkçe", KisaDilAdi = "TR", DilKoduId = 1 }
                );
        }
        private void SeedParaBirimleri(ModelBuilder builder)
        {
            builder.Entity<ParaBirimleri>().HasData(
                new ParaBirimleri()
                {
                    Id = 1,
                    ParaBirimAdi = "Türk Lirası",
                    Kodu = "TRY",
                    DilKoduId = 1
                },
                new ParaBirimleri()
                {
                    Id = 2,
                    ParaBirimAdi = "Dolar",
                    Kodu = "USD",
                    DilKoduId = 2
                },
                new ParaBirimleri()
                {
                    Id = 3,
                    ParaBirimAdi = "Euro",
                    Kodu = "EUR",
                    DilKoduId = 3
                }
                );
        }

        private void SeedKategoriler(ModelBuilder builder)
        {
            builder.Entity<Kategoriler>().HasData(
                new Kategoriler() { Id = 1, ParentKategoriId = 1, Durum = SayfaDurumlari.Aktif }
                );
        }
        private void SeedKategorilerTranslate(ModelBuilder builder)
        {
            builder.Entity<KategorilerTranslate>().HasData(
                new KategorilerTranslate() { Id = 1, KategoriId = 1, KategoriAdi = "Ana Kategori", DilId = 1 }
                );
        }
        private void SeedSayfalar(ModelBuilder builder)
        {
            builder.Entity<Sayfalar>().HasData(
                new Sayfalar() { Id = 1, ParentSayfaId = 1, Durum = SayfaDurumlari.Aktif }
                );
        }
        private void SeedSayfalarTranslate(ModelBuilder builder)
        {
            builder.Entity<SayfalarTranslate>().HasData(
                new SayfalarTranslate() { Id = 1, SayfaId = 1, SayfaAdi = "Ana Kategori", DilId = 1 }
                );
        }

        private void SeedSabitMenuler(ModelBuilder builder)
        {
            builder.Entity<SabitMenuler>().HasData(
                new SabitMenuler()
                {
                    Id = 1,
                    SayfaTipi = SabitSayfaTipleri.Markalar,
                    Durum = SayfaDurumlari.Aktif
                },
                new SabitMenuler()
                {
                    Id = 2,
                    SayfaTipi = SabitSayfaTipleri.Kategoriler,
                    Durum = SayfaDurumlari.Aktif
                },
                 new SabitMenuler()
                 {
                     Id = 3,
                     SayfaTipi = SabitSayfaTipleri.Urunler,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SabitMenuler()
                 {
                     Id = 4,
                     SayfaTipi = SabitSayfaTipleri.BizeUlasin,
                     Durum = SayfaDurumlari.Aktif
                 }
            );
        }
        private void SeedSabitMenulerTranslate(ModelBuilder builder)
        {
            builder.Entity<SabitMenulerTranslate>().HasData(
                new SabitMenulerTranslate()
                {
                    Id = 1,
                    SabitMenuId = 1,
                    MenuAdi = "Markalar",
                    Url = "tum-markalar",
                    DilId = 1
                },
                new SabitMenulerTranslate()
                {
                    Id = 2,
                    SabitMenuId = 2,
                    MenuAdi = "Kategoriler",
                    Url = "tum-kategoriler",
                    DilId = 1
                },
                new SabitMenulerTranslate()
                {
                    Id = 3,
                    SabitMenuId = 3,
                    MenuAdi = "Ürünler",
                    Url = "tum-urunler",
                    DilId = 1
                },
                new SabitMenulerTranslate()
                {
                    Id = 4,
                    SabitMenuId = 4,
                    MenuAdi = "Bize Ulaşın",
                    Url = "iletisim",
                    DilId = 1
                }
            ); ;
        }

        private void SeedSeoUrl(ModelBuilder builder)
        {
            builder.Entity<SeoUrl>().HasData(
                new SeoUrl()
                {
                    Id = 1,
                    SeoTipi = SeoTipleri.Marka,
                    EntityName = SeoUrlTipleri.Markalar,
                    Url = "tum-markalar",
                    EntityId = 1,
                    DilId = 1
                },
               new SeoUrl()
               {
                   Id = 2,
                   SeoTipi = SeoTipleri.TumKategoriler,
                   EntityName = SeoUrlTipleri.Kategoriler,
                   Url = "tum-kategoriler",
                   EntityId = 2,
                   DilId = 1
               },
               new SeoUrl()
               {
                   Id = 3,
                   SeoTipi = SeoTipleri.TumUrunler,
                   EntityName = SeoUrlTipleri.Urunler,
                   Url = "tum-urunler",
                   EntityId = 3,
                   DilId = 1
               },
               new SeoUrl()
               {
                   Id = 4,
                   SeoTipi = SeoTipleri.BizeUlasin,
                   EntityName = SeoUrlTipleri.BizeUlasinSabitMenu,
                   Url = "iletisim",
                   EntityId = 4,
                   DilId = 1
               }
            ); ;
        }

        private void SeedMenuler(ModelBuilder builder)
        {
            builder.Entity<Menuler>().HasData(
                new Menuler() { Id = 1, ParentMenuId = 1, Durum = SayfaDurumlari.Aktif }
                );
        }
        private void SeedMenulerTranslate(ModelBuilder builder)
        {
            builder.Entity<MenulerTranslate>().HasData(
                new MenulerTranslate() { Id = 1, MenuId = 1, MenuAdi = "Ana Menü", DilId = 1 }
                );
        }
        private void SeedSiparisDurumlari(ModelBuilder builder)
        {
            builder.Entity<SiparisDurumlari>().HasData(
                new SiparisDurumlari()
                {
                    Id = (int)SiparisDurumTipleri.EksikSiparis,
                    Sira = 1,
                    Durum = SayfaDurumlari.Aktif
                },
                new SiparisDurumlari()
                {
                    Id = (int)SiparisDurumTipleri.OdemeBekleniyor,
                    Sira = 1,
                    Durum = SayfaDurumlari.Aktif
                },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.SiparisinizHazirlaniyor,
                     Sira = 2,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.SiparisinizOnaylandi,
                     Sira = 3,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.IslemeAlindi,
                     Sira = 4,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.KargoyaVerildi,
                     Sira = 5,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.OdemeBasarisiz,
                     Sira = 6,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.IptalEdildi,
                     Sira = 7,
                     Durum = SayfaDurumlari.Aktif
                 },
                 new SiparisDurumlari()
                 {
                     Id = (int)SiparisDurumTipleri.IadeEdildi,
                     Sira = 8,
                     Durum = SayfaDurumlari.Aktif
                 }
                );
        }
        private void SeedSiparisDurumlariTranslate(ModelBuilder builder)
        {
            builder.Entity<SiparisDurumlariTranslate>().HasData(
                new SiparisDurumlariTranslate()
                {
                    Id = 1,
                    SiparisDurumId = 1,
                    SiparisDurumu = "Eksik Sipariş",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 2,
                    SiparisDurumId = 2,
                    SiparisDurumu = "Ödeme Bekleniyor",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 3,
                    SiparisDurumId = 3,
                    SiparisDurumu = "Siparişiniz Hazırlanıyor",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 4,
                    SiparisDurumId = 4,
                    SiparisDurumu = "Siparişiniz Onaylandı",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 5,
                    SiparisDurumId = 5,
                    SiparisDurumu = "İşleme Alındı",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 6,
                    SiparisDurumId = 6,
                    SiparisDurumu = "Kargoya Verildi",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 7,
                    SiparisDurumId = 7,
                    SiparisDurumu = "Ödeme Başarısız",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 8,
                    SiparisDurumId = 8,
                    SiparisDurumu = "İptal Edildi",
                    DilId = 1
                },
                new SiparisDurumlariTranslate()
                {
                    Id = 9,
                    SiparisDurumId = 9,
                    SiparisDurumu = "İade Edildi",
                    DilId = 1
                }
                );
        }

        private void SeedOdemeMetodlari(ModelBuilder builder)
        {
            builder.Entity<OdemeMetodlari>().HasData(
                new OdemeMetodlari()
                {
                    Id = (int)OdemeMetodTiplieri.BankaHavalesi,
                    SiparisDurumId = (int)SiparisDurumTipleri.OdemeBekleniyor,
                    Sira = 1,
                    Durum = SayfaDurumlari.Aktif,
                },
                new OdemeMetodlari()
                {
                    Id = (int)OdemeMetodTiplieri.KapidaOdeme,
                    SiparisDurumId = (int)SiparisDurumTipleri.OdemeBekleniyor,
                    Sira = 2,
                    Durum = SayfaDurumlari.Aktif,
                },
                new OdemeMetodlari()
                {
                    Id = (int)OdemeMetodTiplieri.MagazadanTeslimAl,
                    SiparisDurumId = (int)SiparisDurumTipleri.OdemeBekleniyor,
                    Sira = 3,
                    Durum = SayfaDurumlari.Aktif,
                },
                new OdemeMetodlari()
                {
                    Id = (int)OdemeMetodTiplieri.Paytr,
                    SiparisDurumId = (int)SiparisDurumTipleri.SiparisinizOnaylandi,
                    Sira = 4,
                    Durum = SayfaDurumlari.Aktif,
                },
                new OdemeMetodlari()
                {
                    Id = (int)OdemeMetodTiplieri.Iyzico,
                    SiparisDurumId = (int)SiparisDurumTipleri.SiparisinizOnaylandi,
                    Sira = 5,
                    Durum = SayfaDurumlari.Aktif,
                }
                );
        }
        private void SeedOdemeMetodlariTranslate(ModelBuilder builder)
        {
            builder.Entity<OdemeMetodlariTranslate>().HasData(
                new OdemeMetodlariTranslate()
                {
                    Id = 1,
                    OdemeMetodId = 1,
                    OdemeAdi = "Banka Havalesi",
                    DilId = 1
                },
                new OdemeMetodlariTranslate()
                {
                    Id = 2,
                    OdemeMetodId = 2,
                    OdemeAdi = "Kapıda Ödeme",
                    DilId = 1
                },
                new OdemeMetodlariTranslate()
                {
                    Id = 3,
                    OdemeMetodId = 3,
                    OdemeAdi = "Mağazadan Teslim Al",
                    DilId = 1
                },
                new OdemeMetodlariTranslate()
                {
                    Id = 4,
                    OdemeMetodId = 4,
                    OdemeAdi = "Paytr",
                    DilId = 1
                }, new OdemeMetodlariTranslate()
                {
                    Id = 5,
                    OdemeMetodId = 5,
                    OdemeAdi = "İyzico",
                    DilId = 1
                }
                );
        }

        private void SeedKargoMetodlari(ModelBuilder builder)
        {
            builder.Entity<KargoMetodlari>().HasData(
                new KargoMetodlari()
                {
                    Id = (int)Enums.KargoMetodlari.Ucretsiz,
                    Sira = 1,
                    Durum = SayfaDurumlari.Aktif,
                },
                 new KargoMetodlari()
                 {
                     Id = (int)Enums.KargoMetodlari.SartliOdeme,
                     Sira = 2,
                     Durum = SayfaDurumlari.Aktif,
                 }
                );
        }
        private void SeedKargoMetodlariTranslate(ModelBuilder builder)
        {
            builder.Entity<KargoMetodlariTranslate>().HasData(
                new KargoMetodlariTranslate()
                {
                    Id = 1,
                    KargoMetodId = 1,
                    KargoAdi = "Ücretsiz Kargo",
                    DilId = 1
                },
                 new KargoMetodlariTranslate()
                 {
                     Id = 2,
                     KargoMetodId = 2,
                     KargoAdi = "Şartlı Ödeme",
                     DilId = 1
                 }
                );
        }
        private void SeedSiteAyarlari(ModelBuilder builder)
        {
            builder.Entity<SiteAyarlari>().HasData(
                new SiteAyarlari()
                {
                    Id = 1,
                    FirmaAdi = "Firma Adı Gelecek",
                    ParaBirimId = 1,
                    AktifDilId = 1,
                }
                );
        }
        private void SeedSiteAyarlariTranslate(ModelBuilder builder)
        {
            builder.Entity<SiteAyarlariTranslate>().HasData(
                new SiteAyarlariTranslate()
                {
                    Id = 1,
                    SiteAyarId = 1,
                    DilId = 1
                }
                );
        }

        private void SeedAdresBilgileri(ModelBuilder builder)
        {
            builder.Entity<AdresBilgileri>().HasData(
                new AdresBilgileri()
                {
                    Id = 1,
                    SiteAyarId = 1,
                }
                );
        }
        private void SeedAdresBilgileriTranslate(ModelBuilder builder)
        {
            builder.Entity<AdresBilgileriTranslate>().HasData(
                new AdresBilgileriTranslate()
                {
                    Id = 1,
                    AdresBilgiId = 1,
                    DilId = 1
                }
                );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            Assembly
                   .GetExecutingAssembly()
                   .GetTypes()
                   .Where(p => !p.IsAbstract && p.GetInterfaces().Any(q => q.Name == nameof(IBaseEntity)))
                   .ToList()
                   .ForEach(p => p.GetMethod(nameof(IBaseEntity.Build)).Invoke(Activator.CreateInstance(p), new[] { builder }));

            //aspnetusers tablosundaki ilişkili bağlantılar kısmı buradan yapılacak.
            builder.Entity<Ilceler>(b =>
            {
                // Each User can have many UserClaims
                builder.Entity<Ilceler>(entity =>
                {
                    entity
                    .HasMany(p => p.Uyeler)
                    .WithOne(p => p.Ilceler)
                    .HasForeignKey(p => p.IlceId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

                builder.Entity<Plasiyer>(entity =>
                {
                    entity
                    .HasMany(p => p.Uyeler)
                    .WithOne(p => p.Plasiyer)
                    .HasForeignKey(p => p.PlasiyerId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

                builder.Entity<AppUser>(entity =>
                {
                    entity
                   .Property(p => p.IskontoOrani)
                   .HasPrecision(18, 4);

                    entity
                   .Property(p => p.CariLimit)
                   .HasPrecision(18, 4);
                });
            });
            //aspnetusers tablosundaki ilişkili bağlantılar kısmı buradan yapılacak.

            base.OnModelCreating(builder);


            this.SeedRoles(builder);
            this.SeedUsers(builder);
            this.SeedUserRoles(builder);
            this.SeedDilKodlari(builder);
            this.SeedDiller(builder);
            this.SeedParaBirimleri(builder);
            this.SeedKategoriler(builder);
            this.SeedKategorilerTranslate(builder);
            this.SeedSayfalar(builder);
            this.SeedSayfalarTranslate(builder);
            this.SeedSabitMenuler(builder);
            this.SeedSabitMenulerTranslate(builder);
            this.SeedMenuler(builder);
            this.SeedMenulerTranslate(builder);
            this.SeedSeoUrl(builder);
            this.SeedSiparisDurumlari(builder);
            this.SeedSiparisDurumlariTranslate(builder);
            this.SeedOdemeMetodlari(builder);
            this.SeedOdemeMetodlariTranslate(builder);
            this.SeedKargoMetodlari(builder);
            this.SeedKargoMetodlariTranslate(builder);
            this.SeedSiteAyarlari(builder);
            this.SeedSiteAyarlariTranslate(builder);
            this.SeedAdresBilgileri(builder);
            this.SeedAdresBilgileriTranslate(builder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .Build();

            var provider = configuration["Application:DatabaseProvider"];


            if (provider == "SqlServer")
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                optionsBuilder.UseLazyLoadingProxies();
            }
            else
            {
                optionsBuilder.UseMySql(configuration.GetConnectionString("Mysql"), ServerVersion.AutoDetect(configuration.GetConnectionString("Mysql")));
                optionsBuilder.UseLazyLoadingProxies();
            }

        }

        public virtual DbSet<DilKodlari> DilKodlari { get; set; }
        public virtual DbSet<Diller> Diller { get; set; }
        public virtual DbSet<DilCeviri> DilCeviri { get; set; }
        public virtual DbSet<DilCeviriTranslate> DilCeviriTranslate { get; set; }
        public virtual DbSet<Adresler> Adresler { get; set; }
        public virtual DbSet<AlisverisListem> AlisverisListem { get; set; }
        public virtual DbSet<Begeniler> Begeniler { get; set; }
        public virtual DbSet<Kdv> Kdv { get; set; }
        public virtual DbSet<MesajKonulari> MesajKonulari { get; set; }
        public virtual DbSet<Mesajlar> Mesajlar { get; set; }
        public virtual DbSet<Bayiler> Bayiler { get; set; }
        public virtual DbSet<Fiyatlar> Fiyatlar { get; set; }
        public virtual DbSet<Plasiyer> Plasiyer { get; set; }
        public virtual DbSet<OdemeMetodlari> OdemeMetodlari { get; set; }
        public virtual DbSet<OdemeMetodlariTranslate> OdemeMetodlariTranslate { get; set; }
        public virtual DbSet<KargoMetodlari> KargoMetodlari { get; set; }
        public virtual DbSet<KargoMetodlariTranslate> KargoMetodlariTranslate { get; set; }
        public virtual DbSet<Sepet> Sepet { get; set; }
        public virtual DbSet<Siparisler> Siparisler { get; set; }
        public virtual DbSet<SiparisUrunleri> SiparisUrunleri { get; set; }
        public virtual DbSet<SiparisUrunSecenekleri> SiparisUrunSecenekleri { get; set; }
        public virtual DbSet<SiparisDurumlari> SiparisDurumlari { get; set; }
        public virtual DbSet<SiparisDurumlariTranslate> SiparisDurumlariTranslate { get; set; }
        public virtual DbSet<SiparisGecmisleri> SiparisGecmisleri { get; set; }
        public virtual DbSet<Markalar> Markalar { get; set; }
        public virtual DbSet<Kategoriler> Kategoriler { get; set; }
        public virtual DbSet<KategorilerTranslate> KategorilerTranslate { get; set; }
        public virtual DbSet<KategoriBanner> KategoriBanner { get; set; }
        public virtual DbSet<Urunler> Urunler { get; set; }
        public virtual DbSet<UrunlerTranslate> UrunlerTranslate { get; set; }
        public virtual DbSet<UrunSecenekleri> UrunSecenekleri { get; set; }
        public virtual DbSet<UrunSecenekleriTranslate> UrunSecenekleriTranslate { get; set; }
        public virtual DbSet<UrunSecenekDegerleri> UrunSecenekDegerleri { get; set; }
        public virtual DbSet<UrunSecenekDegerleriTranslate> UrunSecenekDegerleriTranslate { get; set; }
        public virtual DbSet<UrunToUrunSecenek> UrunToUrunSecenek { get; set; }
        public virtual DbSet<UrunToUrunSecenekToUrunDeger> UrunToUrunSecenekToUrunDeger { get; set; }
        public virtual DbSet<UrunToKategori> UrunToKategori { get; set; }
        public virtual DbSet<UrunOzellikGruplari> UrunOzellikGruplari { get; set; }
        public virtual DbSet<UrunOzellikGruplariTranslate> UrunOzellikGruplariTranslate { get; set; }
        public virtual DbSet<UrunToOzellik> UrunToOzellik { get; set; }
        public virtual DbSet<UrunOzellikleri> UrunOzellikleri { get; set; }
        public virtual DbSet<UrunOzellikleriTranslate> UrunOzellikleriTranslate { get; set; }
        public virtual DbSet<UrunResimleri> UrunResimleri { get; set; }
        public virtual DbSet<UrunToBenzerUrun> UrunToBenzerUrun { get; set; }
        public virtual DbSet<UrunToTamamlayiciUrun> UrunToTamamlayiciUrun { get; set; }
        public virtual DbSet<Kuponlar> Kuponlar { get; set; }
        public virtual DbSet<KuponToUrun> KuponToUrun { get; set; }
        public virtual DbSet<KuponToSiparis> KuponToSiparis { get; set; }
        public virtual DbSet<OneCikanKategoriler> OneCikanKategoriler { get; set; }
        public virtual DbSet<OneCikanKategorilerTranslate> OneCikanKategorilerTranslate { get; set; }
        public virtual DbSet<OneCikanUrunToKategoriler> OneCikanUrunToKategoriler { get; set; }

        public virtual DbSet<OneCikanUrunler> OneCikanUrunler { get; set; }
        public virtual DbSet<OneCikanUrunlerTranslate> OneCikanUrunlerTranslate { get; set; }
        public virtual DbSet<OneCikanUrunResimleri> OneCikanUrunResimleri { get; set; }

        public virtual DbSet<DosyaKategorileri> DosyaKategorileri { get; set; }
        public virtual DbSet<DosyaKategorileriTranslate> DosyaKategorileriTranslate { get; set; }
        public virtual DbSet<Dosyalar> Dosyalar { get; set; }
        public virtual DbSet<DosyalarTranslate> DosyalarTranslate { get; set; }
        public virtual DbSet<DosyaGaleri> DosyaGaleri { get; set; }
        public virtual DbSet<Sayfalar> Sayfalar { get; set; }
        public virtual DbSet<SayfalarTranslate> SayfalarTranslate { get; set; }
        public virtual DbSet<SayfaOzellikGruplari> SayfaOzellikGruplari { get; set; }
        public virtual DbSet<SayfaOzellikGruplariTranslate> SayfaOzellikGruplariTranslate { get; set; }
        public virtual DbSet<SayfaToOzellik> SayfaToOzellik { get; set; }
        public virtual DbSet<SayfaOzellikleri> SayfaOzellikleri { get; set; }
        public virtual DbSet<SayfaOzellikleriTranslate> SayfaOzellikleriTranslate { get; set; }
        public virtual DbSet<SayfaResimleri> SayfaResimleri { get; set; }
        public virtual DbSet<SayfaToSayfalar> SayfaToSayfalar { get; set; }
        public virtual DbSet<Ulkeler> Ulkeler { get; set; }
        public virtual DbSet<Iller> Iller { get; set; }
        public virtual DbSet<Ilceler> Ilceler { get; set; }
        public virtual DbSet<Yorumlar> Yorumlar { get; set; }
        public virtual DbSet<Takvim> Takvim { get; set; }
        public virtual DbSet<TakvimTranslate> TakvimTranslate { get; set; }
        public virtual DbSet<FotografGalerileri> FotografGalerileri { get; set; }
        public virtual DbSet<FotografGaleriResimleri> FotografGaleriResimleri { get; set; }
        public virtual DbSet<FotografGalerileriTranslate> FotografGalerileriTranslate { get; set; }
        public virtual DbSet<VideoKategorileri> VideoKategorileri { get; set; }
        public virtual DbSet<VideoKategorileriTranslate> VideoKategorileriTranslate { get; set; }
        public virtual DbSet<Videolar> Videolar { get; set; }
        public virtual DbSet<VideolarTranslate> VideolarTranslate { get; set; }
        public virtual DbSet<Slaytlar> Slaytlar { get; set; }
        public virtual DbSet<SlaytlarTranslate> SlaytlarTranslate { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<BannerTranslate> BannerTranslate { get; set; }
        public virtual DbSet<BannerResim> BannerResim { get; set; }
        public virtual DbSet<BannerResimTranslate> BannerResimTranslate { get; set; }
        public virtual DbSet<UrunToSlayt> UrunToSlayt { get; set; }
        public virtual DbSet<Formlar> Formlar { get; set; }
        public virtual DbSet<FormlarTranslate> FormlarTranslate { get; set; }
        public virtual DbSet<FormBasliklari> FormBasliklari { get; set; }
        public virtual DbSet<FormBasliklariTranslate> FormBasliklariTranslate { get; set; }
        public virtual DbSet<FormDegerleri> FormDegerleri { get; set; }
        public virtual DbSet<FormDegerleriTranslate> FormDegerleriTranslate { get; set; }
        public virtual DbSet<FormBasvurulari> FormBasvurulari { get; set; }
        public virtual DbSet<FormCevaplari> FormCevaplari { get; set; }
        public virtual DbSet<Ekipler> Ekipler { get; set; }
        public virtual DbSet<EkiplerTranslate> EkiplerTranslate { get; set; }
        public virtual DbSet<Menuler> Menuler { get; set; }
        public virtual DbSet<MenulerTranslate> MenulerTranslate { get; set; }
        public virtual DbSet<SabitMenuler> SabitMenuler { get; set; }
        public virtual DbSet<SabitMenulerTranslate> SabitMenulerTranslate { get; set; }
        public virtual DbSet<AdresBilgileri> AdresBilgileri { get; set; }
        public virtual DbSet<AdresBilgileriTranslate> AdresBilgileriTranslate { get; set; }
        public virtual DbSet<AdresBilgileriTelefonlar> AdresBilgileriTelefonlar { get; set; }
        public virtual DbSet<AdresBilgileriTelefonlarTranslate> AdresBilgileriTelefonlarTranslate { get; set; }
        public virtual DbSet<SayfaFormu> SayfaFormu { get; set; }
        public virtual DbSet<Moduller> Moduller { get; set; }
        public virtual DbSet<ModullerTranslate> ModullerTranslate { get; set; }
        public virtual DbSet<OneCikanKategoriToKategoriler> OneCikanKategoriToKategoriler { get; set; }
        public virtual DbSet<OneCikanUrunToUrunler> OneCikanUrunToUrunler { get; set; }
        public virtual DbSet<ParaBirimleri> ParaBirimleri { get; set; }
        public virtual DbSet<Kur> Kur { get; set; }
        public virtual DbSet<Paytr> Paytr { get; set; }
        public virtual DbSet<PaytrIframeTransaction> PaytrIframeTransaction { get; set; }
        public virtual DbSet<CariOdeme> CariOdeme { get; set; }
        public virtual DbSet<SiteAyarlari> SiteAyarlari { get; set; }
        public virtual DbSet<SiteAyarlariTranslate> SiteAyarlariTranslate { get; set; }
        public virtual DbSet<SeoUrl> SeoUrl { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }

    }
}