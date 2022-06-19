namespace ML_Trackingstore;
public interface ITrainingDataloader
{
    IEnumerable<DataObject> GetTrainingData();
}