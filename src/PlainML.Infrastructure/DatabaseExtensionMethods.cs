using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PlainML.Infrastructure;

public static class DatabaseExtensionMethods
{
    public static IServiceCollection UsePlainMLSqLite(this IServiceCollection services)
    {
        SqliteConnection _connection = new ("Filename=:memory:");
        _connection.Open();
        return services.AddDbContextFactory<PlainMLContext>(options => options.UseSqlite(_connection));
    }

    public static IServiceCollection UsePlainMLInMemoryDatabase(this IServiceCollection services, string databaseName, Action<InMemoryDbContextOptionsBuilder>? inMemoryOptionsAction = null)
    {
        return services.AddDbContextFactory<PlainMLContext>(options => options.UseInMemoryDatabase(databaseName, inMemoryOptionsAction));
    }

    public static IServiceCollection UsePlainMLSQLServer(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<PlainMLContext>(options => options.UseSqlServer(connectionString));
    }

    public static IServiceCollection UsePlainMLMySQLServer(this IServiceCollection services)
    {
        throw new NotImplementedException(nameof(UsePlainMLMySQLServer));
        //TODO: UseMySQLServer
    }

    public static IServiceCollection UsePlainMLPostgreSQLServer(this IServiceCollection services)
    {
        throw new NotImplementedException(nameof(UsePlainMLPostgreSQLServer));
        //TODO: UsePostgreSQLServer
    }
}
