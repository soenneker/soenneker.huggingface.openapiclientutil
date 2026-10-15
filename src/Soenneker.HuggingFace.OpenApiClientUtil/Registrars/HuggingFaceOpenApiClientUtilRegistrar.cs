using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.HuggingFace.HttpClients.Registrars;
using Soenneker.HuggingFace.OpenApiClientUtil.Abstract;

namespace Soenneker.HuggingFace.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class HuggingFaceOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="HuggingFaceOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddHuggingFaceOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHuggingFaceOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IHuggingFaceOpenApiClientUtil, HuggingFaceOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="HuggingFaceOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHuggingFaceOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHuggingFaceOpenApiHttpClientAsSingleton()
                .TryAddScoped<IHuggingFaceOpenApiClientUtil, HuggingFaceOpenApiClientUtil>();

        return services;
    }
}
