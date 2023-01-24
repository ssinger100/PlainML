
# PlainML

Kurze Beschreibung (TODO)

[![build](https://github.com/ssinger100/PlainML/actions/workflows/dotnet_test.yml/badge.svg?branch=master)](https://github.com/ssinger100/PlainML/actions/workflows/dotnet_test.yml)

## Features

* Experimenttracking (Runs with parameters, metrics and artifacts)
* Integration of trainingprocesses
* Deployment to different targets (dev, production)
* Manage artifacts (deployment, caching)
* Visualization

## Getting Started

1. Install VSCode
1. Install C# extensions for VSCode
1. Create project
1. Install nuget package to project:
    ```
    dotnet add package PlainML
    ```
1. Database and artifactstorage configuration with dependency injection:
    ```
    dotnet add package PlainML.Infrastructure
    ```
1. Apply migration when a new major version is avaiable
    ```
    var s = new PlainMLService(dbContextFactory, artifactStorage);
    await s.Migrate();
    ```
1. Use code
    ```
    TODO: example code
    ```
1. Use UI

## Examples

* watch src/PlainML.Samples/Program.cs

## NuGet Packages

- https://www.nuget.org/packages/PlainML/
- https://www.nuget.org/packages/PlainML.Infrastructure/
- https://www.nuget.org/packages/PlainML.Core/

## Roadmap

- [ ] Develop unstable version 0.0.*
- [ ] Create initial stable version 1.0.0
- [ ] Create docs
- [ ] Rest-interface
- [ ] Web-interface for visualization and Manage Experiments (run-table, graphs,...)

## Contributing

TODO
