namespace FluentRegex.Core.Tests
{
    public class CharPatternTests
    {
        [Fact]
        public void Digit_MatchesDigits()
        {
            var pattern = Pattern.Digit();
            Assert.True(pattern.IsMatch("5"));
            Assert.True(pattern.IsMatch("0"));
            Assert.False(pattern.IsMatch("a"));
            Assert.False(pattern.IsMatch("ab"));
        }

        [Fact]
        public void Letter_MatchesLetters()
        {
            var pattern = Pattern.Letter();
            Assert.True(pattern.IsMatch("a"));
            Assert.True(pattern.IsMatch("Z"));
            Assert.False(pattern.IsMatch("5"));
            Assert.False(pattern.IsMatch("ab"));
        }

        [Fact]
        public void AnyChar_MatchesAnyChar()
        {
            var pattern = Pattern.AnyChar();
            Assert.True(pattern.IsMatch("a"));
            Assert.True(pattern.IsMatch("5"));
            Assert.True(pattern.IsMatch(" "));
        }

        [Fact]
        public void Whitespace_MatchesWhitespace()
        {
            var pattern = Pattern.Whitespace();
            Assert.True(pattern.IsMatch(" "));
            Assert.True(pattern.IsMatch("\t"));
            Assert.True(pattern.IsMatch("\n"));
            Assert.False(pattern.IsMatch("a"));
        }

        [Fact]
        public void Char_SpecificChars()
        {
            var pattern = Pattern.Char('@', '.', '_');
            Assert.True(pattern.IsMatch("@"));
            Assert.True(pattern.IsMatch("."));
            Assert.True(pattern.IsMatch("_"));
            Assert.False(pattern.IsMatch("a"));
        }

        [Fact]
        public void Range_MatchesRange()
        {
            var pattern = Pattern.Range('a', 'z');
            Assert.True(pattern.IsMatch("m"));
            Assert.True(pattern.IsMatch("a"));
            Assert.True(pattern.IsMatch("z"));
            Assert.False(pattern.IsMatch("A"));
        }

        [Fact]
        public void NotChar_ExcludesChars()
        {
            var pattern = Pattern.NotChar('a', 'b', 'c');
            Assert.False(pattern.IsMatch("a"));
            Assert.False(pattern.IsMatch("b"));
            Assert.True(pattern.IsMatch("d"));
            Assert.True(pattern.IsMatch("5"));
        }

        [Fact]
        public void NotRange_ExcludesRange()
        {
            var pattern = Pattern.NotRange('a', 'z');
            Assert.False(pattern.IsMatch("m"));
            Assert.True(pattern.IsMatch("5"));
            Assert.True(pattern.IsMatch("A"));
        }

        [Fact]
        public void Char_ThrowsOnEmptySet()
        {
            Assert.Throws<ArgumentException>(() => Pattern.Char());
        }

        [Fact]
        public void Range_ThrowsWhenFromGreaterThanTo()
        {
            Assert.Throws<ArgumentException>(() => Pattern.Range('z', 'a'));
        }
    }

}
