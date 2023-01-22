using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlainML;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlainML.Entities;
using PlainML.Infrastructure;

namespace IntegrationTests;

[TestClass]
public class IntegrationTest1
{
    readonly ServiceProvider _provider;

    public IntegrationTest1()
    {
        _provider = new ServiceCollection()
            .UsePlainMLSqLite()
            .UseFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();
    }

    [TestInitialize]
    public async Task InitSQLLite()
    {
        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<PlainMLContext>>();
        using var context = await dbContextFactory.CreateDbContextAsync();
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }

    [TestMethod]
    public async Task CreateExperimentTest()
    {
        string experimentName = "TestExperiment1";

        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<PlainMLContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new PlainMLService(dbContextFactory, artifactStorage);
        var result = await store.GetOrCreateExperiment(experimentName);

        Assert.AreEqual(experimentName, result.Name);
        Assert.AreEqual(0, result.Runs.Count);
    }

    [TestMethod]
    public async Task StartTrainingTest()
    {
        string experimentName = "TestExperiment1";

        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<PlainMLContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new PlainMLService(dbContextFactory, artifactStorage);

        int runId = await store.StartRun(experimentName);

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

        await store.EndRun(runId, parameters, null, metrics);

        Run run = await store.GetRun(runId);
        Assert.AreEqual("p1", run.Parameters[0].Name);
        //TODO: Asserts
    }

    [TestMethod]
    public async Task GetDeployedRunTest()
    {
        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<PlainMLContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new PlainMLService(dbContextFactory, artifactStorage);
       // store.GetDeployedRun();
    }

    [TestMethod]
    public async Task GetArtifactsTest()
    {
        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<PlainMLContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new PlainMLService(dbContextFactory, artifactStorage);

        // Create run
        int runId = await store.StartRun("Test");
        await store.EndRun(runId, null, null, null);
        string path = "./Artifacts";
        // await store.GetArtifacts(runId, path);
        // int filesCount = Directory.EnumerateFiles(path).Count();
        Assert.Inconclusive();
       // Assert.AreEqual(1, filesCount);
    }
}