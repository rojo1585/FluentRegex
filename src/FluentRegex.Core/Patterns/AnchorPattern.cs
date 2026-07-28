using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A zero-width assertion pattern (anchor) such as ^, $, \b, \B.
    /// Anchors are atomic (precedence 3) — they never need grouping.
    /// Created via <see cref="Pattern.Start"/>, <see cref="Pattern.End"/>,
    /// <see cref="Pattern.WordBoundary"/>, <see cref="Pattern.NotWordBoundary"/>.
    /// </summary>
    public sealed class AnchorPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }
        internal override int Precedence => 3;
        internal override bool IsZeroWidth => true;
        internal AnchorPattern(string expression)
        {
            Expression = expression;
        }
    }
}
