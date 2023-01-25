using Microsoft.Extensions.DependencyInjection;
using PlainML;
using PlainML.Entities;
using PlainML.Infrastructure;

// Use dependency injection to configure database and artifact storage
var _provider = new ServiceCollection()
            .UsePlainMLSqLite()
            .UseArtifactStorageFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();

const string experimentName = "TestExperiment";
const string artifactsPath = "./Artifacts";

// Create database
PlainMLService s = _provider.GetRequiredService<PlainMLService>();
await s.Migrate();

// Train model
int rundId = await s.StartRun(experimentName);
float metricValue = await TrainModel(artifactsPath);
await s.EndRun(
    rundId,
    parameters: new[] { new Parameter(){ Name = "Parameter1", Value = 1.123f } },
    parameters_StringType: new[] { new Parameter_StringType(){ Name = "Trainers", Value = "LightGbm, OneVersusAllTrainer" } },
    metrics: new[] { new Metric(){ Name = "Trainers", Value = metricValue } },
    artifactsPath);

// Deploy model
await s.DeployRun(rundId);

// Use model
var deployedRun = await s.GetDeployedRun(experimentName) ?? throw new NullReferenceException();
await s.DownloadArtifacts(deployedRun.Id, "./DownloadedArtifacts");

Console.WriteLine("Artifacts of run in ./DownloadedArtifacts:");
foreach (var item in Directory.EnumerateFiles("./DownloadedArtifacts"))
{
    Console.WriteLine(item);
}



async static Task<float> TrainModel(string artifactsPath)
{
    Console.WriteLine("Training...");
    await Task.Delay(100);

    if (Directory.Exists(artifactsPath))
    {
        Console.WriteLine("Directory exists. Delete it!");
        Directory.Delete(artifactsPath, true);
    }

    Directory.CreateDirectory(artifactsPath);
    await File.WriteAllTextAsync(Path.Combine(artifactsPath, "TestFile.bin"), "0011010101001");

    return 0.1f;
}