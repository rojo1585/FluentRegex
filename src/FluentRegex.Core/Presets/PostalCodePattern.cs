namespace FluentRegex.Core.Presets
{
    /// <summary>
    /// Configurable postal code pattern with country presets.
    /// Use via <see cref="Presets.PostalCode"/>.
    /// </summary>
    public sealed class PostalCodePattern : Pattern
    {
        public override string Expression { get; }
        protected internal override int Precedence => 1;
        internal string? Country { get; }

        private static readonly Dictionary<string, string> CountryPatterns = new()
        {
            ["US"] = @"\d{5}(?:-\d{4})?",
            ["UK"] = @"[A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}",
            ["MX"] = @"\d{5}",
            ["CA"] = @"[A-Z]\d[A-Z] ?\d[A-Z]\d",
            ["DE"] = @"\d{5}",
            ["JP"] = @"\d{3}-?\d{4}",
            ["BR"] = @"\d{5}-?\d{3}",
            ["FR"] = @"\d{5}",
            ["IT"] = @"\d{5}",
            ["ES"] = @"\d{5}",
            ["IN"] = @"\d{6}",
        };

        internal PostalCodePattern(string? country = null, string? customPattern = null)
        {
            Country = country?.ToUpper();

            if (customPattern is not null)
                Expression = customPattern;
            else if (country is not null && CountryPatterns.TryGetValue(country, out var pattern))
                Expression = pattern;
            else
                Expression = "[A-Z0-9]{3,10}";
        }

        /// <summary>
        /// Sets the pattern to a known country format.
        /// Supported countries: US, UK, MX, CA, DE, JP, BR, FR, IT, ES, IN.
        /// </summary>
        public PostalCodePattern ForCountry(string countryCode) => new(countryCode);

        /// <summary>
        /// Sets a custom regex pattern for the postal code.
        /// </summary>
        public PostalCodePattern Custom(string pattern) => new(customPattern: pattern);
    }

}
