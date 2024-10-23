using Blog.Api.Configurations;
using Blog.Data.Context;
using Blog.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/myapp-{Date}.txt");

builder.AddDatabaseSelector();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MeuDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Autor, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<MeuDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ResolveDependencies();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
