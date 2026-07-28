namespace FluentRegex.Core.Tests
{
    public class PrecedenceTests
    {
        // --- Concat + Alternation ---

        [Fact]
        public void Concat_WrapsAlternation_Left()
        {
            var p = (Pattern.Literal("cat") | Pattern.Literal("dog")) + Pattern.Literal("s");
            Assert.Equal("(?:cat|dog)s", (string)p);
            Assert.True(p.IsMatch("cats"));
            Assert.True(p.IsMatch("dogs"));
            Assert.False(p.IsMatch("cat"));
            Assert.False(p.IsMatch("dog"));
        }

        [Fact]
        public void Concat_WrapsAlternation_Right()
        {
            var p = Pattern.Literal("s") + (Pattern.Literal("cat") | Pattern.Literal("dog"));
            Assert.Equal("s(?:cat|dog)", (string)p);
            Assert.True(p.IsMatch("scat"));
            Assert.True(p.IsMatch("sdog"));
            Assert.False(p.IsMatch("scatdog"));
        }

        [Fact]
        public void Concat_WrapsAlternation_Both()
        {
            var p = (Pattern.Literal("big") | Pattern.Literal("small")) +
                    Pattern.Literal(" ") +
                    (Pattern.Literal("cat") | Pattern.Literal("dog"));
            Assert.Equal(@"(?:big|small) (?:cat|dog)", (string)p);
        }

        // --- Quantifier + Alternation ---

        [Fact]
        public void Quantifier_WrapsAlternation()
        {
            var p = (Pattern.Literal("a") | Pattern.Literal("b")).OneOrMore();
            Assert.Equal("(?:a|b)+", (string)p);
            Assert.True(p.IsMatch("ab"));
            Assert.True(p.IsMatch("aaa"));
            Assert.True(p.IsMatch("bbb"));
            Assert.False(p.IsMatch(""));
            Assert.False(p.IsMatch("ac"));
        }

        // --- Quantifier + Concatenation ---

        [Fact]
        public void Quantifier_WrapsConcatenation()
        {
            var p = (Pattern.Literal("a") + Pattern.Literal("b")).OneOrMore();
            Assert.Equal("(?:ab)+", (string)p);
            Assert.True(p.IsMatch("ab"));
            Assert.True(p.IsMatch("abab"));
            Assert.False(p.IsMatch("aabb"));
            Assert.False(p.IsMatch("a"));
        }

        // --- No unnecessary wrapping ---

        [Fact]
        public void Concat_DoesNotWrapAtom()
        {
            var p = Pattern.Literal("hello") + Pattern.Literal(" ") + Pattern.Literal("world");
            Assert.Equal(@"hello world", (string)p);
        }

        [Fact]
        public void Quantifier_DoesNotWrapAtom()
        {
            var p = Pattern.Digit().OneOrMore();
            Assert.Equal(@"\d+", (string)p);

            var q = Pattern.Letter().Repeat(3);
            Assert.Equal("[a-zA-Z]{3}", (string)q);
        }

        [Fact]
        public void Quantifier_DoesNotWrapQuantified()
        {
            var p = Pattern.AnyChar().OneOrMore().Optional();
            Assert.Equal("(?:.+)?", (string)p);
        }

        [Fact]
        public void Concat_DoesNotWrapQuantified()
        {
            var p = Pattern.Literal("a").OneOrMore() + Pattern.Literal("b");
            Assert.Equal("a+b", (string)p);
        }

        [Fact]
        public void Complex_AlternationConcatQuantifier()
        {
            var inner = (Pattern.Literal("a") | Pattern.Literal("b")) + Pattern.Literal("c");
            var p = inner.OneOrMore();
            Assert.Equal("(?:(?:a|b)c)+", (string)p);
            Assert.True(p.IsMatch("acbc"));
            Assert.True(p.IsMatch("ac"));
            Assert.False(p.IsMatch("a"));
        }

        [Fact]
        public void Complex_PhoneWithOptionalCode()
        {
            var code = Pattern.Literal("+52").Optional();
            var digits = Pattern.Digit().Repeat(10);
            var phone = code + digits;
            Assert.Equal(@"(?:\+52)?\d{10}", (string)phone);
            Assert.True(phone.IsMatch("5512345678"));
            Assert.True(phone.IsMatch("+525512345678"));
            Assert.False(phone.IsMatch("123"));
        }



    }
}
