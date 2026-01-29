using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EticaretWebCoreMigrationMysql.Migrations
{
    /// <inheritdoc />
    public partial class Mig01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DilCeviri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Anahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DilCeviri", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DilKodlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DilKodu = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DilKodlari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Dosyalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DosyaTipi = table.Column<int>(type: "int", nullable: false),
                    SayfaTipi = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    SayfaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosyalar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ekipler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdSoyad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebSite = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Facebook = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Instagram = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Twitter = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Linkedin = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pinterest = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GooglePlus = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Youtube = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Whatsapp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ekipler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormBasvurulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BasvuruDurumu = table.Column<int>(type: "int", nullable: false),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    BasvuruTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormBasvurulari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FotografGalerileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GaleriTipi = table.Column<int>(type: "int", nullable: false),
                    GaleriSayfaTipi = table.Column<int>(type: "int", nullable: false),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SilmeYetkisi = table.Column<int>(type: "int", nullable: false),
                    AdminSolMenu = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotografGalerileri", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KargoMetodlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoMetodlari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentKategoriId = table.Column<int>(type: "int", nullable: false),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbResim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Vitrin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kategoriler_Kategoriler_ParentKategoriId",
                        column: x => x.ParentKategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kdv",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KdvAdi = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KdvOrani = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kdv", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kuponlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KuponAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kod = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OranTipi = table.Column<int>(type: "int", nullable: false),
                    Indirim = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ToplamTutar = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kuponlar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Markalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MarkaAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markalar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentMenuId = table.Column<int>(type: "int", nullable: false),
                    SeoUrlTipi = table.Column<int>(type: "int", nullable: false),
                    MenuTipi = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    MenuKolon = table.Column<int>(type: "int", nullable: false),
                    MenuYeri = table.Column<int>(type: "int", nullable: false),
                    SekmeDurumu = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menuler_Menuler_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Moduller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModulTipi = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moduller", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OneCikanKategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    ModulId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanKategoriler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OneCikanUrunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    ModulId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanUrunler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SabitMenuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaTipi = table.Column<int>(type: "int", nullable: false),
                    BreadcrumbResim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SabitMenuler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaFormu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirmaAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ad = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Soyad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Konu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mesaj = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    SayfaFormTipi = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    KonaklamaTipi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GirisTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CikisTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaFormu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaOzellikGruplari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaOzellikGruplari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiparisDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDurumlari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Slaytlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ArkaplanResim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim3 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim4 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim5 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BackgroundColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FontColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slaytlar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Takvim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Renk = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takvim", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ulkeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UlkeAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ulkeler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunOzellikGruplari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunOzellikGruplari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunSecenekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SecenekTipi = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunSecenekleri", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VideoKategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SilmeYetkisi = table.Column<int>(type: "int", nullable: false),
                    AdminSolMenu = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoKategorileri", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BannerResim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BannerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerResim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerResim_Banner_BannerId",
                        column: x => x.BannerId,
                        principalTable: "Banner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Diller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DilAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaDilAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    DilKoduId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diller_DilKodlari_DilKoduId",
                        column: x => x.DilKoduId,
                        principalTable: "DilKodlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ParaBirimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParaBirimAdi = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kodu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilKoduId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParaBirimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParaBirimleri_DilKodlari_DilKoduId",
                        column: x => x.DilKoduId,
                        principalTable: "DilKodlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FotografGaleriResimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Resim = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    FotografGaleriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotografGaleriResimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotografGaleriResimleri_FotografGalerileri_FotografGaleriId",
                        column: x => x.FotografGaleriId,
                        principalTable: "FotografGalerileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UrunKodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IndirimliFiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    StokTipi = table.Column<int>(type: "int", nullable: false),
                    Stok = table.Column<int>(type: "int", nullable: false),
                    BreadcrumbResim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Vitrin = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    KdvId = table.Column<int>(type: "int", nullable: true),
                    MarkaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Urunler_Kdv_KdvId",
                        column: x => x.KdvId,
                        principalTable: "Kdv",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Urunler_Markalar_MarkaId",
                        column: x => x.MarkaId,
                        principalTable: "Markalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OneCikanKategoriToKategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OneCikanKategoriId = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanKategoriToKategoriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCikanKategoriToKategoriler_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCikanKategoriToKategoriler_OneCikanKategoriler_OneCikanKa~",
                        column: x => x.OneCikanKategoriId,
                        principalTable: "OneCikanKategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaOzellikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaOzellikGrupId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaOzellikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaOzellikleri_SayfaOzellikGruplari_SayfaOzellikGrupId",
                        column: x => x.SayfaOzellikGrupId,
                        principalTable: "SayfaOzellikGruplari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OdemeMetodlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiparisDurumId = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeMetodlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdemeMetodlari_SiparisDurumlari_SiparisDurumId",
                        column: x => x.SiparisDurumId,
                        principalTable: "SiparisDurumlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Iller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UlkeId = table.Column<int>(type: "int", nullable: false),
                    IlAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Plaka = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Iller_Ulkeler_UlkeId",
                        column: x => x.UlkeId,
                        principalTable: "Ulkeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunOzellikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunOzellikGrupId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunOzellikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunOzellikleri_UrunOzellikGruplari_UrunOzellikGrupId",
                        column: x => x.UrunOzellikGrupId,
                        principalTable: "UrunOzellikGruplari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunSecenekDegerleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunSecenekId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunSecenekDegerleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunSecenekDegerleri_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Videolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VideoKategoriId = table.Column<int>(type: "int", nullable: false),
                    VideoTipi = table.Column<int>(type: "int", nullable: false),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SilmeYetkisi = table.Column<int>(type: "int", nullable: false),
                    AdminSolMenu = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videolar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videolar_VideoKategorileri_VideoKategoriId",
                        column: x => x.VideoKategoriId,
                        principalTable: "VideoKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BannerResimTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BannerAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    BannerResimId = table.Column<int>(type: "int", nullable: false),
                    SeoUrlTipi = table.Column<int>(type: "int", nullable: false),
                    UrlTipi = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerResimTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerResimTranslate_BannerResim_BannerResimId",
                        column: x => x.BannerResimId,
                        principalTable: "BannerResim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerResimTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BannerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BannerAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    BannerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerTranslate_Banner_BannerId",
                        column: x => x.BannerId,
                        principalTable: "Banner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DilCeviriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Deger = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    DilCeviriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DilCeviriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DilCeviriTranslate_DilCeviri_DilCeviriId",
                        column: x => x.DilCeviriId,
                        principalTable: "DilCeviri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DilCeviriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DosyalarTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DosyaAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosya = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    DosyaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyalarTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyalarTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DosyalarTranslate_Dosyalar_DosyaId",
                        column: x => x.DosyaId,
                        principalTable: "Dosyalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EkiplerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SabitMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EkiplerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EkiplerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EkiplerTranslate_Ekipler_SabitMenuId",
                        column: x => x.SabitMenuId,
                        principalTable: "Ekipler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FotografGalerileriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GaleriAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    FotografGaleriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotografGalerileriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotografGalerileriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FotografGalerileriTranslate_FotografGalerileri_FotografGaler~",
                        column: x => x.FotografGaleriId,
                        principalTable: "FotografGalerileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KargoMetodlariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KargoAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    KargoMetodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoMetodlariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KargoMetodlariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KargoMetodlariTranslate_KargoMetodlari_KargoMetodId",
                        column: x => x.KargoMetodId,
                        principalTable: "KargoMetodlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KategoriBanner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategoriBanner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KategoriBanner_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KategoriBanner_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KategorilerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KategoriAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UstAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SolAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorilerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KategorilerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KategorilerTranslate_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MenulerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenulerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenulerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenulerTranslate_Menuler_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModullerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModulAdi = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    ModulId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModullerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModullerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModullerTranslate_Moduller_ModulId",
                        column: x => x.ModulId,
                        principalTable: "Moduller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OneCikanKategorilerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModulAdi = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    OneCikanKategoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanKategorilerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCikanKategorilerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCikanKategorilerTranslate_OneCikanKategoriler_OneCikanKat~",
                        column: x => x.OneCikanKategoriId,
                        principalTable: "OneCikanKategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OneCikanUrunlerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModulAdi = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    OneCikanUrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanUrunlerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunlerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunlerTranslate_OneCikanUrunler_OneCikanUrunId",
                        column: x => x.OneCikanUrunId,
                        principalTable: "OneCikanUrunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SabitMenulerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SabitMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SabitMenulerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SabitMenulerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SabitMenulerTranslate_SabitMenuler_SabitMenuId",
                        column: x => x.SabitMenuId,
                        principalTable: "SabitMenuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaOzellikGruplariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GrupAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SayfaOzellikGrupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaOzellikGruplariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaOzellikGruplariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SayfaOzellikGruplariTranslate_SayfaOzellikGruplari_SayfaOzel~",
                        column: x => x.SayfaOzellikGrupId,
                        principalTable: "SayfaOzellikGruplari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SeoUrl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DilId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityName = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    SeoTipi = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeoUrl_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiparisDurumlariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiparisDurumu = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SiparisDurumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDurumlariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisDurumlariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiparisDurumlariTranslate_SiparisDurumlari_SiparisDurumId",
                        column: x => x.SiparisDurumId,
                        principalTable: "SiparisDurumlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiteAyarlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AktifDilId = table.Column<int>(type: "int", nullable: false),
                    FirmaAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Facebook = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Instagram = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Twitter = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Linkedin = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pinterest = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GooglePlus = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Youtube = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Whatsapp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeaderKod = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterKod = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UstLogo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterLogo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MobilLogo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Favicon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailLogo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PopupDurum = table.Column<int>(type: "int", nullable: false),
                    EmailHost = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailAdresi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailSifre = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailPort = table.Column<int>(type: "int", nullable: false),
                    EmailSSL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    GonderilecekMail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailTipi = table.Column<int>(type: "int", nullable: false),
                    MailBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailKonu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MailGonderildiMesaji = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExchangeVersiyon = table.Column<int>(type: "int", nullable: false),
                    SinirsiKategoriDurum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAyarlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteAyarlari_Diller_AktifDilId",
                        column: x => x.AktifDilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SlaytlarTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlaytBaslik = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SlaytBaslik2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SlaytBaslik3 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SlaytBaslik4 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SlaytBaslik5 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ButonAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ButonAdi2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Video = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YoutubeVideo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SlaytId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlaytlarTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SlaytlarTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SlaytlarTranslate_Slaytlar_SlaytId",
                        column: x => x.SlaytId,
                        principalTable: "Slaytlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TakvimTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Baslik = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    TakvimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakvimTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakvimTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TakvimTranslate_Takvim_TakvimId",
                        column: x => x.TakvimId,
                        principalTable: "Takvim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunOzellikGruplariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GrupAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    UrunOzellikGrupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunOzellikGruplariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunOzellikGruplariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunOzellikGruplariTranslate_UrunOzellikGruplari_UrunOzellik~",
                        column: x => x.UrunOzellikGrupId,
                        principalTable: "UrunOzellikGruplari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunSecenekleriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SecenekAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunSecenekleriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunSecenekleriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunSecenekleriTranslate_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VideoKategorileriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KategoriAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    VideoKategoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoKategorileriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoKategorileriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoKategorileriTranslate_VideoKategorileri_VideoKategoriId",
                        column: x => x.VideoKategoriId,
                        principalTable: "VideoKategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TLKur = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ParaBirimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kur_ParaBirimleri_ParaBirimId",
                        column: x => x.ParaBirimId,
                        principalTable: "ParaBirimleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KuponToUrun",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KuponId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KuponToUrun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KuponToUrun_Kuponlar_KuponId",
                        column: x => x.KuponId,
                        principalTable: "Kuponlar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KuponToUrun_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OneCikanUrunToUrunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OneCikanUrunId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCikanUrunToUrunler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunToUrunler_OneCikanUrunler_OneCikanUrunId",
                        column: x => x.OneCikanUrunId,
                        principalTable: "OneCikanUrunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OneCikanUrunToUrunler_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunlerTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunAdi = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ozellik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YoutubeResim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Video = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosya = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosya2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunlerTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunlerTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunlerTranslate_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunResimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResimAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    UrunResimKategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunResimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunResimleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToBenzerUrun",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    BenzerUrunId = table.Column<int>(type: "int", nullable: false),
                    BenzerUrunId1 = table.Column<int>(type: "int", nullable: true),
                    KategorilerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToBenzerUrun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToBenzerUrun_Kategoriler_KategorilerId",
                        column: x => x.KategorilerId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UrunToBenzerUrun_Urunler_BenzerUrunId",
                        column: x => x.BenzerUrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UrunToBenzerUrun_Urunler_BenzerUrunId1",
                        column: x => x.BenzerUrunId1,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToKategori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToKategori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToKategori_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UrunToKategori_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToSlayt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SlaytId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToSlayt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToSlayt_Slaytlar_SlaytId",
                        column: x => x.SlaytId,
                        principalTable: "Slaytlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UrunToSlayt_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToTamamlayiciUrun",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    TamamlayiciUrunId = table.Column<int>(type: "int", nullable: false),
                    TamamlayiciUrunId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToTamamlayiciUrun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToTamamlayiciUrun_Urunler_TamamlayiciUrunId",
                        column: x => x.TamamlayiciUrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UrunToTamamlayiciUrun_Urunler_TamamlayiciUrunId1",
                        column: x => x.TamamlayiciUrunId1,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToUrunSecenek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunSecenekId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToUrunSecenek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToUrunSecenek_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UrunToUrunSecenek_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaOzellikleriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OzellikAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SayfaOzellikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaOzellikleriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaOzellikleriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SayfaOzellikleriTranslate_SayfaOzellikleri_SayfaOzellikId",
                        column: x => x.SayfaOzellikId,
                        principalTable: "SayfaOzellikleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OdemeMetodlariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OdemeAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    OdemeMetodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeMetodlariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdemeMetodlariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdemeMetodlariTranslate_OdemeMetodlari_OdemeMetodId",
                        column: x => x.OdemeMetodId,
                        principalTable: "OdemeMetodlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Paytr",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MagazaNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MagazaParola = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MagazaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TestModu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaksitSayisi = table.Column<int>(type: "int", nullable: false),
                    MaksimumTaksitSayisi = table.Column<int>(type: "int", nullable: false),
                    BasariliSiparisDurumId = table.Column<int>(type: "int", nullable: false),
                    HataliSiparisDurumId = table.Column<int>(type: "int", nullable: false),
                    DilId = table.Column<int>(type: "int", nullable: true),
                    ParaBirimId = table.Column<int>(type: "int", nullable: true),
                    OdemeMetodlariId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paytr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paytr_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Paytr_OdemeMetodlari_OdemeMetodlariId",
                        column: x => x.OdemeMetodlariId,
                        principalTable: "OdemeMetodlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Paytr_ParaBirimleri_ParaBirimId",
                        column: x => x.ParaBirimId,
                        principalTable: "ParaBirimleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Paytr_SiparisDurumlari_BasariliSiparisDurumId",
                        column: x => x.BasariliSiparisDurumId,
                        principalTable: "SiparisDurumlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Paytr_SiparisDurumlari_HataliSiparisDurumId",
                        column: x => x.HataliSiparisDurumId,
                        principalTable: "SiparisDurumlari",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bayiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IlId = table.Column<int>(type: "int", nullable: false),
                    BayiAdi = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Harita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bayiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bayiler_Iller_IlId",
                        column: x => x.IlId,
                        principalTable: "Iller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fiyatlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IlId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fiyatlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fiyatlar_Iller_IlId",
                        column: x => x.IlId,
                        principalTable: "Iller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IlId = table.Column<int>(type: "int", nullable: false),
                    IlceAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ilceler_Iller_IlId",
                        column: x => x.IlId,
                        principalTable: "Iller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunOzellikleriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OzellikAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    UrunOzellikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunOzellikleriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunOzellikleriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunOzellikleriTranslate_UrunOzellikleri_UrunOzellikId",
                        column: x => x.UrunOzellikId,
                        principalTable: "UrunOzellikleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToOzellik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UrunOzellikId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToOzellik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToOzellik_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunToOzellik_UrunOzellikleri_UrunOzellikId",
                        column: x => x.UrunOzellikId,
                        principalTable: "UrunOzellikleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UrunToOzellik_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunSecenekDegerleriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DegerAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    UrunSecenekDegerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunSecenekDegerleriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunSecenekDegerleriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunSecenekDegerleriTranslate_UrunSecenekDegerleri_UrunSecen~",
                        column: x => x.UrunSecenekDegerId,
                        principalTable: "UrunSecenekDegerleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VideolarTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VideoAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VideoLinki = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideolarTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideolarTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideolarTranslate_Videolar_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdresBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiteAyarId = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdresBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdresBilgileri_SiteAyarlari_SiteAyarId",
                        column: x => x.SiteAyarId,
                        principalTable: "SiteAyarlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiteAyarlariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MetaBaslik = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeaderAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Popup = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SiteAyarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAyarlariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteAyarlariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteAyarlariTranslate_SiteAyarlari_SiteAyarId",
                        column: x => x.SiteAyarId,
                        principalTable: "SiteAyarlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UrunToUrunSecenekToUrunDeger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Adet = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UrunToUrunSecenekId = table.Column<int>(type: "int", nullable: false),
                    UrunSecenekId = table.Column<int>(type: "int", nullable: false),
                    UrunSecenekDegerId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrunToUrunSecenekToUrunDeger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrunToUrunSecenekToUrunDeger_UrunSecenekDegerleri_UrunSecene~",
                        column: x => x.UrunSecenekDegerId,
                        principalTable: "UrunSecenekDegerleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunToUrunSecenekToUrunDeger_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UrunToUrunSecenekToUrunDeger_UrunToUrunSecenek_UrunToUrunSec~",
                        column: x => x.UrunToUrunSecenekId,
                        principalTable: "UrunToUrunSecenek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UrunToUrunSecenekToUrunDeger_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Soyad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirmaAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilResmi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiDairesi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiNumarasi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gsm = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiLevhasi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Uyetipi = table.Column<int>(type: "int", nullable: false),
                    IlceId = table.Column<int>(type: "int", nullable: true),
                    IpAdres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UyeDurumu = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Ilceler_IlceId",
                        column: x => x.IlceId,
                        principalTable: "Ilceler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdresBilgileriTelefonlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdresBilgiId = table.Column<int>(type: "int", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdresBilgileriTelefonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdresBilgileriTelefonlar_AdresBilgileri_AdresBilgiId",
                        column: x => x.AdresBilgiId,
                        principalTable: "AdresBilgileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdresBilgileriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdresBasligi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Faks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gsm = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Harita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HaritaLink = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalismaSaatlari = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    AdresBilgiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdresBilgileriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdresBilgileriTranslate_AdresBilgileri_AdresBilgiId",
                        column: x => x.AdresBilgiId,
                        principalTable: "AdresBilgileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdresBilgileriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Adresler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    IlceId = table.Column<int>(type: "int", nullable: false),
                    AdresAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Soyad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaAdres = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostaKodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gsm = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirmaAdi = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiDairesi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiNumarasi = table.Column<long>(type: "bigint", nullable: true),
                    VergiLevhasi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaTuru = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adresler_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Adresler_Ilceler_IlceId",
                        column: x => x.IlceId,
                        principalTable: "Ilceler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlisverisListem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlisverisListem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlisverisListem_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlisverisListem_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Begeniler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    UyelerId = table.Column<int>(type: "int", nullable: true),
                    UrunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Begeniler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Begeniler_AspNetUsers_UyelerId",
                        column: x => x.UyelerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Begeniler_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Mesaj = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogTipi = table.Column<int>(type: "int", nullable: false),
                    UyeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MesajKonulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    GonderilenUyeId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: false),
                    Konu = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MesajKonulari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MesajKonulari_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MesajKonulari_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentSayfaId = table.Column<int>(type: "int", nullable: false),
                    SayfaTipi = table.Column<int>(type: "int", nullable: false),
                    SSS = table.Column<int>(type: "int", nullable: false),
                    EntityName = table.Column<int>(type: "int", nullable: false),
                    Hit = table.Column<int>(type: "int", nullable: false),
                    BreadcrumbResim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SinirsizAltSayfaDurumu = table.Column<int>(type: "int", nullable: false),
                    UyeId = table.Column<int>(type: "int", nullable: true),
                    SilmeYetkisi = table.Column<int>(type: "int", nullable: false),
                    AdminSolMenu = table.Column<int>(type: "int", nullable: false),
                    KisayolMenuAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ikon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ikon2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Vitrin = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    KategorilerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayfalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sayfalar_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sayfalar_Kategoriler_KategorilerId",
                        column: x => x.KategorilerId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sayfalar_Sayfalar_ParentSayfaId",
                        column: x => x.ParentSayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sepet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: true),
                    CookieId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrunId = table.Column<int>(type: "int", nullable: true),
                    UrunSecenek = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adet = table.Column<int>(type: "int", nullable: false),
                    KuponKodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EklenmeTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sepet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sepet_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sepet_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdresBilgileriTelefonlarTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Telefon = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    AdresBilgileriTelefonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdresBilgileriTelefonlarTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdresBilgileriTelefonlarTranslate_AdresBilgileriTelefonlar_A~",
                        column: x => x.AdresBilgileriTelefonId,
                        principalTable: "AdresBilgileriTelefonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdresBilgileriTelefonlarTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Siparisler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: true),
                    AdresId = table.Column<int>(type: "int", nullable: true),
                    SiparisTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SiparisGuncellemeTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SiparisNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaAd = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaSoyad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaFirmaAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaAdres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaUlke = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaIl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaturaIlce = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatAd = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatSoyad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatFirmaAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatAdres = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatUlke = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatIl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeslimatIlce = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KargoMetodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OdemeMetodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiDairesi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VergiNumarasi = table.Column<long>(type: "bigint", nullable: true),
                    ParaBirimiKodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiparisNotu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ip = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KargoUcreti = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ToplamFiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SiparisDurumu = table.Column<int>(type: "int", nullable: false),
                    KargoKodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KargoUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siparisler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Siparisler_Adresler_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adresler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Siparisler_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MesajKonuId = table.Column<int>(type: "int", nullable: false),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    GonderilenUyeId = table.Column<int>(type: "int", nullable: false),
                    Mesaj = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MesajTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OkunmaDurumu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesajlar_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesajlar_MesajKonulari_MesajKonuId",
                        column: x => x.MesajKonuId,
                        principalTable: "MesajKonulari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormBasliklari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormBasliklari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormBasliklari_Sayfalar_SayfaId",
                        column: x => x.SayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfalarTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaAdi = table.Column<string>(type: "varchar(750)", maxLength: 750, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SayfaAdiAltAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KisaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YoutubeVideoLink = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BreadcrumbAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaBaslik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MetaAnahtar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ButonAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ButonUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosya = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    SayfaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfalarTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfalarTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SayfalarTranslate_Sayfalar_SayfaId",
                        column: x => x.SayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaResimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Resim = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    DilId = table.Column<int>(type: "int", nullable: true),
                    SayfaResimKategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaResimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaResimleri_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SayfaResimleri_Sayfalar_SayfaId",
                        column: x => x.SayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaToOzellik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaOzellikId = table.Column<int>(type: "int", nullable: false),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaToOzellik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaToOzellik_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SayfaToOzellik_SayfaOzellikleri_SayfaOzellikId",
                        column: x => x.SayfaOzellikId,
                        principalTable: "SayfaOzellikleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SayfaToOzellik_Sayfalar_SayfaId",
                        column: x => x.SayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SayfaToSayfalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    SayfalarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayfaToSayfalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayfaToSayfalar_Sayfalar_SayfaId",
                        column: x => x.SayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SayfaToSayfalar_Sayfalar_SayfalarId",
                        column: x => x.SayfalarId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Yorumlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UyeId = table.Column<int>(type: "int", nullable: false),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: true),
                    AdSoyad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sehir = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Resim = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Yorum = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    YorumTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    YorumDurumu = table.Column<int>(type: "int", nullable: false),
                    Yildiz = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorumlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yorumlar_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Sayfalar_SayfaId",
                        column: x => x.SayfaId,
                        principalTable: "Sayfalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KuponToSiparis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KuponId = table.Column<int>(type: "int", nullable: false),
                    SiparisId = table.Column<int>(type: "int", nullable: false),
                    UyeId = table.Column<int>(type: "int", nullable: true),
                    IndirimTutari = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KuponToSiparis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KuponToSiparis_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KuponToSiparis_Kuponlar_KuponId",
                        column: x => x.KuponId,
                        principalTable: "Kuponlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KuponToSiparis_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaytrIframeTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MerchantOid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiparisId = table.Column<int>(type: "int", nullable: false),
                    ToplamFiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OdenenTutar = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IadeEdilenTutar = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IadeEdilenTarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IadeDurumu = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IadeEdilenDurum = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EklemeTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaytrIframeTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaytrIframeTransaction_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiparisGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiparisId = table.Column<int>(type: "int", nullable: false),
                    SiparisDurumId = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EklenmeTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisGecmisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisGecmisleri_SiparisDurumlari_SiparisDurumId",
                        column: x => x.SiparisDurumId,
                        principalTable: "SiparisDurumlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiparisGecmisleri_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiparisUrunleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiparisId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: true),
                    UrunAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marka = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UrunKodu = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Not = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adet = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Kdv = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Toplam = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisUrunleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisUrunleri_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiparisUrunleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiparisUrunSecenekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiparisId = table.Column<int>(type: "int", nullable: false),
                    UrunId = table.Column<int>(type: "int", nullable: true),
                    UrunSecenekId = table.Column<int>(type: "int", nullable: true),
                    UrunSecenekDegerId = table.Column<int>(type: "int", nullable: true),
                    SecenekAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecenekDegeri = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecenekTipi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisUrunSecenekleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisUrunSecenekleri_Siparisler_SiparisId",
                        column: x => x.SiparisId,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiparisUrunSecenekleri_UrunSecenekDegerleri_UrunSecenekDeger~",
                        column: x => x.UrunSecenekDegerId,
                        principalTable: "UrunSecenekDegerleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiparisUrunSecenekleri_UrunSecenekleri_UrunSecenekId",
                        column: x => x.UrunSecenekId,
                        principalTable: "UrunSecenekleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SiparisUrunSecenekleri_Urunler_UrunId",
                        column: x => x.UrunId,
                        principalTable: "Urunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormBasliklariTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormBasligi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    FormBaslikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormBasliklariTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormBasliklariTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormBasliklariTranslate_FormBasliklari_FormBaslikId",
                        column: x => x.FormBaslikId,
                        principalTable: "FormBasliklari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Formlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormBaslikId = table.Column<int>(type: "int", nullable: false),
                    FormTuru = table.Column<int>(type: "int", nullable: false),
                    Zorunlumu = table.Column<int>(type: "int", nullable: false),
                    Genislik = table.Column<int>(type: "int", nullable: false),
                    TexboxTipi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formlar_FormBasliklari_FormBaslikId",
                        column: x => x.FormBaslikId,
                        principalTable: "FormBasliklari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormCevaplari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SayfaId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    FormBasvuruId = table.Column<int>(type: "int", nullable: false),
                    Cevap = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormCevaplari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormCevaplari_FormBasvurulari_FormBasvuruId",
                        column: x => x.FormBasvuruId,
                        principalTable: "FormBasvurulari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormCevaplari_Formlar_FormId",
                        column: x => x.FormId,
                        principalTable: "Formlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormDegerleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDegerleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormDegerleri_Formlar_FormId",
                        column: x => x.FormId,
                        principalTable: "Formlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormlarTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaceHolder = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HataMesaji = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormlarTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormlarTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormlarTranslate_Formlar_FormId",
                        column: x => x.FormId,
                        principalTable: "Formlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormDegerleriTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DegerAdi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DilId = table.Column<int>(type: "int", nullable: false),
                    FormDegerId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDegerleriTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormDegerleriTranslate_Diller_DilId",
                        column: x => x.DilId,
                        principalTable: "Diller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormDegerleriTranslate_FormDegerleri_FormDegerId",
                        column: x => x.FormDegerId,
                        principalTable: "FormDegerleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormDegerleriTranslate_Formlar_FormId",
                        column: x => x.FormId,
                        principalTable: "Formlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "83a1e43c-5174-4054-8217-3fd0f1e74a76", "Administrator", "ADMINISTRATOR" },
                    { 2, "276ee438-6aef-45c9-9b17-91a808e40bd2", "Yonetici", "YONETICI" },
                    { 3, "e5dd047b-f71b-4816-b0fa-cf25de6478a2", "Uye", "UYE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Ad", "Adres", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirmaAdi", "Gsm", "IlceId", "IpAdres", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "ParentId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilResmi", "SecurityStamp", "Soyad", "Tarih", "TwoFactorEnabled", "UserName", "UyeDurumu", "Uyetipi", "VergiDairesi", "VergiLevhasi", "VergiNumarasi" },
                values: new object[] { 1, 0, "Eyyup", null, "ba203703-f623-47bf-9f37-fad0ce83f255", "info@areltasarim.com", true, null, null, null, null, false, null, "INFO@ARELTASARIM.COM", "ARELTASARIM", null, "AQAAAAIAAYagAAAAEIYQpJ145yYlcnKHDbRAOb5Y3znNNFRSNFq/JwaL8w1F6wpJZC0l2obHt4MqpNkDbQ==", "11111", false, null, "b7c7cfe3-1ad1-47c0-879c-39afffe0d4da", "Balta", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "areltasarim", 0, 0, null, null, null });

            migrationBuilder.InsertData(
                table: "DilKodlari",
                columns: new[] { "Id", "DilKodu" },
                values: new object[,]
                {
                    { 1, "tr-TR" },
                    { 2, "en-US" },
                    { 3, "de-DE" }
                });

            migrationBuilder.InsertData(
                table: "KargoMetodlari",
                columns: new[] { "Id", "Durum", "Fiyat", "Sira" },
                values: new object[,]
                {
                    { 1, 1, 0m, 1 },
                    { 2, 1, 0m, 2 }
                });

            migrationBuilder.InsertData(
                table: "Kategoriler",
                columns: new[] { "Id", "BreadcrumbResim", "Durum", "ParentKategoriId", "Resim", "Sira", "Vitrin" },
                values: new object[] { 1, null, 1, 1, null, null, 0 });

            migrationBuilder.InsertData(
                table: "Menuler",
                columns: new[] { "Id", "Durum", "EntityId", "MenuKolon", "MenuTipi", "MenuYeri", "ParentMenuId", "SekmeDurumu", "SeoUrlTipi", "Sira" },
                values: new object[] { 1, 1, null, 0, 0, 0, 1, 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "SabitMenuler",
                columns: new[] { "Id", "BreadcrumbResim", "Durum", "SayfaTipi", "Sira" },
                values: new object[,]
                {
                    { 1, null, 1, 75, 0 },
                    { 2, null, 1, 140, 0 },
                    { 3, null, 1, 130, 0 },
                    { 4, null, 1, 200, 0 }
                });

            migrationBuilder.InsertData(
                table: "Sayfalar",
                columns: new[] { "Id", "AdminSolMenu", "BreadcrumbResim", "Durum", "EntityName", "Hit", "Ikon", "Ikon2", "KategorilerId", "KisayolMenuAdi", "ParentSayfaId", "SSS", "SayfaTipi", "SilmeYetkisi", "SinirsizAltSayfaDurumu", "Sira", "Tarih", "UyeId", "Vitrin" },
                values: new object[] { 1, 0, null, 1, 0, 0, null, null, null, null, 1, 0, 0, 0, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0 });

            migrationBuilder.InsertData(
                table: "SiparisDurumlari",
                columns: new[] { "Id", "Durum", "Sira" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 1 },
                    { 3, 1, 2 },
                    { 4, 1, 3 },
                    { 5, 1, 4 },
                    { 6, 1, 5 },
                    { 7, 1, 6 },
                    { 8, 1, 7 },
                    { 9, 1, 8 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Diller",
                columns: new[] { "Id", "DilAdi", "DilKoduId", "Durum", "KisaDilAdi", "Resim", "Sira" },
                values: new object[] { 1, "Türkçe", 1, 0, "TR", null, 0 });

            migrationBuilder.InsertData(
                table: "OdemeMetodlari",
                columns: new[] { "Id", "Durum", "SiparisDurumId", "Sira" },
                values: new object[,]
                {
                    { 1, 1, 2, 1 },
                    { 2, 1, 2, 2 },
                    { 3, 1, 2, 3 },
                    { 4, 1, 4, 4 },
                    { 5, 1, 4, 5 }
                });

            migrationBuilder.InsertData(
                table: "ParaBirimleri",
                columns: new[] { "Id", "DilKoduId", "Durum", "Kodu", "ParaBirimAdi", "Sira" },
                values: new object[,]
                {
                    { 1, 1, 0, "TRY", "Türk Lirası", null },
                    { 2, 2, 0, "USD", "Dolar", null },
                    { 3, 3, 0, "EUR", "Euro", null }
                });

            migrationBuilder.InsertData(
                table: "KargoMetodlariTranslate",
                columns: new[] { "Id", "Aciklama", "DilId", "KargoAdi", "KargoMetodId" },
                values: new object[,]
                {
                    { 1, null, 1, "Ücretsiz Kargo", 1 },
                    { 2, null, 1, "Şartlı Ödeme", 2 }
                });

            migrationBuilder.InsertData(
                table: "KategorilerTranslate",
                columns: new[] { "Id", "Aciklama", "AltAciklama", "BreadcrumbAciklama", "BreadcrumbAdi", "DilId", "KategoriAdi", "KategoriId", "KisaAciklama", "MetaAciklama", "MetaAnahtar", "MetaBaslik", "SolAciklama", "UstAciklama" },
                values: new object[] { 1, null, null, null, null, 1, "Ana Kategori", 1, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "MenulerTranslate",
                columns: new[] { "Id", "DilId", "MenuAdi", "MenuId", "Url" },
                values: new object[] { 1, 1, "Ana Menü", 1, null });

            migrationBuilder.InsertData(
                table: "OdemeMetodlariTranslate",
                columns: new[] { "Id", "Aciklama", "DilId", "OdemeAdi", "OdemeMetodId" },
                values: new object[,]
                {
                    { 1, null, 1, "Banka Havalesi", 1 },
                    { 2, null, 1, "Kapıda Ödeme", 2 },
                    { 3, null, 1, "Mağazadan Teslim Al", 3 },
                    { 4, null, 1, "Paytr", 4 },
                    { 5, null, 1, "İyzico", 5 }
                });

            migrationBuilder.InsertData(
                table: "SabitMenulerTranslate",
                columns: new[] { "Id", "Aciklama", "BreadcrumbAciklama", "BreadcrumbAdi", "DilId", "MenuAdi", "MetaAciklama", "MetaAnahtar", "MetaBaslik", "SabitMenuId", "Url" },
                values: new object[,]
                {
                    { 1, null, null, null, 1, "Markalar", null, null, null, 1, "tum-markalar" },
                    { 2, null, null, null, 1, "Kategoriler", null, null, null, 2, "tum-kategoriler" },
                    { 3, null, null, null, 1, "Ürünler", null, null, null, 3, "tum-urunler" },
                    { 4, null, null, null, 1, "Bize Ulaşın", null, null, null, 4, "iletisim" }
                });

            migrationBuilder.InsertData(
                table: "SayfalarTranslate",
                columns: new[] { "Id", "Aciklama", "BreadcrumbAciklama", "BreadcrumbAdi", "ButonAdi", "ButonUrl", "DilId", "Dosya", "KisaAciklama", "MetaAciklama", "MetaAnahtar", "MetaBaslik", "Resim", "Resim2", "SayfaAdi", "SayfaAdiAltAciklama", "SayfaId", "YoutubeVideoLink" },
                values: new object[] { 1, null, null, null, null, null, 1, null, null, null, null, null, null, null, "Ana Kategori", null, 1, null });

            migrationBuilder.InsertData(
                table: "SeoUrl",
                columns: new[] { "Id", "DilId", "EntityId", "EntityName", "SeoTipi", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, 75, 1, "tum-markalar" },
                    { 2, 1, 2, 140, 5, "tum-kategoriler" },
                    { 3, 1, 3, 130, 3, "tum-urunler" },
                    { 4, 1, 4, 200, 9, "iletisim" }
                });

            migrationBuilder.InsertData(
                table: "SiparisDurumlariTranslate",
                columns: new[] { "Id", "DilId", "SiparisDurumId", "SiparisDurumu" },
                values: new object[,]
                {
                    { 1, 1, 1, "Eksik Sipariş" },
                    { 2, 1, 2, "Ödeme Bekleniyor" },
                    { 3, 1, 3, "Siparişiniz Hazırlanıyor" },
                    { 4, 1, 4, "Siparişiniz Onaylandı" },
                    { 5, 1, 5, "İşleme Alındı" },
                    { 6, 1, 6, "Kargoya Verildi" },
                    { 7, 1, 7, "Ödeme Başarısız" },
                    { 8, 1, 8, "İptal Edildi" },
                    { 9, 1, 9, "İade Edildi" }
                });

            migrationBuilder.InsertData(
                table: "SiteAyarlari",
                columns: new[] { "Id", "AktifDilId", "EmailAdresi", "EmailHost", "EmailPort", "EmailSSL", "EmailSifre", "ExchangeVersiyon", "Facebook", "Favicon", "FirmaAdi", "FooterKod", "FooterLogo", "GonderilecekMail", "GooglePlus", "HeaderKod", "Instagram", "Linkedin", "MailBaslik", "MailGonderildiMesaji", "MailKonu", "MailLogo", "MailTipi", "MobilLogo", "Pinterest", "PopupDurum", "SinirsiKategoriDurum", "Twitter", "UstLogo", "Whatsapp", "Youtube" },
                values: new object[] { 1, 1, null, null, 0, false, null, 0, null, null, "Firma Adı Gelecek", null, null, null, null, null, null, null, null, null, null, null, 0, null, null, 0, 0, null, null, null, null });

            migrationBuilder.InsertData(
                table: "AdresBilgileri",
                columns: new[] { "Id", "Durum", "Resim", "Sira", "SiteAyarId" },
                values: new object[] { 1, 0, null, 0, 1 });

            migrationBuilder.InsertData(
                table: "SiteAyarlariTranslate",
                columns: new[] { "Id", "DilId", "FooterAciklama", "HeaderAciklama", "MetaAciklama", "MetaAnahtar", "MetaBaslik", "Popup", "SiteAyarId" },
                values: new object[] { 1, 1, null, null, null, null, null, null, 1 });

            migrationBuilder.InsertData(
                table: "AdresBilgileriTranslate",
                columns: new[] { "Id", "Adres", "AdresBasligi", "AdresBilgiId", "CalismaSaatlari", "DilId", "Email", "Faks", "Gsm", "Harita", "HaritaLink", "Telefon" },
                values: new object[] { 1, null, null, 1, null, 1, null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_AdresBilgileri_SiteAyarId",
                table: "AdresBilgileri",
                column: "SiteAyarId");

            migrationBuilder.CreateIndex(
                name: "IX_AdresBilgileriTelefonlar_AdresBilgiId",
                table: "AdresBilgileriTelefonlar",
                column: "AdresBilgiId");

            migrationBuilder.CreateIndex(
                name: "IX_AdresBilgileriTelefonlarTranslate_AdresBilgileriTelefonId",
                table: "AdresBilgileriTelefonlarTranslate",
                column: "AdresBilgileriTelefonId");

            migrationBuilder.CreateIndex(
                name: "IX_AdresBilgileriTelefonlarTranslate_DilId",
                table: "AdresBilgileriTelefonlarTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_AdresBilgileriTranslate_AdresBilgiId",
                table: "AdresBilgileriTranslate",
                column: "AdresBilgiId");

            migrationBuilder.CreateIndex(
                name: "IX_AdresBilgileriTranslate_DilId",
                table: "AdresBilgileriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Adresler_IlceId",
                table: "Adresler",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_Adresler_UyeId",
                table: "Adresler",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_AlisverisListem_UrunId",
                table: "AlisverisListem",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_AlisverisListem_UyeId",
                table: "AlisverisListem",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IlceId",
                table: "AspNetUsers",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannerResim_BannerId",
                table: "BannerResim",
                column: "BannerId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerResimTranslate_BannerResimId",
                table: "BannerResimTranslate",
                column: "BannerResimId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerResimTranslate_DilId",
                table: "BannerResimTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerTranslate_BannerId",
                table: "BannerTranslate",
                column: "BannerId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerTranslate_DilId",
                table: "BannerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Bayiler_IlId",
                table: "Bayiler",
                column: "IlId");

            migrationBuilder.CreateIndex(
                name: "IX_Begeniler_UrunId",
                table: "Begeniler",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Begeniler_UyelerId",
                table: "Begeniler",
                column: "UyelerId");

            migrationBuilder.CreateIndex(
                name: "IX_DilCeviriTranslate_DilCeviriId",
                table: "DilCeviriTranslate",
                column: "DilCeviriId");

            migrationBuilder.CreateIndex(
                name: "IX_DilCeviriTranslate_DilId",
                table: "DilCeviriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Diller_DilKoduId",
                table: "Diller",
                column: "DilKoduId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyalarTranslate_DilId",
                table: "DosyalarTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyalarTranslate_DosyaId",
                table: "DosyalarTranslate",
                column: "DosyaId");

            migrationBuilder.CreateIndex(
                name: "IX_EkiplerTranslate_DilId",
                table: "EkiplerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_EkiplerTranslate_SabitMenuId",
                table: "EkiplerTranslate",
                column: "SabitMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Fiyatlar_IlId",
                table: "Fiyatlar",
                column: "IlId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBasliklari_SayfaId",
                table: "FormBasliklari",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBasliklariTranslate_DilId",
                table: "FormBasliklariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBasliklariTranslate_FormBaslikId",
                table: "FormBasliklariTranslate",
                column: "FormBaslikId");

            migrationBuilder.CreateIndex(
                name: "IX_FormCevaplari_FormBasvuruId",
                table: "FormCevaplari",
                column: "FormBasvuruId");

            migrationBuilder.CreateIndex(
                name: "IX_FormCevaplari_FormId",
                table: "FormCevaplari",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDegerleri_FormId",
                table: "FormDegerleri",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDegerleriTranslate_DilId",
                table: "FormDegerleriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDegerleriTranslate_FormDegerId",
                table: "FormDegerleriTranslate",
                column: "FormDegerId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDegerleriTranslate_FormId",
                table: "FormDegerleriTranslate",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Formlar_FormBaslikId",
                table: "Formlar",
                column: "FormBaslikId");

            migrationBuilder.CreateIndex(
                name: "IX_FormlarTranslate_DilId",
                table: "FormlarTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_FormlarTranslate_FormId",
                table: "FormlarTranslate",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografGalerileriTranslate_DilId",
                table: "FotografGalerileriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografGalerileriTranslate_FotografGaleriId",
                table: "FotografGalerileriTranslate",
                column: "FotografGaleriId");

            migrationBuilder.CreateIndex(
                name: "IX_FotografGaleriResimleri_FotografGaleriId",
                table: "FotografGaleriResimleri",
                column: "FotografGaleriId");

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_IlId",
                table: "Ilceler",
                column: "IlId");

            migrationBuilder.CreateIndex(
                name: "IX_Iller_UlkeId",
                table: "Iller",
                column: "UlkeId");

            migrationBuilder.CreateIndex(
                name: "IX_KargoMetodlariTranslate_DilId",
                table: "KargoMetodlariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_KargoMetodlariTranslate_KargoMetodId",
                table: "KargoMetodlariTranslate",
                column: "KargoMetodId");

            migrationBuilder.CreateIndex(
                name: "IX_KategoriBanner_DilId",
                table: "KategoriBanner",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_KategoriBanner_KategoriId",
                table: "KategoriBanner",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategoriler_ParentKategoriId",
                table: "Kategoriler",
                column: "ParentKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_KategorilerTranslate_DilId",
                table: "KategorilerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_KategorilerTranslate_KategoriId",
                table: "KategorilerTranslate",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_KuponToSiparis_KuponId",
                table: "KuponToSiparis",
                column: "KuponId");

            migrationBuilder.CreateIndex(
                name: "IX_KuponToSiparis_SiparisId",
                table: "KuponToSiparis",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_KuponToSiparis_UyeId",
                table: "KuponToSiparis",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_KuponToUrun_KuponId",
                table: "KuponToUrun",
                column: "KuponId");

            migrationBuilder.CreateIndex(
                name: "IX_KuponToUrun_UrunId",
                table: "KuponToUrun",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Kur_ParaBirimId",
                table: "Kur",
                column: "ParaBirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UyeId",
                table: "Logs",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_ParentMenuId",
                table: "Menuler",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenulerTranslate_DilId",
                table: "MenulerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_MenulerTranslate_MenuId",
                table: "MenulerTranslate",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MesajKonulari_UrunId",
                table: "MesajKonulari",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_MesajKonulari_UyeId",
                table: "MesajKonulari",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_MesajKonuId",
                table: "Mesajlar",
                column: "MesajKonuId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_UyeId",
                table: "Mesajlar",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_ModullerTranslate_DilId",
                table: "ModullerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_ModullerTranslate_ModulId",
                table: "ModullerTranslate",
                column: "ModulId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeMetodlari_SiparisDurumId",
                table: "OdemeMetodlari",
                column: "SiparisDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeMetodlariTranslate_DilId",
                table: "OdemeMetodlariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_OdemeMetodlariTranslate_OdemeMetodId",
                table: "OdemeMetodlariTranslate",
                column: "OdemeMetodId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanKategorilerTranslate_DilId",
                table: "OneCikanKategorilerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanKategorilerTranslate_OneCikanKategoriId",
                table: "OneCikanKategorilerTranslate",
                column: "OneCikanKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanKategoriToKategoriler_KategoriId",
                table: "OneCikanKategoriToKategoriler",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanKategoriToKategoriler_OneCikanKategoriId",
                table: "OneCikanKategoriToKategoriler",
                column: "OneCikanKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunlerTranslate_DilId",
                table: "OneCikanUrunlerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunlerTranslate_OneCikanUrunId",
                table: "OneCikanUrunlerTranslate",
                column: "OneCikanUrunId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunToUrunler_OneCikanUrunId",
                table: "OneCikanUrunToUrunler",
                column: "OneCikanUrunId");

            migrationBuilder.CreateIndex(
                name: "IX_OneCikanUrunToUrunler_UrunId",
                table: "OneCikanUrunToUrunler",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_ParaBirimleri_DilKoduId",
                table: "ParaBirimleri",
                column: "DilKoduId");

            migrationBuilder.CreateIndex(
                name: "IX_Paytr_BasariliSiparisDurumId",
                table: "Paytr",
                column: "BasariliSiparisDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_Paytr_DilId",
                table: "Paytr",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Paytr_HataliSiparisDurumId",
                table: "Paytr",
                column: "HataliSiparisDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_Paytr_OdemeMetodlariId",
                table: "Paytr",
                column: "OdemeMetodlariId");

            migrationBuilder.CreateIndex(
                name: "IX_Paytr_ParaBirimId",
                table: "Paytr",
                column: "ParaBirimId");

            migrationBuilder.CreateIndex(
                name: "IX_PaytrIframeTransaction_SiparisId",
                table: "PaytrIframeTransaction",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_SabitMenulerTranslate_DilId",
                table: "SabitMenulerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SabitMenulerTranslate_SabitMenuId",
                table: "SabitMenulerTranslate",
                column: "SabitMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Sayfalar_KategorilerId",
                table: "Sayfalar",
                column: "KategorilerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sayfalar_ParentSayfaId",
                table: "Sayfalar",
                column: "ParentSayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sayfalar_UyeId",
                table: "Sayfalar",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfalarTranslate_DilId",
                table: "SayfalarTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfalarTranslate_SayfaId",
                table: "SayfalarTranslate",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaOzellikGruplariTranslate_DilId",
                table: "SayfaOzellikGruplariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaOzellikGruplariTranslate_SayfaOzellikGrupId",
                table: "SayfaOzellikGruplariTranslate",
                column: "SayfaOzellikGrupId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaOzellikleri_SayfaOzellikGrupId",
                table: "SayfaOzellikleri",
                column: "SayfaOzellikGrupId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaOzellikleriTranslate_DilId",
                table: "SayfaOzellikleriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaOzellikleriTranslate_SayfaOzellikId",
                table: "SayfaOzellikleriTranslate",
                column: "SayfaOzellikId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaResimleri_DilId",
                table: "SayfaResimleri",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaResimleri_SayfaId",
                table: "SayfaResimleri",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaToOzellik_DilId",
                table: "SayfaToOzellik",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaToOzellik_SayfaId",
                table: "SayfaToOzellik",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaToOzellik_SayfaOzellikId",
                table: "SayfaToOzellik",
                column: "SayfaOzellikId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaToSayfalar_SayfaId",
                table: "SayfaToSayfalar",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_SayfaToSayfalar_SayfalarId",
                table: "SayfaToSayfalar",
                column: "SayfalarId");

            migrationBuilder.CreateIndex(
                name: "IX_SeoUrl_DilId",
                table: "SeoUrl",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_Sepet_UrunId",
                table: "Sepet",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Sepet_UyeId",
                table: "Sepet",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDurumlariTranslate_DilId",
                table: "SiparisDurumlariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDurumlariTranslate_SiparisDurumId",
                table: "SiparisDurumlariTranslate",
                column: "SiparisDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisGecmisleri_SiparisDurumId",
                table: "SiparisGecmisleri",
                column: "SiparisDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisGecmisleri_SiparisId",
                table: "SiparisGecmisleri",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_AdresId",
                table: "Siparisler",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_UyeId",
                table: "Siparisler",
                column: "UyeId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisUrunleri_SiparisId",
                table: "SiparisUrunleri",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisUrunleri_UrunId",
                table: "SiparisUrunleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisUrunSecenekleri_SiparisId",
                table: "SiparisUrunSecenekleri",
                column: "SiparisId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisUrunSecenekleri_UrunId",
                table: "SiparisUrunSecenekleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisUrunSecenekleri_UrunSecenekDegerId",
                table: "SiparisUrunSecenekleri",
                column: "UrunSecenekDegerId");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisUrunSecenekleri_UrunSecenekId",
                table: "SiparisUrunSecenekleri",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteAyarlari_AktifDilId",
                table: "SiteAyarlari",
                column: "AktifDilId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteAyarlariTranslate_DilId",
                table: "SiteAyarlariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteAyarlariTranslate_SiteAyarId",
                table: "SiteAyarlariTranslate",
                column: "SiteAyarId");

            migrationBuilder.CreateIndex(
                name: "IX_SlaytlarTranslate_DilId",
                table: "SlaytlarTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_SlaytlarTranslate_SlaytId",
                table: "SlaytlarTranslate",
                column: "SlaytId");

            migrationBuilder.CreateIndex(
                name: "IX_TakvimTranslate_DilId",
                table: "TakvimTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_TakvimTranslate_TakvimId",
                table: "TakvimTranslate",
                column: "TakvimId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_KdvId",
                table: "Urunler",
                column: "KdvId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MarkaId",
                table: "Urunler",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunlerTranslate_DilId",
                table: "UrunlerTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunlerTranslate_UrunId",
                table: "UrunlerTranslate",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikGruplariTranslate_DilId",
                table: "UrunOzellikGruplariTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikGruplariTranslate_UrunOzellikGrupId",
                table: "UrunOzellikGruplariTranslate",
                column: "UrunOzellikGrupId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikleri_UrunOzellikGrupId",
                table: "UrunOzellikleri",
                column: "UrunOzellikGrupId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikleriTranslate_DilId",
                table: "UrunOzellikleriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunOzellikleriTranslate_UrunOzellikId",
                table: "UrunOzellikleriTranslate",
                column: "UrunOzellikId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunResimleri_UrunId",
                table: "UrunResimleri",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunSecenekDegerleri_UrunSecenekId",
                table: "UrunSecenekDegerleri",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunSecenekDegerleriTranslate_DilId",
                table: "UrunSecenekDegerleriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunSecenekDegerleriTranslate_UrunSecenekDegerId",
                table: "UrunSecenekDegerleriTranslate",
                column: "UrunSecenekDegerId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunSecenekleriTranslate_DilId",
                table: "UrunSecenekleriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunSecenekleriTranslate_UrunSecenekId",
                table: "UrunSecenekleriTranslate",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToBenzerUrun_BenzerUrunId",
                table: "UrunToBenzerUrun",
                column: "BenzerUrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToBenzerUrun_BenzerUrunId1",
                table: "UrunToBenzerUrun",
                column: "BenzerUrunId1");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToBenzerUrun_KategorilerId",
                table: "UrunToBenzerUrun",
                column: "KategorilerId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToKategori_KategoriId",
                table: "UrunToKategori",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToKategori_UrunId",
                table: "UrunToKategori",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToOzellik_DilId",
                table: "UrunToOzellik",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToOzellik_UrunId",
                table: "UrunToOzellik",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToOzellik_UrunOzellikId",
                table: "UrunToOzellik",
                column: "UrunOzellikId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToSlayt_SlaytId",
                table: "UrunToSlayt",
                column: "SlaytId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToSlayt_UrunId",
                table: "UrunToSlayt",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToTamamlayiciUrun_TamamlayiciUrunId",
                table: "UrunToTamamlayiciUrun",
                column: "TamamlayiciUrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToTamamlayiciUrun_TamamlayiciUrunId1",
                table: "UrunToTamamlayiciUrun",
                column: "TamamlayiciUrunId1");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToUrunSecenek_UrunId",
                table: "UrunToUrunSecenek",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToUrunSecenek_UrunSecenekId",
                table: "UrunToUrunSecenek",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToUrunSecenekToUrunDeger_UrunId",
                table: "UrunToUrunSecenekToUrunDeger",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToUrunSecenekToUrunDeger_UrunSecenekDegerId",
                table: "UrunToUrunSecenekToUrunDeger",
                column: "UrunSecenekDegerId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToUrunSecenekToUrunDeger_UrunSecenekId",
                table: "UrunToUrunSecenekToUrunDeger",
                column: "UrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_UrunToUrunSecenekToUrunDeger_UrunToUrunSecenekId",
                table: "UrunToUrunSecenekToUrunDeger",
                column: "UrunToUrunSecenekId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoKategorileriTranslate_DilId",
                table: "VideoKategorileriTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoKategorileriTranslate_VideoKategoriId",
                table: "VideoKategorileriTranslate",
                column: "VideoKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Videolar_VideoKategoriId",
                table: "Videolar",
                column: "VideoKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_VideolarTranslate_DilId",
                table: "VideolarTranslate",
                column: "DilId");

            migrationBuilder.CreateIndex(
                name: "IX_VideolarTranslate_VideoId",
                table: "VideolarTranslate",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_SayfaId",
                table: "Yorumlar",
                column: "SayfaId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_UrunId",
                table: "Yorumlar",
                column: "UrunId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_UyeId",
                table: "Yorumlar",
                column: "UyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdresBilgileriTelefonlarTranslate");

            migrationBuilder.DropTable(
                name: "AdresBilgileriTranslate");

            migrationBuilder.DropTable(
                name: "AlisverisListem");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BannerResimTranslate");

            migrationBuilder.DropTable(
                name: "BannerTranslate");

            migrationBuilder.DropTable(
                name: "Bayiler");

            migrationBuilder.DropTable(
                name: "Begeniler");

            migrationBuilder.DropTable(
                name: "DilCeviriTranslate");

            migrationBuilder.DropTable(
                name: "DosyalarTranslate");

            migrationBuilder.DropTable(
                name: "EkiplerTranslate");

            migrationBuilder.DropTable(
                name: "Fiyatlar");

            migrationBuilder.DropTable(
                name: "FormBasliklariTranslate");

            migrationBuilder.DropTable(
                name: "FormCevaplari");

            migrationBuilder.DropTable(
                name: "FormDegerleriTranslate");

            migrationBuilder.DropTable(
                name: "FormlarTranslate");

            migrationBuilder.DropTable(
                name: "FotografGalerileriTranslate");

            migrationBuilder.DropTable(
                name: "FotografGaleriResimleri");

            migrationBuilder.DropTable(
                name: "KargoMetodlariTranslate");

            migrationBuilder.DropTable(
                name: "KategoriBanner");

            migrationBuilder.DropTable(
                name: "KategorilerTranslate");

            migrationBuilder.DropTable(
                name: "KuponToSiparis");

            migrationBuilder.DropTable(
                name: "KuponToUrun");

            migrationBuilder.DropTable(
                name: "Kur");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MenulerTranslate");

            migrationBuilder.DropTable(
                name: "Mesajlar");

            migrationBuilder.DropTable(
                name: "ModullerTranslate");

            migrationBuilder.DropTable(
                name: "OdemeMetodlariTranslate");

            migrationBuilder.DropTable(
                name: "OneCikanKategorilerTranslate");

            migrationBuilder.DropTable(
                name: "OneCikanKategoriToKategoriler");

            migrationBuilder.DropTable(
                name: "OneCikanUrunlerTranslate");

            migrationBuilder.DropTable(
                name: "OneCikanUrunToUrunler");

            migrationBuilder.DropTable(
                name: "Paytr");

            migrationBuilder.DropTable(
                name: "PaytrIframeTransaction");

            migrationBuilder.DropTable(
                name: "SabitMenulerTranslate");

            migrationBuilder.DropTable(
                name: "SayfaFormu");

            migrationBuilder.DropTable(
                name: "SayfalarTranslate");

            migrationBuilder.DropTable(
                name: "SayfaOzellikGruplariTranslate");

            migrationBuilder.DropTable(
                name: "SayfaOzellikleriTranslate");

            migrationBuilder.DropTable(
                name: "SayfaResimleri");

            migrationBuilder.DropTable(
                name: "SayfaToOzellik");

            migrationBuilder.DropTable(
                name: "SayfaToSayfalar");

            migrationBuilder.DropTable(
                name: "SeoUrl");

            migrationBuilder.DropTable(
                name: "Sepet");

            migrationBuilder.DropTable(
                name: "SiparisDurumlariTranslate");

            migrationBuilder.DropTable(
                name: "SiparisGecmisleri");

            migrationBuilder.DropTable(
                name: "SiparisUrunleri");

            migrationBuilder.DropTable(
                name: "SiparisUrunSecenekleri");

            migrationBuilder.DropTable(
                name: "SiteAyarlariTranslate");

            migrationBuilder.DropTable(
                name: "SlaytlarTranslate");

            migrationBuilder.DropTable(
                name: "TakvimTranslate");

            migrationBuilder.DropTable(
                name: "UrunlerTranslate");

            migrationBuilder.DropTable(
                name: "UrunOzellikGruplariTranslate");

            migrationBuilder.DropTable(
                name: "UrunOzellikleriTranslate");

            migrationBuilder.DropTable(
                name: "UrunResimleri");

            migrationBuilder.DropTable(
                name: "UrunSecenekDegerleriTranslate");

            migrationBuilder.DropTable(
                name: "UrunSecenekleriTranslate");

            migrationBuilder.DropTable(
                name: "UrunToBenzerUrun");

            migrationBuilder.DropTable(
                name: "UrunToKategori");

            migrationBuilder.DropTable(
                name: "UrunToOzellik");

            migrationBuilder.DropTable(
                name: "UrunToSlayt");

            migrationBuilder.DropTable(
                name: "UrunToTamamlayiciUrun");

            migrationBuilder.DropTable(
                name: "UrunToUrunSecenekToUrunDeger");

            migrationBuilder.DropTable(
                name: "VideoKategorileriTranslate");

            migrationBuilder.DropTable(
                name: "VideolarTranslate");

            migrationBuilder.DropTable(
                name: "Yorumlar");

            migrationBuilder.DropTable(
                name: "AdresBilgileriTelefonlar");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "BannerResim");

            migrationBuilder.DropTable(
                name: "DilCeviri");

            migrationBuilder.DropTable(
                name: "Dosyalar");

            migrationBuilder.DropTable(
                name: "Ekipler");

            migrationBuilder.DropTable(
                name: "FormBasvurulari");

            migrationBuilder.DropTable(
                name: "FormDegerleri");

            migrationBuilder.DropTable(
                name: "FotografGalerileri");

            migrationBuilder.DropTable(
                name: "KargoMetodlari");

            migrationBuilder.DropTable(
                name: "Kuponlar");

            migrationBuilder.DropTable(
                name: "Menuler");

            migrationBuilder.DropTable(
                name: "MesajKonulari");

            migrationBuilder.DropTable(
                name: "Moduller");

            migrationBuilder.DropTable(
                name: "OneCikanKategoriler");

            migrationBuilder.DropTable(
                name: "OneCikanUrunler");

            migrationBuilder.DropTable(
                name: "OdemeMetodlari");

            migrationBuilder.DropTable(
                name: "ParaBirimleri");

            migrationBuilder.DropTable(
                name: "SabitMenuler");

            migrationBuilder.DropTable(
                name: "SayfaOzellikleri");

            migrationBuilder.DropTable(
                name: "Siparisler");

            migrationBuilder.DropTable(
                name: "Takvim");

            migrationBuilder.DropTable(
                name: "UrunOzellikleri");

            migrationBuilder.DropTable(
                name: "Slaytlar");

            migrationBuilder.DropTable(
                name: "UrunSecenekDegerleri");

            migrationBuilder.DropTable(
                name: "UrunToUrunSecenek");

            migrationBuilder.DropTable(
                name: "Videolar");

            migrationBuilder.DropTable(
                name: "AdresBilgileri");

            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "Formlar");

            migrationBuilder.DropTable(
                name: "SiparisDurumlari");

            migrationBuilder.DropTable(
                name: "SayfaOzellikGruplari");

            migrationBuilder.DropTable(
                name: "Adresler");

            migrationBuilder.DropTable(
                name: "UrunOzellikGruplari");

            migrationBuilder.DropTable(
                name: "UrunSecenekleri");

            migrationBuilder.DropTable(
                name: "Urunler");

            migrationBuilder.DropTable(
                name: "VideoKategorileri");

            migrationBuilder.DropTable(
                name: "SiteAyarlari");

            migrationBuilder.DropTable(
                name: "FormBasliklari");

            migrationBuilder.DropTable(
                name: "Kdv");

            migrationBuilder.DropTable(
                name: "Markalar");

            migrationBuilder.DropTable(
                name: "Diller");

            migrationBuilder.DropTable(
                name: "Sayfalar");

            migrationBuilder.DropTable(
                name: "DilKodlari");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropTable(
                name: "Ilceler");

            migrationBuilder.DropTable(
                name: "Iller");

            migrationBuilder.DropTable(
                name: "Ulkeler");
        }
    }
}
