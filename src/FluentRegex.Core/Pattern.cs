using FluentRegex.Core.Patterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core
{
    /// <summary>/// Base abstract class for all regex patterns
    /// Provides fluent API, operators, quantifiers, and direct usage methods
    /// </summary>
    public abstract class Pattern
    {
        /// <summary>
        /// Gets the regular expression string represented by this pattern.
        /// </summary>
        public abstract string Expression { get; }
        /// <summary>
        /// Creates a pattern that matches any single character (equivalent to .).
        /// </summary>
        public static AnyCharPattern AnyChar() => new();

        /// <summary>
        /// Creates a pattern that matches any character in the specified set.
        /// </summary>
        /// <param name="chars">The characters to match.</param>
        public static CharSetPattern Char(params char[] chars) => new(chars);

        /// <summary>
        /// Creates a pattern that matches any character in the specified range.
        /// </summary>
        /// <param name="from">Start of the range (inclusive).</param>
        /// <param name="to">End of the range (inclusive).</param>
        public static CharSetPattern Range(char from, char to) => new(from, to);

        /// <summary>
        /// Creates a pattern that matches any character NOT in the specified set.
        /// </summary>
        /// <param name="chars">The characters to exclude.</param>
        public static CharSetPattern NotChar(params char[] chars) => new(chars, negated: true);

        /// <summary>
        /// Creates a pattern that matches any character NOT in the specified range.
        /// </summary>
        /// <param name="from">Start of the range (inclusive).</param>
        /// <param name="to">End of the range (inclusive).</param>
        public static CharSetPattern NotRange(char from, char to) => new(from, to, negated: true);
        /// <summary>
        /// Creates a pattern that matches a single digit (equivalent to \d).
        /// </summary>
        public static DigitPattern Digit() => new();
    }
}
