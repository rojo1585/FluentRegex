using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
    public class QuantifierTests
    {
        [Fact]
        public void Any_ZeroOrMore()
        {
            var pattern = Pattern.Digit().Any();
            Assert.True(pattern.IsMatch(""));
            Assert.True(pattern.IsMatch("5"));
            Assert.True(pattern.IsMatch("123"));
            Assert.False(pattern.IsMatch("abc"));
        }

        [Fact]
        public void OneOrMore_OneOrMore()
        {
            var pattern = Pattern.Letter().OneOrMore();
            Assert.False(pattern.IsMatch(""));
            Assert.True(pattern.IsMatch("a"));
            Assert.True(pattern.IsMatch("abc"));
        }

        [Fact]
        public void Optional_ZeroOrOne()
        {
            var pattern = Pattern.Literal("s").Optional();
            Assert.True(pattern.IsMatch(""));
            Assert.True(pattern.IsMatch("s"));
            Assert.False(pattern.IsMatch("ss"));
        }

        [Fact]
        public void Repeat_ExactCount()
        {
            var pattern = Pattern.Digit().Repeat(4);
            Assert.True(pattern.IsMatch("1234"));
            Assert.False(pattern.IsMatch("123"));
            Assert.False(pattern.IsMatch("12345"));
        }

        [Fact]
        public void Repeat_Range()
        {
            var pattern = Pattern.Digit().Repeat(2, 4);
            Assert.False(pattern.IsMatch("1"));
            Assert.True(pattern.IsMatch("12"));
            Assert.True(pattern.IsMatch("123"));
            Assert.True(pattern.IsMatch("1234"));
            Assert.False(pattern.IsMatch("12345"));
        }

        [Fact]
        public void AtLeast_Minimum()
        {
            var pattern = Pattern.Letter().AtLeast(3);
            Assert.False(pattern.IsMatch("ab"));
            Assert.True(pattern.IsMatch("abc"));
            Assert.True(pattern.IsMatch("abcd"));
        }

        [Fact]
        public void Repeat_NegativeCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Pattern.Digit().Repeat(-1));
        }
    }

}
