namespace FluentRegex.Core.Presets
{
    /// <summary>
    /// IBAN (International Bank Account Number) pattern.
    /// Validates generic IBAN format: 2 letter country code + 2 check digits + 15-30 alphanumeric.
    /// </summary>
    public sealed class IbanPattern : Pattern
    {
        public override string Expression { get; }
        protected internal override int Precedence => 1;

        internal IbanPattern()
        {
            Expression = @"[A-Z]{2}\d{2}[A-Z0-9]{4}(?:[A-Z0-9]{4,}){3}";
        }
    }
}
