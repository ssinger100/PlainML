namespace ML_Trackingstore.Entities;

public class Deployment
{
    public int Id { get; set; }

    public int ExperimentId { get; set; }
    public Experiment Experiment { get; set; } = null!;

    public int DeploymenttargetId { get; set; }
    public Deploymenttarget Deploymenttarget { get; set; } = null!;

    public int RunId { get; set; }
    public Run Run { get; set; } = null!;
}
