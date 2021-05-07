using System.Threading.Tasks;
using GrainInterfaces.Game;
using Orleans;

namespace GrainInterfaces.Player
{
    public interface IPlayerBase : IGrainWithStringKey, IGameObserver
    {
        Task<IPlayerInfo> Info();

        Task Subscribe(IGameBase game);

        Task Unsubscribe(IGameBase game);
    }
}