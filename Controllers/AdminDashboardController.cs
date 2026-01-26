using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin")]
public class AdminDashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminDashboardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<AdminDashboardDto>> Get()
    {
        var dto = new AdminDashboardDto
        {
            ProjectCount = await _db.Projects.CountAsync(),
            ServiceCount = await _db.ServiceItems.CountAsync(),
            ReferenceCount = await _db.ReferenceItems.CountAsync(),
            ContactMessageCount = await _db.ContactMessages.CountAsync()
        };

        return Ok(dto);
    }
}
