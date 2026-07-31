using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
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
}
