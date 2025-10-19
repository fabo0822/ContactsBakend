using Domain.Entities; // Para usar la clase Contact
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllAsync(); // Obtener todos los contactos
        Task<Contact> GetByIdAsync(int id); // Obtener un contacto por ID
        Task<Contact> AddAsync(Contact contact); // Añadir un contacto
        Task UpdateAsync(Contact contact); // Actualizar un contacto
        Task DeleteAsync(int id); // Eliminar un contacto
        Task SetFavoriteAsync(int id, bool isFavorite); // Marcar o desmarcar favorito
    }
}