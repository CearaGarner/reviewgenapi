using ReviewGen.API.Services.Abstractions;
using SentimentAnalyzer;
using SentimentAnalyzer.Models;
using Sentiment = ReviewGen.API.Models.Sentiment;

namespace ReviewGen.API.Services;

public class SentimentAnalyzer : ISentimentAnalyzer
{
    public Task<Sentiment> GetAnalysis(string text, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(MapResult(Sentiments.Predict(text)));
    }

    private static Sentiment MapResult(SentimentPrediction sentimentPrediction)
    {
        return sentimentPrediction switch
        {
            { Prediction: true, Score: > 10f } => Sentiment.Positive,
            { Prediction: true, Score: > 0 and < 10f } => Sentiment.Neutral,
            { Prediction: false, Score: < 0 and > -10f } => Sentiment.Mixed,
            { Prediction: false, Score: < -10f } => Sentiment.Negative,
            _ => throw new ArgumentOutOfRangeException(nameof(sentimentPrediction), sentimentPrediction,
                "Sentiment analysis result does not fall into expected value range")
        };
    }
}