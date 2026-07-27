using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core.Patterns
{
    /// <summary>
    /// A pattern that matches a single digit (0-9), equivalent to \d in regex.
    /// </summary>
    public sealed class DigitPattern : Pattern
    {
        /// <inheritdoc />
        public override string Expression { get; } = @"\d";
    }
}
