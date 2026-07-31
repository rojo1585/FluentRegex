using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
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
}
