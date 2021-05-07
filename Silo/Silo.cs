using System;
using System.Threading.Tasks;
using Grains.Game;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace Silo
{
    public class Silo
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "games-dev";
                    options.ServiceId = "games-service";
                })
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GameProvider).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddSimpleMessageStreamProvider("GameMessagesProvider")
                .AddMemoryGrainStorage("GameProviderStore")
                .AddMemoryGrainStorage("GameStateStore")
                .AddMemoryGrainStorage("PlayerStateStore")
                .AddMemoryGrainStorage("PubSubStore");

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
