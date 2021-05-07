using System;
using System.Collections.Generic;
using GrainInterfaces.Game;

namespace Grains.Game.Tumbler
{
    [Serializable]
    public class TumblerGameState : GameStateBase
    {
        public IList<Guid> Players { get; set; } = new List<Guid>();
        public Dictionary<Guid, IList<Dice>> RolledDices { get; set; } = new Dictionary<Guid, IList<Dice>>();
        public int PlayerTurnIndex { get; set; } = 0;
        public Guid WinningPlayer { get; set; } = Guid.Empty;
    }
}