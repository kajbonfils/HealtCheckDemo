using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HCWebApp.Controllers;
using System;
using HealthChecks.UI.Client;

namespace HCWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IStockServiceRepository, StockServiceRepository>();
            services.AddControllersWithViews();

            services.AddHealthChecks()
            .AddSqlServer(Configuration.GetConnectionString("database"), failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded)
            .AddUrlGroup(new Uri(Configuration.GetSection("API:HCServiceHealth").Value), "HCWebService", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded, timeout: TimeSpan.FromMilliseconds(200));
            ;

            // TODO: Demo6
            services.AddHealthChecksUI().AddInMemoryStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                // TODO: Demo6
                endpoints.MapHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // TODO: Demo6
                endpoints.MapHealthChecksUI(setupOptions: setup =>
                {
                    setup.UIPath = "/healthui";
                });
            });

            // TODO: Demo6
            app.UseHealthChecksUI();

        }
    }
}
