using System;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using Grains.Game.Tumbler;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Grains.Player.Tumbler
{
    public class TumblerPlayer : Player<TumblerPlayerState>
    {
        private ITumbler _tumbler;

        public TumblerPlayer([PersistentState("playerState", "PlayerStateStore")]
            IPersistentState<TumblerPlayerState> state, ILogger<TumblerPlayer> logger)
            : base(state, logger)
        {
        }

        protected override async Task HandleGameMessage(GameMessage message)
        {
            var info = await Info();
            switch (message.Type)
            {
                case MessageType.Notification:
                    Logger.LogInformation($"Game Notification:Player {info.UserId}: {message.Message}");
                    return;
                case MessageType.GamePlay:
                    if (info.UserKey != message.Player)
                    {
                        return;
                    }

                    var rolled = (await _tumbler.Tumble()).ToList();
                    var game = GrainFactory.GetGrain<IGame<TumblerGameRequest, TumblerGameResponse>>(message.Game, "Grains.Game.Tumbler.TumblerGame");

                    await game.Play(this, new TumblerGameRequest(rolled[0], rolled[1]));
                    
                    //save roll
                    foreach (var dice in rolled)
                    {
                        PlayerState.Rolls.Add(dice);
                    }

                    await SaveState();
                    Logger.LogInformation($"Game Play:Player {(await Info()).UserId}: {rolled[0]},{rolled[1]}");
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override Task OnActivateAsync()
        {
            _tumbler = GrainFactory.GetGrain<ITumbler>("Tumbler");
            return base.OnActivateAsync();
        }
    }
}