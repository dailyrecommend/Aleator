using Core;

namespace Dice.Abstractions
{
    public interface IDiceEffect
    {
        string Id { get; }
        // 이번 획득량에 반응. 필요하면 ctx 참조.
        void OnScore(ref int chipsGain, ref int multGain, GameContext ctx);
        string GetDescription(Data.DiceRecord record);
    }
}