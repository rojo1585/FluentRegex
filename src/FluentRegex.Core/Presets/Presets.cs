namespace FluentRegex.Core.Presets;

/// <summary>
/// Provides pre-built regular expression patterns for common validation scenarios.
/// <para>
/// <b>Simple presets</b> (e.g. <see cref="SimpleEmail"/>) return a fixed <see cref="Pattern"/>.
/// They cover common cases but are intentionally simplified — see each method's XML docs
/// for specific limitations.
/// </para>
/// <para>
/// <b>Configurable presets</b> (e.g. <see cref="Password"/>) return a fluent builder class.
/// Chain methods to customize the pattern to your needs.
/// </para>
/// </summary>
public static class Presets
{
    #region Simple presets (fixed patterns)

    /// <summary>
    /// Matches a basic email-like string (local-part@domain).
    /// <para>
    /// <b>Limitations:</b> This is a simplified pattern, not an RFC 5322 compliant validator.
    /// It does not handle quoted strings, comments, internationalized domains (IDN),
    /// IP-address literals, or TLDs longer than the matched range.
    /// Suitable for quick sanity checks, not for authoritative email validation.
    /// </para>
    /// <para>Expression: <c>[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}</c></para>
    /// </summary>
    public static Pattern SimpleEmail() => new SimplePattern(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

    /// <summary>
    /// Matches a basic HTTP/HTTPS URL with optional path.
    /// <para>
    /// <b>Limitations:</b> Does not handle FTP, other schemes, ports, query strings,
    /// fragments, authentication, IPv6 hosts, or internationalized domains.
    /// Suitable for quick sanity checks, not for full URI parsing.
    /// </para>
    /// <para>Expression: <c>https?://[a-zA-Z0-9.-]+(?:/[^\s]*)?</c></para>
    /// </summary>
    public static Pattern SimpleUrl() => new SimplePattern(@"https?://[a-zA-Z0-9.-]+(?:/[^\s]*)?");

    /// <summary>
    /// Matches a sequence of 13 to 19 digits (credit card number length).
    /// <para>
    /// <b>Limitations:</b> Only validates length. Does not perform Luhn checksum
    /// validation, does not distinguish card networks (Visa, MasterCard, Amex, etc.),
    /// and does not handle formatted input (spaces or dashes).
    /// </para>
    /// <para>Expression: <c>\d{13,19}</c></para>
    /// </summary>
    public static Pattern SimpleCreditCard() => new SimplePattern(@"\d{13,19}");

    /// <summary>
    /// Matches a valid IPv4 address (0.0.0.0 – 255.255.255.255).
    /// <para>Each octet is validated against the 0–255 range.</para>
    /// <para>Expression: <c>(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)(?:\.(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)){3}</c></para>
    /// </summary>
    public static Pattern IPv4() => new SimplePattern(
        @"(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)(?:\.(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)){3}");

    /// <summary>
    /// Matches a full IPv6 address (8 groups of 1-4 hex digits, colon-separated).
    /// <para>
    /// <b>Limitations:</b> Does not handle <c>::</c> shorthand (zero-compression),
    /// embedded IPv4 (<c>::ffff:192.168.1.1</c>), or zone IDs.
    /// Only matches the expanded 8-group form.
    /// </para>
    /// <para>Expression: <c>[0-9a-fA-F]{1,4}(?::[0-9a-fA-F]{1,4}){7}</c></para>
    /// </summary>
    public static Pattern IPv6() => new SimplePattern(@"[0-9a-fA-F]{1,4}(?::[0-9a-fA-F]{1,4}){7}");

    /// <summary>
    /// Matches a hex color code in <c>#RGB</c> or <c>#RRGGBB</c> format.
    /// <para>Expression: <c>#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?</c></para>
    /// </summary>
    public static Pattern HexColor() => new SimplePattern(@"#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?");

    /// <summary>
    /// Matches a standard UUID (8-4-4-4-12 hex digits with dashes).
    /// <para>
    /// Matches any UUID variant/version. Does not validate version bits or variant field.
    /// </para>
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
    /// Matches a generic IBAN format: 2-letter country code + 2 check digits + BBAN (variable length).
    /// <para>
    /// <b>Limitations:</b> Validates the structural format only.
    /// Does not perform the IBAN check-digit algorithm. Country-specific
    /// BBAN length rules are not enforced (total length 16-34 chars accepted).
    /// </para>
    /// <para>Expression: <c>[A-Z]{2}\d{2}[A-Z0-9]{4}(?:[A-Z0-9]{4,}){3}</c></para>
    /// </summary>
    public static Pattern Iban() => new SimplePattern(@"[A-Z]{2}\d{2}[A-Z0-9]{4}(?:[A-Z0-9]{4,}){3}");

    #endregion

    #region Configurable presets (fluent builders)

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
    /// Creates a configurable postal code pattern. Use <c>ForCountry()</c> for known formats.
    /// <para>Default: 3-10 alphanumeric characters (generic fallback).</para>
    /// </summary>
    public static PostalCodePattern PostalCode() => new();

    #endregion
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
