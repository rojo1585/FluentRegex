namespace FluentRegex.Core.Presets
{
    /// <summary>
    /// A configurable password pattern. Use via <see cref="Presets.Password"/>.
    /// Immutable — each fluent method returns a new instance.
    /// </summary>
    public sealed class PasswordPattern : Pattern
    {
        public override string Expression { get; }
        protected internal override int Precedence => 1;

        private readonly int _minLength;
        private readonly int _maxLength;
        private readonly bool _requireUppercase;
        private readonly bool _requireLowercase;
        private readonly bool _requireDigit;
        private readonly bool _requireSpecial;
        private readonly char[]? _allowedSpecialChars;

        internal PasswordPattern(int minLength = 8,
                                 int? maxLength = null,
                                 bool requireUppercase = true,
                                 bool requireLowercase = true,
                                 bool requireDigit = true,
                                 bool requireSpecial = false,
                                 char[]? allowedSpecialChars = null)
        {
            _minLength = minLength;
            _maxLength = maxLength ?? 128;
            _requireUppercase = requireUppercase;
            _requireLowercase = requireLowercase;
            _requireDigit = requireDigit;
            _requireSpecial = requireSpecial;
            _allowedSpecialChars = allowedSpecialChars;

            var parts = new List<string> { "A-Z" };
            if (_requireLowercase) parts.Add("a-z");
            else if (!_requireUppercase) { parts.Add("a-z"); parts.Add("A-Z"); }
            if (_requireDigit) parts.Add("0-9");
            if (_requireSpecial)
            {
                var special = allowedSpecialChars ?? ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '='];
                foreach (var c in special) parts.Add(EscapeCharClass(c));
            }

            var charClass = string.Join("", parts);
            var quantifier = _maxLength == _minLength
                ? $"{{{_minLength}}}"
                : $"{{{_minLength},{_maxLength}}}";
            Expression = $"[{charClass}]{quantifier}";
        }

        /// <summary>
        /// Sets the minimum password length. Default is 8.
        /// </summary>
        public PasswordPattern MinLength(int min) => new(min, _maxLength, _requireUppercase, _requireLowercase, _requireDigit, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Sets the maximum password length. Default is 128.
        /// </summary>
        public PasswordPattern MaxLength(int max) => new(_minLength, max, _requireUppercase, _requireLowercase, _requireDigit, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Whether to require at least one uppercase letter. Default is true.
        /// </summary>
        public PasswordPattern RequireUppercase(bool require = true) => new(_minLength, _maxLength, require, _requireLowercase, _requireDigit, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Whether to require at least one lowercase letter. Default is true.
        /// </summary>
        public PasswordPattern RequireLowercase(bool require = true) => new(_minLength, _maxLength, _requireUppercase, require, _requireDigit, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Whether to require at least one digit. Default is true.
        /// </summary>
        public PasswordPattern RequireDigit(bool require = true) => new(_minLength, _maxLength, _requireUppercase, _requireLowercase, require, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Whether to require at least one special character. Default is false.
        /// </summary>
        public PasswordPattern RequireSpecial(bool require = true) => new(_minLength, _maxLength, _requireUppercase, _requireLowercase, _requireDigit, require, _allowedSpecialChars);

        /// <summary>
        /// Sets the set of allowed special characters. Default includes common symbols.
        /// Only used when <see cref="RequireSpecial"/> is enabled.
        /// </summary>
        public PasswordPattern AllowedSpecialChars(params char[] chars) => new(_minLength, _maxLength, _requireUppercase, _requireLowercase, _requireDigit, _requireSpecial, chars);

        private static string EscapeCharClass(char c) => c switch
        {
            ']' => @"\]",
            '\\' => @"\\\\",
            '^' => @"\^",
            '-' => @"\-",
            _ => c.ToString()
        };
    }
}
