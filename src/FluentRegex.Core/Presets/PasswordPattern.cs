namespace FluentRegex.Core.Presets
{

    /// <summary>
    /// A configurable password pattern. Use via <see cref="Presets.Password"/>.
    /// Immutable — each fluent method returns a new instance.
    /// <para>
    /// The <c>Allow*</c> methods control which character categories are <b>permitted</b>
    /// in the password's character class. They do <b>not</b> enforce that at least one
    /// character from each category is present.
    /// </para>
    /// <para>
    /// Default: allows uppercase, lowercase, and digits (8–128 chars).
    /// Special characters are not allowed by default.
    /// </para>
    /// </summary>
    public sealed class PasswordPattern : Pattern
    {
        public override string Expression { get; }
       protected internal override int Precedence => 1;

        private readonly int _minLength;
        private readonly int _maxLength;
        private readonly bool _allowUppercase;
        private readonly bool _allowLowercase;
        private readonly bool _allowDigits;
        private readonly bool _allowSpecial;
        private readonly char[]? _specialChars;

        private static readonly char[] DefaultSpecialChars =
            ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '='];

        internal PasswordPattern(
            int minLength = 8,
            int? maxLength = null,
            bool allowUppercase = true,
            bool allowLowercase = true,
            bool allowDigits = true,
            bool allowSpecial = false,
            char[]? specialChars = null)
        {
            _minLength = minLength;
            _maxLength = maxLength ?? 128;
            _allowUppercase = allowUppercase;
            _allowLowercase = allowLowercase;
            _allowDigits = allowDigits;
            _allowSpecial = allowSpecial;
            _specialChars = specialChars;

            Expression = BuildExpression();
        }

        /// <summary>
        /// Sets the minimum password length. Default is 8.
        /// </summary>
        public PasswordPattern MinLength(int min) => new(min, _maxLength, _allowUppercase, _allowLowercase, _allowDigits, _allowSpecial, _specialChars);

        /// <summary>
        /// Sets the maximum password length. Default is 128.
        /// </summary>
        public PasswordPattern MaxLength(int max) => new(_minLength, max, _allowUppercase, _allowLowercase, _allowDigits, _allowSpecial, _specialChars);

        /// <summary>
        /// Allows uppercase letters (A-Z) in the password. Default is true.
        /// <para>Set to <c>false</c> to exclude uppercase from the allowed character set.</para>
        /// </summary>
        public PasswordPattern AllowUppercase(bool allow = true) => new(_minLength, _maxLength, allow, _allowLowercase, _allowDigits, _allowSpecial, _specialChars);

        /// <summary>
        /// Allows lowercase letters (a-z) in the password. Default is true.
        /// <para>Set to <c>false</c> to exclude lowercase from the allowed character set.</para>
        /// </summary>
        public PasswordPattern AllowLowercase(bool allow = true) => new(_minLength, _maxLength, _allowUppercase, allow, _allowDigits, _allowSpecial, _specialChars);

        /// <summary>
        /// Allows digits (0-9) in the password. Default is true.
        /// <para>Set to <c>false</c> to exclude digits from the allowed character set.</para>
        /// </summary>
        public PasswordPattern AllowDigits(bool allow = true) => new(_minLength, _maxLength, _allowUppercase, _allowLowercase, allow, _allowSpecial, _specialChars);

        /// <summary>
        /// Allows special characters in the password. Default is false.
        /// <para>When enabled, uses a default set of common symbols unless overridden via
        /// <see cref="WithSpecialChars"/>.</para>
        /// </summary>
        public PasswordPattern AllowSpecial(bool allow = true) => new(_minLength, _maxLength, _allowUppercase, _allowLowercase, _allowDigits, allow, _specialChars);

        /// <summary>
        /// Sets the exact set of allowed special characters and enables special character support.
        /// <para>Replaces the default special character set. Equivalent to calling
        /// <see cref="AllowSpecial"/>(<c>true</c>) with a custom character list.</para>
        /// </summary>
        public PasswordPattern WithSpecialChars(params char[] chars) => new(_minLength, _maxLength, _allowUppercase, _allowLowercase, _allowDigits, true, chars);

        private string BuildExpression()
        {
            var allowedParts = new List<string>();

            if (_allowUppercase) allowedParts.Add("A-Z");
            if (_allowLowercase) allowedParts.Add("a-z");
            if (_allowDigits) allowedParts.Add("0-9");

            if (_allowSpecial)
            {
                var special = _specialChars ?? DefaultSpecialChars;
                foreach (var c in special) allowedParts.Add(EscapeCharClass(c));
            }

            var charClass = string.Join("", allowedParts);
            var quantifier = _maxLength == _minLength
                ? $"{{{_minLength}}}"
                : $"{{{_minLength},{_maxLength}}}";

            return $"[{charClass}]{quantifier}";
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