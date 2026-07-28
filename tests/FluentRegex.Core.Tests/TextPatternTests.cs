namespace FluentRegex.Core.Tests
{


    public class TextPatternTests
    {
        [Fact]
        public void Text_DefaultMatchesLetters()
        {
            var pattern = Pattern.Text();
            Assert.True(pattern.IsMatch("hello"));
            Assert.True(pattern.IsMatch("HelloWorld"));
            Assert.False(pattern.IsMatch("hello123"));
            Assert.False(pattern.IsMatch(""));
        }

        [Fact]
        public void Text_MinLength()
        {
            var pattern = Pattern.Text().MinLength(3);
            Assert.True(pattern.IsMatch("abc"));
            Assert.False(pattern.IsMatch("ab"));
        }

        [Fact]
        public void Text_MaxLength()
        {
            var pattern = Pattern.Text().MaxLength(5);
            Assert.True(pattern.IsMatch("abc"));
            Assert.True(pattern.IsMatch("abcde"));
            Assert.False(pattern.IsMatch("abcdef"));
            Assert.True(pattern.IsMatch(""));
        }

        [Fact]
        public void Text_MinAndMaxLength()
        {
            var pattern = Pattern.Text().MinLength(2).MaxLength(5);
            Assert.False(pattern.IsMatch("a"));
            Assert.True(pattern.IsMatch("ab"));
            Assert.True(pattern.IsMatch("abc"));
            Assert.True(pattern.IsMatch("abcde"));
            Assert.False(pattern.IsMatch("abcdef"));
        }

        [Fact]
        public void Text_AllowDigits()
        {
            var pattern = Pattern.Text().AllowDigits();
            Assert.True(pattern.IsMatch("hello123"));
            Assert.True(pattern.IsMatch("test"));
        }

        [Fact]
        public void Text_AllowChars()
        {
            var pattern = Pattern.Text().AllowChars('_', '-');
            Assert.True(pattern.IsMatch("hello_world"));
            Assert.True(pattern.IsMatch("some-thing"));
            Assert.False(pattern.IsMatch("hello.world"));
        }

        [Fact]
        public void Text_CombinedOptions()
        {
            var pattern = Pattern.Text().MinLength(3).MaxLength(20).AllowDigits().AllowChars('_', '.');
            Assert.True(pattern.IsMatch("user_01"));
            Assert.True(pattern.IsMatch("admin.name"));
            Assert.False(pattern.IsMatch("ab"));
        }

        [Fact]
        public void Text_FluentChainIsImmutable()
        {
            var base_ = Pattern.Text();
            var withMin = base_.MinLength(3);
            var withDigits = base_.AllowDigits();

            // base_ should still be just letters, one or more
            Assert.DoesNotContain("0-9", base_.Expression);
            Assert.Contains("0-9", withDigits.Expression);
            Assert.Contains("{3", withMin.Expression);
        }

        [Fact]
        public void Text_ExpressionFormat()
        {
            Assert.Equal("[a-zA-Z]+", Pattern.Text().Expression);
            Assert.Equal("[a-zA-Z]{3,}", Pattern.Text().MinLength(3).Expression);
            Assert.Equal("[a-zA-Z]{0,5}", Pattern.Text().MaxLength(5).Expression);
            Assert.Equal("[a-zA-Z0-9]+", Pattern.Text().AllowDigits().Expression);
        }
    }
}
