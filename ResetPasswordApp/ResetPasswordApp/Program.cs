using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ResetPasswordApp.Extensions;

namespace ResetPasswordApp
{
    class Program
    {
        private static ResetPasswordService _resetPasswordService;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Application Started");
            RegisterResolveDependencies();
            await _resetPasswordService.SendPasswordResetEmail();
            Console.WriteLine("Application finished");
        }

        private static void RegisterResolveDependencies()
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.AddServiceLibrary();
            _resetPasswordService = serviceProvider.GetService<ResetPasswordService>();
        }
    }
}
