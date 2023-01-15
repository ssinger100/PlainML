using Microsoft.EntityFrameworkCore;
using ML_Trackingstore.Entities;
using System.Diagnostics;

namespace ML_Trackingstore;

public class MLOpsTrackingStore
{
    readonly IDbContextFactory<MLTrackingstoreContext> _dbContextFactory;
    readonly Stopwatch _stopWatch = new();

    public MLOpsTrackingStore(IDbContextFactory<MLTrackingstoreContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Experiment> CreateExperiment(string experimentName)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        var experiment = db.MLModels.Add(new Experiment()
        {
            Name = experimentName
        }).Entity;
        await db.SaveChangesAsync();
        return experiment;
    }

    public async Task DeleteExperiment(string experimentName)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        throw new NotImplementedException();
    }

    public async Task<Experiment[]> GetExperiments()
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
         
        throw new NotImplementedException();
    }

    public async Task<Experiment> GetExperiment(string experimentName)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        throw new NotImplementedException();
    }

    public async Task<int> StartRun(string experimentName, CancellationToken token = default)
    {
        if (_stopWatch.IsRunning)
        {            
            throw new InvalidOperationException("StartTraining kann nicht 2x hintereinander aufgerufen werden. Bitte zunächst FinishedTraining ausführen.");
        }
        else
        {
            var experiment = await GetExperiment(experimentName);

            using var db = await _dbContextFactory.CreateDbContextAsync(token);


            _stopWatch.Reset();
            _stopWatch.Start();
        }

        await Task.CompletedTask;
        return -1;
    }

    public async Task<int> EndRun(string experimentName, Parameter[]? parameters, Parameter_StringType[]? parameters_StringType, Metric[]? metrics)
    {
        if (_stopWatch.IsRunning)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            _stopWatch.Stop();           


            _stopWatch.Reset();
        }
        else
        {
            throw new InvalidOperationException("StartTraining muss vor FinishedTraining aufgerufen werden.");
        }

        await Task.CompletedTask;
        return -1;
    }

    public async Task<Run> GetRun(int runId, CancellationToken token = default)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync(token);
        var run = await db.Set<Run>().FirstOrDefaultAsync(x => x.Id == runId, token);
        return run ?? throw new KeyNotFoundException();
    }

    public async Task DeployRun(string experimentname, int runId, string? deploymentTargetName)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        throw new NotImplementedException();
    }
}
