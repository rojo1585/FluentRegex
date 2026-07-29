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
        private readonly char[]? _extraChars;

        internal UsernamePattern(int minLength = 3, int maxLength = 30, char[]? extraChars = null)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            _extraChars = extraChars;

            var chars = "a-zA-Z0-9_";
            if (extraChars is { Length: > 0 })
                chars += new string(extraChars);

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
        /// </summary>
        public UsernamePattern AllowChars(params char[] chars) => new(_minLength, _maxLength, chars);
    }
}
