using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.API.Services;

public class SampleProvider : ISampleProvider
{
    public string[] GetSamples()
    {
        var file = File.ReadAllLines("datasample/reviews.txt");
        return file.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
    }
}