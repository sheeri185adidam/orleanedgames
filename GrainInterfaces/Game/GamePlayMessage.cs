using System;

namespace GrainInterfaces.Game
{
    public class GamePlayMessage : GameMessage
    {
        public GamePlayMessage(Guid game, Guid player) 
            : base(game, player, MessageType.GamePlay, string.Empty)
        {
        }
    }
}