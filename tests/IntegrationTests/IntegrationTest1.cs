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
            .UseSQLLite()
            .UseFilesystem()
            .AddTransient<MLOpsTrackingStore>()
            .BuildServiceProvider();
    }

    [TestInitialize]
    public async Task InitSQLLite()
    {
        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();
        using var context = await dbContextFactory.CreateDbContextAsync();
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }

    [TestMethod]
    public async Task CreateExperimentTest()
    {
        string experimentName = "TestExperiment1";

        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new MLOpsTrackingStore(dbContextFactory, artifactStorage);
        var result = await store.GetOrCreateExperiment(experimentName);

        Assert.AreEqual(experimentName, result.Name);
        Assert.AreEqual(0, result.Runs.Count);
    }

    [TestMethod]
    public async Task StartTrainingTest()
    {
        string experimentName = "TestExperiment1";

        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new MLOpsTrackingStore(dbContextFactory, artifactStorage);

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
        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new MLOpsTrackingStore(dbContextFactory, artifactStorage);
       // store.GetDeployedRun();
    }

    [TestMethod]
    public async Task GetArtifactsTest()
    {
        var dbContextFactory = _provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();
        var artifactStorage = _provider.GetRequiredService<IArtifactStorage>();
        var store = new MLOpsTrackingStore(dbContextFactory, artifactStorage);
       // store.GetDeployedRun();
       // store.GetArtifacts();
    }
}