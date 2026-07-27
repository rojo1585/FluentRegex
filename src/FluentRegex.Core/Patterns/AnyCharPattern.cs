namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches any single character except newline, equivalent to . in regex.
    /// </summary>
    public sealed class AnyCharPattern : Pattern
    {
        public override string Expression { get; } = ".";
    }
}
