using CourseLedger.Models;

namespace CourseLedger.Tests;

public class NameFormatAttributeTests
{
    private readonly NameFormatAttribute _attribute = new();

    [Fact]
    public void IsValid_AcceptsTwoWordName()
    {
        Assert.True(_attribute.IsValid("John Smith"));
    }

    [Fact]
    public void IsValid_RejectsSingleWord()
    {
        Assert.False(_attribute.IsValid("John"));
    }

    [Fact]
    public void IsValid_RejectsThreeWords()
    {
        Assert.False(_attribute.IsValid("John Paul Smith"));
    }

    [Fact]
    public void IsValid_AllowsNullForRequiredToHandle()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_TrimsExtraSpaces_ButStillRequiresExactlyTwoParts()
    {
        Assert.True(_attribute.IsValid("  John   Smith  "));
    }

    [Fact]
    public void IsValid_RejectsHyphenatedSingleTokenAsOneWord()
    {
        Assert.False(_attribute.IsValid("Mary-Jane"));
    }
}
