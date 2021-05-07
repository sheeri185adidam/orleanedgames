using System;

namespace GrainInterfaces.Game
{
    public enum MessageType
    {
        Notification = 1,
        GamePlay = 2,
    }

    public abstract class GameMessage
    {
        protected GameMessage(Guid game, Guid player, MessageType type, string message)
        {
            Game = game;
            Player = player;
            Type = type;
            Message = message ?? throw new ArgumentNullException(nameof(game));
        }

        public Guid Game { get; }
        public Guid Player { get; }
        public MessageType Type { get; }
        public string Message { get; }
    }
}