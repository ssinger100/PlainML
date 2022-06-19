using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML_Trackingstore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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

    public void Dispose() => _connection.Dispose();
}