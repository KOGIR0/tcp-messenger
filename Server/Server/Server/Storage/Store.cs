using System;
using System.Collections.Concurrent;
using System.Text;

namespace Server.Server.Storage
{
    static class Store
    {
        private static ConcurrentDictionary<Guid, Guid> idCodeMap = new ConcurrentDictionary<Guid, Guid>();

        public static void Add(Guid key, Guid value)
        {
            idCodeMap[key] = value;
        }

        public static Guid Get(Guid key)
        {
            return idCodeMap[key];
        }

        public static bool ContainsKey(Guid key)
        {
            return idCodeMap.ContainsKey(key);
        }
    }
}
