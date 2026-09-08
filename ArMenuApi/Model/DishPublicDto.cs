namespace ArMenuApi.Models;

// Shape returned by GET /api/restaurants/{id}/dishes/public — deliberately
// matches the field names the AR frontend's DISHES array already uses
// (targetIndex, name, ingredients, allergens, videoSrc), so index.html
// can fetch this and drop it straight in with minimal changes.
public class DishPublicDto
{
    public int TargetIndex { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Allergens { get; set; } = string.Empty;
    public string VideoSrc { get; set; } = string.Empty;
}