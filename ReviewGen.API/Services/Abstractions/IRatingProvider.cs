namespace ReviewGen.API.Services.Abstractions;

public interface IRatingProvider
{
    Task<decimal> Rate(string text, CancellationToken cancellationToken = default);
}