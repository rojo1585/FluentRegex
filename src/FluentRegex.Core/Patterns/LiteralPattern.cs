using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches an exact literal string.
    /// All special regex characters are automatically escaped.
    /// </summary>
    public sealed class LiteralPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// Creates a new literal pattern from the specified string.
        /// Special regex characters (e.g. ., *, +, ?, [, (, etc.) are automatically escaped.
        /// </summary>
        /// <param name="value">The literal text to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        public LiteralPattern(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Expression = Regex.Escape(value);
        }
    }
}
