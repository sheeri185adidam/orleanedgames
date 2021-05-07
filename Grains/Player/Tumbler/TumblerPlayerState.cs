using System;
using System.Collections.Generic;
using GrainInterfaces.Game;

namespace Grains.Player.Tumbler
{
    [Serializable]
    public class TumblerPlayerState : PlayerStateBase
    {
        public IList<Dice> Rolls { get; set; } = new List<Dice>();
    }
}