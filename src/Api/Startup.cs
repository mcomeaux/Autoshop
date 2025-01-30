using Autoshop.Application;
using Autoshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Autoshop.Application.Common.Interfaces;
using Autoshop.Api.Services;

namespace Autoshop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddOptions();
            services.AddResponseCaching();

            services.AddHttpClient();

            services.AddInfrastructure(Configuration, Environment);
            services.AddApplication(Configuration);
            //services.AddApplicationMessageHandlers();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddOpenApiDocument(config =>
            {
                config.Title = "Autoshop WebAPI";
            });

            
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        //don't serialize null values. reduces data transfered.
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                    .AddMetrics();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCustomExceptionHandler();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi();
            
            

            app.UseRouting();
            app.UseResponseCaching();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }


    }
}
