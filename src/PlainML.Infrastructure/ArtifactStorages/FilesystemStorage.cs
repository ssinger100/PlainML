using PlainML.Entities;

namespace PlainML.Infrastructure.ArtifactStorages;

public class FilesystemStorage : IArtifactStorage
{
    readonly string _basepath;
    public FilesystemStorage(string basepath)
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