using Microsoft.Extensions.DependencyInjection;

namespace ML_Tackingstore.Infrastructure;

public static class ArtifactStorageExtensionMethods
{
    public static IServiceCollection UseLocalFilesystem(this IServiceCollection serviceCollection, string? directory)
    {
        //TODO: UseLocalFilesystem
        return serviceCollection;
    }
}
