using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreEntity;
using EticaretWebCoreEntity.Enums;
using EticaretWebCoreEntity.Opak;
using EticaretWebCoreHelper;
using EticaretWebCoreService;
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
    public class JobIslemPlasiyer : IJob
    {
        private readonly AppDbContext _context;
        private readonly OpakDbContext _opakDbContext;

        public JobIslemPlasiyer(AppDbContext context, OpakDbContext opakDbContext)
        {
            _context = context;
            _opakDbContext = opakDbContext;
        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            var result = new ResultViewModel();

            try
            {
                var plasiyerList = await _opakDbContext.TBLPLASIYERSB
                    .ToListAsync();

                foreach (var item in plasiyerList)
                {
                    var mevcut = await _context.Plasiyer
                        .FirstOrDefaultAsync(p => p.Kod == item.KOD);

                    if (mevcut != null)
                        continue; 

                    var yeniPlasiyer = new Plasiyer
                    {
                        PlasiyerId = item.ID,
                        AdSoyad = item.ADI,
                        Gsm = item.TELEFON,
                        Email = item.EMAIL,
                        Kod = item.KOD,
                        Grup = item.GRUP
                    };
                    await _context.Plasiyer.AddAsync(yeniPlasiyer);
                }
                await _context.SaveChangesAsync();

                Console.WriteLine($"{plasiyerList.Count} plasiyer başarıyla aktarıldı.");
            }
            catch (Exception hata)
            {
                    Console.WriteLine($"Hata: {hata.Message}");
            }
        }
    }

}
