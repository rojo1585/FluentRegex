namespace FluentRegex.Core.Patterns
{

    /// <summary>
    /// A pattern that matches integer numbers.
    /// By default matches one or more digits (0-9). Optionally allows a sign (+ or -).
    /// </summary>
    public sealed class IntegerPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// IntegerPattern expressions include quantifiers (e.g. "\d+", "-?\d+").
        /// Precedence 1 (concatenation) ensures correct grouping when composed.
        /// </summary>
       protected internal override int Precedence => 1;

        private readonly bool _allowNegative;
        private readonly bool _allowSign;
        private readonly int? _minDigits;
        private readonly int? _maxDigits;

        /// <summary>
        /// Creates a new IntegerPattern with the specified configuration.
        /// </summary>
        internal IntegerPattern(
            bool allowNegative = false,
            bool allowSign = false,
            int? minDigits = null,
            int? maxDigits = null)
        {
            if (minDigits.HasValue && minDigits.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(minDigits), "MinDigits must be non-negative.");
            if (maxDigits.HasValue && maxDigits.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(maxDigits), "MaxDigits must be non-negative.");
            if (minDigits.HasValue && maxDigits.HasValue && maxDigits.Value < minDigits.Value)
                throw new ArgumentException("MaxDigits must be greater than or equal to MinDigits.");

            _allowNegative = allowNegative;
            _allowSign = allowSign;
            _minDigits = minDigits;
            _maxDigits = maxDigits;

            Expression = BuildExpression();
        }

        /// <summary>
        /// Allows an optional negative sign (-) before the number.
        /// </summary>
        public IntegerPattern AllowNegative() =>
            new(true, false, _minDigits, _maxDigits);

        /// <summary>
        /// Allows both positive (+) and negative (-) signs before the number.
        /// </summary>
        public IntegerPattern AllowSign() =>
            new(false, true, _minDigits, _maxDigits);

        /// <summary>
        /// Sets the minimum number of digits.
        /// </summary>
        public IntegerPattern MinDigits(int min) =>
            new(_allowNegative, _allowSign, min, _maxDigits);

        /// <summary>
        /// Sets the maximum number of digits.
        /// </summary>
        public IntegerPattern MaxDigits(int max) =>
            new(_allowNegative, _allowSign, _minDigits, max);

        private string BuildExpression()
        {
            var sign = _allowSign ? "[+-]?" : _allowNegative ? "-?" : "";
            var digits = (_minDigits, _maxDigits) switch
            {
                (null, null) => @"\d+",
                (int min, null) => $@"\d{{{min},}}",
                (null, int max) => $@"\d{{1,{max}}}",
                (int min, int max) when min == max => $@"\d{{{min}}}",
                (int min, int max) => $@"\d{{{min},{max}}}"
            };

            return sign + digits;
        }
    }
}
