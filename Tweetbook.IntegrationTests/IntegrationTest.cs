using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Tweetbook.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTest()
        {
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
                            var dbContext = serviceScope.ServiceProvider.GetService<DataContext>();
                            dbContext.Database.EnsureCreated();

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
            _httpClient = appFactory.CreateClient();

        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }

        protected async Task AuthenticateAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<Response<PostResponse>> CreatePostAsync(CreatePostRequest createPostRequest)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Posts.Create, createPostRequest);
            return await response.Content.ReadAsAsync<Response<PostResponse>>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "ego@ist.be",
                Password = "WhatTheFuck123_"
            });
            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}
