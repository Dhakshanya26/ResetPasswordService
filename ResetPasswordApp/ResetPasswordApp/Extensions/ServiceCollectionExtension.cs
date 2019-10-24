using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ResetPasswordApp.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceProvider AddServiceLibrary(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            services.AddLogging(x => x.AddConsole());
            services.AddDbContext<MyDbContext>(
                b => b.UseSqlServer(configuration.GetSection("connectionString").Value));
            services.AddTransient<IEmailService,EmailService>();
            services.AddTransient<ResetPasswordService>();
            
            services.AddSingleton<IConfiguration>(configuration);
            return services.BuildServiceProvider();
        }
    }
}
