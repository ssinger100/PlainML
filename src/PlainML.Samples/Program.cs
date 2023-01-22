using Microsoft.Extensions.DependencyInjection;
using PlainML;
using PlainML.Entities;
using PlainML.Infrastructure;

Console.WriteLine("Hello, World!");

var _provider = new ServiceCollection()
            .UsePlainMLSqLite()
            .UseArtifactStorageFilesystem()
            .AddTransient<PlainMLService>()
            .BuildServiceProvider();

string experimentName = "TestExperiment";

PlainMLService s = _provider.GetRequiredService<PlainMLService>();
await s.EnsureCreated(); //TODO: Uncomment
//await s.Migrate();
int rundId = await s.StartRun(experimentName);
await Task.Delay(100);
await s.EndRun(rundId, null, null, null);

Run? lastRun = await s.GetLastRun(experimentName);
Console.WriteLine(lastRun?.Experiment?.Name);