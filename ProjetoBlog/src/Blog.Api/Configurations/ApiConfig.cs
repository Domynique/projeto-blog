using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services) 
        {
            services.AddControllers()
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });

            services.Configure<ApiBehaviorOptions>(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;

                    });
            
            services.AddCors(options =>
                    {
                        options.AddPolicy("Development",
                                builder =>
                                    builder
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
                
                    });

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            
            app.UseAuthorization();


            return app;
        }
    }
}
