using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Internal
{
    /// <summary>
    /// Internal pattern that represents alternation (OR) between two patterns.
    /// Created by the | operator.
    /// </summary>
    internal sealed class AlternationPattern : Pattern
    {
        public override string Expression { get; }

        internal AlternationPattern(Pattern left, Pattern right)
        {
            Expression = $"{WrapIfNeeded(left)}|{WrapIfNeeded(right)}";
        }

        /// <summary>
        /// Wraps a pattern in a non-capturing group if it contains an alternation operator.
        /// This ensures correct operator precedence.
        /// </summary>
        private static string WrapIfNeeded(Pattern pattern)
        {
            var expr = pattern.Expression;
            if (ContainsTopLevelAlternation(expr))
                return $"(?:{expr})";
            return expr;
        }

        /// <summary>
        /// Checks if the expression contains a top-level alternation operator.
        /// A simple heuristic: if | appears outside of brackets and groups.
        /// </summary>
        private static bool ContainsTopLevelAlternation(string expr)
        {
            var depth = 0;
            var inCharClass = false;

            for (var i = 0; i < expr.Length; i++)
            {
                var c = expr[i];

                if (c == '\\' && i + 1 < expr.Length)
                {
                    i++;
                    continue;
                }

                if (inCharClass)
                {
                    if (c == ']')
                        inCharClass = false;
                    continue;
                }

                if (c == '[')
                {
                    inCharClass = true;
                    continue;
                }

                if (c is '(' or '[' or '{')
                {
                    depth++;
                    continue;
                }

                if (c is ')' or ']' or '}')
                {
                    depth--;
                    continue;
                }

                if (c == '|' && depth == 0)
                    return true;
            }

            return false;
        }
    }
}
