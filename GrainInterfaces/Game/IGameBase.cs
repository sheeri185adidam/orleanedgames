using System.Threading.Tasks;
using GrainInterfaces.Player;
using Orleans;

namespace GrainInterfaces.Game
{
    public interface IGameBase : IGrainWithGuidKey
    {
        Task<bool> IsActive();
        Task Start();
        Task Join(IPlayerBase player);
        Task Leave(IPlayerBase player);
    }
}