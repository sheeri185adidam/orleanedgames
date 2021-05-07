using System;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using GrainInterfaces.Player;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;

namespace Client
{
    public class Client
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                await using var client = await ConnectClient();
                DoClientWork(client).GetAwaiter().GetResult();
                Console.ReadKey();

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("MakeKey sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "games-dev";
                    options.ServiceId = "games-service";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IGrainFactory client)
        {
            var provider = client.GetGrain<IGameProvider>("GameProvider");
            
            var gameGuid = await provider.AddGame(new GameInfo("TumblerGame", Guid.NewGuid()));
            Console.WriteLine($"Added game `TumblerGame: {gameGuid}`");

            Console.WriteLine("Provider Games: ");
            foreach (var game in await provider.Games())
            {
                Console.WriteLine($"Game 1: {game.Name}, {game.Guid}");
            }

            const int nplayers = 20;
            Console.WriteLine($"Adding {nplayers} Players....");

            var gameGrain = client.GetGrain<IGameBase>(gameGuid, "Grains.Game.Tumbler.TumblerGame");
            var player = new IPlayerBase[nplayers];
            for (var i = 0; i < nplayers; i++)
            {
                player[i] = client.GetGrain<IPlayerBase>(PlayerHelper.MakeKey(Guid.NewGuid(), i + 1),
                    "Grains.Player.Tumbler.TumblerPlayer");
                await gameGrain.Join(player[i]);
            }

            Console.WriteLine("Starting the tumbler game...");
            await gameGrain.Start();

/*            // create two players
            var blackjackGame = client.GetGrain<IGame>(blackjack);
            var player1 = client.GetGrain<IPlayer>(Guid.NewGuid());
            await blackjackGame.Join(player1);

            var baccaratGame = client.GetGrain<IGame>(baccarat);
            var player2 = client.GetGrain<IPlayer>(Guid.NewGuid());
            await baccaratGame.Join(player2);

            var player3 = client.GetGrain<IPlayer>(Guid.NewGuid());
            await baccaratGame.Join(player3);*/
        }
    }
}
