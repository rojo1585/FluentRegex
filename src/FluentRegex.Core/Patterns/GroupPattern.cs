using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{

    /// <summary>
    /// Wraps a pattern in a non-capturing group (?:...).
    /// Use this for grouping by precedence without capturing.
    /// For capturing, use <see cref="NamedGroup"/> instead.
    /// </summary>
    public sealed class GroupPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }

        protected internal override int Precedence => 3;

        internal GroupPattern(Pattern inner)
        {
            Expression = $"(?:{inner.Expression})";
        }
    }
}
