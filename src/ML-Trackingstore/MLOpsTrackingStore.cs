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

    public async Task<Experiment> CreateModel(string experimentname)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        var model = db.MLModels.Add(new Experiment()
        {
            Name = experimentname
        }).Entity;
        await db.SaveChangesAsync();
        return model;
    }

    public async Task DeleteModel(string experimentname)
    {
        throw new NotImplementedException();
    }

    public async Task<Experiment[]> GetModels()
    {
        throw new NotImplementedException();
    }

    public async Task<Experiment> GetModel(string modelname)
    {
        throw new NotImplementedException();
    }



    public void StartRun(string experimentname)
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

    public async Task EndRun(string experimentname, Parameter[]? parameters, Parameter_StringType[]? parameters_StringType, Metric[]? metrics)
    {
        if (_stopWatch.IsRunning)
        {
            _stopWatch.Stop();

            


            _stopWatch.Reset();
        }
        else
        {
            throw new InvalidOperationException("StartTraining muss vor FinishedTraining aufgerufen werden.");
        }
    }


    public async Task DeployRun(string experimentname, Run run, string? deploymentTargetName)
    {
        throw new NotImplementedException();
    }
}
