namespace FluentRegex.Core.Tests.Presets
{
    public class UsernamePatternTests
    {
        [Fact]
        public void Default_Expression_IsCorrect()
        {
            var p = Core.Presets.Presets.Username();
            Assert.Equal("[a-zA-Z_][a-zA-Z0-9_]{2,29}", p.Expression);
        }

        [Fact]
        public void MinLength_ChangesExpression()
        {
            var p = Core.Presets.Presets.Username().MinLength(5);
            Assert.Equal("[a-zA-Z_][a-zA-Z0-9_]{4,29}", p.Expression);
        }

        [Fact]
        public void MaxLength_ChangesExpression()
        {
            var p = Core.Presets.Presets.Username().MaxLength(20);
            Assert.Equal("[a-zA-Z_][a-zA-Z0-9_]{2,19}", p.Expression);
        }

        [Fact]
        public void AllowChars_AddsExtraCharacters()
        {
            var p = Core.Presets.Presets.Username().AllowChars('.', '-');
            Assert.Equal(@"[a-zA-Z_][a-zA-Z0-9_.\-]{2,29}", p.Expression);
        }

        [Fact]
        public void AllowChars_EscapesSpecialCharClassCharacters()
        {
            var p = Core.Presets.Presets.Username().AllowChars(']', '\\', '^', '-');
            Assert.Equal(@"[a-zA-Z_][a-zA-Z0-9_\]\\\^\-]{2,29}", p.Expression);
            Assert.True(p.IsMatch("ab]"));
            Assert.True(p.IsMatch("ab\\"));
        }

        [Fact]
        public void MustStartWithLetterOrUnderscore_NotDigit()
        {
            var p = Core.Presets.Presets.Username();
            Assert.False(p.IsMatch("1invalid"));
            Assert.False(p.IsMatch("2user"));
        }

        [Theory]
        [InlineData("john_doe")]
        [InlineData("Alice")]
        [InlineData("_private")]
        public void IsMatch_ValidUsernames_ReturnsTrue(string username)
        {
            Assert.True(Core.Presets.Presets.Username().IsMatch(username));
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("1user")]
        [InlineData("user name")]
        public void IsMatch_InvalidUsernames_ReturnsFalse(string username)
        {
            Assert.False(Core.Presets.Presets.Username().IsMatch(username));
        }

        [Fact]
        public void Immutability_ChangingOneDoesNotAffectAnother()
        {
            var p1 = Core.Presets.Presets.Username();
            var p2 = p1.AllowChars('.');

            Assert.DoesNotContain(".", p1.Expression);
            Assert.Contains(".", p2.Expression);
        }
        [Fact]
        public void AllowChars_AccumulatesAcrossCalls()
        {
            var p = Core.Presets.Presets.Username().AllowChars('.').AllowChars('-');
            Assert.Equal(@"[a-zA-Z_][a-zA-Z0-9_.\-]{2,29}", p.Expression);
            Assert.True(p.IsMatch("a.b"));
            Assert.True(p.IsMatch("a-b"));
            Assert.True(p.IsMatch("a.-b"));
        }
    }
}
