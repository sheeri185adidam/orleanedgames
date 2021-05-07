using System;

namespace GrainInterfaces.Player
{
    public interface IPlayerInfo
    {
        public Guid UserKey { get; }
        public long UserId { get;}
    }
}