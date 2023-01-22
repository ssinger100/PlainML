﻿using Microsoft.Extensions.DependencyInjection;
using PlainML.Infrastructure.ArtifactStorages;

namespace PlainML.Infrastructure;

public static class ArtifactStorageExtensionMethods
{
    public static IServiceCollection UseArtifactStorageFilesystem(this IServiceCollection services, string? directory = null)
    {
        directory ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ML-Tackingstore", "Storage");
        return services.AddTransient<IArtifactStorage>(x => new FilesystemStorage(directory));
    }

    public static IServiceCollection UseArtifactStorageSQLServer()
    {
        throw new NotImplementedException(nameof(UseArtifactStorageSQLServer));
    }

    public static IServiceCollection UseArtifactStorageMicrosoftAzure()
    {
        throw new NotImplementedException(nameof(UseArtifactStorageMicrosoftAzure));
    }
}
