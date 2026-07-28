using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
    public class GroupTests
    {
        [Fact]
        public void Group_NonCapturing_Expression()
        {
            var p = Pattern.Group(Pattern.Digit().OneOrMore());
            Assert.Equal(@"(?:\d+)", (string)p);
        }

        [Fact]
        public void Group_PrecedenceIsAtomic()
        {
            var inner = Pattern.Literal("ab");
            var grouped = Pattern.Group(inner);
            var p = grouped.OneOrMore();
            Assert.Equal(@"(?:ab)+", (string)p);
        }

        [Fact]
        public void Group_WithAlternation()
        {
            var p = Pattern.Group(Pattern.Literal("a") | Pattern.Literal("b")).OneOrMore();
            Assert.Equal(@"(?:a|b)+", (string)p);
            Assert.True(p.IsMatch("ab"));
            Assert.True(p.IsMatch("ba"));
            Assert.False(p.IsMatch("ac"));
        }

        [Fact]
        public void NamedGroup_Expression()
        {
            var p = Pattern.NamedGroup("year", Pattern.Digit().Repeat(4));
            Assert.Equal(@"(?<year>\d{4})", (string)p);
            Assert.Equal("year", p.Name);
        }

        [Fact]
        public void NamedGroup_CanExtract()
        {
            var year = Pattern.NamedGroup("year", Pattern.Digit().Repeat(4));
            var match = year.Match("Year: 2025");
            Assert.True(match.Success);
            Assert.Equal("2025", match.Groups["year"].Value);
        }

        [Fact]
        public void NamedGroup_EmptyName_Throws()
        {
            Assert.Throws<ArgumentException>(() => Pattern.NamedGroup("", Pattern.Digit()));
        }
        [Theory]
        [InlineData("my-group")]
        [InlineData("first name")]
        [InlineData("year!")]
        [InlineData("1year")]
        [InlineData("-start")]
        [InlineData("@tag")]
        [InlineData(" ")]
        public void NamedGroup_InvalidName_Throws(string invalidName)
        {
            var ex = Assert.Throws<ArgumentException>(() => Pattern.NamedGroup(invalidName, Pattern.Digit()));
            Assert.Equal("name", ex.ParamName);
        }

        [Theory]
        [InlineData("year")]
        [InlineData("_private")]
        [InlineData("group1")]
        [InlineData("CamelCase")]
        [InlineData("snake_case")]
        [InlineData("_")]
        [InlineData("a")]
        [InlineData("ABC123")]
        public void NamedGroup_ValidName_Accepted(string validName)
        {
            var p = Pattern.NamedGroup(validName, Pattern.Digit());
            Assert.Equal(@$"(?<{validName}>\d)", (string)p);
        }
        [Fact]
        public void NamedGroup_NullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Pattern.NamedGroup(null!, Pattern.Digit()));
        }
        [Fact]
        public void NamedGroup_InComplexPattern()
        {
            var date = Pattern.NamedGroup("day", Pattern.Digit().Repeat(2)) +
                       Pattern.Literal("/") +
                       Pattern.NamedGroup("month", Pattern.Digit().Repeat(2)) +
                       Pattern.Literal("/") +
                       Pattern.NamedGroup("year", Pattern.Digit().Repeat(4));

            var match = date.Match("Date: 15/06/2025 end");
            Assert.True(match.Success);
            Assert.Equal("15", match.Groups["day"].Value);
            Assert.Equal("06", match.Groups["month"].Value);
            Assert.Equal("2025", match.Groups["year"].Value);
        }
    }
}
