namespace Blog.App.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(AutoMapperProfileConfig).Assembly);
            
            return services;
        }
    }
}
