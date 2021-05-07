using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces.Game
{
    public interface ITumbler : IGrainWithStringKey
    {
        Task<IEnumerable<Dice>> Tumble();
    }
}