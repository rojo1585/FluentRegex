namespace FluentRegex.Core.Tests.Presets
{
    public class UUIDTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.UUID();
            Assert.Equal(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", p.Expression);
        }

        [Theory]
        [InlineData("550e8400-e29b-41d4-a716-446655440000")]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
        public void IsMatch_ValidUUIDs_ReturnsTrue(string uuid)
        {
            Assert.True(Core.Presets.Presets.UUID().IsMatch(uuid));
        }

        [Theory]
        [InlineData("550e8400-e29b-41d4-a716")]
        [InlineData("not-a-uuid")]
        [InlineData("550e8400-e29b-41d4-a716-44665544000g")]
        public void IsMatch_InvalidUUIDs_ReturnsFalse(string uuid)
        {
            Assert.False(Core.Presets.Presets.UUID().IsMatch(uuid));
        }
    }
}
