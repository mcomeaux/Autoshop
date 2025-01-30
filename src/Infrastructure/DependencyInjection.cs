using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Autoshop.Application.Common.Interfaces;
using Autoshop.Infrastructure.Services;

namespace Autoshop.Infrastructure
{
    public static class DependencyInjection
    {
        //public static readonly ILoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new NLogLoggerProvider() });

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging(true);
                //.UseLoggerFactory(MyLoggerFactory);
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            //services.AddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


            services.AddTransient<IDateTime, DateTimeService>();


            return services;
        }
    }
}
