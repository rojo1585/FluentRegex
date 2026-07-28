using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// Wraps a pattern in a named capturing group (?&lt;name&gt;...).
    /// Named groups are atomic (precedence 3) since they're already self-contained.
    /// </summary>
    public sealed class NamedGroupPattern : Pattern
    {
        private static readonly Regex ValidGroupName = new("^[A-Za-z_][A-Za-z0-9_]*$",
        RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// The name of the capturing group.
        /// </summary>
        public string Name { get; }

       protected internal override int Precedence => 3;

        internal NamedGroupPattern(string name, Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(name);

            if (name.Length == 0 || !ValidGroupName.IsMatch(name))
                throw new ArgumentException(@"Group name must start with a letter or underscore, followed by letters, digits, or underscores. Examples: 'year', '_temp', 'group1'.", nameof(name));

            Name = name;
            Expression = $"(?<{name}>{inner.Expression})";
        }
    }
}
