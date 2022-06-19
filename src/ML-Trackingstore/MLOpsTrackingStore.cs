using Microsoft.EntityFrameworkCore;
using ML_Trackingstore.Entities;
using System.Diagnostics;

namespace ML_Trackingstore;

public class MLOpsTrackingStore
{
    readonly IDbContextFactory<MLTrackingstoreContext> _dbContextFactory;
    Stopwatch _stopWatch = new();

    public MLOpsTrackingStore(IDbContextFactory<MLTrackingstoreContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<MLModel> CreateModel(string modelname)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        var model = db.MLModels.Add(new MLModel()
        {
            Name = modelname
        }).Entity;
        await db.SaveChangesAsync();
        return model;
    }

    public async Task DeleteModel(string modelname)
    {
        throw new NotImplementedException();
    }

    public async Task<MLModel[]> GetModels()
    {
        throw new NotImplementedException();
    }

    public async Task<MLModel> GetModel(string modelname)
    {
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

            


            _stopWatch.Reset();
        }
        else
        {
            throw new InvalidOperationException("StartTraining muss vor FinishedTraining aufgerufen werden.");
        }
    }


    public async Task DeployRun(string modelname, Run run, string? deploymentTargetName)
    {
        throw new NotImplementedException();
    }
}
