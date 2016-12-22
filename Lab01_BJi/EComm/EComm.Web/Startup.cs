using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EComm.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /* config the web api access policy after the policy has been added in Configure() */
            services.AddCors(options =>
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();

                }));
            
            services.AddDbContext<ECommContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(("Defaultconnection")))
                );

            services.AddMemoryCache();
            services.AddSession();
            services.AddAuthorization(options => {
                options.AddPolicy("AdminsOnly", policy =>
                  policy.RequireClaim(ClaimTypes.Role, "Admin"));
            });

            // Add framework services.
            services.AddMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.InputFormatters.Add(new XmlSerializerInputFormatter());
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            /* send all errors to /error/{status_code} page */
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            /* support static files, such as html, javascript, etc. */
            app.UseStaticFiles();

            /* support http session */
            app.UseSession();

            /* use cokkie middleware to support authentication. */
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                LoginPath = new PathString("/Auth/Login"),
                AccessDeniedPath = new PathString("/Auth/Forbidden"),
                AutomaticChallenge = true
            });


            /* allow web api request from all domains, even for request from outside of domain */
            app.UseCors("AllowAll");

            /* define the routing */
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
