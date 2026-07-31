using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
