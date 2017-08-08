using AutoMapper;
using EG.One.DotNetCoreTemplate.Core;
using EG.One.DotNetCoreTemplate.Infrastructure;
using EG.One.DotNetCoreTemplate.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using PrometheusTools.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace EG.One.DotNetCoreTemplate
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private IConfigurationRoot Configuration { get; }
        private readonly string _routePrefix = "demoprefix/api";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Prometheus Middleware
            var prometheusOptions = new PrometheusMiddlewareOptions();
            prometheusOptions.ExcludeRoutes.Add("swagger");

            services.AddMvc(options =>
            {
                options.Filters.Add(new GlobalExceptionFilter(_env));
                options.UseCentralRoutePrefix(new RouteAttribute(_routePrefix));
                options.Filters.Add(new PrometheusActionFilter(prometheusOptions));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            if (_env.IsDevelopment())
            {
                //Setup swagger
                services.AddSwaggerGen(setup =>
                {
                    setup.OperationFilter<CustomSwaggerHeadersFilter>();
                    
                    setup.SwaggerDoc("v1", new Info()
                    {
                        Description = "ReplaceMe API documentation",
                        Title = "EG.One.DotNetCoreTemplate.API",
                        Version = "v1"
                    });
                    setup.DescribeAllEnumsAsStrings();

                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "EG.One.DotNetCoreTemplate.xml");
                    setup.IncludeXmlComments(filePath);
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = Configuration.GetSection("IdentityServer:Url").Value,
                AllowedScopes = { "DemoApp" },

                RequireHttpsMetadata = false
            });

            if (env.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = $"{_routePrefix}/swagger/{{documentName}}/swagger.json";
                });
                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = $"{_routePrefix}/swagger";
                    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                    c.DocExpansion("None");
                });

                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void ConfigureDbSettings(IServiceCollection services)
        {
            string connection;

            // if appsettings.production is used, the connectionstring comes from kubernetes via environment variable
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                connection = Environment.GetEnvironmentVariable("EGONEDB");
            }
            else
            {
                connection = Configuration.GetSection("DbSettings:Connection").Value;
            }

            var userSettings = new DbSettings()
            {
                Database = Configuration.GetSection("DbSettings:Database").Value,
                DemoCollection = Configuration.GetSection("DbSettings:DemoCollection").Value,
                Connection = connection
            };
            services.AddSingleton(userSettings);
        }
    }
}
