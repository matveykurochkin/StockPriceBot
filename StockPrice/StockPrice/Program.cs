using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using StockPrice.Internal;

namespace StockPrice;

class Program
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    static async Task Main(string[] args)
    {
        try
        {
            using IHost host = Host.CreateDefaultBuilder()
                .UseNLog()
                .ConfigureHostConfiguration(cfgBuilder => { cfgBuilder.AddJsonFile("appsettings.json"); })
                .ConfigureServices(
                    services => { services.AddHostedService<Run>(); })
                .UseWindowsService(options => { options.ServiceName = ".NET StockBot"; })
                .Build();

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error running app");
        }
    }
}