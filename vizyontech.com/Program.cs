using EticaretWebCoreEntity;
using EticaretWebCoreHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Quartz;
using EticaretWebCoreService;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using EticaretWebCoreHelper.CustomTagHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using vizyontech.com.Code;
using FluentValidation.AspNetCore;
using FluentValidation;
using EticaretWebCoreFluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using vizyontech.com;
using EticaretWebCoreCaching;
using GoogleReCaptcha.V3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Processors;
using Quartz.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using EticaretWebCoreService.ZiraatPay;
using EticaretWebCoreService.InstagramService;
using EticaretWebCoreViewModel;
using EticaretWebCoreService.CariOdeme;
using EticaretWebCoreService.OpakOdeme;
var builder = WebApplication.CreateBuilder(args);
var Configuration = GetConfiguration();
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//builder.Services.AddControllersWithViews(opts =>
//{
//    opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
//});



builder.Services.AddDbContext<AppDbContext>(options =>
{
    switch (builder.Configuration.GetValue<string>("Application:DatabaseProvider"))
    {
        case "Mysql":
            options.UseMySql(
            builder.Configuration.GetConnectionString("Mysql"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Mysql")),
            config =>
            {
                config.MigrationsAssembly("EticaretWebCoreMigrationMysql");

            }).UseLazyLoadingProxies();
            break;
        case "SqlServer":
        default:
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("SqlServer"),
                config =>
                {
                    config.MigrationsAssembly("EticaretWebCoreMigrationSqlServer");
                }).UseLazyLoadingProxies();
            break;
    }
},ServiceLifetime.Transient);

builder.Services.AddDbContext<OpakDbContext>(options =>
{
    switch (builder.Configuration.GetValue<string>("Application:OpakProvider"))
    {
        case "Mysql":
            options.UseMySql(
            builder.Configuration.GetConnectionString("Mysql"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Mysql")),
            config =>
            {
                config.MigrationsAssembly("EticaretWebCoreMigrationMysql");

            }).UseLazyLoadingProxies();
            break;
        case "OpakSqlServer":
        default:
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("OpakSqlServer"),
                config =>
                {
                    config.MigrationsAssembly("EticaretWebCoreMigrationSqlServer");
                }).UseLazyLoadingProxies();
            break;
    }
}, ServiceLifetime.Transient);


builder.Services.AddDataProtection();

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{

    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRÞSTUÜVWXYZ0123456789-._";

    opt.Password.RequiredLength = 3;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;

}).AddPasswordValidator<CustomPasswordValidator>().

AddEntityFrameworkStores<AppDbContext>().
AddUserValidator<CustomUserValidator>().
AddErrorDescriber<CustomIdentityErrorDescriber>().
AddDefaultTokenProviders();

builder.Services.AddDistributedRedisCache(options =>
{
    options.InstanceName = "Eticaret";
    options.Configuration = Configuration.GetSection("CahceSettings").GetSection("ConnectionString").Value.ToString();
});



//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
//    options.Secure = CookieSecurePolicy.SameAsRequest;
//    options.OnAppendCookie = cookieContext =>
//        AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
//    options.OnDeleteCookie = cookieContext =>
//        AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
//});

//builder.Services.AddAuthentication(opt => { opt.DefaultScheme = "BayiAuth"; })
//                  .AddCookie("AdminAuth", opt => { opt.CookieManager = new ChunkingCookieManager(); opt.Cookie.SameSite = SameSiteMode.None; opt.Cookie.HttpOnly = true; opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; opt.SlidingExpiration = true; opt.Cookie.MaxAge = opt.ExpireTimeSpan = TimeSpan.FromDays(1); opt.LoginPath = "/Admin/Account/GirisYap"; opt.LogoutPath = "/Admin/Account/CikisYap"; opt.AccessDeniedPath = "/Admin/Account/AccessDenied"; })
//                  .AddCookie("BayiAuth", opt => { opt.CookieManager = new ChunkingCookieManager(); opt.Cookie.SameSite = SameSiteMode.None; opt.Cookie.HttpOnly = true; opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; opt.SlidingExpiration = true; opt.Cookie.MaxAge = opt.ExpireTimeSpan = TimeSpan.FromDays(1); opt.LoginPath = "/Account/GirisYap"; opt.LogoutPath = "/Account/CikisYap"; opt.AccessDeniedPath = "/Account/AccessDenied"; });


builder.Services.AddAuthentication(opt => { opt.DefaultScheme = "BayiAuth"; })
                  .AddCookie("AdminAuth", opt => { opt.Cookie.SameSite = SameSiteMode.Lax; opt.SlidingExpiration = true; opt.Cookie.MaxAge = opt.ExpireTimeSpan = TimeSpan.FromDays(1); opt.LoginPath = "/Admin/Account/GirisYap"; opt.LogoutPath = "/Admin/Account/CikisYap"; opt.AccessDeniedPath = "/Admin/Account/AccessDenied"; })
                  .AddCookie("BayiAuth", opt => { opt.Cookie.SameSite = SameSiteMode.Lax; opt.SlidingExpiration = true; opt.Cookie.MaxAge = opt.ExpireTimeSpan = TimeSpan.FromDays(1); opt.LoginPath = "/girisyap"; opt.LogoutPath = "/cikisyap"; opt.AccessDeniedPath = "/Account/AccessDenied"; });



//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<SayfalarServis>();
builder.Services.AddScoped<SayfaFormuServis>();
builder.Services.AddScoped<SepetServis>();
builder.Services.AddScoped<AlisverisListemServis>();
builder.Services.AddScoped<KasaServis>();
builder.Services.AddScoped<LogsServis>();
builder.Services.AddScoped<MarkalarServis>();
builder.Services.AddScoped<KategorilerServis>();
builder.Services.AddScoped<UrunlerServis>();
builder.Services.AddScoped<UrunOzellikGruplariServis>();
builder.Services.AddScoped<UrunOzellikleriServis>();
builder.Services.AddScoped<OneCikanUrunlerServis>();
builder.Services.AddScoped<KuponServis>();
builder.Services.AddScoped<SlaytlarServis>();
builder.Services.AddScoped<UlkelerServis>();
builder.Services.AddScoped<UrunSecenekleriServis>();
builder.Services.AddScoped<SayfaOzellikGruplariServis>();
builder.Services.AddScoped<SayfaOzellikleriServis>();
builder.Services.AddScoped<UyelerServis>();
builder.Services.AddScoped<AppUser>();
builder.Services.AddScoped<AdresServis>();
builder.Services.AddScoped<XmlProductImportServis>();
builder.Services.AddScoped<SiparislerServis>();
builder.Services.AddScoped<PaytrServis>();
builder.Services.AddScoped<MenulerServis>();
builder.Services.AddScoped<SeoServis>();
builder.Services.AddScoped<ZiraatPayServis>();
builder.Services.AddScoped<HelperServis>();
builder.Services.AddScoped<CariOdemeServis>();
builder.Services.AddScoped<OpakServis>();
builder.Services.AddScoped<InstagramService>();


builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<SignInManager<AppUser>>();
builder.Services.AddScoped<RoleManager<AppRole>>();

builder.Services.AddScoped<AnalyticsService>();

builder.Services.AddSingleton<B2BSifreService>();

//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(MappingProfile));


AppDbContext db = new();

builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
     options =>
     {
         try
         {
             var varsayilanDil = db.SiteAyarlari.ToList()?.FirstOrDefault();

             List<CultureInfo> dilKodlari = db.DilKodlari.Select(x => new CultureInfo(x.DilKodu)).ToList();

             options.DefaultRequestCulture = new RequestCulture(varsayilanDil?.AktifDil.DilKodlari.DilKodu ?? "tr-TR");
             options.SupportedCultures = dilKodlari;
             options.SupportedUICultures = dilKodlari;
             options.RequestCultureProviders = new List<IRequestCultureProvider>

             {
                        new QueryStringRequestCultureProvider(),
                        new CookieRequestCultureProvider()
             };
         }
         catch
         {

         }


     });

builder.Services.AddAuthorization();


builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

builder.Services.AddSingleton<IFileProvider>(physicalProvider);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddCacheServices(Configuration);

builder.Services.AddFluentValidationAutoValidation()
                 .AddFluentValidationClientsideAdapters()
                 .AddValidatorsFromAssemblyContaining(typeof(Program));


builder.Services.AddScoped<IValidator<Markalar>, MarkaValidator>();
builder.Services.AddScoped<IValidator<KategorilerTranslate>, KategoriValidator>();
builder.Services.AddScoped<IValidator<SayfalarTranslate>, SayfaValidator>();
builder.Services.AddScoped<IValidator<UrunlerTranslate>, UrunValidator>();



builder.Services.AddRazorPages().AddRazorPagesOptions(options => { options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()); });


builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

builder.Services.Configure<ZiraatPaySettings>(Configuration.GetSection("ZiraatPay"));


RotativaConfiguration.Setup(builder.Environment.ContentRootPath, "wwwroot/Rotativa");

builder.Services.AddTransient<IProgressReporterFactory, ProgressReporterFactory>();

builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
    o.MaximumReceiveMessageSize = int.MaxValue;
});


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; 
});

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue; 
    options.MultipartHeadersLengthLimit = int.MaxValue;
});


builder.Services.AddImageSharp(options =>
{
    options.Configuration = SixLabors.ImageSharp.Configuration.Default;
    options.BrowserMaxAge = TimeSpan.FromDays(7);
    options.CacheMaxAge = TimeSpan.FromDays(365);
    options.CacheHashLength = 8;
    options.OnParseCommandsAsync = _ => Task.CompletedTask;
    options.OnBeforeSaveAsync = _ => Task.CompletedTask;
    options.OnProcessedAsync = _ => Task.CompletedTask;
    options.OnPrepareResponseAsync = _ => Task.CompletedTask;
})
.SetRequestParser<QueryCollectionRequestParser>()
.Configure<PhysicalFileSystemCacheOptions>(options =>
{
    options.CacheFolder = "cache";
})
.SetCache<PhysicalFileSystemCache>()
.SetCacheKey<UriRelativeLowerInvariantCacheKey>()
.SetCacheHash<SHA256CacheHash>()
.ClearProviders()
.AddProvider<PhysicalFileSystemProvider>()
.ClearProcessors()
.AddProcessor<ResizeWebProcessor>()
.AddProcessor<FormatWebProcessor>() // Format processor eklendi
.AddProcessor<BackgroundColorWebProcessor>()
.AddProcessor<QualityWebProcessor>()
.Configure<FormatWebProcessor>(options =>
{

});


//builder.Services.AddQuartz(q =>
//{
//    // JobIslemKur için Job ve Trigger tanýmlamalarý
//    var jobKeyKur = new JobKey("JobIslemKur");
//    q.AddJob<JobIslemKur>(opts => opts.WithIdentity(jobKeyKur));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKeyKur) // Bu job için trigger
//        .WithIdentity("ImmediateTrigger_Kur") // Hemen tetiklenecek trigger
//        .StartNow());

//    q.AddTrigger(opts => opts
//        .ForJob(jobKeyKur) // Bu job için trigger
//        .WithIdentity("CronTrigger_Kur") // Cron trigger
//        .WithCronSchedule("0 45 16 ? * *") // Her gün saat 16:15
//        .WithDescription("Kur Ýþlemleri Job'u"));

//    // JobIslemUyeler için Job ve Trigger tanýmlamalarý
//    var jobKeyUyeler = new JobKey("JobIslemUyeler");
//    q.AddJob<JobIslemUyeler>(opts => opts.WithIdentity(jobKeyUyeler));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKeyUyeler) // Bu job için trigger
//        .WithIdentity("ImmediateTrigger_Uyeler") // Hemen tetiklenecek trigger
//        .StartNow());

//    q.AddTrigger(opts => opts
//        .ForJob(jobKeyUyeler) // Bu job için trigger
//        .WithIdentity("CronTrigger_Uyeler") // Cron trigger
//        .WithCronSchedule("0 0 0 ? * *") // Her gün saat 00:00
//        .WithDescription("Üye Ýþlemleri Job'u"));



//    // JobIslemUrunler için Job ve Trigger tanýmlamalarý
//    var jobKeyUrunler = new JobKey("JobIslemUrunler");
//    q.AddJob<JobIslemUrunler>(opts => opts.WithIdentity(jobKeyUrunler));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKeyUrunler) // Bu job için trigger
//        .WithIdentity("ImmediateTrigger_Urunler") // Hemen tetiklenecek trigger
//        .StartNow());

//    q.AddTrigger(opts => opts
//        .ForJob(jobKeyUrunler) // Bu job için trigger
//        .WithIdentity("CronTrigger_Urunler") // Cron trigger
//        .WithCronSchedule("0 0 0 ? * *") // Her gün saat 00:00
//        .WithDescription("Üye Ýþlemleri Job'u"));
//});

//builder.Services.AddQuartzServer(options =>
//{
//    // when shutting down we want jobs to complete gracefully
//    options.WaitForJobsToComplete = true;
//});


builder.Services.AddQuartz(q =>
{
    // JobIslemKur için Job ve Trigger tanýmlamalarý
    var jobKeyKur = new JobKey("JobIslemKur");
    q.AddJob<JobIslemKur>(opts => opts.WithIdentity(jobKeyKur));

    q.AddTrigger(opts => opts
        .ForJob(jobKeyKur)
        .WithIdentity("ImmediateTrigger_Kur")
        .StartNow());

    q.AddTrigger(opts => opts
        .ForJob(jobKeyKur)
        .WithIdentity("CronTrigger_Kur")
        .WithCronSchedule("0 45 16 ? * *")
        .WithDescription("Kur Ýþlemleri Job'u"));

    // JobIslemUyeler için Job ve Trigger tanýmlamalarý
    var jobKeyUyeler = new JobKey("JobIslemUyeler");
    q.AddJob<JobIslemUyeler>(opts => opts.WithIdentity(jobKeyUyeler));

    q.AddTrigger(opts => opts
        .ForJob(jobKeyUyeler)
        .WithIdentity("ImmediateTrigger_Uyeler")
        .StartNow());

    q.AddTrigger(opts => opts
        .ForJob(jobKeyUyeler)
        .WithIdentity("CronTrigger_Uyeler")
        .WithCronSchedule("0 0 0 ? * *")
        .WithDescription("Üye Ýþlemleri Job'u"));

    // JobIslemUrunler için Job ve Trigger tanýmlamalarý
    var jobKeyUrunler = new JobKey("JobIslemUrunler");
    q.AddJob<JobIslemUrunler>(opts => opts.WithIdentity(jobKeyUrunler));

    q.AddTrigger(opts => opts
        .ForJob(jobKeyUrunler)
        .WithIdentity("ImmediateTrigger_Urunler")
        .StartNow());

    q.AddTrigger(opts => opts
        .ForJob(jobKeyUrunler)
        .WithIdentity("CronTrigger_Urunler")
        .WithCronSchedule("0 0 0 ? * *")
        .WithDescription("Üye Ýþlemleri Job'u"));


    var jobKeyPlasiyer = new JobKey("JobIslemPlasiyer");
    q.AddJob<JobIslemPlasiyer>(opts => opts.WithIdentity(jobKeyPlasiyer));

    q.AddTrigger(opts => opts
        .ForJob(jobKeyPlasiyer)
        .WithIdentity("ImmediateTrigger_Plasiyer")
        .StartNow());

    q.AddTrigger(opts => opts
        .ForJob(jobKeyPlasiyer)
        .WithIdentity("CronTrigger_Plasiyer")
        .WithCronSchedule("0 0 0 ? * *")
        .WithDescription("Plasiyer Job'u"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);




var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseBrowserLink();

}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    //HTTPS YE YÖNLENDÝRMEK ÝÇÝN AÞAÐIDAKÝ KODLARI AKTÝF ET

    //var options = new RewriteOptions();
    //options.AddRedirectToHttps();
    //options.Rules.Add(new RedirectToWwwRule());
    //app.UseRewriter(options);
}

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Hata oluþtu: " + ex.Message);
        throw;
    }
});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Admin/CkEditorElfinder/Assets")),
//    RequestPath = "/Admin/CkEditorElfinder/Assets"
//});

app.UseHttpsRedirection();

var config = app.Configuration;

//app.UseBrowserLink();

app.UseSession();
app.UseImageSharp();


app.UseStaticFiles();
app.UseDefaultFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ?? Sadece belirli path’e özel kontrol
app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString().ToLower();
    if (path == "/filemanager/dialog.php")
    {
        var result = await context.AuthenticateAsync("AdminAuth");

        if (!result.Succeeded)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Yetkisiz eriþim!");
            return;
        }
    }

    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = new PathString("/filemanager"),
    FileProvider = new PhysicalFileProvider(Path.GetFullPath(Path.Combine(Assembly.GetEntryAssembly().Location, "../filemanager"))),

});

app.UseResponsiveFileManager();



app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);

app.UseCookiePolicy();


//Proje baþlayýnca tablolarý otomatik oluþturma
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.Migrate();
}
//Proje baþlayýnca tablolarý otomatik oluþturma

app.MapAreaControllerRoute(
     name: "AreaAdmin",
     areaName: "Admin",
     pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
   name: "home",
    pattern: "/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
   name: "urlseoredirect",
    pattern: "{url?}/{controller=UrlSeo}/{action=Index}");





app.MapRazorPages();

app.MapHub<LoadingBarHub>("/loadingBarProgress");

app.Run();


IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
         .AddJsonFile($"appsettings.Development.json", optional: true).AddEnvironmentVariables();

    var config = builder.Build();


    return builder.Build();
}