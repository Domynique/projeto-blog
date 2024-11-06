using Blog.App.Configurations;
using Blog.Core.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityConfig();

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapperConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.UseDbMigrationHelper();

app.Run();
