using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
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
}
