using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalesService.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MassTransit;
using ProductService.Stub.MessageContracts;
using SalesService.Web.Consumer;
using GreenPipes;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace SalesService.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IBusControl _bus;

        private const string CorsPolicyName = "DontDoThisInProduction";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(CorsPolicyName, builder => {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            services.AddApiVersioning();
            services.AddControllers();
            services.AddDbContext<SalesDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesService", Version = "v1.0" });
            });                
            services.AddHealthChecks();
            
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ProductChangeConsumer>>();
            var dbcontext = serviceProvider.GetService<SalesDbContext>();
            _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(Configuration["RabbitMQUri"]), h => 
                { 
                    h.Username(Configuration["RabbitMQUser"]);
                    h.Password(Configuration["RabbitMQPassword"]);
                });
                cfg.ReceiveEndpoint("web-service-endpoint", e => 
                {
                    e.PrefetchCount = 10;
                    e.UseMessageRetry(x => x.Interval(2,100));
                    e.Consumer(() => new ProductChangeConsumer(logger,dbcontext)); 
                });
            });

            services.AddSingleton<IBus>(_bus);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesService V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            lifetime.ApplicationStarted.Register(() => _bus.Start());
            lifetime.ApplicationStopping.Register(() => _bus.Stop());
        }
    }
}
