using System;

namespace GrainInterfaces.Player
{
    public static class PlayerHelper
    {
        public static string MakeKey(Guid userKey, long userId)
        {
            return $"{userKey}|{userId}";
        }

        public static IPlayerInfo ParseKey(string key)
        {
            var parsed = key.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return new PlayerInfo(Guid.Parse(parsed[0]), Convert.ToInt64(parsed[1]));
        }
    }
}