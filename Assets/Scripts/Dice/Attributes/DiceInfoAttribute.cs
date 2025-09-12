using System;

namespace Dice.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DiceInfoAttribute : Attribute
    {
        public readonly string effectId;
        public DiceInfoAttribute(string effectId) { this.effectId = effectId; }
    }
}