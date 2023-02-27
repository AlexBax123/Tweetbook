using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetbook.Installer;
using Tweetbook.Data;

namespace Tweetbook.SpecFlow
{
    public class TestStartup: Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }
        public new void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
            //base.ConfigureServices(services);
            services.RemoveAll(typeof(DbContextOptions<DataContext>));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
            var sp = services.BuildServiceProvider();
            using (var serviceScope = sp.CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<DataContext>();
                db.Database.EnsureCreated();
                var roleStore = new RoleStore<IdentityRole>(db);
                roleStore.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole { Name = "POSTER", NormalizedName = "POSTER" }).Wait();
                // db.SaveChanges();

            }
        }

        public new void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
        }
    }
}
