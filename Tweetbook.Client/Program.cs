using Tweetbook.Client.Services;
using TweetbookApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
var x = new Uri(builder.Configuration["BaseUrl"]);
builder.Services.AddHttpClient<ITweetbookHelper, TweetbookHelper>(client =>
{
    client.BaseAddress = x ;
});
//builder.Services.AddTransient<ITweetbookHelper, TweetbookHelper>();

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.From;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.Run();
