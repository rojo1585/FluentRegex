using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
    public class PasswordPatternTestsPasswordPatternTests
    {
        [Fact]
        public void Default_Expression_IsHonest()
        {
            var p = Core.Presets.Presets.Password();
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
}
