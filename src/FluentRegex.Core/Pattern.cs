using FluentRegex.Core.Patterns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Creates a pattern that matches an exact literal string.
        /// Special regex characters are automatically escaped.
        /// </summary>
        /// <param name="value">The literal text to match.</param>
        public static LiteralPattern Literal(string value) => new(value);

        /// <summary>
        /// Creates a pattern that matches any whitespace character (equivalent to \s).
        /// </summary>
        public static WhitespacePattern Whitespace() => new();

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
        /// <summary>
        /// Creates a pattern that matches a single letter a-z or A-Z.
        /// </summary>
        public static LetterPattern Letter() => new();

        /// <summary>
        /// Creates a text pattern that matches alphabetic characters with configurable options.
        /// </summary>
        public static TextPattern Text() => new();

        /// <summary>
        /// Creates a pattern that matches integer numbers.
        /// </summary>
        public static IntegerPattern Integer() => new();
        /// <summary>
        /// Implicitly converts a Pattern to a <see cref="Regex"/> instance.
        /// </summary>
        public static implicit operator Regex(Pattern pattern) => new Regex(pattern.Expression);

        /// <summary>
        /// Implicitly converts a Pattern to its regex string representation.
        /// </summary>
        public static implicit operator string(Pattern pattern) => pattern.Expression;

        /// <summary>
        /// Indicates whether the specified input string matches the pattern.
        /// </summary>
        public bool IsMatch(string input) => Regex.IsMatch(input, Expression);

        /// <summary>
        /// Searches the input string for the first occurrence of the pattern.
        /// </summary>
        public Match Match(string input) => Regex.Match(input, Expression);

        /// <summary>
        /// Searches the input string for all occurrences of the pattern.
        /// </summary>
        public MatchCollection Matches(string input) => Regex.Matches(input, Expression);

        /// <summary>
        /// Replaces all occurrences of the pattern in the input string with the replacement string.
        /// </summary>
        public string Replace(string input, string replacement) => Regex.Replace(input, Expression, replacement);

        /// <summary>
        /// Splits the input string at each occurrence of the pattern.
        /// </summary>
        public string[] Split(string input) => [.. Regex.Split(input, Expression)];

        /// <summary>
        /// Returns the regex string representation of this pattern.
        /// </summary>
        public override string ToString() => Expression;
    }
}

