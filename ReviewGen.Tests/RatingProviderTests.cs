using FluentAssertions;
using NSubstitute;
using ReviewGen.API.Models;
using ReviewGen.API.Services;
using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.Tests;

public class RatingProviderTests
{
    private readonly ISentimentAnalyzer _sentimentAnalyzer;
    private readonly RatingProvider _sut;

    public RatingProviderTests()
    {
        _sentimentAnalyzer = Substitute.For<ISentimentAnalyzer>();
        _sut = new RatingProvider(_sentimentAnalyzer);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public async Task Rate_ReturnsExpectedRange(Sentiment sentiment, decimal[] expectedRatings)
    {
        //arrange
        _sentimentAnalyzer.GetAnalysis(default).ReturnsForAnyArgs(sentiment);

        //act
        var rating = await _sut.Rate("review text");

        //assert
        rating.Should().BeOneOf(expectedRatings);
    }

    [Fact]
    public async Task ThrowsOnUnknownSentiment()
    {
        //arrange
        _sentimentAnalyzer.GetAnalysis(default).ReturnsForAnyArgs((Sentiment)5);

        //act
        Func<Task> act = async () => await _sut.Rate("review text");

        //assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    public static IEnumerable<object[]> GetTestData()
    {
        yield return [Sentiment.Positive, new[] { 4m, 4.5m, 5m }];
        yield return [Sentiment.Neutral, new[] { 3m, 3.5m }];
        yield return [Sentiment.Mixed, new[] { 2.5m, 3m }];
        yield return [Sentiment.Negative, new[] { 1m, 1.5m, 2m }];
    }
}