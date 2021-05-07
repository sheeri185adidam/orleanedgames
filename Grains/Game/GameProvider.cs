using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace Grains.Game
{
    [Serializable]
    public class GameProviderState
    {
        public Dictionary<Guid, GameInfo> Games { get; set; } = new Dictionary<Guid, GameInfo>();
    }

    public class GameProvider : Grain, IGameProvider
    {
        private readonly ILogger _logger;
        private readonly IPersistentState<GameProviderState> _games;

        public GameProvider(
            [PersistentState("games", "GameProviderStore")] IPersistentState<GameProviderState> games, 
            ILogger<GameProvider> logger)
        {
            _games = games;
            _logger = logger;
        }

        public virtual async Task RemoveGame(Guid game)
        {
            var collection = await GamesCollection;

            if (!collection.Games.ContainsKey(game))
            {
                return;
            }

            collection.Games.Remove(game);
            await SaveChanges();

            _logger.LogInformation($"GameProvider: Removed game {game}");
        }

        public async Task<Guid> AddGame(GameInfo game)
        {
            var collection = await GamesCollection;
            if (collection == null)
            {
                collection = new GameProviderState();
            }
            else
            {
                if (collection.Games.ContainsKey(game.Guid))
                {
                    throw new InvalidOperationException($"Game {game.Name}, {game.Guid} already exists");
                }
            }

            collection.Games.Add(game.Guid, game);
            await SaveChanges();
            _logger.LogInformation($"GameProvider: Added game {game.Name}, {game.Guid}");
            return game.Guid;
        }

        public virtual async Task<IEnumerable<GameInfo>> Games()
        {
            var collection = await GamesCollection;
            return await Task.FromResult(
                collection.Games.Count > 0 ? collection.Games.Values : Enumerable.Empty<GameInfo>());
        }

        protected virtual Task<GameProviderState> GamesCollection =>
            Task.FromResult(_games.State ?? new GameProviderState());

        protected virtual async Task SaveChanges()
        {
            await _games.WriteStateAsync();
        }
    }
}