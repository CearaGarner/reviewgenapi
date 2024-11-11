using Azure;
using Azure.AI.TextAnalytics;
using Azure.Core;
using Markov;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ReviewGen.API.Configuration;
using ReviewGen.API.Services;
using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.API.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AzureClientOptions>(configuration.GetSection("AzureClient"));
        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AzureClientOptions>>();
            var credential = new AzureKeyCredential(options.Value.Key);
            var clientOptions = new TextAnalyticsClientOptions
            {
                Retry =
                {
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    Mode = RetryMode.Exponential
                }
            };
            return new TextAnalyticsClient(options.Value.Endpoint, credential, clientOptions);
        });

        services.TryAddSingleton<ISentimentAnalyzer>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AzureClientOptions>>();
            //if options are not initialized => use offline implementation, else use Azure Text Analytics
            if (string.IsNullOrEmpty(options.Value.Key) || options.Value.Endpoint is null)
                return new Services.SentimentAnalyzer();

            return new AzureTextAnalytics(
                provider.GetRequiredService<TextAnalyticsClient>(),
                provider.GetRequiredService<ILogger<AzureTextAnalytics>>());
        });
        services.TryAddSingleton<IRatingProvider, RatingProvider>();
        services.TryAddSingleton<ISampleProvider, SampleProvider>();
        services.TryAddSingleton<IReviewGenerator>(provider =>
        {
            var sampleProvider = provider.GetRequiredService<ISampleProvider>();
            var chain = BuildMarkovChain(sampleProvider);
            return new ReviewGenerator(chain, provider.GetRequiredService<IRatingProvider>());
        });

        return services;
    }

    private static MarkovChain<string> BuildMarkovChain(ISampleProvider sampleProvider)
    {
        var chain = new MarkovChain<string>(2);
        var samples = sampleProvider.GetSamples();
        foreach (var sample in samples)
            chain.Add(sample.Split((char[]) [' ', ',', ';', '-', ':'],
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));

        return chain;
    }
}