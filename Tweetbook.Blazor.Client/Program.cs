using Tweetbook.Blazor.Client;
using Tweetbook.Blazor.Client.Components;
using Tweetbook.Blazor.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
var x = new Uri(builder.Configuration["BaseUrl"]);
builder.Services.AddHttpClient<ITweetbookHelper, TweetbookHelper>(client =>
{
    client.BaseAddress = x;
});

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.From;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
        options.Cookie.Name = ".Blazor.Session"; // <--- Add line
});
builder.Services.AddScoped<CookieStorageAccessor>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthorization();
app.UseSession();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//begin SetString() hack
app.Use(async delegate (HttpContext Context, Func<Task> Next)
{
    //this throwaway session variable will "prime" the SetString() method
    //to allow it to be called after the response has started
    var TempKey = Guid.NewGuid().ToString(); //create a random key
    Context.Session.Set(TempKey, Array.Empty<byte>()); //set the throwaway session variable
    Context.Session.Remove(TempKey); //remove the throwaway session variable
    await Next(); //continue on with the request
});
//end SetString() hack
app.Run();
