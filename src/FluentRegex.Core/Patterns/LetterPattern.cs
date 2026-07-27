namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches a single ASCII letter (a-z or A-Z), equivalent to [a-zA-Z] in regex.
    /// </summary>
    public sealed class LetterPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; } = "[a-zA-Z]";
    }
}
