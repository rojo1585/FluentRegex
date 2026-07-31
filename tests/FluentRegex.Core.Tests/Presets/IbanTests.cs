using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
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

}
