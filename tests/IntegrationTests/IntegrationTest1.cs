using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML_Trackingstore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ML_Trackingstore.Entities;

namespace IntegrationTests;

[TestClass]
public class IntegrationTest1
{
    readonly SqliteConnection _connection;
    readonly IDbContextFactory<MLTrackingstoreContext> _dbContextFactory;

    public IntegrationTest1()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var provider = new ServiceCollection()
            .AddDbContextFactory<MLTrackingstoreContext>(options => options.UseSqlite(_connection))
            .BuildServiceProvider();

        _dbContextFactory = provider.GetRequiredService<IDbContextFactory<MLTrackingstoreContext>>();

        using var context = _dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }

    [TestMethod]
    public async Task CreateExperimentTest()
    {
        string experimentName = "TestExperiment1";

        var store = new MLOpsTrackingStore(_dbContextFactory);
        var result = await store.CreateExperiment(experimentName);

        Assert.AreEqual(experimentName, result.Name);
        Assert.AreEqual(0, result.Runs.Count);
    }

    [TestMethod]
    public async Task StartTrainingTest()
    {
        string experimentName = "TestExperiment1";

        var parameters = new Parameter[]
        {
            new(){ Name = "p1", Value = 1 },
            new(){ Name = "p2", Value = 2 }
        };

        var store = new MLOpsTrackingStore(_dbContextFactory);
        store.StartRun(experimentName);

        await Task.Delay(10); // Long running training process

        var metrics = new Metric[]
        {
            new(){ Name = "m1", Value = 0.45f }
        };

        await store.EndRun(experimentName, parameters, null, metrics);
    }

    public void Dispose() => _connection.Dispose();
}