using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentRegex.Core.Presets;
namespace FluentRegex.Core.Tests.Presets
{


    public class EmailTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.SimpleEmail();
            Assert.Equal(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", p.Expression);
        }

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("first.last@domain.co.uk")]
        [InlineData("user+tag@sub.domain.org")]
        public void IsMatch_ValidEmails_ReturnsTrue(string email)
        {
            Assert.True(Core.Presets.Presets.SimpleEmail().IsMatch(email));
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("@missinglocal.com")]
        [InlineData("user@.com")]
        public void IsMatch_InvalidEmails_ReturnsFalse(string email)
        {
            Assert.False(Core.Presets.Presets.SimpleEmail().IsMatch(email));
        }
    }

    public class UrlTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.SimpleUrl();
            Assert.Equal(@"https?://[a-zA-Z0-9.-]+(?:/[^\s]*)?", p.Expression);
        }

        [Theory]
        [InlineData("https://example.com")]
        [InlineData("http://sub.domain.org/path/to/page")]
        [InlineData("https://api.example.com/v1/users?id=42")]
        public void IsMatch_ValidUrls_ReturnsTrue(string url)
        {
            Assert.True(Core.Presets.Presets.SimpleUrl().IsMatch(url));
        }

        [Theory]
        [InlineData("ftp://example.com")]
        [InlineData("example.com")]
        [InlineData("https://")]
        public void IsMatch_InvalidUrls_ReturnsFalse(string url)
        {
            Assert.False(Core.Presets.Presets.SimpleUrl().IsMatch(url));
        }
    }

    public class IPv4Tests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.IPv4();
            Assert.Equal(@"(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)(?:\.(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)){3}", p.Expression);
        }

        [Theory]
        [InlineData("192.168.1.1")]
        [InlineData("0.0.0.0")]
        [InlineData("255.255.255.255")]
        public void IsMatch_ValidIPv4_ReturnsTrue(string ip)
        {
            Assert.True(Core.Presets.Presets.IPv4().IsMatch(ip));
        }

        [Theory]
        [InlineData("256.1.1.1")]
        [InlineData("192.168.1")]
        [InlineData("192.168.1.1.1")]
        public void IsMatch_InvalidIPv4_ReturnsFalse(string ip)
        {
            Assert.False(Core.Presets.Presets.IPv4().IsMatch(ip));
        }
    }

    public class IPv6Tests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.IPv6();
            Assert.Equal(@"[0-9a-fA-F]{1,4}(?::[0-9a-fA-F]{1,4}){7}", p.Expression);
        }

        [Theory]
        [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        [InlineData("ffff:ffff:ffff:ffff:ffff:ffff:ffff:ffff")]
        [InlineData("0000:0000:0000:0000:0000:0000:0000:0001")]
        public void IsMatch_ValidIPv6_ReturnsTrue(string ip)
        {
            Assert.True(Core.Presets.Presets.IPv6().IsMatch(ip));
        }

        [Theory]
        [InlineData("2001:db8::1")]
        [InlineData("::1")]
        [InlineData("2001:db8")]
        public void IsMatch_InvalidIPv6_ReturnsFalse(string ip)
        {
            Assert.False(Core.Presets.Presets.IPv6().IsMatch(ip));
        }
    }

    public class HexColorTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.HexColor();
            Assert.Equal(@"#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?", p.Expression);
        }

        [Theory]
        [InlineData("#fff")]
        [InlineData("#aabbcc")]
        [InlineData("#012345")]
        public void IsMatch_ValidHexColors_ReturnsTrue(string color)
        {
            Assert.True(Core.Presets.Presets.HexColor().IsMatch(color));
        }

        [Theory]
        [InlineData("#gggggg")]
        [InlineData("123456")]
        [InlineData("#12")]
        public void IsMatch_InvalidHexColors_ReturnsFalse(string color)
        {
            Assert.False(Core.Presets.Presets.HexColor().IsMatch(color));
        }
    }

    public class UUIDTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.UUID();
            Assert.Equal(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", p.Expression);
        }

        [Theory]
        [InlineData("550e8400-e29b-41d4-a716-446655440000")]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
        public void IsMatch_ValidUUIDs_ReturnsTrue(string uuid)
        {
            Assert.True(Core.Presets.Presets.UUID().IsMatch(uuid));
        }

        [Theory]
        [InlineData("550e8400-e29b-41d4-a716")]
        [InlineData("not-a-uuid")]
        [InlineData("550e8400-e29b-41d4-a716-44665544000g")]
        public void IsMatch_InvalidUUIDs_ReturnsFalse(string uuid)
        {
            Assert.False(Core.Presets.Presets.UUID().IsMatch(uuid));
        }
    }

    public class MacAddressTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.MacAddress();
            Assert.Equal(@"[0-9a-fA-F]{2}(?::[0-9a-fA-F]{2}){5}", p.Expression);
        }

        [Theory]
        [InlineData("00:1A:2B:3C:4D:5E")]
        [InlineData("ff:ff:ff:ff:ff:ff")]
        [InlineData("aa:bb:cc:dd:ee:00")]
        public void IsMatch_ValidMacAddresses_ReturnsTrue(string mac)
        {
            Assert.True(Core.Presets.Presets.MacAddress().IsMatch(mac));
        }

        [Theory]
        [InlineData("00:1A:2B:3C:4D")]
        [InlineData("001A2B3C4D5E")]
        [InlineData("GG:HH:II:JJ:KK:LL")]
        public void IsMatch_InvalidMacAddresses_ReturnsFalse(string mac)
        {
            Assert.False(Core.Presets.Presets.MacAddress().IsMatch(mac));
        }
    }

    public class CreditCardTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.SimpleCreditCard();
            Assert.Equal(@"\d{13,19}", p.Expression);
        }

        [Theory]
        [InlineData("4111111111111111")]
        [InlineData("378282246310005")]
        [InlineData("1234567890123")]
        public void IsMatch_ValidCreditCards_ReturnsTrue(string card)
        {
            Assert.True(Core.Presets.Presets.SimpleCreditCard().IsMatch(card));
        }

        [Theory]
        [InlineData("411111111111")]
        [InlineData("12345678901234567890")]
        [InlineData("4111-1111-1111-1111")]
        public void IsMatch_InvalidCreditCards_ReturnsFalse(string card)
        {
            Assert.False(Core.Presets.Presets.SimpleCreditCard().IsMatch(card));
        }
    }

    public class PasswordPatternTests
    {
        [Fact]
        public void Default_Expression_IsHonest()
        {
            var p = Core.Presets.Presets.Password();
            // Default: allows uppercase, lowercase, digits; 8-128 length; no lookaheads
            Assert.Equal("[A-Za-z0-9]{8,128}", p.Expression);
        }

        [Fact]
        public void MinLength_ChangesQuantifier()
        {
            var p = Core.Presets.Presets.Password().MinLength(12);
            Assert.Equal("[A-Za-z0-9]{12,128}", p.Expression);
        }

        [Fact]
        public void MaxLength_ChangesQuantifier()
        {
            var p = Core.Presets.Presets.Password().MaxLength(64);
            Assert.Equal("[A-Za-z0-9]{8,64}", p.Expression);
        }

        [Fact]
        public void ExactLength_UsesExactQuantifier()
        {
            var p = Core.Presets.Presets.Password().MinLength(10).MaxLength(10);
            Assert.Equal("[A-Za-z0-9]{10}", p.Expression);
        }

        [Fact]
        public void AllowUppercase_False_ExcludesUppercaseFromCharClass()
        {
            var p = Core.Presets.Presets.Password().AllowUppercase(false);
            Assert.Equal("[a-z0-9]{8,128}", p.Expression);
        }

        [Fact]
        public void AllowLowercase_False_ExcludesLowercaseFromCharClass()
        {
            var p = Core.Presets.Presets.Password().AllowLowercase(false);
            Assert.Equal("[A-Z0-9]{8,128}", p.Expression);
        }

        [Fact]
        public void AllowDigits_False_ExcludesDigitsFromCharClass()
        {
            var p = Core.Presets.Presets.Password().AllowDigits(false);
            Assert.Equal("[A-Za-z]{8,128}", p.Expression);
        }

        [Fact]
        public void AllowSpecial_True_AddsDefaultSpecialChars()
        {
            var p = Core.Presets.Presets.Password().AllowSpecial();
            Assert.Contains("!", p.Expression);
            Assert.Contains("@", p.Expression);
            Assert.Contains("#", p.Expression);

            Assert.DoesNotContain("(?=", p.Expression);
        }

        [Fact]
        public void AllowSpecial_False_ExcludesSpecialChars()
        {
            var p = Core.Presets.Presets.Password().AllowSpecial().AllowSpecial(false);
            Assert.DoesNotContain("!", p.Expression);
            Assert.DoesNotContain("@", p.Expression);
        }

        [Fact]
        public void WithSpecialChars_SetsCustomCharsAndEnablesSpecial()
        {
            var p = Core.Presets.Presets.Password().WithSpecialChars('!', '?');
            Assert.Contains("!", p.Expression);
            Assert.Contains("?", p.Expression);
            Assert.DoesNotContain("@", p.Expression);
        }

        [Fact]
        public void Immutability_ChangingOneInstanceDoesNotAffectAnother()
        {
            var p1 = Core.Presets.Presets.Password();
            var p2 = p1.AllowSpecial();

            Assert.DoesNotContain("!", p1.Expression);
            Assert.Contains("!", p2.Expression);
        }

        [Theory]
        [InlineData("Abcdef12")]
        [InlineData("Password123")]
        [InlineData("A1b2c3d4")]
        [InlineData("abcdefgh")]
        [InlineData("ABCDEFGH")]
        [InlineData("Abcdefgh")]
        public void IsMatch_MatchingStrings_ReturnsTrue(string password)
        {
            Assert.True(Core.Presets.Presets.Password().IsMatch(password));
        }

        [Theory]
        [InlineData("Abc12")]
        [InlineData("abc defgh")]
        [InlineData("Abcdef1!")]
        public void IsMatch_NonMatchingStrings_ReturnsFalse(string password)
        {
            Assert.False(Core.Presets.Presets.Password().IsMatch(password));
        }

        [Fact]
        public void AllowSpecial_PasswordWithSpecialMatches()
        {
            var p = Core.Presets.Presets.Password().AllowSpecial();
            Assert.True(p.IsMatch("Abcdef1!"));
            Assert.True(p.IsMatch("abcdefgh"));
        }

        [Fact]
        public void AllowUppercase_False_RejectsUppercase()
        {
            var p = Core.Presets.Presets.Password().AllowUppercase(false);
            Assert.True(p.IsMatch("abcdefgh12"));
            Assert.False(p.IsMatch("Abcdefgh12"));
        }

        [Fact]
        public void AllowDigits_False_RejectsDigits()
        {
            var p = Core.Presets.Presets.Password().AllowDigits(false);
            Assert.True(p.IsMatch("Abcdefgh"));
            Assert.False(p.IsMatch("Abcdef12"));
        }

        [Fact]
        public void OnlyLowercaseAndDigits_PinPattern()
        {
            var p = Core.Presets.Presets.Password()
                .AllowUppercase(false)
                .MinLength(4)
                .MaxLength(6);
            Assert.Equal("[a-z0-9]{4,6}", p.Expression);
            Assert.True(p.IsMatch("abcd12"));
            Assert.False(p.IsMatch("Abcd12"));
        }
        #region Validation

        [Fact]
        public void MinLength_Zero_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Core.Presets.Presets.Password().MinLength(0));
        }

        [Fact]
        public void MinLength_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Core.Presets.Presets.Password().MinLength(-1));
        }

        [Fact]
        public void MaxLength_Zero_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Core.Presets.Presets.Password().MaxLength(0));
        }

        [Fact]
        public void MaxLength_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Core.Presets.Presets.Password().MaxLength(-5));
        }

        [Fact]
        public void MaxLength_LessThanMinLength_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Core.Presets.Presets.Password().MinLength(20).MaxLength(10));
        }

        [Fact]
        public void WithSpecialChars_EmptyArray_Throws()
        {
            Assert.Throws<ArgumentException>(() => Core.Presets.Presets.Password().WithSpecialChars());
        }

        [Fact]
        public void AllCategoriesDisabled_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => Core.Presets.Presets.Password()
                .AllowUppercase(false)
                .AllowLowercase(false)
                .AllowDigits(false));
        }

        [Fact]
        public void MinLength_EqualToMaxLength_IsValid()
        {
            var p = Core.Presets.Presets.Password().MinLength(10).MaxLength(10);
            Assert.Equal("[A-Za-z0-9]{10}", p.Expression);
        }

        #endregion
    }
    public class UsernamePatternTests
    {
        [Fact]
        public void Default_Expression_IsCorrect()
        {
            var p = Core.Presets.Presets.Username();
            Assert.Equal("[a-zA-Z_][a-zA-Z0-9_]{2,29}", p.Expression);
        }

        [Fact]
        public void MinLength_ChangesExpression()
        {
            var p = Core.Presets.Presets.Username().MinLength(5);
            Assert.Equal("[a-zA-Z_][a-zA-Z0-9_]{4,29}", p.Expression);
        }

        [Fact]
        public void MaxLength_ChangesExpression()
        {
            var p = Core.Presets.Presets.Username().MaxLength(20);
            Assert.Equal("[a-zA-Z_][a-zA-Z0-9_]{2,19}", p.Expression);
        }

        [Fact]
        public void AllowChars_AddsExtraCharacters()
        {
            var p = Core.Presets.Presets.Username().AllowChars('.', '-');
            Assert.Equal(@"[a-zA-Z_][a-zA-Z0-9_.\-]{2,29}", p.Expression);
        }

        [Fact]
        public void AllowChars_EscapesSpecialCharClassCharacters()
        {
            var p = Core.Presets.Presets.Username().AllowChars(']', '\\', '^', '-');
            Assert.Equal(@"[a-zA-Z_][a-zA-Z0-9_\]\\\^\-]{2,29}", p.Expression);
            Assert.True(p.IsMatch("ab]"));
            Assert.True(p.IsMatch("ab\\"));
        }

        [Fact]
        public void MustStartWithLetterOrUnderscore_NotDigit()
        {
            var p = Core.Presets.Presets.Username();
            Assert.False(p.IsMatch("1invalid"));
            Assert.False(p.IsMatch("2user"));
        }

        [Theory]
        [InlineData("john_doe")]
        [InlineData("Alice")]
        [InlineData("_private")]
        public void IsMatch_ValidUsernames_ReturnsTrue(string username)
        {
            Assert.True(Core.Presets.Presets.Username().IsMatch(username));
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("1user")]
        [InlineData("user name")]
        public void IsMatch_InvalidUsernames_ReturnsFalse(string username)
        {
            Assert.False(Core.Presets.Presets.Username().IsMatch(username));
        }

        [Fact]
        public void Immutability_ChangingOneDoesNotAffectAnother()
        {
            var p1 = Core.Presets.Presets.Username();
            var p2 = p1.AllowChars('.');

            Assert.DoesNotContain(".", p1.Expression);
            Assert.Contains(".", p2.Expression);
        }
        [Fact]
        public void AllowChars_AccumulatesAcrossCalls()
        {
            var p = Core.Presets.Presets.Username().AllowChars('.').AllowChars('-');
            Assert.Equal(@"[a-zA-Z_][a-zA-Z0-9_.\-]{2,29}", p.Expression);
            Assert.True(p.IsMatch("a.b"));
            Assert.True(p.IsMatch("a-b"));
            Assert.True(p.IsMatch("a.-b"));
        }
    }
    public class IbanTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.Iban();
            Assert.Equal(@"[A-Z]{2}\d{2}[A-Z0-9]{4}(?:[A-Z0-9]{4,}){3}", p.Expression);
        }

        [Theory]
        [InlineData("GB82WEST12345698765432")]
        [InlineData("DE89370400440532013000")]
        [InlineData("FR7630006000011234567890189")]
        public void IsMatch_ValidIBANs_ReturnsTrue(string iban)
        {
            Assert.True(Core.Presets.Presets.Iban().IsMatch(iban));
        }

        [Theory]
        [InlineData("GB82WEST12345")]
        [InlineData("1234567890")]
        [InlineData("gb82west12345698765432")]
        public void IsMatch_InvalidIBANs_ReturnsFalse(string iban)
        {
            Assert.False(Core.Presets.Presets.Iban().IsMatch(iban));
        }
    }

    public class PostalCodePatternTests
    {
        [Fact]
        public void Default_Expression_IsGeneric()
        {
            var p = Core.Presets.Presets.PostalCode();
            Assert.Equal("[A-Z0-9]{3,10}", p.Expression);
        }

        [Fact]
        public void ForCountry_US_ReturnsUSPattern()
        {
            var p = Core.Presets.Presets.PostalCode().ForCountry("US");
            Assert.Equal(@"\d{5}(?:-\d{4})?", p.Expression);
        }

        [Fact]
        public void ForCountry_UK_ReturnsUKPattern()
        {
            var p = Core.Presets.Presets.PostalCode().ForCountry("UK");
            Assert.Equal(@"[A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}", p.Expression);
        }

        [Fact]
        public void Custom_SetsCustomPattern()
        {
            var p = Core.Presets.Presets.PostalCode().Custom(@"\d{6}");
            Assert.Equal(@"\d{6}", p.Expression);
        }

        [Theory]
        [InlineData("12345", "US")]
        [InlineData("90210-1234", "US")]
        [InlineData("SW1A 1AA", "UK")]
        [InlineData("EC1A1BB", "UK")]
        public void IsMatch_ValidPostalCodes_ReturnsTrue(string postalCode, string country)
        {
            Assert.True(Core.Presets.Presets.PostalCode().ForCountry(country).IsMatch(postalCode));
        }

        [Theory]
        [InlineData("1234", "US")]
        [InlineData("123456", "US")]
        public void IsMatch_InvalidPostalCodes_ReturnsFalse(string postalCode, string country)
        {
            Assert.False(Core.Presets.Presets.PostalCode().ForCountry(country).IsMatch(postalCode));
        }

        [Fact]
        public void UnknownCountry_ReturnsGenericPattern()
        {
            var p = Core.Presets.Presets.PostalCode().ForCountry("XX");
            Assert.Equal("[A-Z0-9]{3,10}", p.Expression);
        }

        [Fact]
        public void Immutability_ChangingOneDoesNotAffectAnother()
        {
            var p1 = Core.Presets.Presets.PostalCode();
            var p2 = p1.ForCountry("US");

            Assert.Equal("[A-Z0-9]{3,10}", p1.Expression);
            Assert.Equal(@"\d{5}(?:-\d{4})?", p2.Expression);
        }
    }
}
