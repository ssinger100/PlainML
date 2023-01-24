using Microsoft.Extensions.DependencyInjection;
using PlainML;
using PlainML.Infrastructure;

Console.WriteLine("Hello, World!");

// Use dependency injection to configure database and artifact storage
var _provider = new ServiceCollection()
            .UsePlainMLSqLite()
            .UseArtifactStorageFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();

string experimentName = "TestExperiment";
string artifactsPath = "./Artifacts";

// Create database
PlainMLService s = _provider.GetRequiredService<PlainMLService>();
await s.Migrate();

// Train model
int rundId = await s.StartRun(experimentName);
await TrainModel(artifactsPath);
await s.EndRun(rundId, null, null, null, artifactsPath);

// Deploy model
await s.DeployRun(rundId);

// Use model
var deployedRun = await s.GetDeployedRun(experimentName) ?? throw new NullReferenceException();
await s.DownloadArtifacts(deployedRun.Id, "./DownloadedArtifacts");

Console.WriteLine("Files in ./DownloadedArtifacts:");
foreach (var item in Directory.EnumerateFiles("./DownloadedArtifacts"))
{
    Console.WriteLine(item);
}

async static Task TrainModel(string artifactsPath)
{
    Console.WriteLine("Training...");
    await Task.Delay(100);

    if (Directory.Exists(artifactsPath))
    {
        Directory.Delete(artifactsPath, true);
    }
    Directory.CreateDirectory(artifactsPath);
    await File.WriteAllTextAsync("TestFile.bin", "0011010101001");
}