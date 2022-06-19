using Microsoft.EntityFrameworkCore;
using ML_Trackingstore.Entities;

namespace ML_Trackingstore;

public class MLOpsTrackingStore
{
    readonly IDbContextFactory<MLTrackingstoreContext> _dbContextFactory;

    public MLOpsTrackingStore(IDbContextFactory<MLTrackingstoreContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<MLModel> CreateModel(string name)
    {
        using var db = await _dbContextFactory.CreateDbContextAsync();
        var model = db.MLModels.Add(new MLModel()
        {
            Name = name
        }).Entity;
        await db.SaveChangesAsync();
        return model;
    }


    //Task<MLModel> CreateModel(string name);
    //Task DeleteModel(string name);
    //Task<MLModel[]> GetModels();
    //Task<MLModel> GetModel(string name);

    //Task DeployRun(Run run);

    //void StartTraining();
    //void FinishedTraining();
}
