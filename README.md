
# PlainML

This library should simplify the tracking process of Machine-Learning (ML) training tasks. When you train a ML-model you can save parameters and metrics of the training attempts (named as "runs"). Also you can store artifacts and deploy it to a deploymenttarget like "dev" or "prod". On this way you can access them on an client application the deployed model.

[![build](https://github.com/ssinger100/PlainML/actions/workflows/dotnet_test.yml/badge.svg?branch=master)](https://github.com/ssinger100/PlainML/actions/workflows/dotnet_test.yml)

## Features

* Experimenttracking (Runs with parameters, metrics and artifacts)
* Integration of trainingprocesses
* Deployment to different targets (dev, production)
* Manage artifacts (deployment, caching)
* Visualization

## Getting Started

1. [Install VSCode](https://code.visualstudio.com/)
1. [Install C# extensions for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
1. Create project
    ```
    mkdir PlainMLExample
    cd PlainMLExample
    dotnet new console
    ```
1. Install nuget package to project
    ```
    dotnet add package PlainML
    dotnet add package PlainML.Infrastructure
    ```
1. Add usings on the top of program.cs-File
    ```
    using Microsoft.Extensions.DependencyInjection;
    using PlainML;
    using PlainML.Entities;
    using PlainML.Infrastructure;
    ```
1. Use dependency injection to configure services
    ```
    var _provider = new ServiceCollection()
        .UsePlainMLSqLite()  // other providers like SQL-Server are avaiable
        .UseArtifactStorageFilesystem() // other providers like SQL-Server are avaiable
        .AddTransient<PlainMLService>()
        .BuildServiceProvider();
    ```
1. Apply migration when init the database the first time or a new major version is avaiable
    ```
    var s = new PlainMLService(dbContextFactory, artifactStorage);
    await s.Migrate();
    ```
1. Use code
    ```
    const string experimentName = "TestExperiment";
    const string artifactsPath = "./Artifacts";

    // Create database
    PlainMLService s = _provider.GetRequiredService<PlainMLService>();
    await s.Migrate();

    // Train model
    int rundId = await s.StartRun(experimentName);
    float metricValue = await TrainModel(artifactsPath);
    await s.EndRun(
        rundId,
        parameters: new[] { new Parameter(){ Name = "Parameter1", Value = 1.123f } },
        parameters_StringType: new[] { new Parameter_StringType(){ Name = "Trainers", Value = "LightGbm, OneVersusAllTrainer" } },
        metrics: new[] { new Metric(){ Name = "MicroAccuracy", Value = metricValue } },
        artifactsPath);

    // Deploy model
    await s.DeployRun(rundId);

    // Use model
    var deployedRun = await s.GetDeployedRun(experimentName) ?? throw new NullReferenceException();
    await s.DownloadArtifacts(deployedRun.Id, "./DownloadedArtifacts");

    Console.WriteLine("Artifacts of run in ./DownloadedArtifacts:");
    foreach (var item in Directory.EnumerateFiles("./DownloadedArtifacts"))
    {
        Console.WriteLine(item);
    }

    async static Task<float> TrainModel(string artifactsPath)
    {
        Console.WriteLine("Training...");
        await Task.Delay(100);

        if (Directory.Exists(artifactsPath))
        {
            Console.WriteLine("Directory exists. Delete it!");
            Directory.Delete(artifactsPath, true);
        }

        Directory.CreateDirectory(artifactsPath);
        await File.WriteAllTextAsync(Path.Combine(artifactsPath, "TestFile.bin"), "0011010101001");

        return 0.1f;
    }
    ```

## Examples

* [src/PlainML.Samples/Program.cs](https://github.com/ssinger100/PlainML/blob/master/src/PlainML.Samples/Program.cs)

## NuGet Packages

- https://www.nuget.org/packages/PlainML/
- https://www.nuget.org/packages/PlainML.Infrastructure/
- https://www.nuget.org/packages/PlainML.Core/

## Roadmap

- [x] Develop unstable version 0.1.*
- [ ] Create initial stable version 1.0.0
- [ ] ML.Net integration
- [ ] Create docs
- [ ] Rest-interface
- [ ] Web-interface for visualization and Manage Experiments (run-table, graphs,...)
