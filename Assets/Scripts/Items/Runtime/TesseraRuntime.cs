using System.Collections.Generic;
using Data;

namespace Items.Runtime
{
    public sealed class TesseraRuntime
    {
        public readonly TesseraRecord record;
        public readonly Dictionary<string, string> param;

        public TesseraRuntime(TesseraRecord record)
        {
            this.record = record;
            param = record.@params ?? new Dictionary<string, string>();
        }

        public int GetInt(string key, int defVal = 0)
        {
            return param.TryGetValue(key, out var s) && int.TryParse(s, out var v) ? v : defVal;
        }

        public string GetString(string key, string defVal = "")
        {
            return param.TryGetValue(key, out var s) ? s : defVal;
        }
    }
}