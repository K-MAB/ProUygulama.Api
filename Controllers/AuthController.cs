using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;
using ProUygulama.Api.Helpers;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // ============================
    // LOGIN
    // POST /api/auth/login
    // ============================
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _db.AdminUsers
            .FirstOrDefaultAsync(x => x.Email == dto.Email && x.IsActive);

        if (user == null)
            return Unauthorized("E-posta veya şifre hatalı.");

        if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("E-posta veya şifre hatalı.");

        var token = GenerateJwt(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Role = user.Role.ToString(),
            FullName = user.FullName,
            Email = user.Email
        });
    }

    // ============================
    // SEED FIRST ADMIN (DEV ONLY)
    // POST /api/auth/seed-admin
    // ============================
    [HttpPost("seed-admin")]
    public async Task<IActionResult> SeedAdmin()
    {
        if (!HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            return Forbid();

        var exists = await _db.AdminUsers.AnyAsync();
        if (exists)
            return BadRequest("Admin zaten var.");

        var admin = new AdminUser
        {
            Id = Guid.NewGuid(),
            FullName = "System Admin",
            Email = "admin@asmab.com",
            PasswordHash = PasswordHasher.Hash("Admin123!"),
            Role = AdminRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.AdminUsers.Add(admin);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Admin oluşturuldu", email = admin.Email, password = "Admin123!" });
    }

    private string GenerateJwt(AdminUser user)
    {
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var audience = _config["Jwt:Audience"]!;
        var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"]!);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
