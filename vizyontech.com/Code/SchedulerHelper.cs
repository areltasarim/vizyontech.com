using vizyontech.com.Code;
using Quartz;
using Quartz.Impl;

namespace vizyontech.com.Code
{
    public static class SchedulerHelper
    {
        public static async Task SchedulerSetup(IServiceProvider serviceProvider)
        {
            // ISchedulerFactory kullanarak IScheduler oluştur
            var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            // Scheduler'i başlat
            await scheduler.Start();

            // JobIslemUyeler tanımlama
            var jobUyeler = JobBuilder.Create<JobIslemUyeler>()
                .WithIdentity("JobIslemUyeler")
                .Build();

            var triggerUyeler = TriggerBuilder.Create()
                .WithIdentity("CronTrigger_Uyeler")
                .StartNow() // Uygulama başladığında hemen tetiklenir
                .WithCronSchedule("0 11 14 ? * *") // Her gün saat 14:11
                .Build();

            await scheduler.ScheduleJob(jobUyeler, triggerUyeler);


            // JobIslemUyeler tanımlama
            var jobUrunler = JobBuilder.Create<JobIslemUrunler>()
                .WithIdentity("JobIslemUrunler")
                .Build();

            var triggerUrunler = TriggerBuilder.Create()
                .WithIdentity("CronTrigger_Urunler")
                .StartNow() // Uygulama başladığında hemen tetiklenir
                .WithCronSchedule("0 00 00 ? * *") // Her gün saat 00:00
                .Build();

            await scheduler.ScheduleJob(jobUrunler, triggerUrunler);


            // JobIslemPlasiyer tanımlama
            var jobPlasiyer = JobBuilder.Create<JobIslemPlasiyer>()
                .WithIdentity("JobIslemPlasiyer")
                .Build();

            var triggerPlasiyer = TriggerBuilder.Create()
                .WithIdentity("CronTrigger_Plasiyer")
                .StartNow() // Uygulama başladığında hemen tetiklenir
                .WithCronSchedule("0 30 15 ? * *") // Her gün saat 15:30
                .Build();
            await scheduler.ScheduleJob(jobPlasiyer, triggerPlasiyer);

            // JobIslemKur tanımlama
            var jobKur = JobBuilder.Create<JobIslemKur>()
                .WithIdentity("JobIslemKur")
                .Build();

            var triggerKur = TriggerBuilder.Create()
                .WithIdentity("CronTrigger_Kur")
                .StartNow() // Uygulama başladığında hemen tetiklenir
                .WithCronSchedule("0 30 15 ? * *") // Her gün saat 15:30
                .Build();

            await scheduler.ScheduleJob(jobKur, triggerKur);

            Console.WriteLine("Tüm job'lar başarıyla planlandı.");
        }
    }
}
