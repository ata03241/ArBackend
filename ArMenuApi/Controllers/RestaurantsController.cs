using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArMenuApi.Data;
using ArMenuApi.Models;

namespace ArMenuApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly ArMenuDbContext _db;

    public RestaurantsController(ArMenuDbContext db)
    {
        _db = db;
    }

    // GET /api/restaurants
    [HttpGet]
    public async Task<ActionResult<List<Restaurant>>> GetAll()
    {
        return await _db.Restaurants.ToListAsync();
    }

    // GET /api/restaurants/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Restaurant>> GetOne(int id)
    {
        var restaurant = await _db.Restaurants.FindAsync(id);
        if (restaurant is null) return NotFound();
        return restaurant;
    }

    // POST /api/restaurants
    [HttpPost]
    public async Task<ActionResult<Restaurant>> Create(Restaurant restaurant)
    {
        _db.Restaurants.Add(restaurant);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = restaurant.Id }, restaurant);
    }

    // PUT /api/restaurants/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Restaurant updated)
    {
        var restaurant = await _db.Restaurants.FindAsync(id);
        if (restaurant is null) return NotFound();

        restaurant.Name = updated.Name;
        restaurant.ContactEmail = updated.ContactEmail;
        if (!string.IsNullOrEmpty(updated.ArTargetFileUrl))
            restaurant.ArTargetFileUrl = updated.ArTargetFileUrl;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET /api/restaurants/5/target-file
    [HttpGet("{id}/target-file")]
    public async Task<IActionResult> GetTargetFile(int id)
    {
        var restaurant = await _db.Restaurants.FindAsync(id);
        if (restaurant is null) return NotFound();
        if (string.IsNullOrEmpty(restaurant.ArTargetFileUrl))
            return NotFound("This restaurant hasn't uploaded a compiled targets.mind file yet.");

        return Redirect(restaurant.ArTargetFileUrl);
    }
}