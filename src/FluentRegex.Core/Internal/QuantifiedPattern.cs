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
            Expression = $"{inner.WrapIfBelow(3)}{quantifier}";
        }
    }
}
