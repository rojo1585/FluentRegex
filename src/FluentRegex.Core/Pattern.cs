using FluentRegex.Core.Patterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRegex.Core
{
    /// <summary>/// Base abstract class for all regex patterns
    /// Provides fluent API, operators, quantifiers, and direct usage methods
    /// </summary>
    public abstract class Pattern
    {
        /// <summary>
        /// Gets the regular expression string represented by this pattern.
        /// </summary>
        public abstract string Expression { get; }
        /// <summary>
        /// Creates a pattern that matches any single character (equivalent to .).
        /// </summary>
        public static AnyCharPattern AnyChar() => new();
    }
}
