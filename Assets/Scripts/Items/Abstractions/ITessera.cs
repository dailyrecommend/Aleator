using Core;

namespace Items.Abstractions
{
    public interface ITessera
    {
        string Id { get; }
        bool CanTrigger(GameContext context, Runtime.TesseraRuntime runtime);
        void Trigger(GameContext context, Runtime.TesseraRuntime runtime);
        string GetDescription(Data.TesseraRecord record);
        void OnEquip(Runtime.TesseraRuntime runtime);
        void OnUnequip(Runtime.TesseraRuntime runtime);
    }
}