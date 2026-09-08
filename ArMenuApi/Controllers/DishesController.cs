using Microsoft.AspNetCore.Mvc;

namespace ArMenuApi.Controllers;

[ApiController]
[Route("api/uploads")]
public class UploadsController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public UploadsController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("video")]
    public async Task<ActionResult<object>> UploadVideo(IFormFile file)
        => await SaveFile(file, "videos", new[] { ".mp4", ".mov", ".webm" });

    [HttpPost("image")]
    public async Task<ActionResult<object>> UploadImage(IFormFile file)
        => await SaveFile(file, "images", new[] { ".jpg", ".jpeg", ".png", ".webp" });

    [HttpPost("target-file")]
    public async Task<ActionResult<object>> UploadTargetFile(IFormFile file)
        => await SaveFile(file, "targets", new[] { ".mind" });

    private async Task<ActionResult<object>> SaveFile(IFormFile file, string subfolder, string[] allowedExtensions)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file received.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return BadRequest($"File type {extension} not allowed. Allowed: {string.Join(", ", allowedExtensions)}");

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var publicUrl = $"/uploads/{subfolder}/{uniqueFileName}";
        return Ok(new { url = publicUrl });
    }
}