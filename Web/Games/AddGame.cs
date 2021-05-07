using System.Threading;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Web.Contracts;

namespace Web.Games
{
    public class AddGame : Endpoint.WithRequest<AddGameRequest>.WithResponse<AddGameResponse>
    {
        private readonly IClusterClient _client;

        public AddGame(IClusterClient client)
        {
            _client = client;
        }

        [Route("/v1/games/")]
        [HttpPost]
        public override async Task<ActionResult<AddGameResponse>> HandleAsync(AddGameRequest request, CancellationToken token = default)
        {
            /*var client = _client.GetGrain<IGameProvider>("GameProvider");
            var guid = await client.AddGame(request.Game);
            return new AddGameResponse
            {
                Name = request.Game,
                Guid = guid
            };*/

            return new AddGameResponse();
        }
    }
}