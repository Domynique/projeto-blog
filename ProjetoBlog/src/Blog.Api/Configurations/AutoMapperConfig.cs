namespace Blog.Api.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(AutoMapperSettings).Assembly);
            
            return services;
        }
    }
}
