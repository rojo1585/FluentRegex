using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{


    public class UsageMethodTests
    {
        [Fact]
        public void IsMatch_Works()
        {
            var pattern = Pattern.Digit().Repeat(3);
            Assert.True(pattern.IsMatch("123"));
            Assert.False(pattern.IsMatch("abc"));
        }

        [Fact]
        public void IsMatch_IsFullMatch()
        {
            var pattern = Pattern.Digit().Repeat(3);
            Assert.True(pattern.IsMatch("123"));
            Assert.False(pattern.IsMatch("1234"));
            Assert.False(pattern.IsMatch("a123"));
        }

        [Fact]
        public void ContainsMatch_IsPartialMatch()
        {
            var pattern = Pattern.Letter();
            Assert.True(pattern.ContainsMatch("ab"));
            Assert.True(pattern.ContainsMatch("123a"));
            Assert.False(pattern.ContainsMatch("123"));
        }

        [Fact]
        public void Match_ReturnsFirstMatch()
        {
            var pattern = Pattern.Digit().OneOrMore();
            var match = pattern.Match("abc 123 def 456");
            Assert.True(match.Success);
            Assert.Equal("123", match.Value);
        }

        [Fact]
        public void Matches_ReturnsAllMatches()
        {
            var pattern = Pattern.Digit().OneOrMore();
            var matches = pattern.Matches("abc 123 def 456");
            Assert.Equal(2, matches.Count);
            Assert.Equal("123", matches[0].Value);
            Assert.Equal("456", matches[1].Value);
        }

        [Fact]
        public void Replace_ReplacesAll()
        {
            var pattern = Pattern.Digit().OneOrMore();
            var result = pattern.Replace("abc 123 def 456", "#");
            Assert.Equal("abc # def #", result);
        }

        [Fact]
        public void Split_SplitsOnPattern()
        {
            var pattern = Pattern.Literal(",");
            var result = pattern.Split("a,b,c");
            Assert.Equal(["a", "b", "c"], result);
        }
    }

}
