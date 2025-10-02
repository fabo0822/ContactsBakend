using Domain.Entities; // Usamos la clase Contact.
using System.Collections.Generic; // Para listas de contactos.
using System.Threading.Tasks; // Para operaciones asíncronas (no bloquean el servidor).

namespace DataAccess
{
    public interface IContactRepository // Esta es una "promesa" de qué métodos tendrá el repositorio.
    {
        Task<IEnumerable<Contact>> GetAllAsync(); // Promete devolver todos los contactos.
        Task<Contact> GetByIdAsync(int id); // Promete buscar un contacto por ID.
        Task<Contact> AddAsync(Contact contact); // Promete añadir un nuevo contacto.
        Task UpdateAsync(Contact contact); // Promete actualizar un contacto.
        Task DeleteAsync(int id); // Promete eliminar un contacto por ID.
    }
}