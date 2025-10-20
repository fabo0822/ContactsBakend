using DataAccess;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces; 
using BusinessLogic.Services; 
using Domain.Entities;

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
                "http://localhost:3000",
                "http://localhost:5174"
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

// Configurar middleware para servir archivos estáticos (imágenes)
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// Seed de datos de ejemplo (30 contactos) solo si la tabla está vacía
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
	db.Database.EnsureCreated();
	if (!db.Contacts.Any())
	{
		var seedContacts = new List<Contact>
		{
			new Contact { FirstName = "Liam", LastName = "Smith", Email = "liam.smith@example.com", IsFavorite = true },
			new Contact { FirstName = "Olivia", LastName = "Johnson", Email = "olivia.johnson@example.com" },
			new Contact { FirstName = "Noah", LastName = "Williams", Email = "noah.williams@example.com" },
			new Contact { FirstName = "Emma", LastName = "Brown", Email = "emma.brown@example.com" },
			new Contact { FirstName = "Oliver", LastName = "Jones", Email = "oliver.jones@example.com" },
			new Contact { FirstName = "Ava", LastName = "Garcia", Email = "ava.garcia@example.com" },
			new Contact { FirstName = "Elijah", LastName = "Miller", Email = "elijah.miller@example.com" },
			new Contact { FirstName = "Sophia", LastName = "Davis", Email = "sophia.davis@example.com" },
			new Contact { FirstName = "William", LastName = "Rodriguez", Email = "william.rodriguez@example.com" },
			new Contact { FirstName = "Isabella", LastName = "Martinez", Email = "isabella.martinez@example.com" },
			new Contact { FirstName = "James", LastName = "Hernandez", Email = "james.hernandez@example.com" },
			new Contact { FirstName = "Mia", LastName = "Lopez", Email = "mia.lopez@example.com" },
			new Contact { FirstName = "Benjamin", LastName = "Gonzalez", Email = "benjamin.gonzalez@example.com" },
			new Contact { FirstName = "Charlotte", LastName = "Wilson", Email = "charlotte.wilson@example.com" },
			new Contact { FirstName = "Lucas", LastName = "Anderson", Email = "lucas.anderson@example.com" },
			new Contact { FirstName = "Amelia", LastName = "Thomas", Email = "amelia.thomas@example.com" },
			new Contact { FirstName = "Henry", LastName = "Taylor", Email = "henry.taylor@example.com" },
			new Contact { FirstName = "Harper", LastName = "Moore", Email = "harper.moore@example.com" },
			new Contact { FirstName = "Alexander", LastName = "Jackson", Email = "alexander.jackson@example.com" },
			new Contact { FirstName = "Evelyn", LastName = "Martin", Email = "evelyn.martin@example.com" },
			new Contact { FirstName = "Michael", LastName = "Lee", Email = "michael.lee@example.com" },
			new Contact { FirstName = "Abigail", LastName = "Perez", Email = "abigail.perez@example.com" },
			new Contact { FirstName = "Daniel", LastName = "Thompson", Email = "daniel.thompson@example.com" },
			new Contact { FirstName = "Emily", LastName = "White", Email = "emily.white@example.com" },
			new Contact { FirstName = "Jacob", LastName = "Harris", Email = "jacob.harris@example.com" },
			new Contact { FirstName = "Elizabeth", LastName = "Sanchez", Email = "elizabeth.sanchez@example.com" },
			new Contact { FirstName = "Ethan", LastName = "Clark", Email = "ethan.clark@example.com" },
			new Contact { FirstName = "Avery", LastName = "Ramirez", Email = "avery.ramirez@example.com" },
			new Contact { FirstName = "Sebastian", LastName = "Lewis", Email = "sebastian.lewis@example.com" },
			new Contact { FirstName = "Sofia", LastName = "Robinson", Email = "sofia.robinson@example.com" }
		};

		db.Contacts.AddRange(seedContacts);
		db.SaveChanges();
	}
}

app.Run();