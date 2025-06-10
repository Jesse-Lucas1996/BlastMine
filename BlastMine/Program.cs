using BlastMine.Configurations;
using BlastMine.Data;
using BlastMine.Models;
using BlastMine.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); 

var databaseSettings = builder.Configuration.GetSection("Database").Get<DatabaseSettings>();
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddSwaggerConfiguration();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = Path.Combine(Directory.GetCurrentDirectory(), "clientApp", "dist");
});

builder
    .Services.AddDatabase(databaseSettings)
    .AddJwtAuthentication(jwtSettings)
    .AddCorsConfiguration();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureSpa();

app.MapControllers();

if (
    builder.Configuration.GetValue<bool>("Database:ApplyMigrationsAtStartup")
    || builder.Configuration.GetValue<bool>("ApplyMigrations")
)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();