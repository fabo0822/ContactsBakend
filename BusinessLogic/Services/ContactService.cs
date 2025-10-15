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
            // Validaciones
            if (string.IsNullOrWhiteSpace(contact.FirstName))
                throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(contact.LastName))
                throw new ArgumentException("Last name is required.");
            if (string.IsNullOrWhiteSpace(contact.Email))
                throw new ArgumentException("Email is required.");

            // Validar email único (simplificado, el índice único en la BD también lo asegura)
            var existingContact = await _repository.GetAllAsync();
            if (existingContact.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Email already exists.");

            return await _repository.AddAsync(contact); // Añade el contacto
        }

        public async Task UpdateAsync(Contact contact)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(contact.FirstName))
                throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(contact.LastName))
                throw new ArgumentException("Last name is required.");
            if (string.IsNullOrWhiteSpace(contact.Email))
                throw new ArgumentException("Email is required.");

            // Verificar que el contacto existe
            var existing = await _repository.GetByIdAsync(contact.Id);
            if (existing == null)
                throw new ArgumentException($"Contact with ID {contact.Id} not found.");

            // Validar email único (si cambió)
            if (!existing.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase))
            {
                var contacts = await _repository.GetAllAsync();
                if (contacts.Any(c => c.Email.Equals(contact.Email, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException("Email already exists.");
            }

            await _repository.UpdateAsync(contact); // Actualiza el contacto
        }

        public async Task DeleteAsync(int id)
        {
            // Verificar que el contacto existe
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null)
                throw new ArgumentException($"Contact with ID {id} not found.");

            await _repository.DeleteAsync(id); // Elimina el contacto
        }
    }
}