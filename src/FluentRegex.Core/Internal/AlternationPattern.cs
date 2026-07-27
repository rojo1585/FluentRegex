namespace FluentRegex.Core.Internal
{
    /// <summary>
    /// Internal pattern that represents alternation (OR) between two patterns.
    /// Created by the | operator.
    /// </summary>
    internal sealed class AlternationPattern : Pattern
    {
        public override string Expression { get; }
        internal override int Precedence => 0;

        internal AlternationPattern(Pattern left, Pattern right)
        {
            // Alternation has the lowest precedence, so WrapIfBelow(0) wraps nothing.
            // This is correct: a|b|c is unambiguous regardless of grouping.
            Expression = $"{left.WrapIfBelow(0)}|{right.WrapIfBelow(0)}";
        }
    }
}
