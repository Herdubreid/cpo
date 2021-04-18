using Celin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Celin
{
    class Program
    {
        static void Main(string[] args)
        {
            var store = new Store();
            store.SetValue("BaseUrl", "https://demo.steltix.com/jderest/v2/");
            store.SetValue("Username", "DEMO");
            store.SetValue("Password", "DEMO");

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddLogging(logger =>
                {
                    logger.SetMinimumLevel(LogLevel.Error)
                          .AddConsole();
                })
                .AddSingleton<IStore>(store)
                .AddSingleton<E1>()
                .AddSingleton<Doc.Settings>()
                .BuildServiceProvider());

            var settings = new Doc.Settings();
            Console.WriteLine("{0} {1} {2}", settings.BaseUrl, settings.Username, settings.Password);

        }
    }
}
