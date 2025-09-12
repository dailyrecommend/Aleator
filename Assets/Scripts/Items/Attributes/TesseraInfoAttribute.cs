using System;

namespace Items.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TesseraInfoAttribute : Attribute
    {
        public readonly string effectId;

        public TesseraInfoAttribute(string effectId)
        {
            this.effectId = effectId;
        }
    }
}