namespace FluentRegex.Core.Presets
{/// <summary>
 /// A configurable password pattern. Use via <see cref="Presets.Password"/>.
 /// Immutable — each fluent method returns a new instance.
 /// <para>
 /// When a category is required (e.g. <see cref="RequireUppercase"/>), the generated
 /// regex uses a lookahead to enforce at least one character from that category.
 /// The character class only includes categories that are required —
 /// setting a category to <c>false</c> removes it from the allowed set entirely.
 /// </para>
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

        internal PasswordPattern(
            int minLength = 8,
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

            Expression = BuildExpression();
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
        /// Requires at least one uppercase letter (A-Z). Default is true.
        /// <para>Uses a lookahead assertion — the password must contain at least one uppercase letter.</para>
        /// </summary>
        public PasswordPattern RequireUppercase(bool require = true) => new(_minLength, _maxLength, require, _requireLowercase, _requireDigit, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Requires at least one lowercase letter (a-z). Default is true.
        /// <para>Uses a lookahead assertion — the password must contain at least one lowercase letter.</para>
        /// </summary>
        public PasswordPattern RequireLowercase(bool require = true) => new(_minLength, _maxLength, _requireUppercase, require, _requireDigit, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Requires at least one digit (0-9). Default is true.
        /// <para>Uses a lookahead assertion — the password must contain at least one digit.</para>
        /// </summary>
        public PasswordPattern RequireDigit(bool require = true) => new(_minLength, _maxLength, _requireUppercase, _requireLowercase, require, _requireSpecial, _allowedSpecialChars);

        /// <summary>
        /// Requires at least one special character. Default is false.
        /// <para>Uses a lookahead assertion — the password must contain at least one special character.</para>
        /// </summary>
        public PasswordPattern RequireSpecial(bool require = true) => new(_minLength, _maxLength, _requireUppercase, _requireLowercase, _requireDigit, require, _allowedSpecialChars);

        /// <summary>
        /// Sets the set of allowed special characters. Default includes common symbols.
        /// Only used when <see cref="RequireSpecial"/> is enabled.
        /// </summary>
        public PasswordPattern AllowedSpecialChars(params char[] chars) => new(_minLength, _maxLength, _requireUppercase, _requireLowercase, _requireDigit, _requireSpecial, chars);

        private string BuildExpression()
        {
            var allowedParts = new List<string>();
            var lookaheads = new List<string>();

            allowedParts.Add("A-Z");
            if (_requireUppercase)
                lookaheads.Add("(?=.*[A-Z])");

            if (_requireLowercase)
            {
                allowedParts.Add("a-z");
                lookaheads.Add("(?=.*[a-z])");
            }
            else if (!_requireUppercase)
            {
                allowedParts.Add("a-z");
            }

            if (_requireDigit)
            {
                allowedParts.Add("0-9");
                lookaheads.Add(@"(?=.*\d)");
            }

            if (_requireSpecial)
            {
                var special = _allowedSpecialChars ?? ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '='];
                var escapedSpecial = special.Select(EscapeCharClass).ToArray();
                foreach (var s in escapedSpecial) allowedParts.Add(s);
                lookaheads.Add($"(?=.*[{string.Join("", escapedSpecial)}])");
            }

            var charClass = string.Join("", allowedParts);
            var quantifier = _maxLength == _minLength
                ? $"{{{_minLength}}}"
                : $"{{{_minLength},{_maxLength}}}";

            var prefix = lookaheads.Count > 0 ? string.Join("", lookaheads) : "";
            return $"{prefix}[{charClass}]{quantifier}";
        }

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