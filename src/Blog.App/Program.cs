<<<<<<< HEAD
using Blog.App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

=======
using Blog.App.Configurations;
using Blog.Core.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityConfig();

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapperConfig();

>>>>>>> 7b94a7c (Ajustes projeto BlogApp)
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
<<<<<<< HEAD
=======
    app.UseDeveloperExceptionPage();
>>>>>>> 7b94a7c (Ajustes projeto BlogApp)
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
<<<<<<< HEAD
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
=======
>>>>>>> 7b94a7c (Ajustes projeto BlogApp)
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

<<<<<<< HEAD
=======
app.UseDbMigrationHelper();

>>>>>>> 7b94a7c (Ajustes projeto BlogApp)
app.Run();
