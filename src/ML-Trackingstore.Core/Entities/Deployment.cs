namespace ML_Trackingstore.Entities;

public class Deployment
{
    public Experiment Experiment { get; set; }
    public Deploymenttarget Deploymenttarget { get; set; }
    public Run Run { get; set; }
}
