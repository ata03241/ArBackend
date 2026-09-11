using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArMenuApi.Data;
using ArMenuApi.Models;

namespace ArMenuApi.Controllers;

[ApiController]
[Route("api")]
public class DishesController : ControllerBase
{
    private readonly ArMenuDbContext _db;

    public DishesController(ArMenuDbContext db)
    {
        _db = db;
    }

    // GET /api/restaurants/5/dishes  — full data, for the dashboard
    [HttpGet("restaurants/{restaurantId}/dishes")]
    public async Task<ActionResult<List<Dish>>> GetByRestaurant(int restaurantId)
    {
        return await _db.Dishes
            .Where(d => d.RestaurantId == restaurantId)
            .OrderBy(d => d.TargetIndex)
            .ToListAsync();
    }

    // GET /api/restaurants/5/dishes/public  — trimmed data, for the AR frontend
    [HttpGet("restaurants/{restaurantId}/dishes/public")]
    public async Task<ActionResult<List<DishPublicDto>>> GetPublicByRestaurant(int restaurantId)
    {
        var dishes = await _db.Dishes
            .Where(d => d.RestaurantId == restaurantId && d.IsActive)
            .OrderBy(d => d.TargetIndex)
            .Select(d => new DishPublicDto
            {
                TargetIndex = d.TargetIndex,
                Name = d.Name,
                Ingredients = d.Ingredients,
                Allergens = d.Allergens,
                VideoSrc = d.VideoUrl ?? string.Empty
            })
            .ToListAsync();

        return dishes;
    }

    // GET /api/dishes/12
    [HttpGet("dishes/{id}")]
    public async Task<ActionResult<Dish>> GetOne(int id)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish is null) return NotFound();
        return dish;
    }

    // POST /api/restaurants/5/dishes
    [HttpPost("restaurants/{restaurantId}/dishes")]
    public async Task<ActionResult<Dish>> Create(int restaurantId, Dish dish)
    {
        var restaurantExists = await _db.Restaurants.AnyAsync(r => r.Id == restaurantId);
        if (!restaurantExists) return NotFound($"Restaurant {restaurantId} not found.");

        dish.RestaurantId = restaurantId;
        _db.Dishes.Add(dish);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = dish.Id }, dish);
    }

    // PUT /api/dishes/12
    [HttpPut("dishes/{id}")]
    public async Task<IActionResult> Update(int id, Dish updated)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish is null) return NotFound();

        dish.TargetIndex = updated.TargetIndex;
        dish.Name = updated.Name;
        dish.Ingredients = updated.Ingredients;
        dish.Allergens = updated.Allergens;
        dish.MenuImageUrl = updated.MenuImageUrl;
        dish.VideoUrl = updated.VideoUrl;
        dish.IsActive = updated.IsActive;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/dishes/12
    [HttpDelete("dishes/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish is null) return NotFound();

        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}