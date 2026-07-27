using FluentRegex.Core.Internal;
using FluentRegex.Core.Patterns;
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
        /// Gets the operator precedence of this pattern.
        /// Higher values bind tighter. Used internally to determine when
        /// non-capturing groups (?:...) are needed for correct semantics.
        /// <para>
        /// 0 = Alternation (lowest) | 1 = Concatenation |
        /// 2 = Quantified | 3 = Atomic — literal, char class, etc. (highest)
        /// </para>
        /// </summary>
        internal virtual int Precedence => 3;

        /// <summary>
        /// Indicates whether this pattern is a zero-width assertion (anchor, lookaround).
        /// Zero-width patterns do not consume input characters and should not be quantified.
        /// </summary>
        internal virtual bool IsZeroWidth => false;

        /// <summary>
        /// Returns the expression wrapped in a non-capturing group
        /// if this pattern's precedence is below <paramref name="minPrecedence"/>.
        /// </summary>
        internal string WrapIfBelow(int minPrecedence) =>
            Precedence < minPrecedence ? $"(?:{Expression})" : Expression;
        /// <summary>
        /// Matches zero or more occurrences (equivalent to * in regex).
        /// </summary>
        public Pattern Any() => new QuantifiedPattern(this, "*");

        /// <summary>
        /// Matches one or more occurrences (equivalent to + in regex).
        /// </summary>
        public Pattern OneOrMore() => new QuantifiedPattern(this, "+");

        /// <summary>
        /// Matches zero or one occurrence (equivalent to ? in regex).
        /// </summary>
        public Pattern Optional() => new QuantifiedPattern(this, "?");

        /// <summary>
        /// Matches exactly <paramref name="count"/> occurrences (equivalent to {n} in regex).
        /// </summary>
        /// <param name="count">Exact number of occurrences to match.</param>
        public Pattern Repeat(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative.");
            return new QuantifiedPattern(this, $"{{{count}}}");
        }

        /// <summary>
        /// Matches between <paramref name="min"/> and <paramref name="max"/> occurrences
        /// (equivalent to {min,max} in regex).
        /// </summary>
        public Pattern Repeat(int min, int max)
        {
            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min), "Min must be non-negative.");
            if (max < min)
                throw new ArgumentOutOfRangeException(nameof(max), "Max must be greater than or equal to min.");
            return new QuantifiedPattern(this, $"{{{min},{max}}}");
        }

        /// <summary>
        /// Matches <paramref name="min"/> or more occurrences
        /// (equivalent to {min,} in regex).
        /// </summary>
        public Pattern AtLeast(int min)
        {
            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min), "Min must be non-negative.");
            return new QuantifiedPattern(this, $"{{{min},}}");
        }

        /// <summary>
        /// Concatenates two patterns (equivalent to writing them side by side in regex).
        /// </summary>
        public static Pattern operator +(Pattern left, Pattern right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return new ConcatPattern(left, right);
        }

        /// <summary>
        /// Concatenates a pattern with a literal string.
        /// </summary>
        public static Pattern operator +(Pattern left, string right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return new ConcatPattern(left, new LiteralPattern(right));
        }

        /// <summary>
        /// Concatenates a literal string with a pattern.
        /// </summary>
        public static Pattern operator +(string left, Pattern right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return new ConcatPattern(new LiteralPattern(left), right);
        }

        /// <summary>
        /// Alternation: matches either the left or right pattern (equivalent to | in regex).
        /// </summary>
        public static Pattern operator |(Pattern left, Pattern right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);
            return new AlternationPattern(left, right);
        }

        /// <summary>
        /// Negation: wraps the pattern as a negative lookahead or character class negation
        /// depending on the pattern type.
        /// </summary>
        public static Pattern operator !(Pattern pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern);
            return new NegationPattern(pattern);
        }


        /// <summary>
        /// Implicitly converts a Pattern to a <see cref="Regex"/> instance.
        /// </summary>
        public static implicit operator Regex(Pattern pattern) => new Regex(pattern.Expression);

        /// <summary>
        /// Implicitly converts a Pattern to its regex string representation.
        /// </summary>
        public static implicit operator string(Pattern pattern) => pattern.Expression;

        /// <summary>
        /// Indicates whether the entire input string matches the pattern (full match).
        /// Wraps the expression in <c>^(?:...)$</c> for validation semantics.
        /// </summary>
        public bool IsMatch(string input) => Regex.IsMatch(input, $"^(?:{Expression})$");
        /// <summary>
        /// Indicates whether the input string contains a match for the pattern (partial match).
        /// Use this when you need to search within a larger string.
        /// </summary>
        public bool ContainsMatch(string input) => Regex.IsMatch(input, Expression);
        /// <summary>
        ///Searches the input string for all occurrences of the pattern(partial match).
        /// Does not add anchors.
        /// </summary>
        public Match Match(string input) => Regex.Match(input, Expression);

        /// <summary>
        /// Searches the input string for all occurrences of the pattern.
        /// Does not add anchors.
        /// </summary>
        public MatchCollection Matches(string input) => Regex.Matches(input, Expression);

        /// <summary>
        /// Replaces all occurrences of the pattern in the input string with the replacement string.
        /// Does not add anchors.
        /// </summary>
        public string Replace(string input, string replacement) => Regex.Replace(input, Expression, replacement);

        /// <summary>
        /// Splits the input string at each occurrence of the pattern.
        /// Does not add anchors.
        /// </summary>
        public string[] Split(string input) => [.. Regex.Split(input, Expression)];


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

        /// <summary>.
        /// Creates a pattern that matches integer numbers.
        /// </summary>
        public static IntegerPattern Integer() => new();

        #region Static Factory Methods — Groups

        /// <summary>
        /// Wraps a pattern in a non-capturing group (?:...).
        /// </summary>
        public static GroupPattern Group(Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(inner);
            return new GroupPattern(inner);
        }

        /// <summary>
        /// Wraps a pattern in a named capturing group (?&lt;name&gt;...).
        /// </summary>
        public static NamedGroupPattern NamedGroup(string name, Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(inner);
            return new NamedGroupPattern(name, inner);
        }

        #endregion

        #region Static Factory Methods — Anchors

        /// <summary>
        /// Matches the start of the string (equivalent to ^).
        /// </summary>
        public static AnchorPattern Start() => new("^");

        /// <summary>
        /// Matches the end of the string (equivalent to $).
        /// </summary>
        public static AnchorPattern End() => new("$");

        /// <summary>
        /// Matches a word boundary (equivalent to \b).
        /// </summary>
        public static AnchorPattern WordBoundary() => new("\\b");

        /// <summary>
        /// Matches a non-word boundary (equivalent to \B).
        /// </summary>
        public static AnchorPattern NotWordBoundary() => new("\\B");

        #endregion

        #region Static Factory Methods — Lookarounds

        /// <summary>
        /// Positive lookahead: asserts that the pattern matches after the current position.
        /// </summary>
        public static LookaroundPattern LookAhead(Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(inner);
            return new LookaroundPattern("?=", inner);
        }

        /// <summary>
        /// Negative lookahead: asserts that the pattern does NOT match after the current position.
        /// </summary>
        public static LookaroundPattern LookAheadNot(Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(inner);
            return new LookaroundPattern("?!", inner);
        }

        /// <summary>
        /// Positive lookbehind: asserts that the pattern matches before the current position.
        /// </summary>
        public static LookaroundPattern LookBehind(Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(inner);
            return new LookaroundPattern("?<=", inner);
        }

        /// <summary>
        /// Negative lookbehind: asserts that the pattern does NOT match before the current position.
        /// </summary>
        public static LookaroundPattern LookBehindNot(Pattern inner)
        {
            ArgumentNullException.ThrowIfNull(inner);
            return new LookaroundPattern("?<!", inner);
        }

        #endregion
        /// <summary>
        /// Returns the regex string representation of this pattern.
        /// </summary>
        public override string ToString() => Expression;
    }
}

