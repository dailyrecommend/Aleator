using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data
{
    // 외부 패키지 없이 간단 CSV 파서 (따옴표 최소 지원)
    public static class CsvReader
    {
        private const char SEP = ',';
        private const char QUOTE = '"';

        // 기존: 파일 경로 읽기
        public static List<string[]> ReadAll(string path)
        {
            using var sr = new StreamReader(path, Encoding.UTF8);
            string text = sr.ReadToEnd();
            return ReadAllFromString(text);
        }

        // 신규: 문자열 전체를 받아 파싱
        public static List<string[]> ReadAllFromString(string text)
        {
            var rows = new List<string[]>();
            if (string.IsNullOrEmpty(text))
                return rows;

            using var sr = new StringReader(text);
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length == 0)
                {
                    rows.Add(Array.Empty<string>());
                    continue;
                }
                rows.Add(Split(line));
            }

            return rows;
        }

        private static string[] Split(string line)
        {
            var list = new List<string>();
            var sb = new StringBuilder();
            bool inQuote = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == QUOTE)
                {
                    if (inQuote && i + 1 < line.Length && line[i + 1] == QUOTE)
                    {
                        sb.Append(QUOTE);
                        i++;
                    }
                    else
                    {
                        inQuote = !inQuote;
                    }
                    continue;
                }

                if (c == SEP && !inQuote)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                    continue;
                }

                sb.Append(c);
            }

            list.Add(sb.ToString());
            return list.ToArray();
        }
    }
}
