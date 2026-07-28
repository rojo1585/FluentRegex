using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests
{
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

}
