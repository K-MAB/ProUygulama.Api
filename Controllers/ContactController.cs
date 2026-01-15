using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _db;

    public ContactController(AppDbContext db)
    {
        _db = db;
    }

    // =====================================
    // PUBLIC - SEND MESSAGE
    // POST /api/contact
    // =====================================
    [HttpPost]
    public async Task<IActionResult> Send(CreateContactMessageDto dto)
    {
        var message = new ContactMessage
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Subject = dto.Subject,
            Message = dto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _db.ContactMessages.Add(message);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Mesajınız başarıyla gönderildi." });
    }

    // =====================================
    // ADMIN - LIST
    // GET /api/contact/admin
    // =====================================
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.ContactMessages
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactMessageListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Subject = x.Subject,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();

        return Ok(list);
    }

    // =====================================
    // ADMIN - DETAIL + MARK AS READ
    // GET /api/contact/admin/{id}
    // =====================================
    [Authorize(Roles = "Admin")]
    [HttpGet("admin/{id:guid}")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var message = await _db.ContactMessages.FindAsync(id);
        if (message == null)
            return NotFound();

        if (!message.IsRead)
        {
            message.IsRead = true;
            await _db.SaveChangesAsync();
        }

        return Ok(message);
    }

    // =====================================
    // ADMIN - HARD DELETE
    // DELETE /api/contact/{id}
    // =====================================
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var message = await _db.ContactMessages.FindAsync(id);
        if (message == null)
            return NotFound();

        _db.ContactMessages.Remove(message);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Mesaj silindi." });
    }
}
