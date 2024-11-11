using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.API.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static void Warmup(this IApplicationBuilder app)
    {
        //This will cause Markov chain to be initialized on application startup, to avoid this lengthy process on first request 
        var reviewGenerator = app.ApplicationServices.GetRequiredService<IReviewGenerator>();
    }
}