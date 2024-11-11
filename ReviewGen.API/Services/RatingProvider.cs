using ReviewGen.API.Services.Abstractions;
using Sentiment = ReviewGen.API.Models.Sentiment;

namespace ReviewGen.API.Services;

public class RatingProvider(ISentimentAnalyzer sentimentAnalyzer) : IRatingProvider
{
    private static readonly decimal[] Bad = [1m, 1.5m, 2m];
    private static readonly decimal[] BelowAverage = [2.5m, 3m];
    private static readonly decimal[] AboveAverage = [3m, 3.5m];
    private static readonly decimal[] Good = [4m, 4.5m, 5m];

    private static readonly Random Random = new();

    public async Task<decimal> Rate(string text, CancellationToken cancellationToken = default)
    {
        var prediction = await sentimentAnalyzer.GetAnalysis(text, cancellationToken);
        return MapResult(prediction);
    }

    private static decimal MapResult(Sentiment sentiment)
    {
        return sentiment switch
        {
            Sentiment.Positive => Good[Random.Next(Good.Length)],
            Sentiment.Neutral => AboveAverage[Random.Next(AboveAverage.Length)],
            Sentiment.Mixed => BelowAverage[Random.Next(BelowAverage.Length)],
            Sentiment.Negative => Bad[Random.Next(Bad.Length)],
            _ => throw new ArgumentOutOfRangeException(nameof(sentiment), sentiment, null)
        };
    }
}