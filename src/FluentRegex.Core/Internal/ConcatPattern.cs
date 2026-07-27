using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Internal
{
    /// <summary>
    /// Internal pattern that represents the concatenation of two patterns (side by side).
    /// Created by the + operator.
    /// </summary>
    internal sealed class ConcatPattern : Pattern
    {
        public override string Expression { get; }

        internal ConcatPattern(Pattern left, Pattern right)
        {
            Expression = $"{left.Expression}{right.Expression}";
        }
    }
}
