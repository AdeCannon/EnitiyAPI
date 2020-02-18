using FOA.Entity.DataAccess;
using FOA.Entity.DataAccess.Repositories;
using FOA.WebApi.Extensions.HttpContext;
using FOA.WebApi.Extensions.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace FOA.Service.Discovery.API
{
    /// <summary>
    /// Set up the web api
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get; set; }

        /// <summary>
        /// The Start up CTOR
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped(_ => IdentityProvider.GetUserIdentity(IdentityProvider.UserIdentityType.AutoDetect));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<GTPSContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:GtpsConnectionString"]));

            services.AddTransient<IDataAccess, DataAccess>();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin",
            //        builder => builder
            //            .WithOrigins(this.Configuration.GetValue<string>("AllowAddressList"))
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .SetPreflightMaxAge(new TimeSpan(1, 0, 0, 0))
            //            .AllowCredentials());
            //});

            // IISDefaults requires the following import:
            // using Microsoft.AspNetCore.Server.IISIntegration;
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Front Office Entity API - " + this.CurrentEnvironment.EnvironmentName.ToUpper(),
                    Description = "Front Office FOA Entity API -Swagger Documentation",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Support", Email = "FOA_Support@GAM.Com", Url = "www.GAM.com" }
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseCors("AllowSpecificOrigin");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            this.CurrentEnvironment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseStaticFiles();

            // expose a static http context
            app.UseStaticHttpContext();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            app.UseSwagger(o =>
            {
                o.RouteTemplate = "docs/{documentName}/docs.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("../docs/v1/docs.json", "API v1");
            });
        }

        private string GetXmlCommentsPath()
        {
            var app = System.AppContext.BaseDirectory;
            return System.IO.Path.Combine(app, "FOA.Entity.API.xml");
        }
    }
}
