using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using GrainInterfaces.Player;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Grains.Game.Tumbler
{
    public class TumblerGame : Game<TumblerGameState, TumblerGameRequest, TumblerGameResponse>
    {
        public TumblerGame([PersistentState("gameState", "GameStateStore")]
            IPersistentState<TumblerGameState> gameState,
            ILogger<Game<TumblerGameState, TumblerGameRequest, TumblerGameResponse>> logger) : base(gameState, logger)
        {
        }

        public override async Task Start()
        {
            GameState.Active = true;
            await SaveState();
            await RequestPlay(GameState.Players[GameState.PlayerTurnIndex]);
        }

        public override Task<bool> IsActive()
        {
            var status = GameState?.Active ?? false;
            return Task.FromResult(status);
        }

        public override async Task Join(IPlayerBase player)
        {
            if (GameState.Active)
            {
                return;
            }

            var info = await player.Info();
            if (GameState.Players.Contains(info.UserKey))
            {
                return;
            }

            GameState.Players.Add(info.UserKey);
            await SaveState();
            await base.Join(player);
        }

        public override async Task Leave(IPlayerBase player)
        {
            if (GameState.Active)
            {
                return;
            }

            var info = await player.Info();
            if (GameState.Players.Contains(info.UserKey))
            {
                GameState.Players.Remove(info.UserKey);
                await SaveState();
                await base.Leave(player);
            }
        }


        public override async Task<TumblerGameResponse> Play(IPlayerBase player, TumblerGameRequest request)
        {
            var result = new TumblerGameResponse();
            if (!GameState.Active)
            {
                return result;
            }

            if (GameState.PlayerTurnIndex >= GameState.Players.Count)
            {
                return result;
            }

            var info = await player.Info();

            var playing = GameState.Players[GameState.PlayerTurnIndex];
            if (playing != info.UserKey) // it is not this player's turn
            {
                return result;
            }

            if (!GameState.RolledDices.ContainsKey(playing))
            {
                GameState.RolledDices[playing] = new List<Dice>();
            }

            GameState.RolledDices[playing].Add(request.Dice1);
            GameState.RolledDices[playing].Add(request.Dice2);

            GameState.PlayerTurnIndex++;

            if (GameState.PlayerTurnIndex < GameState.Players.Count)
            {
                await SaveState();

                //notify next player of his/her turn
                await RequestPlay(GameState.Players[GameState.PlayerTurnIndex]);
                return result;
            }

            var winner = Guid.Empty;
            var winnerPoints = 0;
            foreach (var (guid, rolled) in GameState.RolledDices)
            {
                var sum = rolled.Sum(play => play.Sum);
                if (sum <= winnerPoints)
                {
                    continue;
                }

                winnerPoints = sum;
                winner = guid;
            }

            GameState.Active = false; // game has ended
            GameState.WinningPlayer = winner;
            await SaveState(); 

            await NotifyPlayers($"Player {winner} is the winner by scoring {winnerPoints} points!");

            return result;
        }
    }
}