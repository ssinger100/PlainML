using Microsoft.EntityFrameworkCore;
using PlainML.Entities;
using System.Diagnostics;

namespace PlainML;

public class PlainMLService
{
    readonly IDbContextFactory<PlainMLContext> _dbContextFactory;
    readonly IArtifactStorage _artifactStorage;
    readonly Stopwatch _stopWatch = new();

    public PlainMLService(IDbContextFactory<PlainMLContext> dbContextFactory, IArtifactStorage artifactStorage)
    {
        _dbContextFactory = dbContextFactory;
        _artifactStorage = artifactStorage;
    }

    public async Task Migrate(CancellationToken token = default)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync(token);
        await context.Database.EnsureCreatedAsync(token);
     //   Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.Migrate();
     //   context.Database.Migrate();
    }

    public async Task<Experiment> GetOrCreateExperiment(string experimentName)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();

        var experiment = await db.Set<Experiment>().FirstOrDefaultAsync(x => x.Name == experimentName);
        experiment ??= db.MLModels.Add(new Experiment()
        {
            Name = experimentName
        }).Entity;

        await db.SaveChangesAsync();
        return experiment;
    }

    public async Task DeleteExperiment(string experimentName, CancellationToken token = default)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync(token);
        var experiment = await db.Set<Experiment>().FirstOrDefaultAsync(x => x.Name == experimentName, token);
        if (experiment != null)
        {
            db.Set<Experiment>().Remove(experiment);
            await db.SaveChangesAsync(token);
        }
    }

    public async Task<Experiment[]> GetExperiments()
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Set<Experiment>().ToArrayAsync();
    }

    public async Task<int> StartRun(string experimentName, CancellationToken token = default)
    {
        if (_stopWatch.IsRunning)
        {
            throw new InvalidOperationException("StartTraining kann nicht 2x hintereinander aufgerufen werden. Bitte zunächst FinishedTraining ausführen.");
        }
        else
        {
            var experiment = await GetOrCreateExperiment(experimentName);

            using var db = await _dbContextFactory.CreateDbContextAsync(token);
            db.Attach(experiment);

            Run run = new();

            _stopWatch.Reset();
            _stopWatch.Start();

            run.Duration = _stopWatch.Elapsed;
            experiment.Runs.Add(run);
            await db.SaveChangesAsync(token);
            return run.Id;
        }
    }

    public async Task EndRun(int runId, Parameter[]? parameters, Parameter_StringType[]? parameters_StringType, Metric[]? metrics)
    {
        if (_stopWatch.IsRunning)
        {
            _stopWatch.Stop();
            TimeSpan duration = _stopWatch.Elapsed;
            _stopWatch.Reset();

            using var db = await _dbContextFactory.CreateDbContextAsync();
            Run? run = await db.Set<Run>().FirstOrDefaultAsync(x => x.Id == runId);

            if (run == null)
            {
                throw new KeyNotFoundException(nameof(runId));
            }

            run.Duration = duration;

            if(parameters != null) { run.Parameters.AddRange(parameters); }
            if(parameters_StringType != null) { run.Parameter_StringType.AddRange(parameters_StringType); }
            if(metrics != null) { run.Metrics.AddRange(metrics); }

            await db.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("StartTraining muss vor FinishedTraining aufgerufen werden.");
        }
    }

    public async Task<Run> GetRun(int runId, CancellationToken token = default)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync(token);
        var run = await db.Set<Run>()
            .Include(x => x.Parameters)
            .Include(x => x.Parameter_StringType)
            .Include(x => x.Metrics)
            .FirstOrDefaultAsync(x => x.Id == runId, token);
        return run ?? throw new KeyNotFoundException();
    }

    public async Task<Run?> GetLastRun(string experimentName, CancellationToken token = default)
    {
        var experiment = await GetOrCreateExperiment(experimentName);

        using var db = await _dbContextFactory.CreateDbContextAsync(token);
        db.Attach(experiment);

        var run = await db.Set<Run>()
            .OrderByDescending(x => x.DateTimeOffset)
            .FirstOrDefaultAsync(token);
        return run;
    }

    public async Task DeployRun(int runId, string deploymentTargetName = "production")
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();

        var run = await db.Set<Run>().FirstOrDefaultAsync(x => x.Id == runId) ?? throw new NullReferenceException();

        var target = await db.Set<Deploymenttarget>().FirstOrDefaultAsync(x => x.Name == deploymentTargetName);
        if (target == null)
        {
            target = db.Set<Deploymenttarget>().Add(new()
            {
                Name = deploymentTargetName
            }).Entity;
            await db.SaveChangesAsync();
        }

        //Delete old deployment
        var olddeployment = await db.Set<Deployment>()
            .FirstOrDefaultAsync(x => x.ExperimentId == run.ExperimentId && x.DeploymenttargetId == target.Id);
        if (olddeployment != null)
        {
            db.Set<Deployment>().Remove(olddeployment);
            await db.SaveChangesAsync();
        }

        db.Set<Deployment>().Add(new()
        {
            DeploymenttargetId = target.Id,
            ExperimentId = run.ExperimentId
        });

        await db.SaveChangesAsync();
    }

    public async Task<Run?> GetDeployedRun(string experimentName, string deploymentTargetName)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.Set<Deployment>().Where(x => x.Experiment!.Name == experimentName && x.Deploymenttarget!.Name == deploymentTargetName)
            .Select(x => x.Run)
            .FirstOrDefaultAsync();
    }

    public async Task GetArtifacts(int runId, string localpath)
    {
        await _artifactStorage.Download(runId, localpath);
    }
}
