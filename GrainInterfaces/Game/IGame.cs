using System.Threading.Tasks;
using GrainInterfaces.Player;

namespace GrainInterfaces.Game
{
    public interface IGame<in TRequest, TResponse> : IGameBase
    {
        Task<TResponse> Play(IPlayerBase player, TRequest request);
    }
}