﻿namespace PlainML.Entities;

public class Experiment
{
    public int Id { get; set; }
    public string Name { get; set; } = "New Model";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Run> Runs { get; set; } = new List<Run>();
}
