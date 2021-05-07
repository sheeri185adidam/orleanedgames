using System;
using Orleans.Concurrency;

namespace GrainInterfaces.Player
{
    [Immutable]
    public class PlayerInfo : IPlayerInfo
    {
        public PlayerInfo(Guid userKey, long userId)
        {
            UserKey = userKey;
            UserId = userId;
        }

        public Guid UserKey { get; }
        public long UserId { get; }
    }
}