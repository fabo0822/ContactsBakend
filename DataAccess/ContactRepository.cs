using Domain.Entities; // Usamos la clase Contact.
using Microsoft.EntityFrameworkCore; // Para funciones de EF.
using System.Collections.Generic; // Para listas.
using System.Threading.Tasks; // Para operaciones asíncronas.

namespace DataAccess
{
    public class ContactRepository : IContactRepository // Implementa la interfaz anterior.
    {
        private readonly ContactsDbContext _context; // Almacena el contexto para usar la BD.

        public ContactRepository(ContactsDbContext context) // Constructor que recibe el contexto.
        {
            _context = context; // Guarda el contexto para usarlo en los métodos.
        }

        public async Task<IEnumerable<Contact>> GetAllAsync() // Obtiene todos los contactos.
        {
            return await _context.Contacts.ToListAsync(); // Usa EF para leer todos de la BD.
        }

        public async Task<Contact> GetByIdAsync(int id) // Busca un contacto por ID.
        {
            return await _context.Contacts.FindAsync(id); // Busca en la BD.
        }

        public async Task<Contact> AddAsync(Contact contact) // Añade un nuevo contacto.
        {
            _context.Contacts.Add(contact); // Añade a la colección.
            await _context.SaveChangesAsync(); // Guarda en PostgreSQL.
            return contact; // Devuelve el contacto añadido.
        }

        public async Task UpdateAsync(Contact contact) // Actualiza un contacto.
        {
            _context.Contacts.Update(contact); // Marca para actualizar.
            await _context.SaveChangesAsync(); // Guarda los cambios.
        }

        public async Task DeleteAsync(int id) // Elimina un contacto.
        {
            var contact = await GetByIdAsync(id); // Busca el contacto.
            if (contact != null) // Si existe.
            {
                _context.Contacts.Remove(contact); // Lo elimina.
                await _context.SaveChangesAsync(); // Guarda los cambios.
            }
        }
    }
}