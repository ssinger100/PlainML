namespace PlainML.Entities;

public class Run
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public TimeSpan? Duration { get; set; }

    public int ExperimentId { get; set; }
    public Experiment? Experiment { get; set; }

    public List<Parameter> Parameters { get; set; } = new();
    public List<Parameter_StringType> Parameter_StringType { get; set; } = new();
    public List<Metric> Metrics { get; set; } = new();

    public string? ArtifactStoragePath { get; set; } = null;
    public byte[]? ZipByteArray { get; set; } = Array.Empty<byte>();
}
