using Domain.Entities; // Usamos la clase Contact.
using Microsoft.EntityFrameworkCore; // Para funciones de EF.
using System.Collections.Generic; // Para listas.
using System.Threading.Tasks; // Para operaciones asíncronas.

namespace DataAccess
{
    public class ContactRepository : IContactRepository // Cumple el contrato de la interfaz.
    {
        private readonly ContactsDbContext _context; // Almacena el DbContext para acceder a SQL Server.

        public ContactRepository(ContactsDbContext context) // Constructor: Recibe el DbContext.
        {
            _context = context; // Guarda el DbContext para usarlo.
        }

        public async Task<IEnumerable<Contact>> GetAllAsync() // Obtiene todos los contactos.
        {
            return await _context.Contacts.ToListAsync(); // Lee todos los contactos de la BD.
        }

        public async Task<Contact> GetByIdAsync(int id) // Busca un contacto por ID.
        {
            return await _context.Contacts.FindAsync(id); // Busca en la BD usando EF.
        }

        public async Task<Contact> AddAsync(Contact contact) // Añade un nuevo contacto.
        {
            _context.Contacts.Add(contact); // Añade a la colección de contactos.
            await _context.SaveChangesAsync(); // Guarda en SQL Server.
            return contact; // Devuelve el contacto añadido.
        }

        public async Task UpdateAsync(Contact contact) // Actualiza un contacto.
        {
            _context.Contacts.Update(contact); // Marca el contacto para actualizar.
            await _context.SaveChangesAsync(); // Guarda los cambios en la BD.
        }

        public async Task DeleteAsync(int id) // Elimina un contacto.
        {
            var contact = await GetByIdAsync(id); // Busca el contacto por ID.
            if (contact != null) // Si existe.
            {
                _context.Contacts.Remove(contact); // Lo elimina.
                await _context.SaveChangesAsync(); // Guarda los cambios.
            }
        }
    }
}