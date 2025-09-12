using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dice.Abstractions;
using Dice.Attributes;

namespace Dice.Registry
{
    public static class DiceRegistry
    {
        static readonly Dictionary<string, Type> map = new(StringComparer.Ordinal);
        static bool inited;

        public static void Init()
        {
            if (inited) return;
            var iface = typeof(IDiceEffect);

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = asm.GetTypes(); }
                catch (ReflectionTypeLoadException e) { types = e.Types.Where(t => t != null).ToArray()!; }

                foreach (var t in types)
                {
                    if (t == null || t.IsAbstract || !iface.IsAssignableFrom(t)) continue;
                    var info = t.GetCustomAttribute<DiceInfoAttribute>();
                    if (info == null || string.IsNullOrWhiteSpace(info.effectId)) continue;
                    map[info.effectId] = t;
                }
            }
            inited = true;
        }

        public static IDiceEffect Create(string effectId)
        {
            if (!inited) Init();
            if (!map.TryGetValue(effectId, out var type))
                throw new Exception($"dice effect not found: {effectId}");
            return (IDiceEffect)Activator.CreateInstance(type)!;
        }
    }
}