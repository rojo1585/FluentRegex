namespace FluentRegex.Core.Tests
{
    public class BackreferenceTests
    {
        [Fact]
        public void NamedBackreference_Expression()
        {
            var p = Pattern.Backreference("word");
            Assert.Equal(@"\k<word>", (string)p);
        }

        [Fact]
        public void NumberedBackreference_Expression()
        {
            var p = Pattern.Backreference(1);
            Assert.Equal(@"\1", (string)p);

            var q = Pattern.Backreference(3);
            Assert.Equal(@"\3", (string)q);
        }

        [Fact]
        public void NamedBackreference_MatchesSameText()
        {
            var word = Pattern.NamedGroup("word", Pattern.Letter().OneOrMore());
            var p = word + " " + Pattern.Backreference("word");

            Assert.Equal(@"(?<word>[a-zA-Z]+) \k<word>", (string)p);
            Assert.True(p.ContainsMatch("hello hello"));
            Assert.False(p.ContainsMatch("hello world"));
        }

        [Fact]
        public void NamedBackreference_CanExtract()
        {
            var word = Pattern.NamedGroup("word", Pattern.Letter().OneOrMore());
            var p = word + " " + Pattern.Backreference("word");

            var match = p.Match("test test");
            Assert.True(match.Success);
            Assert.Equal("test test", match.Value);
            Assert.Equal("test", match.Groups["word"].Value);
        }

        [Fact]
        public void NumberedBackreference_WithNamedGroup()
        {
            var p = Pattern.NamedGroup("w", Pattern.Letter().OneOrMore()) + " " + Pattern.Backreference(1);

            Assert.Equal(@"(?<w>[a-zA-Z]+) \1", (string)p);
            Assert.True(p.ContainsMatch("abc abc"));
            Assert.False(p.ContainsMatch("abc xyz"));
        }


        [Fact]
        public void Backreference_DoubledWord()
        {
            var word = Pattern.NamedGroup("w", Pattern.Letter().OneOrMore());
            var space = Pattern.Literal(" ");
            var p = Pattern.WordBoundary() + word + space + Pattern.Backreference("w") + Pattern.WordBoundary();

            Assert.True(p.ContainsMatch("the the cat"));
            Assert.True(p.ContainsMatch("is is duplicated"));
            Assert.False(p.ContainsMatch("the cat sat"));
        }

        [Fact]
        public void NamedBackreference_NullName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Pattern.Backreference(null!));
        }

        [Fact]
        public void NamedBackreference_EmptyName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Pattern.Backreference(""));
        }

        [Fact]
        public void NamedBackreference_InvalidName_Throws()
        {
            Assert.Throws<ArgumentException>(() => Pattern.Backreference("my-group"));
            Assert.Throws<ArgumentException>(() => Pattern.Backreference("1abc"));
        }

        [Fact]
        public void NumberedBackreference_ZeroOrNegative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Pattern.Backreference(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Pattern.Backreference(-1));
        }

        [Fact]
        public void Backreference_Properties()
        {
            var named = Pattern.Backreference("test");
            Assert.Equal("test", named.Name);
            Assert.Null(named.Number);

            var numbered = Pattern.Backreference(2);
            Assert.Null(numbered.Name);
            Assert.Equal(2, numbered.Number);
        }

        [Fact]
        public void Backreference_IsNotZeroWidth()
        {
            var p = Pattern.Backreference(1).Optional();
            Assert.Equal(@"\1?", (string)p);
        }
    }
}
