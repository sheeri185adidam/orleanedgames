using System;

namespace GrainInterfaces.Game
{
    public class GameInfo
    {
        public GameInfo(string name, Guid guid)
        {
            Name = name;
            Guid = guid;
        }

        public string Name { get; }
        public Guid Guid { get; }
    }
}