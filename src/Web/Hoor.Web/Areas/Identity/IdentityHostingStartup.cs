using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Hoor.Web.Areas.Identity.IdentityHostingStartup))]

namespace Hoor.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
