using FluentRegex.Core.Literals;
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
        private static readonly string[] Syntax = ["?=", "?!", "?<=", "?<!"];

        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// The kind of lookaround this pattern represents.
        /// </summary>
        internal LookaroundKind Kind { get; }

        internal override int Precedence => 3;

        internal override bool IsZeroWidth => true;

        internal LookaroundPattern(LookaroundKind kind, Pattern inner)
        {
            Kind = kind;
            Expression = $"({Syntax[(int)kind]}{inner.Expression})";
        }
    }

}
