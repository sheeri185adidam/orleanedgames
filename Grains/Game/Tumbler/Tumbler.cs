using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterfaces.Game;
using Orleans;
using Orleans.Concurrency;

namespace Grains.Game.Tumbler
{
    [StatelessWorker]
    public class Tumbler : Grain, ITumbler
    {
        public async Task<IEnumerable<Dice>> Tumble()
        {
            var random = new Random();
            var result = new List<Dice>
            {
                new Dice(random.Next(1, 7), random.Next(1, 7)),
                new Dice(random.Next(1, 7), random.Next(1, 7)),
            };

            return await Task.FromResult(result);
        }
    }
}