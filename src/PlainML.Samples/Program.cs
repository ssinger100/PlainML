using Microsoft.Extensions.DependencyInjection;
using PlainML;
using PlainML.Entities;
using PlainML.Infrastructure;

Console.WriteLine("Hello, World!");

var _provider = new ServiceCollection()
            .UsePlainMLSqLite()
            .UseArtifactStorageFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();

string experimentName = "TestExperiment";
string deploymentTargetName = "Development";

PlainMLService s = _provider.GetRequiredService<PlainMLService>();

await s.EnsureCreated();
//await s.Migrate();

int rundId = await s.StartRun(experimentName);
await Task.Delay(100); // Training...
string artifactsPath = "./Artifacts";
if (Directory.Exists(artifactsPath))
{
    Directory.Delete(artifactsPath, true);
}
Directory.CreateDirectory(artifactsPath);
await File.WriteAllTextAsync("TestFile.bin", "0011010101001");
await s.EndRun(rundId, null, null, null, artifactsPath);

await s.DeployRun(rundId, deploymentTargetName);

var deployedRun = await s.GetDeployedRun(experimentName, deploymentTargetName) ?? throw new NullReferenceException();
await s.DownloadArtifacts(deployedRun.Id, "./DownloadedArtifacts");

Console.WriteLine("Files in ./DownloadedArtifacts:");
foreach (var item in Directory.EnumerateFiles("./DownloadedArtifacts"))
{
    Console.WriteLine(item);
}