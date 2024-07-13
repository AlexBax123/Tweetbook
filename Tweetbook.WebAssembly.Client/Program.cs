using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tweetbook.WebAssembly.Client;
using Tweetbook.WebAssembly.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<CookieStorageAccessor>();
var x = new Uri("https://localhost:7023");
builder.Services.AddHttpClient<ITweetbookHelper, TweetbookHelper>(client =>
{
    client.BaseAddress = x;
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = x });
builder.Services.AddTransient<ITweetbookHelper, TweetbookHelper>();

await builder.Build().RunAsync();
