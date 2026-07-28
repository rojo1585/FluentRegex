using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
    public class OperatorTests
    {
        [Fact]
        public void Concat_TwoPatterns()
        {
            var pattern = Pattern.Literal("hello") + " " + Pattern.Literal("world");
            Assert.True(pattern.IsMatch("hello world"));
            Assert.False(pattern.IsMatch("hello"));
            Assert.Equal("hello world", (string)pattern);
        }

        [Fact]
        public void Concat_PatternPlusString()
        {
            var pattern = Pattern.Literal("user") + "@" + Pattern.Literal("domain");
            Assert.True(pattern.IsMatch("user@domain"));
        }

        [Fact]
        public void Concat_StringPlusPattern()
        {
            var pattern = "prefix_" + Pattern.Digit().Repeat(3);
            Assert.True(pattern.IsMatch("prefix_123"));
            Assert.False(pattern.IsMatch("prefix_12"));
        }

        [Fact]
        public void Alternation_OrOperator()
        {
            var pattern = Pattern.Literal("cat") | Pattern.Literal("dog");
            Assert.True(pattern.IsMatch("cat"));
            Assert.True(pattern.IsMatch("dog"));
            Assert.False(pattern.IsMatch("bird"));
        }

        [Fact]
        public void Alternation_MultipleWithOperators()
        {
            var pattern = Pattern.Literal("red") | Pattern.Literal("green") | Pattern.Literal("blue");
            Assert.True(pattern.IsMatch("red"));
            Assert.True(pattern.IsMatch("green"));
            Assert.True(pattern.IsMatch("blue"));
            Assert.False(pattern.IsMatch("yellow"));
        }
    }

}
