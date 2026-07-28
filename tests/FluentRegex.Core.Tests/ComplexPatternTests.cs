namespace FluentRegex.Core.Tests
{
    public class ComplexPatternTests
    {
        [Fact]
        public void SimpleEmail_LikePattern()
        {
            var local = Pattern.Text().AllowDigits().AllowChars('.', '_', '%', '+', '-');
            var domain = Pattern.Text().AllowDigits().AllowChars('-', '.').MinLength(2);
            var email = local + "@" + domain;

            Assert.True(email.IsMatch("user@example.com"));
            Assert.True(email.IsMatch("john.doe@company.co.uk"));
            Assert.False(email.IsMatch("@example.com"));
            Assert.False(email.IsMatch("user@"));
        }

        [Fact]
        public void PhoneNumber_LikePattern()
        {
            var code = Pattern.Literal("+52").Optional();
            var digits = Pattern.Digit().Repeat(10);
            var phone = code + digits;

            Assert.True(phone.IsMatch("5512345678"));
            Assert.True(phone.IsMatch("+525512345678"));
            Assert.False(phone.IsMatch("123"));
        }

        [Fact]
        public void DateLikePattern()
        {
            var day = Pattern.Digit().Repeat(2);
            var sep = Pattern.Literal("/");
            var month = Pattern.Digit().Repeat(2);
            var year = Pattern.Digit().Repeat(4);
            var date = day + sep + month + sep + year;

            Assert.True(date.IsMatch("15/06/2025"));
            Assert.False(date.IsMatch("2025-06-15"));
        }

        [Fact]
        public void Alternation_WithConcatenation()
        {
            var http = Pattern.Literal("http") + Pattern.Literal("s").Optional();
            var ftp = Pattern.Literal("ftp");
            var protocol = http | ftp;

            Assert.True(protocol.IsMatch("http"));
            Assert.True(protocol.IsMatch("https"));
            Assert.True(protocol.IsMatch("ftp"));
            Assert.False(protocol.IsMatch("ssh"));
        }
    }

}
