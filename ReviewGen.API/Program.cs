using ReviewGen.API.DependencyInjection;
using ReviewGen.API.Middleware;
using ReviewGen.API.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        pb => pb.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAllOrigins");
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionLoggingMiddleware>();

app.MapGet("/api/generate", async (IReviewGenerator reviewGenerator) => await reviewGenerator.GenerateReview())
    .WithName("GenerateReview")
    .WithDescription("Get a generated review")
    .WithSummary("Get a generated review")
    .WithOpenApi();

app.Warmup();

app.Run();

public partial class Program
{
    /*required for tests*/
}