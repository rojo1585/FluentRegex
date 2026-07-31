namespace FluentRegex.Core.Tests.Presets
{
    public class HexColorTests
    {
        [Fact]
        public void Expression_MatchesExpected()
        {
            var p = Core.Presets.Presets.HexColor();
            Assert.Equal(@"#[0-9a-fA-F]{3}(?:[0-9a-fA-F]{3})?", p.Expression);
        }

        [Theory]
        [InlineData("#fff")]
        [InlineData("#aabbcc")]
        [InlineData("#012345")]
        public void IsMatch_ValidHexColors_ReturnsTrue(string color)
        {
            Assert.True(Core.Presets.Presets.HexColor().IsMatch(color));
        }

        [Theory]
        [InlineData("#gggggg")]
        [InlineData("123456")]
        [InlineData("#12")]
        public void IsMatch_InvalidHexColors_ReturnsFalse(string color)
        {
            Assert.False(Core.Presets.Presets.HexColor().IsMatch(color));
        }
    }

}
