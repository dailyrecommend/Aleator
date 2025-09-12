using Core;
using Data;
using Items.Abstractions;
using Items.Attributes;
using Items.Runtime;

namespace Items.Samples
{
    [TesseraInfo("tessera.command")]
    public sealed class TesseraCommand : ITessera
    {
        public string Id => "command_tessera";

        public bool CanTrigger(GameContext context, TesseraRuntime runtime)
        {
            // Any Hand 조건은 상위 호출자가 보장. 여기서는 항상 허용.
            return true;
        }

        public void Trigger(GameContext context, TesseraRuntime runtime)
        {
            var chips = runtime.GetInt("chips", 10);

            context.chipCount += chips;
        }

        public string GetDescription(TesseraRecord record)
        {
            return record.desc;
        }

        public void OnEquip(TesseraRuntime runtime) { }

        public void OnUnequip(TesseraRuntime runtime) { }
    }
}