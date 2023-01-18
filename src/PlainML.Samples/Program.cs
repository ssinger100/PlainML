using Microsoft.Extensions.DependencyInjection;
using PlainML;
using PlainML.Infrastructure;

Console.WriteLine("Hello, World!");

var _provider = new ServiceCollection()
            .UseSQLLite()
            .UseFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();

PlainMLService s = _provider.GetRequiredService<PlainMLService>();
await s.Migrate();
int rundId = await s.StartRun("TestExperiment");
await Task.Delay(100);
await s.EndRun(rundId, null, null, null);