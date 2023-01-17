using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PlainML;

namespace PlainML.Infrastructure;

public static class DatabaseExtensionMethods
{
    readonly static SqliteConnection _connection = new ("Filename=:memory:");

    public static IServiceCollection UseSQLLite(this IServiceCollection services)
    {
        _connection.Open();
        return services.AddDbContextFactory<MLTrackingstoreContext>(options => options.UseSqlite(_connection));
    }

    public static IServiceCollection UseSQLServer()
    {
        throw new NotImplementedException();
        //TODO: UseSQLServer
    }

    public static IServiceCollection UseMySQLServer()
    {
        throw new NotImplementedException();
        //TODO: UseMySQLServer
    }

    public static IServiceCollection UsePostgreSQLServer()
    {
        throw new NotImplementedException();
        //TODO: UsePostgreSQLServer
    }
}
