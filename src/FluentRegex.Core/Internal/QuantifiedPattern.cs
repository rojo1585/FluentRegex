using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Internal
{

    /// <summary>
    /// Internal pattern that applies a quantifier to another pattern.
    /// Created by Any(), OneOrMore(), Optional(), Repeat(), and AtLeast() methods.
    /// </summary>
    internal sealed class QuantifiedPattern : Pattern
    {
        public override string Expression { get; }

        internal override int Precedence => 2;

        internal QuantifiedPattern(Pattern inner, string quantifier)
        {
            Expression = $"{inner.WrapIfBelow(2)}{quantifier}";
        }

        /// <summary>
        /// Determines if a pattern expression needs to be wrapped in a group
        /// before applying a quantifier.
        /// </summary>
        private static bool NeedsGrouping(string expr)
        {
            // Single character expressions and simple char classes don't need grouping
            if (expr.Length <= 1)
                return false;

            // Character classes [abc], [a-z], [^abc] don't need grouping
            if (expr.StartsWith('[') && expr.EndsWith(']'))
                return false;

            // Shorthand classes (\d, \w, \s, etc.) don't need grouping
            if (expr.Length == 2 && expr[0] == '\\')
                return false;

            // Already grouped expressions don't need grouping
            if (expr.StartsWith("(?:") || expr.StartsWith('('))
                return false;

            return true;
        }
    }
}
