using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches characters within a set or range, e.g. [abc] or [a-z] or [^abc].
    /// Created via <see cref="Pattern.Char"/>, <see cref="Pattern.Range"/>,
    /// <see cref="Pattern.NotChar"/>, or <see cref="Pattern.NotRange"/>.
    /// </summary>
    public sealed class CharSetPattern : Pattern
    {
        public override string Expression { get; }
        internal CharSetPattern(char[] chars, bool negated = false)
        {
            if (chars is { Length: 0 })
                throw new ArgumentException("Character set must contain at least one character.", nameof(chars));

            var escaped = string.Concat(chars.Select(EscapeCharClassChar));
            var prefix = negated ? "^" : "";
            Expression = $"[{prefix}{escaped}]";
        }
        internal CharSetPattern(char from, char to, bool negated = false)
        {
            if (from > to)
                throw new ArgumentException($"Range start '{from}' must not be greater than range end '{to}'.");

            var prefix = negated ? "^" : "";
            Expression = $"[{prefix}{EscapeCharClassChar(from)}-{EscapeCharClassChar(to)}]";
        }
        /// <summary>
        /// Escapes characters that have special meaning inside a character class [...].
        /// Inside character classes, only \, ], ^ (at start), and - (in middle) need escaping.
        /// </summary>
        private static string EscapeCharClassChar(char c) => c switch
        {
            '\\' => "\\\\",
            ']' => "\\]",
            '^' => "\\^",
            '-' => "\\-",
            _ => c.ToString()
        };
    }
}
