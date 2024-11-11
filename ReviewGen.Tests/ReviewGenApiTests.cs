using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ReviewGen.API.Models;

namespace ReviewGen.Tests;

public class ReviewGenApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReviewGenApiTests(CustomWebApplicationFactory webApplicationFactory)
    {
        _client = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("http://localhost:8080")
        });
    }

    [Fact]
    public async Task GenerateReview_ReturnsResult()
    {
        //arrange

        //act
        var response = await _client.GetAsync("/api/generate");

        //assert
        response.EnsureSuccessStatusCode();
        var review = await response.Content.ReadFromJsonAsync<Review>();
        Assert.NotNull(review);
        Assert.NotEmpty(review.Text);
        Assert.InRange(review.Rating, 1m, 5m);
    }
}