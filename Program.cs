using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using ProUygulama.Api.Data;
using ProUygulama.Api.Swagger;

var builder = WebApplication.CreateBuilder(args);

/* ============================
   CONTROLLERS & JSON
============================ */
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

/* ============================
   SWAGGER
============================ */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // IFormFile upload hatasını çözen filter
    c.OperationFilter<FileUploadOperationFilter>();
});

/* ============================
   CORS
============================ */
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

/* ============================
   DATABASE
============================ */
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")
    );
});

/* ============================
   FILE UPLOAD LIMITS
============================ */

// Multipart (form-data) limiti → 200 MB
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200 MB
});

// Kestrel request body limiti → 200 MB
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // 200 MB
});

var app = builder.Build();

/* ============================
   MIDDLEWARE PIPELINE
============================ */
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseCors("Dev");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();
