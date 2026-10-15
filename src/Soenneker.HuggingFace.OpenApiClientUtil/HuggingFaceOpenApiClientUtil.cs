using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.HuggingFace.HttpClients.Abstract;
using Soenneker.HuggingFace.OpenApiClientUtil.Abstract;
using Soenneker.HuggingFace.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.HuggingFace.OpenApiClientUtil;

public sealed class HuggingFaceOpenApiClientUtil : IHuggingFaceOpenApiClientUtil
{
    private readonly AsyncSingleton<HuggingFaceOpenApiClient> _client;

    public HuggingFaceOpenApiClientUtil(IHuggingFaceOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<HuggingFaceOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("HuggingFace:ApiKey");
            string authHeaderName = configuration["HuggingFace:AuthHeaderName"] ?? "Authorization";
            string authHeaderValueTemplate = configuration["HuggingFace:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerName: authHeaderName, headerValue: authHeaderValue),
                httpClient: httpClient)
            {
                BaseUrl = configuration["HuggingFace:ClientBaseUrl"] ?? "https://huggingface.co"
            };

            return new HuggingFaceOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<HuggingFaceOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
