using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IShopService,ShopService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "ShopCache";
});

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
