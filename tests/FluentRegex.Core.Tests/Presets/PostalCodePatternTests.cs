using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
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
