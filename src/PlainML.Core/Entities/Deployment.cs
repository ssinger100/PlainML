namespace PlainML.Entities;

public class Deployment
{
    public int Id { get; set; }

    public int ExperimentId { get; set; }
    public Experiment? Experiment { get; set; }

    public int DeploymenttargetId { get; set; }
    public Deploymenttarget? Deploymenttarget { get; set; }

    public int RunId { get; set; }
    public Run? Run { get; set; }
}
