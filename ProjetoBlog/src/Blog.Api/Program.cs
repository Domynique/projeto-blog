using Blog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddApiConfig();

    builder.Services.AddSwaggerConfig();

    builder.Services.AddAutoMapperConfig();

    builder.Services.AddResolveDependencies();

    builder.Services.AddIdentityConfig(builder.Configuration);

    builder.Services.AddDbContext(builder.Configuration);

    builder.Services.AddJwtConfig(builder.Configuration);

var app = builder.Build();

    app.UseSwaggerConfig();

    app.UseApiConfig(app.Environment);

    app.MapControllers();

    app.UseDbMigrationHelper();

    app.Run();
