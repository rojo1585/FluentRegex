namespace FluentRegex.Core.Presets;


/// <summary>
/// Provides pre-built regular expression patterns for common validation scenarios.
/// Simple presets return a <see cref="Pattern"/> directly.
/// Configurable presets return a fluent builder class that extends <see cref="Pattern"/>.
/// </summary>
public static class Presets
{
    /// <summary>
    /// Matches a standard email address.
    /// <para>Expression: <c>[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}</c></para>
    /// </summary>
    public static Pattern Email() => new SimplePattern(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

    /// <summary>
    /// Matches an HTTP/HTTPS URL with optional path.
    /// <para>Expression: <c>https?://[a-zA-Z0-9.-]+(?:/[^\s]*)?</c></para>
    /// </summary>
    public static Pattern Url() => new SimplePattern(@"https?://[a-zA-Z0-9.-]+(?:/[^\s]*)?");

    /// <summary>
    /// Matches a valid IPv4 address (0.0.0.0 – 255.255.255.255).
    /// <para>Expression: <c>(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)(?:\.(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)){3}</c></para>
    /// </summary>
    public static Pattern IPv4() => new SimplePattern(
        @"(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)(?:\.(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)){3}");

    /// <summary>
    /// Matches a full IPv6 address (simplified: 8 groups of 1-4 hex digits).
    /// <para>Expression: <c>[0-9a-fA-F]{1,4}(?::[0-9a-fA-F]{1,4}){7}</c></para>
    /// </summary>
    public static Pattern IPv6() => new SimplePattern(@"[0-9a-fA-F]{1,4}(?::[0-9a-fA-F]{1,4}){7}");

    /// <summary>
    /// Matches a hex color code (#RGB or #RRGGBB).
    /// <para>Expression: <c>#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?</c></para>
    /// </summary>
    public static Pattern HexColor() => new SimplePattern(@"#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?");

    /// <summary>
    /// Matches a standard UUID (8-4-4-4-12 hex digits with dashes).
    /// <para>Expression: <c>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}</c></para>
    /// </summary>
    public static Pattern UUID() => new SimplePattern(
        @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}");

    /// <summary>
    /// Matches a MAC address (six groups of 2 hex digits separated by colons).
    /// <para>Expression: <c>[0-9a-fA-F]{2}(?::[0-9a-fA-F]{2}){5}</c></para>
    /// </summary>
    public static Pattern MacAddress() => new SimplePattern(@"[0-9a-fA-F]{2}(?::[0-9a-fA-F]{2}){5}");

    /// <summary>
    /// Matches a credit card number (13 to 19 digits).
    /// <para>Expression: <c>\d{13,19}</c></para>
    /// </summary>
    public static Pattern CreditCard() => new SimplePattern(@"\d{13,19}");

    /// <summary>
    /// Creates a configurable password pattern. Chain fluent methods to customize requirements.
    /// <para>Default: 8-128 chars, requires uppercase, lowercase, and digit.</para>
    /// </summary>
    public static PasswordPattern Password() => new();

    /// <summary>
    /// Creates a configurable username pattern. Chain fluent methods to customize.
    /// <para>Default: 3-30 chars, alphanumeric + underscore, must start with a letter or underscore.</para>
    /// </summary>
    public static UsernamePattern Username() => new();

    /// <summary>
    /// Matches a generic IBAN format (2 letter country code + 2 check digits + BBAN).
    /// <para>Expression: <c>[A-Z]{2}\d{2}[A-Z0-9]{4}(?:[A-Z0-9]{4,}){3}</c></para>
    /// </summary>
    public static IbanPattern Iban() => new();

    /// <summary>
    /// Creates a configurable postal code pattern. Use <c>ForCountry()</c> for known formats.
    /// <para>Default: 3-10 alphanumeric characters (generic fallback).</para>
    /// </summary>
    public static PostalCodePattern PostalCode() => new();
}

/// <summary>
/// Internal simple pattern that wraps a fixed regex expression string.
/// Used by the <see cref="Presets"/> class for non-configurable presets.
/// </summary>
internal sealed class SimplePattern : Pattern
{
    public override string Expression { get; }
   protected internal override int Precedence => 1;

    internal SimplePattern(string expression)
    {
        Expression = expression;
    }
}
