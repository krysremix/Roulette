using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Repos.Contracts;
using Repos.Implementations;
using Roulette.MiddleWare;
using Services.Contracts;
using Services.Implementations;

namespace Roulette
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

            services.AddControllers(options => {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.AllowEmptyInputInBodyModelBinding = true;
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
            .AddXmlSerializerFormatters();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Roulette", Version = "v1" });
            });

            services.AddSingleton<ILogger>(logger => { 
                var factory = LoggerFactory.Create(builder => { 
                    builder.AddConsole();
                });
                return factory.CreateLogger("Api");
            });
            services.AddSingleton<IConfiguration>(Configuration);

            //Injecting Services
            services.AddTransient<IBetService, BetService>();
            services.AddTransient<ISpinService, SpinService>();

            //Injecting Repos
            services.AddSingleton<IInitializerRepo, InitializerRepo>();
            services.AddTransient<IBetRepo, BetRepo>();
            services.AddTransient<ISpinRepo, SpinRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Roulette v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var logger = app.ApplicationServices.GetRequiredService<ILogger>();
            logger.LogInformation("---- API Started ----");

            var initializerRepo = app.ApplicationServices.GetRequiredService<IInitializerRepo>();
            initializerRepo.Initialize();
        }
    }
}
