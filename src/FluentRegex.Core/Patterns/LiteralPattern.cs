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
        /// Precedence is 3 (atomic) only for single-atom expressions:
        /// a single character ("a") or a single escape sequence ("\d", "\.").
        /// Multi-character literals ("ab", "\+52") have precedence 1 (concatenation)
        /// so that quantifiers correctly wrap the whole literal.
        /// </summary>
        protected internal override int Precedence => Expression.Length switch
        {
            1 => 3,
            2 when Expression[0] == '\\' => 3,
            _ => 1
        };

        /// <summary>
        /// Creates a new literal pattern from the specified string.
        /// Special regex characters (e.g. ., *, +, ?, [, (, etc.) are automatically escaped.
        /// </summary>
        /// <param name="value">The literal text to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        public LiteralPattern(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Expression = EscapeLiteral(value);
        }

        /// <summary>
        /// Escapes only the true regex metacharacters.
        /// Unlike <see cref="Regex.Escape"/>, this does NOT
        /// escape spaces or <c>#</c> (which <c>Regex.Escape</c> does for
        /// <see cref="RegexOptions.IgnorePatternWhitespace"/> safety).
        /// </summary>
        internal static string EscapeLiteral(string value)
        {
            if (value.Length == 0)
                return value;

            var sb = new System.Text.StringBuilder(value.Length);

            foreach (var c in value)
            {
                if (IsRegexMetachar(c))
                    sb.Append('\\').Append(c);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns true for characters that have special meaning in regex.
        /// </summary>
        private static bool IsRegexMetachar(char c) => c is
            '\\' or '.' or '$' or '^' or '{' or '[' or '(' or
            '|' or ')' or '*' or '+' or '?';
    }

}
