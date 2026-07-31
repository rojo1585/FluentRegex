namespace FluentRegex.Core.Tests.Presets
{
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
}
