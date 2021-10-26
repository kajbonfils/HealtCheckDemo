using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HCWebApp.Controllers;
using System.Net.Http;
using System;

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
            .AddServiceCheck(new Uri(Configuration.GetSection("API:HCServiceHealth").Value), failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Degraded());
            ;
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health"); 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
