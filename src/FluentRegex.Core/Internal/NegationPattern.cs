using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Internal
{
    /// <summary>
    /// Internal pattern that negates another pattern using a negative lookahead.
    /// Useful for assertions like "not followed by".
    /// </summary>
    internal sealed class NegationPattern : Pattern
    {
        public override string Expression { get; }

        internal NegationPattern(Pattern inner)
        {
            Expression = $"(?!{inner.Expression})";
        }
    }
}
