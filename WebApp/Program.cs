using System;
using System.Runtime.InteropServices;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Logging.AddConsole();
    builder.Logging.AddEventLog();
}

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    builder.Logging.AddSystemdConsole();
}


builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IShopService,ShopService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddMiniProfiler() // miniprofiler
                    .AddEntityFramework(); // tilføj entity framework profiling

// Add services to the container.
builder.Services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/Shops/List", "/");
                });

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseMiniProfiler();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
