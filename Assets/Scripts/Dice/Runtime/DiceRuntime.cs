using Data;

namespace Dice.Runtime
{
    public sealed class DiceRuntime
    {
        public readonly DiceRecord record;
        public DiceRuntime(DiceRecord record) { this.record = record; }
    }
}