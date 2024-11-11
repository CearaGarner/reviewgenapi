using Azure;
using Azure.AI.TextAnalytics;
using ReviewGen.API.Services.Abstractions;
using Sentiment = ReviewGen.API.Models.Sentiment;

namespace ReviewGen.API.Services;

public class AzureTextAnalytics(TextAnalyticsClient client, ILogger<AzureTextAnalytics> logger) : ISentimentAnalyzer
{
    public async Task<Sentiment> GetAnalysis(string text, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.AnalyzeSentimentAsync(text, cancellationToken: cancellationToken);
            return MapResult(response);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError($"Error occured when trying to connect to the Azure Text Analytics service: {ex.Message}");
            throw;
        }
    }

    private Sentiment MapResult(DocumentSentiment documentSentiment)
    {
        return documentSentiment.Sentiment switch
        {
            TextSentiment.Positive => Sentiment.Positive,
            TextSentiment.Neutral => Sentiment.Neutral,
            TextSentiment.Negative => Sentiment.Negative,
            TextSentiment.Mixed => Sentiment.Mixed,
            _ => throw new ArgumentOutOfRangeException(nameof(documentSentiment), documentSentiment,
                "Sentiment analysis result does not fall into expected value range")
        };
    }
}