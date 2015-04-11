using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace STPS.Framework
{
    public static class StringHelper
    {
        public static bool IsNullOrWhiteSpace(string value)
        {
            return value == null || string.IsNullOrEmpty(value.Trim());
        }

        public static string ToFriendlyName(string value)
        {
            if (value == null) return string.Empty;
            if (value.Trim().Length == 0) return string.Empty;

            string result = value;

            result = string.Concat(result.Substring(0, 1).ToUpperInvariant(), result.Substring(1, result.Length - 1));

            const string pattern = @"([A-Z]+(?![a-z])|\d+|[A-Z][a-z]+|(?![A-Z])[a-z]+)+";

            List<string> words = new List<string>();
            Match match = Regex.Match(result, pattern);
            if (match.Success)
            {
                Group group = match.Groups[1];
                foreach (Capture capture in group.Captures)
                {
                    words.Add(capture.Value);
                }
            }

            return string.Join(" ", words.ToArray());
        }
    }
}
