using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            //services.AddDbContext<DataContext>(options =>
            //{
            //    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            //});
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer("Data Source=tcp:weatherdbsvr.database.windows.net,1433;Initial Catalog=weatherdb;Persist Security Info=False;User ID=wije;Password=Abc_123+;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            return services;
        }
    }
}