namespace ArMenuApi.Models;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;

    // The single compiled .mind file covering ALL of this restaurant's
    // active dishes. MindAR's compiler tool is used externally (there's
    // no reliable way to run its compilation step from a normal .NET
    // backend), so this is just a plain uploaded-file URL — see
    // UploadsController for how it gets here.
    public string? ArTargetFileUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Dish> Dishes { get; set; } = new();
}