namespace FluentRegex.Core.Presets
{
    /// <summary>
    /// A configurable username pattern. Use via <see cref="Presets.Username"/>.
    /// Immutable — each fluent method returns a new instance.
    /// </summary>
    public sealed class UsernamePattern : Pattern
    {
        public override string Expression { get; }
      protected internal override int Precedence => 1;

        private readonly int _minLength;
        private readonly int _maxLength;
        private readonly char[] _extraChars;

        internal UsernamePattern(int minLength = 3, int maxLength = 30, char[]? extraChars = null)
        {
            if (minLength < 1)
                throw new ArgumentOutOfRangeException(nameof(minLength), minLength,
                    "Minimum length must be at least 1.");
            if (maxLength < 1)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength,
                    "Maximum length must be at least 1.");
            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength,
                    $"Maximum length ({maxLength}) must be greater than or equal to minimum length ({minLength}).");

            _minLength = minLength;
            _maxLength = maxLength;
            _extraChars = extraChars ?? [];

            var chars = "a-zA-Z0-9_";
            if (_extraChars.Length > 0)
                chars += string.Concat(_extraChars.Select(EscapeCharClassChar));

            var firstChar = "[a-zA-Z_]";
            var restChar = $"[{chars}]";
            var quantifier = _maxLength == _minLength
                ? $"{{{_minLength - 1}}}"
                : $"{{{_minLength - 1},{_maxLength - 1}}}";
            Expression = $"{firstChar}{restChar}{quantifier}";
        }

        /// <summary>
        /// Sets the minimum username length. Default is 3.
        /// </summary>
        public UsernamePattern MinLength(int min) => new(min, _maxLength, _extraChars);

        /// <summary>
        /// Sets the maximum username length. Default is 30.
        /// </summary>
        public UsernamePattern MaxLength(int max) => new(_minLength, max, _extraChars);

        /// <summary>
        /// Allows additional characters beyond the default alphanumeric + underscore.
        /// <para>Special characters like <c>]</c>, <c>\</c>, <c>^</c>, <c>-</c> are automatically escaped.</para>
        /// <para>Can be called multiple times — characters accumulate.</para>
        /// </summary>
        public UsernamePattern AllowChars(params char[] chars) => new(_minLength, _maxLength, [.. _extraChars, .. chars]);
    }
}
