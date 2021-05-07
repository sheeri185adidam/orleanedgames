using System;

namespace Grains.Game
{
    [Serializable]
    public class GameStateBase
    {
        public bool Active { get; set; } = false;
    }
}