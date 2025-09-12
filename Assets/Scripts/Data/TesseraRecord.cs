using System.Collections.Generic;

namespace Data
{
    public sealed class TesseraRecord
    {
        public string id;
        public string name;
        public string rarity;
        public string desc;
        public string effectId;
        public Dictionary<string, string> @params;
    }
}