using Markov;
using ReviewGen.API.Models;
using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.API.Services;

public class ReviewGenerator(MarkovChain<string> chain, IRatingProvider ratingProvider) : IReviewGenerator
{
    private readonly Random _random = new();

    public async Task<Review> GenerateReview(CancellationToken cancellationToken = default)
    {
        var text = string.Join(' ', chain.Chain(_random));
        var rating = await ratingProvider.Rate(text, cancellationToken);
        var result = new Review(text, rating);
        return result;
    }
}