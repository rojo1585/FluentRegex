namespace FluentRegex.Core.Internal
{

    /// <summary>
    /// Internal pattern that applies a quantifier to another pattern.
    /// Created by Any(), OneOrMore(), Optional(), Repeat(), and AtLeast() methods.
    /// </summary>
    internal sealed class QuantifiedPattern : Pattern
    {
        public override string Expression { get; }

        internal override int Precedence => 2;

        internal QuantifiedPattern(Pattern inner, string quantifier)
        {
            if (inner.IsZeroWidth)
                throw new InvalidOperationException($"Cannot quantify a zero-width assertion. The pattern '{inner.Expression}' does not consume characters, "
                                                    + $"so applying '{quantifier}' is meaningless. "
                                                    + $"Remove the quantifier or use a pattern that consumes characters.");

            Expression = $"{inner.WrapIfBelow(3)}{quantifier}";
        }
    }
}
