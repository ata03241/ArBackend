namespace ArMenuApi.Models;

public class Dish
{
    public int Id { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    // Must match this dish's position in the restaurant's compiled
    // targets.mind file (0 = first photo uploaded to MindAR's compiler,
    // 1 = second, etc). The restaurant owner sets this when they compile
    // their photos — see the README for the manual step this requires.
    public int TargetIndex { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Allergens { get; set; } = string.Empty;

    public string? MenuImageUrl { get; set; }
    public string? VideoUrl { get; set; }

    public bool IsActive { get; set; } = true;
}