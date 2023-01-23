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
        throw new NotImplementedException();
    }

    public Task Upload(Run run, string localpath)
    {
        throw new NotImplementedException();
    }
}