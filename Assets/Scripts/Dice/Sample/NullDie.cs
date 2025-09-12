using Core;
using Data;
using Dice.Abstractions;
using Dice.Attributes;

namespace Dice.Samples
{
    [DiceInfo("dice.none")]
    public sealed class NullDie : IDiceEffect
    {
        public string Id => "default_die";

        public void OnScore(ref int chipsGain, ref int multGain, GameContext ctx)
        {
            // no-op
        }

        public string GetDescription(DiceRecord record) => record.desc;
    }
}