using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soenneker.HuggingFace.HttpClients.Abstract;
using Soenneker.HuggingFace.OpenApiClientUtil.Abstract;
using Soenneker.HuggingFace.OpenApiClientUtil.Registrars;

namespace Soenneker.HuggingFace.OpenApiClientUtil.Tests;

public sealed class HuggingFaceOpenApiClientUtilTests
{
    [Test]
    public async Task Client_uses_configured_endpoint_authentication_and_cached_instance()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["HuggingFace:ApiKey"] = "test-token",
            ["HuggingFace:ClientBaseUrl"] = "https://hub.example.test"
        }).Build();
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(configuration);
        services.AddHuggingFaceOpenApiClientUtilAsSingleton();
        await using ServiceProvider provider = services.BuildServiceProvider();
        var utility = provider.GetRequiredService<IHuggingFaceOpenApiClientUtil>();
        var client = await utility.Get();
        var cached = await utility.Get();
        var httpClient = await provider.GetRequiredService<IHuggingFaceOpenApiHttpClient>().Get();

        await Assert.That(ReferenceEquals(client, cached)).IsTrue();
        await Assert.That(client.Api.WhoamiV2.ToGetRequestInformation().URI.AbsoluteUri).IsEqualTo("https://hub.example.test/api/whoami-v2");
        await Assert.That(httpClient.DefaultRequestHeaders.Authorization!.Scheme).IsEqualTo("Bearer");
        await Assert.That(httpClient.DefaultRequestHeaders.Authorization.Parameter).IsEqualTo("test-token");
    }
}
