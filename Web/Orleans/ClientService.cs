using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Web.Orleans
{
    public class ClientService : IHostedService
    {
        public IClusterClient Client { get; private set; }

        public ClientService(ILoggerProvider loggerProvider)
        {
            Client = new ClientBuilder()
                .UseConsulClustering(consul =>
                    {
                        consul.Address = new Uri("https://consul.dev.dkinternal.com/");
                        consul.KvRootFolder = "orleanedgames";
                    })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "games-dev";
                    options.ServiceId = "games-service";
                })
                .ConfigureLogging(builder => builder.AddProvider(loggerProvider))
                .Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Client.Connect();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Client.Close();
            Client.Dispose();
            Client = null;
        }
    }
}