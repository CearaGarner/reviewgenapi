using Sentiment = ReviewGen.API.Models.Sentiment;

namespace ReviewGen.API.Services.Abstractions;

public interface ISentimentAnalyzer
{
    Task<Sentiment> GetAnalysis(string text, CancellationToken cancellationToken = default);
}