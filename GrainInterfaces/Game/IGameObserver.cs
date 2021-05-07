using Orleans.Streams;

namespace GrainInterfaces.Game
{
    public interface IGameObserver : IAsyncObserver<GameMessage>
    {
    }
}