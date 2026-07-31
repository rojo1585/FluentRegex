using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentRegex.Core.Tests.Presets
{
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
}
