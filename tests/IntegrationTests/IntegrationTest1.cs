using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlainML;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlainML.Entities;
using PlainML.Infrastructure;
using System.IO;
using System.Linq;
using System;

namespace IntegrationTests;

[TestClass]
public class IntegrationTest1
{
    public async static Task<PlainMLService> GetPlainMLService()
    {
        IServiceProvider provider = new ServiceCollection()
            .UsePlainMLSqLite()
            .UseArtifactStorageFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();

        var dbContextFactory = provider.GetRequiredService<IDbContextFactory<PlainMLContext>>();
        var artifactStorage = provider.GetRequiredService<IArtifactStorage>();

        var s = new PlainMLService(dbContextFactory, artifactStorage);
        await s.Migrate();
        return s;
    }

    [TestMethod]
    public async Task CreateExperimentTest()
    {
        const string experimentName = "TestExperiment1";

        PlainMLService s = await GetPlainMLService();
        var result = await s.GetOrCreateExperiment(experimentName);

        Assert.AreEqual(experimentName, result.Name);
        Assert.AreEqual(0, result.Runs.Count);
    }

    [TestMethod]
    public async Task StartTrainingTest()
    {
        const string experimentName = "TestExperiment1";

        PlainMLService s = await GetPlainMLService();

        int runId = await s.StartRun(experimentName);

        await Task.Delay(10); // Long running training process

        var parameters = new Parameter[]
        {
            new(){ Name = "p1", Value = 1 },
            new(){ Name = "p2", Value = 2 }
        };

        var metrics = new Metric[]
        {
            new(){ Name = "m1", Value = 0.45f }
        };

        await s.EndRun(runId, parameters, null, metrics, null);

        Run run = await s.GetRun(runId);
        Assert.AreEqual("p1", run.Parameters[0].Name);
        //TODO: Asserts
    }

    [TestMethod]
    public async Task GetDeployedRunTest()
    {
        const string experimentName = "Experiment1";

        PlainMLService s = await GetPlainMLService();

        // Create run and deploy
        int runId = await s.StartRun(experimentName);
        Assert.AreEqual(1, runId);
        await s.EndRun(runId, null, null, null, "./TestFiles/");
        await s.DeployRun(runId);

        // Create second run
        runId = await s.StartRun(experimentName);
        Assert.AreEqual(2, runId);
        await s.EndRun(runId, null, null, null, "./TestFiles/");
        // dont deploy

        Run? run = await s.GetDeployedRun(experimentName);
        Assert.AreEqual(1, run?.Id);
    }

    [TestMethod]
    public async Task GetArtifactsfromDeployedRunTest()
    {
        const string experimentName = "Experiment1";
        const string pathValidation = "./ArtifactsValidation";
        if(Directory.Exists(pathValidation))
        {
            Directory.Delete(pathValidation, true);
        }

        PlainMLService s = await GetPlainMLService();

        // Create run and deploy
        int runId = await s.StartRun(experimentName);
        await s.EndRun(runId, null, null, null, "./TestFiles/");
        await s.DeployRun(runId);

        Run run = await s.GetDeployedRun(experimentName) ?? throw new NullReferenceException();
        await s.DownloadArtifacts(run.Id, pathValidation);

        int filesCount = Directory.EnumerateFiles(pathValidation).Count();
        Assert.AreEqual(1, filesCount);
    }

    [TestMethod]
    public async Task GetArtifactsTest()
    {
        const string pathValidation = "./ArtifactsValidation";
        if(Directory.Exists(pathValidation))
        {
            Directory.Delete(pathValidation, true);
        }

        PlainMLService s = await GetPlainMLService();

        // Create run with artifacts
        int runId = await s.StartRun("Test");
        await s.EndRun(runId, null, null, null, "./TestFiles/");

        await s.DownloadArtifacts(runId, pathValidation);

        int filesCount = Directory.EnumerateFiles(pathValidation).Count();
        Assert.AreEqual(1, filesCount);
    }
}
