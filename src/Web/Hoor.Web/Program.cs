namespace Hoor.Web
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    public static class Program
    {

        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // تحديد مدة الجلسة
                options.Cookie.HttpOnly = true;
            });

            // إضافة الخدمات الأخرى كـ Identity, EmailSender، إلخ
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
