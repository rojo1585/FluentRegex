using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{

    /// <summary>
    /// Wraps a pattern in a non-capturing group (?:...).
    /// Groups are atomic (precedence 3) since they're already self-contained.
    /// </summary>
    public sealed class GroupPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }

        internal override int Precedence => 3;

        internal GroupPattern(Pattern inner)
        {
            Expression = $"(?:{inner.Expression})";
        }
    }
}
