using FluentRegex.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentRegex.Core.Tests;

public class LiteralPatternTests
{
    [Fact]
    public void Literal_MatchesExactString()
    {
        var pattern = Pattern.Literal("hello");
        Assert.True(pattern.IsMatch("hello"));
        Assert.False(pattern.IsMatch("Hello"));
        Assert.False(pattern.IsMatch("hello!"));
    }

    [Fact]
    public void Literal_EscapesSpecialChars()
    {
        var pattern = Pattern.Literal("1.0");
        Assert.Equal(@"1\.0", (string)pattern);
        Assert.True(pattern.IsMatch("1.0"));
        Assert.False(pattern.IsMatch("1X0"));
    }

    [Fact]
    public void Literal_EmptyString()
    {
        var pattern = Pattern.Literal("");
        Assert.True(pattern.IsMatch(""));
    }

    [Fact]
    public void Literal_ThrowsOnNull()
    {
        Assert.Throws<ArgumentNullException>(() => Pattern.Literal(null!));
    }
}






