using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Game.Systems
{
    public static class SystemHash
    {
        private static Dictionary<Type, int> hashes = new Dictionary<Type, int>();

        static SystemHash()
        {
            Type type = typeof(ISystem);
            foreach (Type assemblyType in Assembly.GetAssembly(typeof(ISystem)).GetTypes().Where(t => !t.IsAbstract && type.IsAssignableFrom(t)))
            {
                int typeHash = assemblyType.ToString().GetHashCode();
                hashes.TryAdd(assemblyType, typeHash);
            }
        }

        public static bool TryGetHash(Type type, out int hash)
        {
            if (hashes.TryGetValue(type, out hash))
            {
                return true;
            }
            return false;
        }
    }
}

