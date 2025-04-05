using Microsoft.EntityFrameworkCore;
using SIAirline.Data;
using SIAirline.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Register the generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapControllers();
app.Run();