using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Items.Abstractions;
using Items.Attributes;

namespace Items.Registry
{
    public static class TesseraRegistry
    {
        private static readonly Dictionary<string, Type> map = new(StringComparer.Ordinal);
        private static bool inited = false;

        public static void Init()
        {
            if (inited) return;

            var iface = typeof(ITessera);

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = asm.GetTypes(); }
                catch (ReflectionTypeLoadException e) { types = e.Types.Where(t => t != null).ToArray()!; }

                foreach (var t in types)
                {
                    if (t == null || !iface.IsAssignableFrom(t) || t.IsAbstract) continue;

                    var info = t.GetCustomAttribute<TesseraInfoAttribute>();
                    if (info == null || string.IsNullOrWhiteSpace(info.effectId)) continue;

                    map[info.effectId] = t;
                }
            }

            inited = true;
        }

        public static ITessera Create(string effectId)
        {
            if (!inited) Init();

            if (!map.TryGetValue(effectId, out var type))
                throw new Exception($"tessera effect not found: {effectId}");

            return (ITessera)Activator.CreateInstance(type)!;
        }
    }
}