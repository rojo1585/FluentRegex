using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
    public class ConversionTests
    {
        [Fact]
        public void ImplicitConversion_ToString()
        {
            string expr = Pattern.Literal("test");
            Assert.Equal("test", expr);
        }

        [Fact]
        public void ImplicitConversion_ToRegex()
        {
            Regex regex = Pattern.Digit().Repeat(3);
            Assert.True(regex.IsMatch("123"));
            Assert.False(regex.IsMatch("ab"));
        }

        [Fact]
        public void ToString_ReturnsExpression()
        {
            var pattern = Pattern.Letter().OneOrMore();
            Assert.Equal(pattern.Expression, pattern.ToString());
        }
    }

}
