using Microsoft.Extensions.DependencyInjection;
using ML_Trackingstore.Infrastructure.ArtifactStorages;

namespace ML_Trackingstore.Infrastructure;

public static class ArtifactStorageExtensionMethods
{
    public static IServiceCollection UseFilesystem(this IServiceCollection services, string? directory = null)
    {
        directory ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ML-Tackingstore", "Storage");
        return services.AddTransient<IArtifactStorage>(x => new FilesystemStorage(directory));
    }

    public static IServiceCollection UseDatabase()
    {
        throw new NotImplementedException(nameof(UseDatabase));
    }
}
