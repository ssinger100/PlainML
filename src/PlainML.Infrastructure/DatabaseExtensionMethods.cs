using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PlainML.Infrastructure;

public static class DatabaseExtensionMethods
{
    readonly static SqliteConnection _connection = new ("Filename=:memory:");

    public static IServiceCollection UseSqLite(this IServiceCollection services)
    {
        _connection.Open();
        return services.AddDbContextFactory<PlainMLContext>(options => options.UseSqlite(_connection));
    }

    public static IServiceCollection UseInMemoryDatabase(this IServiceCollection services, string databaseName, Action<InMemoryDbContextOptionsBuilder>? inMemoryOptionsAction = null)
    {
        return services.AddDbContextFactory<PlainMLContext>(options => options.UseInMemoryDatabase(databaseName));
    }

    public static IServiceCollection UseSQLServer(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContextFactory<PlainMLContext>(options => options.UseSqlServer(connectionString));
    }

    public static IServiceCollection UseMySQLServer(this IServiceCollection services)
    {
        throw new NotImplementedException(nameof(UseMySQLServer));
        //TODO: UseMySQLServer
    }

    public static IServiceCollection UsePostgreSQLServer(this IServiceCollection services)
    {
        throw new NotImplementedException(nameof(UsePostgreSQLServer));
        //TODO: UsePostgreSQLServer
    }
}
