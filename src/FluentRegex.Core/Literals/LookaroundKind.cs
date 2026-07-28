using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Literals
{
    /// <summary>
    /// Identifies the type of lookaround assertion.
    /// </summary>
    internal enum LookaroundKind
    {
        PositiveLookahead,
        NegativeLookahead,
        PositiveLookbehind,
        NegativeLookbehind
    }
}
