using System;

namespace GrainInterfaces.Game
{
    public class GameNotificationMessage : GameMessage
    {
        public GameNotificationMessage(Guid game, string message) 
            : base(game, Guid.Empty, MessageType.Notification, message)
        {
        }
    }
}