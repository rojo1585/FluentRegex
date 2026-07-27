namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches any whitespace character (spaces, tabs, newlines), equivalent to \s in regex.
    /// </summary>
    public sealed class WhitespacePattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; } = "\\s";
    }
}
