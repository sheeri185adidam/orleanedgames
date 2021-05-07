using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Web.Contracts;

namespace Web.Games
{
    public class GetGames : Endpoint.WithoutRequest.WithResponse<GetGamesResponse>
    {
        private readonly IClusterClient _client;

        public GetGames(IClusterClient client)
        {
            _client = client;
        }
        
        [Route("/v1/games/")]
        [HttpGet]
        public override async Task<ActionResult<GetGamesResponse>> HandleAsync(CancellationToken token = default)
        {
            try
            {
                var games = new List<string>();
                var provider = _client.GetGrain<IGameProvider>("GameProvider");
                foreach (var game in await provider.Games())
                {
                    games.Add(game.ToString());
                }

                var response = new GetGamesResponse
                {
                    Games = games
                };

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}