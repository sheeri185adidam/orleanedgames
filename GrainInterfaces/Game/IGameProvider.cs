using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces.Game
{
    public interface IGameProvider : IGrainWithStringKey
    {
        Task<Guid> AddGame(GameInfo game);
        Task RemoveGame(Guid game);
        Task<IEnumerable<GameInfo>> Games();
    }
}