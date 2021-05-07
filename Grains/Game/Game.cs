using System;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using GrainInterfaces.Player;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

namespace Grains.Game
{
    public abstract class Game<TState, TRequest, TResponse> 
        : Grain, IGame<TRequest, TResponse> where TState : new()
    {
        private readonly IPersistentState<TState> _gameState;
        private IAsyncStream<GameMessage> _stream;

        protected ILogger Logger { get; set; }
        protected TState GameState => _gameState.State?? new TState();

        protected Game(IPersistentState<TState> gameState, ILogger<Game<TState, TRequest, TResponse>> logger)
        {
            _gameState = gameState;
            Logger = logger;
        }

        public abstract Task<bool> IsActive();
        public abstract Task Start();
        public abstract Task<TResponse> Play(IPlayerBase player, TRequest request);

        public override Task OnActivateAsync()
        {
            var provider = GetStreamProvider("GameMessagesProvider");
            _stream = provider.GetStream<GameMessage>(this.GetPrimaryKey(), "GameMessages");
            return Task.CompletedTask;
        }

        public virtual async Task Join(IPlayerBase player)
        {
            await player.Subscribe(this);
            await NotifyPlayers($"Player {player.GetPrimaryKeyString()} has joined the Game {this.GetPrimaryKey()}");
        }

        public virtual async Task Leave(IPlayerBase player)
        {
            await player.Unsubscribe(this);
            await NotifyPlayers($"Player {player.GetPrimaryKeyString()} has left the Game {this.GetPrimaryKey()}");
        }

        protected virtual async Task NotifyPlayers(string message)
        {
            await _stream.OnNextAsync(new GameNotificationMessage(this.GetPrimaryKey(), message));
        }

        protected virtual async Task RequestPlay(Guid player)
        {
            await _stream.OnNextAsync(new GamePlayMessage(this.GetPrimaryKey(), player));
        }

        protected virtual async Task SaveState()
        {
            await _gameState.WriteStateAsync();
        }
    }
}