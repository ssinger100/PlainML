namespace ML_Trackingstore.Infrastructure.ArtifactStorages;

public class FilesystemStorage : IArtifactStorage
{
    readonly string _basepath;
    public FilesystemStorage(string basepath)
    {
        _basepath = basepath;
    }

    public Task Download(int runId, string localpath)
    {
        throw new NotImplementedException();
    }

    public Task Upload(int rundId, string localpath)
    {
        throw new NotImplementedException();
    }
}