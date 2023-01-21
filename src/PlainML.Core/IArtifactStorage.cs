using PlainML.Entities;

namespace PlainML;

public interface IArtifactStorage
{
    Task Upload(Run run, string localpath);
    Task Download(Run run, string localpath);
}