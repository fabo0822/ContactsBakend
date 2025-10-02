using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Contact
    {
        [Key] // Esto indica que Id es la clave primaria (única para cada registro en la BD).
        public int Id { get; set; } // Id: Un número único auto-generado por la BD.

        [Required] // Obligatorio: No puedes guardar sin esto.
        [StringLength(100)] // Máximo 100 caracteres para evitar datos muy largos.
        public string FirstName { get; set; } // Primer nombre.

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } // Apellido.

        [Required]
        [StringLength(255)]
        [EmailAddress] // Valida que sea un email válido (ej: con @ y dominio).
        public string Email { get; set; } // Email, debe ser único (lo configuramos después).

        public bool IsFavorite { get; set; } = false; // ¿Es favorito? Predeterminado false.
    }
}