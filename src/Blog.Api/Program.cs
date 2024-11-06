using Blog.Api.Configurations;
using Blog.Core.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddApiConfig();

    builder.Services.AddSwaggerConfig();

    builder.Services.AddAutoMapperConfig();

    builder.Services.AddResolveDependencies();

    builder.Services.AddIdentityConfig();

    builder.Services.AddDbContext(builder.Configuration);

    builder.Services.AddJwtConfig(builder.Configuration);

var app = builder.Build();

    app.UseSwaggerConfig();

    app.UseApiConfig(app.Environment);

    app.MapControllers();

    app.UseDbMigrationHelper();

    app.Run();
