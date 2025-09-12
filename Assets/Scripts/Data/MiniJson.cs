using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data
{
    // Unity 내장에 의존 안 하는 초경량 JSON(객체/배열/문자열/숫자/true/false/null)
    public static class MiniJson
    {
        public static object Deserialize(string json) => new Parser(json).ParseValue();

        private sealed class Parser
        {
            private readonly StringReader _r;
            public Parser(string json) { _r = new StringReader(json); }

            public object ParseValue()
            {
                EatWhitespace();
                var c = Peek;
                if (c == '"') return ParseString();
                if (c == '{') return ParseObject();
                if (c == '[') return ParseArray();
                if (char.IsDigit(c) || c == '-') return ParseNumber();
                var word = ParseWord();
                return word switch { "true" => true, "false" => false, "null" => null, _ => word };
            }

            private Dictionary<string, object> ParseObject()
            {
                var dict = new Dictionary<string, object>();
                _r.Read(); // {
                while (true)
                {
                    EatWhitespace();
                    if (Peek == '}') { _r.Read(); break; }
                    var key = ParseString();
                    EatWhitespace(); _r.Read(); // :
                    var val = ParseValue();
                    dict[key] = val;
                    EatWhitespace();
                    if (Peek == ',') { _r.Read(); continue; }
                    if (Peek == '}') { _r.Read(); break; }
                }
                return dict;
            }

            private List<object> ParseArray()
            {
                var list = new List<object>();
                _r.Read(); // [
                while (true)
                {
                    EatWhitespace();
                    if (Peek == ']') { _r.Read(); break; }
                    list.Add(ParseValue());
                    EatWhitespace();
                    if (Peek == ',') { _r.Read(); continue; }
                    if (Peek == ']') { _r.Read(); break; }
                }
                return list;
            }

            private string ParseString()
            {
                var sb = new StringBuilder();
                _r.Read(); // "
                while (_r.Peek() != -1)
                {
                    var c = (char)_r.Read();
                    if (c == '"') break;
                    if (c == '\\')
                    {
                        c = (char)_r.Read();
                        sb.Append(c switch
                        {
                            '"' => '"', '\\' => '\\', '/' => '/',
                            'b' => '\b', 'f' => '\f', 'n' => '\n', 'r' => '\r', 't' => '\t',
                            'u' => (char)int.Parse(new string(new[] { (char)_r.Read(), (char)_r.Read(), (char)_r.Read(), (char)_r.Read() }), System.Globalization.NumberStyles.HexNumber),
                            _ => c
                        });
                    }
                    else sb.Append(c);
                }
                return sb.ToString();
            }

            private object ParseNumber()
            {
                var s = ParseWord();
                if (s.Contains(".") || s.Contains("e") || s.Contains("E")) return double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
                return long.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            }

            private string ParseWord()
            {
                var sb = new StringBuilder();
                while (_r.Peek() != -1)
                {
                    var c = (char)_r.Peek();
                    if (char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) >= 0) break;
                    sb.Append(c); _r.Read();
                }
                return sb.ToString();
            }

            private void EatWhitespace() { while (_r.Peek() != -1 && char.IsWhiteSpace((char)_r.Peek())) _r.Read(); }
            private char Peek => _r.Peek() == -1 ? '\0' : (char)_r.Peek();
        }
    }
}