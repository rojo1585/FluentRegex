using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
    public class AnchorTests
    {
        [Fact]
        public void StartAndEnd_Expression()
        {
            Assert.Equal("^", (string)Pattern.Start());
            Assert.Equal("$", (string)Pattern.End());
        }

        [Fact]
        public void Start_MatchesAtBeginning()
        {
            var p = Pattern.Start() + Pattern.Literal("hello");
            Assert.True(p.ContainsMatch("hello world"));
            Assert.False(p.ContainsMatch("say hello"));
        }

        [Fact]
        public void End_MatchesAtEnd()
        {
            var p = Pattern.Literal("world") + Pattern.End();
            Assert.True(p.ContainsMatch("hello world"));
            Assert.False(p.ContainsMatch("world peace"));
        }

        [Fact]
        public void StartAndEnd_FullString()
        {
            var p = Pattern.Start() + Pattern.Digit().OneOrMore() + Pattern.End();
            Assert.True(p.ContainsMatch("12345"));
            Assert.False(p.ContainsMatch("abc 123"));
        }

        [Fact]
        public void WordBoundary_Expression()
        {
            Assert.Equal(@"\b", (string)Pattern.WordBoundary());
            Assert.Equal(@"\B", (string)Pattern.NotWordBoundary());
        }

        [Fact]
        public void WordBoundary_IsolatesWords()
        {
            var p = Pattern.WordBoundary() + Pattern.Literal("cat") + Pattern.WordBoundary();
            Assert.True(p.ContainsMatch("the cat sat"));
            Assert.False(p.ContainsMatch("the caterpillar sat"));
        }

        [Fact]
        public void WordBoundary_DoesNotMatchInsideWord()
        {
            var p = Pattern.WordBoundary() + Pattern.Literal("at") + Pattern.WordBoundary();
            Assert.True(p.ContainsMatch("cat at dog"));
            Assert.False(p.ContainsMatch("cat dog"));
        }

        [Fact]
        public void NotWordBoundary_MatchesInsideWord()
        {
            var p = Pattern.NotWordBoundary() + Pattern.Literal("at") + Pattern.NotWordBoundary();
            Assert.True(p.ContainsMatch("caterpillar"));
            Assert.False(p.ContainsMatch("cat dog"));
        }
    }
}
