using BusinessLogic.Interfaces; // Para IContactService
using DataAccess; // Para IContactRepository
using Domain.Entities; // Para Contact
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository; // Conexión al repositorio

        public ContactService(IContactRepository repository)
        {
            _repository = repository; // Inyección de dependencias
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _repository.GetAllAsync(); // Obtiene todos los contactos
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null)
                throw new ArgumentException($"Contact with ID {id} not found.");
            return contact;
        }

        public async Task<Contact> AddAsync(Contact contact)
        {
            // Validar email único (el índice único en la BD también lo asegura)
            var existingContact = await _repository.GetAllAsync();
            if (existingContact.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("El email ya existe en el sistema.");

            return await _repository.AddAsync(contact); // Añade el contacto
        }

        public async Task<Contact> UpdateAsync(int id, Contact contact)
        {
            // Verificar que el contacto existe
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new ArgumentException($"No se encontró el contacto con ID {id}.");

            // Actualizar solo los campos que se proporcionan (cambios parciales)
            if (!string.IsNullOrWhiteSpace(contact.FirstName))
                existing.FirstName = contact.FirstName;
            
            if (!string.IsNullOrWhiteSpace(contact.LastName))
                existing.LastName = contact.LastName;
            
            if (!string.IsNullOrWhiteSpace(contact.Email))
            {
                // Validar email único (si cambió)
                if (!existing.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var contacts = await _repository.GetAllAsync();
                    if (contacts.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                        throw new ArgumentException("El email ya existe en el sistema.");
                }
                existing.Email = contact.Email;
            }

            // Actualizar campos opcionales
            existing.IsFavorite = contact.IsFavorite;
            existing.ImageUrl = contact.ImageUrl;

            await _repository.UpdateAsync(existing); // Actualiza el contacto
            return existing; // Devuelve el contacto actualizado
        }

        public async Task DeleteAsync(int id)
        {
            // Verificar que el contacto existe
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null)
                throw new ArgumentException($"No se encontró el contacto con ID {id}.");

            await _repository.DeleteAsync(id); // Elimina el contacto
        }

    }
}