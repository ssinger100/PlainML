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
    SqliteConnection _connection;
    IDbContextFactory<MLTrackingstoreContext> _dbContextFactory;

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
    public async Task CreateModelTest()
    {
        string modelname = "TestModel";

        var store = new MLOpsTrackingStore(_dbContextFactory);
        var result = await store.CreateModel(modelname);

        Assert.AreEqual(modelname, result.Name);
        Assert.AreEqual(0, result.Runs.Count);
    }

    [TestMethod]
    public async Task StartTrainingTest()
    {
        string modelname = "TestModel";

        var parameters = new Parameter[]
        {
            new(){ Name = "p1", Value = 1 },
            new(){ Name = "p2", Value = 2 }
        };


        var store = new MLOpsTrackingStore(_dbContextFactory);
        store.StartRun(modelname);

        await Task.Delay(10); // Heavy training

        var metrics = new Metric[]
        {
            new(){ Name = "m1", Value = 0.45f }
        };

        await store.EndRun(modelname, parameters, null, metrics);


    }



    public void Dispose() => _connection.Dispose();
}