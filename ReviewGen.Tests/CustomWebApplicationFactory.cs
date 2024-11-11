using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReviewGen.API.Services.Abstractions;

namespace ReviewGen.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var serviceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ISampleProvider));
            services.Remove(serviceDescriptor!);

            services.TryAddSingleton<ISampleProvider, TestSampleProvider>();
        });
    }
}