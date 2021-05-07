using System;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using GrainInterfaces.Player;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

namespace Grains.Player
{
    public abstract class Player<TState> : Grain, IPlayerBase where TState : new()
    {
        private readonly IPersistentState<TState> _playerState;
        private StreamSubscriptionHandle<GameMessage> _gameMessageStream;
        private IPlayerInfo _playerInfo;

        protected ILogger Logger { get; set; }

        protected TState PlayerState => _playerState.State ?? new TState();

        protected Player(IPersistentState<TState> playerState, ILogger<Player<TState>> logger)
        {
            _playerState = playerState;
            Logger = logger;
        }

        protected abstract Task HandleGameMessage(GameMessage message);

        protected virtual async Task SaveState()
        {
            await _playerState.WriteStateAsync();
        }

        public override Task OnActivateAsync()
        {
            _playerInfo = PlayerHelper.ParseKey(this.GetPrimaryKeyString());
            return Task.CompletedTask;
        }

        public async Task<IPlayerInfo> Info()
        {
            return await Task.FromResult(_playerInfo);
        }

        public virtual async Task Subscribe(IGameBase game)
        {
            var provider = base.GetStreamProvider("GameMessagesProvider");
            var stream = provider.GetStream<GameMessage>(game.GetPrimaryKey(), "GameMessages");
            _gameMessageStream = await stream.SubscribeAsync(this);
        }

        public virtual async Task Unsubscribe(IGameBase game)
        {
            await _gameMessageStream.UnsubscribeAsync();
        }

        public virtual Task OnNextAsync(GameMessage item, StreamSequenceToken token = null)
        {
            return HandleGameMessage(item);
        }

        public virtual Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}