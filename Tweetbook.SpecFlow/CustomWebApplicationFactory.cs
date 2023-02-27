using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Tweetbook.SpecFlow
{
    public class CustomWebApplicationFactory: WebApplicationFactory<Startup>
    {
        //protected string ApplicationName;
        protected override IHostBuilder CreateHostBuilder()
        {

            return Host.CreateDefaultBuilder().ConfigureWebHost((builder) =>
            {
                builder.UseStartup<TestStartup>();
            });
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");

            builder.ConfigureAppConfiguration((context, b) =>
            {
                context.HostingEnvironment.ApplicationName = "Tweetbook"; // Required to infer the content root application to test
            });

            base.ConfigureWebHost(builder);
        }

    }
}
