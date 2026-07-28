namespace FluentRegex.Core.Internal
{
    /// <summary>
    /// Internal pattern that represents the concatenation of two patterns (side by side).
    /// Created by the + operator.
    /// Automatically groups operands with lower precedence (e.g. alternations).
    /// </summary>
    internal sealed class ConcatPattern : Pattern
    {
        public override string Expression { get; }
        protected internal override int Precedence => 1;

        internal ConcatPattern(Pattern left, Pattern right)
        {
            Expression = $"{left.WrapIfBelow(1)}{right.WrapIfBelow(1)}";
        }
    }
}
