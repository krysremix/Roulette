using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Repos.Contracts;
using Repos.Implementations;
using Services.Contracts;
using Services.Implementations;
using System;

namespace Test
{
    public static class Helper
    {
        private static IConfiguration _config;
        private static IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder();
                    _config = builder.Build();
                }
                return _config;
            }
        }
        private static IServiceProvider Provider()
        {
            var services = new ServiceCollection();

            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddSingleton<IConfiguration>(Configuration);

            //Injecting Services
            services.AddTransient<IBetService, BetService>();
            services.AddTransient<ISpinService, SpinService>();

            //Injecting Repos
            services.AddSingleton<IInitializerRepo, InitializerRepo>();
            services.AddTransient<IBetRepo, BetRepo>();
            services.AddTransient<ISpinRepo, SpinRepo>();

            return services.BuildServiceProvider();
        }

        public static T GetRequiredService<T>()
        {
            var provider = Provider();
            return provider.GetRequiredService<T>();
        }
    }
}
