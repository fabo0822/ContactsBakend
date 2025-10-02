using Microsoft.EntityFrameworkCore; // Importamos esto para usar las funciones de EF.
using Domain.Entities; // Importamos la clase Contact que creamos antes.

namespace DataAccess // Agrupa las clases de esta capa.
{
    public class ContactsDbContext : DbContext // Heredamos de DbContext, que es el "motor" de EF.
    {
        public DbSet<Contact> Contacts { get; set; } // Esto define una "tabla" llamada Contacts en la BD, basada en la clase Contact.

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options) 
        { 
            // Constructor: Recibe opciones de conexión a la BD (como la dirección y usuario).
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Método que configura la BD al crearse.
        {
            base.OnModelCreating(modelBuilder);
            // Hacemos que el campo Email sea único (no se pueden repetir emails).
            modelBuilder.Entity<Contact>().HasIndex(c => c.Email).IsUnique();
        }
    }
}