namespace ML_Trackingstore.Entities;

public class Run
{
    public int Id { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
    public TimeSpan? Duration { get; set; }

    public int ExperimentId { get; set; }
    public Experiment? Experiment { get; set; }

    public List<Parameter> Parameters { get; set; } = new();
    public List<Parameter_StringType> Parameter_StringType { get; set; } = new();
    public List<Metric> Metrics { get; set; } = new();
}
