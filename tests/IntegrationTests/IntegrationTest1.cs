using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML_Trackingstore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ML_Trackingstore.Entities;
using ML_Tackingstore.Infrastructure;

namespace IntegrationTests;

[TestClass]
public class IntegrationTest1
{
    readonly IDbContextFactory<MLTrackingstoreContext> _dbContextFactory;

    public IntegrationTest1()
    {
        var provider = new ServiceCollection()
            .UseSQLLite()
            .BuildServiceProvider();

        _dbContextFactory = provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();        
    }

    [TestInitialize]
    public async Task InitSQLLite()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        context.Database.EnsureCreated();
        context.Database.Migrate();   
    }

    [TestMethod]
    public async Task CreateExperimentTest()
    {
        string experimentName = "TestExperiment1";

        var store = new MLOpsTrackingStore(_dbContextFactory);
        var result = await store.GetOrCreateExperiment(experimentName);

        Assert.AreEqual(experimentName, result.Name);
        Assert.AreEqual(0, result.Runs.Count);
    }

    [TestMethod]
    public async Task StartTrainingTest()
    {
        string experimentName = "TestExperiment1";        

        var store = new MLOpsTrackingStore(_dbContextFactory);
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
        var store = new MLOpsTrackingStore(_dbContextFactory);
       // store.GetDeployedRun();
    }

    [TestMethod]
    public async Task GetArtifactsTest()
    {
        var store = new MLOpsTrackingStore(_dbContextFactory);
       // store.GetDeployedRun();
       // store.GetArtifacts();
    }
}