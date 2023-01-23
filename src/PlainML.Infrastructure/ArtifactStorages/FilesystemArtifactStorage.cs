using System.IO.Compression;
using PlainML.Entities;

namespace PlainML.Infrastructure.ArtifactStorages;

public class FilesystemArtifactStorage : IArtifactStorage
{
    readonly string _basepath;

    public FilesystemArtifactStorage(string basepath)
    {
        _basepath = basepath;
    }

    public Task Download(Run run, string localpath)
    {
        if (!Directory.Exists(_basepath))
        {
            throw new DirectoryNotFoundException();
        }

        if (!Directory.Exists(localpath))
        {
            Directory.CreateDirectory(localpath);
        }

        string zipPath = CreateZipPath(run);
        ZipFile.ExtractToDirectory(zipPath, localpath); //TODO: Do it async

        return Task.CompletedTask;
    }

    public Task Upload(Run run, string localpath)
    {
        if (!Directory.Exists(_basepath))
        {
            Directory.CreateDirectory(_basepath);
        }

        string zipPath = CreateZipPath(run);
        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }
        ZipFile.CreateFromDirectory(localpath, zipPath, CompressionLevel.Optimal, false); //TODO: Do it async

        return Task.CompletedTask;
    }

    string CreateZipPath(Run run)
    {
        return Path.Combine(_basepath, $"{run.Id}.zip");
    }
}