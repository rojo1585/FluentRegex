using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
    public class LookaroundTests
    {
        [Fact]
        public void LookAhead_Expression()
        {
            var p = Pattern.Digit().Repeat(3) + Pattern.LookAhead(Pattern.End());
            Assert.Equal(@"\d{3}(?=$)", (string)p);
        }

        [Fact]
        public void LookAhead_Positive()
        {
            var p = Pattern.Literal("foo") + Pattern.LookAhead(Pattern.Literal("bar"));
            Assert.True(p.ContainsMatch("foobar"));
            Assert.False(p.ContainsMatch("foobaz"));
        }

        [Fact]
        public void LookAheadNot_Expression()
        {
            var p = Pattern.Digit().OneOrMore() + Pattern.LookAheadNot(Pattern.Literal("0"));
            Assert.Equal(@"\d+(?!0)", (string)p);
        }

        [Fact]
        public void LookAheadNot_Negative()
        {
            var p = Pattern.Digit().OneOrMore() + Pattern.LookAheadNot(Pattern.Digit());
            var match = p.Match("abc 123 def");
            Assert.True(match.Success);
            Assert.Equal("123", match.Value);
        }

        [Fact]
        public void LookBehind_Positive()
        {
            var p = Pattern.LookBehind(Pattern.Literal("foo")) + Pattern.Literal("bar");
            Assert.True(p.ContainsMatch("foobar"));
            Assert.False(p.ContainsMatch("bazbar"));
        }

        [Fact]
        public void LookBehind_Expression()
        {
            var p = Pattern.LookBehind(Pattern.Literal("$")) + Pattern.Digit().Repeat(2);
            Assert.Equal(@"(?<=\$)\d{2}", (string)p);
        }

        [Fact]
        public void LookBehindNot_Negative()
        {
            var p = Pattern.LookBehindNot(Pattern.Digit()) + Pattern.Literal("x");
            Assert.True(p.ContainsMatch("abcx"));
            Assert.False(p.ContainsMatch("123x"));
        }

        [Fact]
        public void LookBehindNot_Expression()
        {
            var p = Pattern.LookBehindNot(Pattern.Digit());
            Assert.Equal(@"(?<!\d)", (string)p);
        }

        [Fact]
        public void Lookaround_PasswordValidation()
        {
            var isolatedDigit = Pattern.LookBehindNot(Pattern.Digit()) +
                                 Pattern.Digit() +
                                 Pattern.LookAheadNot(Pattern.Digit());

            Assert.True(isolatedDigit.ContainsMatch("a1b"));
            Assert.False(isolatedDigit.ContainsMatch("123"));
        }

        [Fact]
        public void LookAround_PrecedenceIsAtomic()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Pattern.LookAhead(Pattern.Literal("x")).OneOrMore());
            Assert.Contains("zero-width", ex.Message);
        }
        [Fact]
        public void LookAround_CanBeConcatenatedWithoutWrapping()
        {
            var p = Pattern.Literal("foo") + Pattern.LookAhead(Pattern.Literal("bar"));
            Assert.Equal(@"foo(?=bar)", (string)p);
        }

    }

}
