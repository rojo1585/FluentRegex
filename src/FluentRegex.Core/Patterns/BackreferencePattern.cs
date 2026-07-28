using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A backreference that matches the same text previously captured by a group.
    /// Supports both named (<c>\k&lt;name&gt;</c>) and numbered (<c>\1</c>) backreferences.
    /// Backreferences are atomic (precedence 3) and consume characters.
    /// </summary>
    public sealed class BackreferencePattern : Pattern
    {
        private static readonly Regex ValidGroupName = new("^[A-Za-z_][A-Za-z0-9_]*$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// The group name if this is a named backreference, or <c>null</c> if numbered.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// The group number if this is a numbered backreference, or <c>null</c> if named.
        /// </summary>
        public int? Number { get; }

        internal override int Precedence => 3;

        internal BackreferencePattern(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(@"Group name cannot be empty.", nameof(name));

            if (!ValidGroupName.IsMatch(name))
                throw new ArgumentException(@"Group name must start with a letter or underscore, followed by letters, digits, or underscores.", nameof(name));

            Name = name;
            Expression = $@"\k<{name}>";
        }

        internal BackreferencePattern(int number)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(nameof(number), "Group number must be a positive integer (1-based).");

            Number = number;
            Expression = $@"\{number}";
        }
    }
}
