﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

namespace Tweetbook.SpecFlow.Hooks
{
    [Binding]
    public sealed class TweetbookHooks
    {
        //protected HttpClient _httpClient;
        private IServiceProvider _serviceProvider;

        private readonly IObjectContainer _objectContainer;

        public TweetbookHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {

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
                            options.UseInMemoryDatabase("TestDb");
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
            //_objectContainer.RegisterInstanceAs(appFactory);
            _serviceProvider = appFactory.Services;
            var httpClient = appFactory.CreateClient();
            _objectContainer.RegisterInstanceAs(httpClient);
            //_objectContainer.RegisterInstanceAs(new Banaan("mybanana"));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
            //TODO: implement logic that has to run after executing each scenario
        }
    }

    //public class Banaan
    //{

    //    public Banaan(string naam)
    //    {
    //        Naam = naam;
    //    }
    //    public string Naam { get; private set; } = "banana";
    //}
}
