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

    public async Task<Experiment> GetOrCreateExperiment(string modelname, CancellationToken token = default)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync(token);
        Experiment? experiment = await db.Set<Experiment>().FirstOrDefaultAsync(x => x.Name == modelname, token);
        if (experiment != null)
        {
            return experiment;
        }
        else
        {
            var model = db.MLModels.Add(new Experiment()
            {
                Name = modelname
            }).Entity;
            await db.SaveChangesAsync(token);
            return model;
        }        
    }

    public async Task DeleteModel(string modelname)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<Experiment[]> GetModels()
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<Experiment> GetModel(string modelname)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }



    public void StartRun(string modelname)
    {
        if (_stopWatch.IsRunning)
        {
            throw new InvalidOperationException("StartTraining kann nicht 2x hintereinander aufgerufen werden. Bitte zunächst FinishedTraining ausführen.");
        }
        else
        {
            _stopWatch.Reset();
            _stopWatch.Start();
        }
    }

    public async Task EndRun(string modelname, Parameter[]? parameters, Parameter_StringType[]? parameters_StringType, Metric[]? metrics)
    {
        if (_stopWatch.IsRunning)
        {
            _stopWatch.Stop();

            await Task.CompletedTask;


            _stopWatch.Reset();
        }
        else
        {
            throw new InvalidOperationException("StartTraining muss vor FinishedTraining aufgerufen werden.");
        }
    }


    public async Task DeployRun(string modelname, Run run, string? deploymentTargetName)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
