using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches alphabetic text with configurable options.
    /// By default matches one or more letters (a-z, A-Z).
    /// Can be extended to include digits and special characters.
    /// </summary>
    public sealed class TextPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; }

        /// <summary>
        /// TextPattern expressions include a quantifier (e.g. "[a-zA-Z]+").
        /// Precedence 1 (concatenation) ensures correct grouping when composed.
        /// </summary>
        internal override int Precedence => 1;

        private readonly int? _minLength;
        private readonly int? _maxLength;
        private readonly bool _allowDigits;
        private readonly char[] _extraChars;

        /// <summary>
        /// Creates a new TextPattern with the specified configuration.
        /// </summary>
        internal TextPattern(
            int? minLength = null,
            int? maxLength = null,
            bool allowDigits = false,
            char[]? extraChars = null)
        {
            if (minLength.HasValue && minLength.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(minLength), "MinLength must be non-negative.");
            if (maxLength.HasValue && maxLength.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), "MaxLength must be non-negative.");
            if (minLength.HasValue && maxLength.HasValue && maxLength.Value < minLength.Value)
                throw new ArgumentException("MaxLength must be greater than or equal to MinLength.");

            _minLength = minLength;
            _maxLength = maxLength;
            _allowDigits = allowDigits;
            _extraChars = extraChars ?? [];

            Expression = BuildExpression();
        }

        /// <summary>
        /// Sets the minimum length of the matched text.
        /// </summary>
        public TextPattern MinLength(int min) =>
            new(min, _maxLength, _allowDigits, _extraChars);

        /// <summary>
        /// Sets the maximum length of the matched text.
        /// </summary>
        public TextPattern MaxLength(int max) =>
            new(_minLength, max, _allowDigits, _extraChars);

        /// <summary>
        /// Allows digits (0-9) in addition to letters.
        /// </summary>
        public TextPattern AllowDigits() =>
            new(_minLength, _maxLength, true, _extraChars);

        /// <summary>
        /// Allows additional specific characters in addition to letters.
        /// </summary>
        /// <param name="chars">Characters to allow.</param>
        public TextPattern AllowChars(params char[] chars) =>
            new(_minLength, _maxLength, _allowDigits, [.. _extraChars, .. chars]);

        private string BuildExpression()
        {
            var charClass = BuildCharClass();

            return (_minLength, _maxLength) switch
            {
                (null, null) => $"{charClass}+",
                (int min, null) => $"{charClass}{{{min},}}",
                (null, int max) => $"{charClass}{{0,{max}}}",
                (int min, int max) when min == max => $"{charClass}{{{min}}}",
                (int min, int max) => $"{charClass}{{{min},{max}}}"
            };
        }

        private string BuildCharClass()
        {
            var parts = new List<string> { "a-zA-Z" };

            if (_allowDigits)
                parts.Add("0-9");

            if (_extraChars.Length > 0)
                parts.Add(string.Concat(_extraChars.Select(EscapeForCharClass)));

            return "[" + string.Join("", parts) + "]";
        }

        private static string EscapeForCharClass(char c) => c switch
        {
            '\\' => "\\\\",
            ']' => "\\]",
            '^' => "\\^",
            '-' => "\\-",
            _ => c.ToString()
        };
    }
}