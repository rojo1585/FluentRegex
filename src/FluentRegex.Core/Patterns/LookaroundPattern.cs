using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A lookaround assertion pattern (lookahead or lookbehind, positive or negative).
    /// Lookarounds are atomic (precedence 3) — they're self-contained assertions.
    /// Created via <see cref="Pattern.LookAhead"/>, <see cref="Pattern.LookAheadNot"/>,
    /// <see cref="Pattern.LookBehind"/>, <see cref="Pattern.LookBehindNot"/>.
    /// </summary>
    public sealed class LookaroundPattern : Pattern
    {
        public override string Expression { get; }
        internal override int Precedence => 3;

        internal LookaroundPattern(string lookaroundType, Pattern inner)
        {
            Expression = $"({lookaroundType}{inner.Expression})";
        }
    }

}
