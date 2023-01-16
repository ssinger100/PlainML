namespace ML_Trackingstore;

public interface IArtifactStorage
{
    Task Upload(int rundId, string localpath);
    Task Download(int runId, string localpath);
}