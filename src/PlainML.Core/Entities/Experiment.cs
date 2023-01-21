namespace PlainML.Entities;

public class Experiment
{
    public int Id { get; set; }
    public string Name { get; set; } = "New Model";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Run> Runs { get; set; } = new List<Run>();
}
