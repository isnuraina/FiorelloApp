using FiorelloApp.Data;
using FiorelloApp.Services;
using FiorelloApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
    //.AddNewtonsoftJson(options =>
    //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        //);

            services.AddDbContext<FiorelloDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("FiorelloApp"));
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(20);
            });
            services.AddHttpContextAccessor();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAdventureService, AdventureService>();
        }
    }
}
