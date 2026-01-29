using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreHelper;
using EticaretWebCoreViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using MySqlConnector;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace vizyontech.com.Code
{
    public class JobIslemKur : IJob
    {

        public virtual async Task Execute(IJobExecutionContext context)
        {
            AppDbContext _context = new AppDbContext();

            _context.ChangeTracker.AutoDetectChangesEnabled = false;


            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            var provider = configuration["Application:DatabaseProvider"];
            string baglanti = "";
            if (provider == "SqlServer")
            {
                baglanti = configuration.GetConnectionString("SqlServer");
            }
            else
            {
                baglanti = configuration.GetConnectionString("Mysql");
            }


            var result = new ResultViewModel();
            try
            {
                using (var connection = new MySqlConnection(baglanti))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {

                            var filtrelenmisParaBirimleri = Enum.GetValues(typeof(ParaBirimi))
                            .Cast<ParaBirimi>()
                            .Where(p => p != ParaBirimi.TRY);

                            foreach (var item in filtrelenmisParaBirimleri)
                            {
                                int parabirimi = (int)item;

                                var kur = GetKur(parabirimi).Result;
                                var parabirimDeger = _context.ParaBirimleri.Where(x => x.Kodu == item.ToString()).FirstOrDefault();

                                var kurVarmi = _context.Kur.Where(x => x.ParaBirimId == parabirimDeger.Id).FirstOrDefault();
                                if (kurVarmi == null)
                                {
                                    var kurEkle = new Kur()
                                    {
                                        ParaBirimId = (int)parabirimDeger.Id,
                                        TLKur = kur,
                                    };
                                    _context.Entry(kurEkle).State = EntityState.Added;
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    kurVarmi.TLKur = kur;
                                    _context.Entry(kurVarmi).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                          
                            }

                            transaction.Commit();
                        }
                        catch (Exception hata)
                        {
                            transaction.Rollback();

                            result.Basarilimi = false;
                            result.MesajDurumu = "danger";
                            result.Mesaj = "Hata Oluştu." + hata.Message;
                        }
                    }
                }

            }
            catch (Exception hata)
            {

            }

        }

        public async Task<decimal> GetKur(int ParaBirimId)
        {
            string today = "http://www.tcmb.gov.tr/kurlar/today.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(today);
            string USD = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            string EURO = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;

            decimal kurdeger = 0;
            switch (ParaBirimId)
            {
                case (int)ParaBirimi.USD:
                    kurdeger = decimal.Parse(USD, NumberStyles.Any, CultureInfo.InvariantCulture);
                    break;
                case (int)ParaBirimi.EUR:
                    kurdeger = decimal.Parse(EURO, NumberStyles.Any, CultureInfo.InvariantCulture);
                    break;
                default:
                    break;
            }

            return kurdeger;
        }
    }
}
