using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// Wraps a pattern in a named capturing group (?&lt;name&gt;...).
    /// Named groups are atomic (precedence 3) since they're already self-contained.
    /// </summary>
    public sealed class NamedGroupPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// The name of the capturing group.
        /// </summary>
        public string Name { get; }

        internal override int Precedence => 3;

        internal NamedGroupPattern(string name, Pattern inner)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Group name cannot be empty.", nameof(name));

            Name = name;
            Expression = $"(?<{name}>{inner.Expression})";
        }
    }
}
