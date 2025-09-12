using System.Collections.Generic;
using Core;
using Data;
using Items.Abstractions;
using Items.Registry;

namespace Items.Runtime
{
    public sealed class TesseraInventory
    {
        private readonly List<TesseraRuntime> list = new();
        public IReadOnlyList<TesseraRuntime> All => list;

        public bool GrantById(TesseraService svc, string id, out TesseraRuntime runtime)
        {
            runtime = null;
            if (!svc || !svc.TryGet(id, out var rec)) return false;

            runtime = new TesseraRuntime(rec);
            list.Add(runtime);
            return true;
        }

        public void TriggerAll(GameContext context)
        {
            foreach (var rt in list)
            {
                ITessera fx = TesseraRegistry.Create(rt.record.effectId);
                if (fx.CanTrigger(context, rt)) fx.Trigger(context, rt);
            }
        }
    }
}