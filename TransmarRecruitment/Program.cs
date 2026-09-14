using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransmarRecruitment.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core DbContext (SQLite by default)
builder.Services.AddDbContext<TransmarRecruitment.Data.ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=transmar.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure database is created on startup (for development)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransmarRecruitment.Data.ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
