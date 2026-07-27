using FluentRegex.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentRegex.Core.Tests;

public class LiteralPatternTests
{
    [Fact]
    public void Literal_MatchesExactString()
    {
        var pattern = Pattern.Literal("hello");
        Assert.True(pattern.IsMatch("hello"));
        Assert.False(pattern.IsMatch("Hello"));
        Assert.False(pattern.IsMatch("hello!"));
    }

    [Fact]
    public void Literal_EscapesSpecialChars()
    {
        var pattern = Pattern.Literal("1.0");
        Assert.Equal(@"1\.0", (string)pattern);
        Assert.True(pattern.IsMatch("1.0"));
        Assert.False(pattern.IsMatch("1X0"));
    }

    [Fact]
    public void Literal_EmptyString()
    {
        var pattern = Pattern.Literal("");
        Assert.True(pattern.IsMatch(""));
    }

    [Fact]
    public void Literal_ThrowsOnNull()
    {
        Assert.Throws<ArgumentNullException>(() => Pattern.Literal(null!));
    }
}

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

public class ConversionTests
{
    [Fact]
    public void ImplicitConversion_ToString()
    {
        string expr = Pattern.Literal("test");
        Assert.Equal("test", expr);
    }

    [Fact]
    public void ImplicitConversion_ToRegex()
    {
        Regex regex = Pattern.Digit().Repeat(3);
        Assert.True(regex.IsMatch("123"));
        Assert.False(regex.IsMatch("ab"));
    }

    [Fact]
    public void ToString_ReturnsExpression()
    {
        var pattern = Pattern.Letter().OneOrMore();
        Assert.Equal(pattern.Expression, pattern.ToString());
    }
}

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
        Assert.False(pattern.IsMatch("1234")); // extra digit — not a full match
        Assert.False(pattern.IsMatch("a123")); // extra char before — not a full match
    }

    [Fact]
    public void ContainsMatch_IsPartialMatch()
    {
        var pattern = Pattern.Letter();
        Assert.True(pattern.ContainsMatch("ab"));    // finds 'a' inside "ab"
        Assert.True(pattern.ContainsMatch("123a"));  // finds 'a' inside "123a"
        Assert.False(pattern.ContainsMatch("123"));  // no letter found
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

public class IntegerPatternTests
{
    [Fact]
    public void Integer_DefaultMatchesDigits()
    {
        var pattern = Pattern.Integer();
        Assert.True(pattern.IsMatch("123"));
        Assert.True(pattern.IsMatch("0"));
        Assert.False(pattern.IsMatch("abc"));
        Assert.False(pattern.IsMatch(""));
        Assert.False(pattern.IsMatch("12.5"));
    }

    [Fact]
    public void Integer_AllowNegative()
    {
        var pattern = Pattern.Integer().AllowNegative();
        Assert.True(pattern.IsMatch("-123"));
        Assert.True(pattern.IsMatch("456"));
        Assert.False(pattern.IsMatch("+123"));
    }

    [Fact]
    public void Integer_AllowSign()
    {
        var pattern = Pattern.Integer().AllowSign();
        Assert.True(pattern.IsMatch("-123"));
        Assert.True(pattern.IsMatch("+456"));
        Assert.True(pattern.IsMatch("789"));
    }

    [Fact]
    public void Integer_MinDigits()
    {
        var pattern = Pattern.Integer().MinDigits(3);
        Assert.True(pattern.IsMatch("123"));
        Assert.False(pattern.IsMatch("12"));
    }

    [Fact]
    public void Integer_MaxDigits()
    {
        var pattern = Pattern.Integer().MaxDigits(4);
        Assert.True(pattern.IsMatch("1234"));
        Assert.False(pattern.IsMatch("12345"));
    }

    [Fact]
    public void Integer_MinMaxDigits()
    {
        var pattern = Pattern.Integer().MinDigits(2).MaxDigits(4);
        Assert.False(pattern.IsMatch("1"));
        Assert.True(pattern.IsMatch("12"));
        Assert.True(pattern.IsMatch("1234"));
        Assert.False(pattern.IsMatch("12345"));
    }

    [Fact]
    public void Integer_ExpressionFormat()
    {
        Assert.Equal(@"\d+", Pattern.Integer().Expression);
        Assert.Equal(@"-?\d+", Pattern.Integer().AllowNegative().Expression);
        Assert.Equal(@"[+-]?\d+", Pattern.Integer().AllowSign().Expression);
        Assert.Equal(@"\d{3,}", Pattern.Integer().MinDigits(3).Expression);
    }
}

public class ComplexPatternTests
{
    [Fact]
    public void SimpleEmail_LikePattern()
    {
        var local = Pattern.Text().AllowDigits().AllowChars('.', '_', '%', '+', '-');
        var domain = Pattern.Text().AllowDigits().AllowChars('-', '.').MinLength(2);
        var email = local + "@" + domain;

        Assert.True(email.IsMatch("user@example.com"));
        Assert.True(email.IsMatch("john.doe@company.co.uk"));
        Assert.False(email.IsMatch("@example.com"));
        Assert.False(email.IsMatch("user@"));
    }

    [Fact]
    public void PhoneNumber_LikePattern()
    {
        var code = Pattern.Literal("+52").Optional();
        var digits = Pattern.Digit().Repeat(10);
        var phone = code + digits;

        Assert.True(phone.IsMatch("5512345678"));
        Assert.True(phone.IsMatch("+525512345678"));
        Assert.False(phone.IsMatch("123"));
    }

    [Fact]
    public void DateLikePattern()
    {
        var day = Pattern.Digit().Repeat(2);
        var sep = Pattern.Literal("/");
        var month = Pattern.Digit().Repeat(2);
        var year = Pattern.Digit().Repeat(4);
        var date = day + sep + month + sep + year;

        Assert.True(date.IsMatch("15/06/2025"));
        Assert.False(date.IsMatch("2025-06-15"));
    }

    [Fact]
    public void Alternation_WithConcatenation()
    {
        var http = Pattern.Literal("http") + Pattern.Literal("s").Optional();
        var ftp = Pattern.Literal("ftp");
        var protocol = http | ftp;

        Assert.True(protocol.IsMatch("http"));
        Assert.True(protocol.IsMatch("https"));
        Assert.True(protocol.IsMatch("ftp"));
        Assert.False(protocol.IsMatch("ssh"));
    }
}

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
            Assert.Throws<ArgumentException>(() => Pattern.Backreference(""));
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
            // Backreferences consume characters, so they CAN be quantified
            var p = Pattern.Backreference(1).Optional();
            Assert.Equal(@"\1?", (string)p);
        }
    }
}