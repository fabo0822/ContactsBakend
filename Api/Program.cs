using DataAccess;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces; 
using BusinessLogic.Services; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para permitir al frontend (React) consumir la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000"
            )
            .AllowCredentials();
    });
});

builder.Services.AddDbContext<ContactsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra el repositorio y el servicio 
builder.Services.AddScoped<IContactRepository, ContactRepository>(); 
builder.Services.AddScoped<IContactService, ContactService>(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();