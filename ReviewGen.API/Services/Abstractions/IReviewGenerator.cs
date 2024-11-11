using ReviewGen.API.Models;

namespace ReviewGen.API.Services.Abstractions;

public interface IReviewGenerator
{
    Task<Review> GenerateReview(CancellationToken cancellationToken = default);
}