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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string FirstName { get; set; } // Primer nombre.

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        public string LastName { get; set; } // Apellido.

        [Required(ErrorMessage = "El email es obligatorio")]
        [StringLength(255, ErrorMessage = "El email no puede exceder 255 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
            ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } // Email, debe ser único (lo configuramos después).

        public bool IsFavorite { get; set; } = false; // ¿Es favorito? Predeterminado false.

        [StringLength(2048, ErrorMessage = "La URL de la imagen no puede exceder 2048 caracteres")]
        public string? ImageUrl { get; set; } // URL pública de la imagen del contacto
    }
}