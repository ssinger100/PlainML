namespace ML_Trackingstore.Entities;

public class Run
{
    public int Id { get; set; }
    public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
    public TimeSpan Duration { get; set; }

    public MLModel MLModel { get; set; } //TODO: Nullref warning
    public ICollection<Parameter> Parameters { get; set; } = new List<Parameter>();
    public ICollection<Parameter_StringType> Parameter_StringType { get; set; } = new List<Parameter_StringType>();
    public ICollection<Metric> Metrics { get; set; } = new List<Metric>();
}
