using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Tweetbook;
using Tweetbook.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Microsoft.AspNetCore.Identity;
using BoDi;
using System.Threading;

namespace Tweetbook.SpecFlow.Hooks
{
    [Binding]
    public sealed class TweetbookHooks
    {
        private IServiceProvider _serviceProvider;

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario("post")]
        public void BeforeScenario(ObjectContainer objectContainer)
        {
            string dbName = Guid.NewGuid().ToString();
        //TODO: implement logic that has to run before executing each scenario
        var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(async services =>
                    {
                        //var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DataContext));
                        services.RemoveAll(typeof(DbContextOptions<DataContext>));
                        services.AddDbContext<DataContext>(options =>
                        {
                        options.UseInMemoryDatabase(dbName);// "TestDb");
                        });
                        var sp = services.BuildServiceProvider();
                        using (var serviceScope = sp.CreateScope())
                        {
                            var db = serviceScope.ServiceProvider.GetService<DataContext>();
                            db.Database.EnsureCreated();
                            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                            if (!await roleManager.RoleExistsAsync("Admin"))
                            {
                                var adminRole = new IdentityRole("Admin");
                                await roleManager.CreateAsync(adminRole);
                            }
                            if (!await roleManager.RoleExistsAsync("Poster"))
                            {
                                var posterRole = new IdentityRole("Poster");
                                await roleManager.CreateAsync(posterRole);
                            }
                        }
                    });
                });
            _serviceProvider = appFactory.Services;
            var httpClient = appFactory.CreateClient();
            objectContainer.BaseContainer.RegisterInstanceAs(httpClient);
        }

        [AfterScenario("post")]
        public void AfterScenario()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
