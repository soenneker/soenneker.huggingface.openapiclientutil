using Soenneker.HuggingFace.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.HuggingFace.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IHuggingFaceOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>Gets the cached, authenticated Hugging Face Hub client.</summary>
    ValueTask<HuggingFaceOpenApiClient> Get(CancellationToken cancellationToken = default);
}
