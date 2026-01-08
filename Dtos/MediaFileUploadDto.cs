using Microsoft.AspNetCore.Http;

namespace ProUygulama.Api.Dtos;

public class MediaFileUploadDto
{
    public IFormFile File { get; set; } = default!;
}
