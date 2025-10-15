using DataAccess; // Para usar ContactsDbContext
using Microsoft.AspNetCore.Mvc; // Para crear el controlador
using Microsoft.EntityFrameworkCore; // Para consultas a la BD

namespace Api.Controllers
{
    [Route("api/[controller]")] // Define la ruta como /api/Test
    [ApiController] // Indica que es un controlador de API
    public class TestController : ControllerBase
    {
        private readonly ContactsDbContext _context; // Conexión a la BD

        public TestController(ContactsDbContext context)
        {
            _context = context; // Inyección de dependencias
        }

        [HttpGet] // Responde a solicitudes GET
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _context.Contacts.ToListAsync(); // Lee todos los contactos
            return Ok(contacts); // Devuelve la lista en formato JSON
        }
    }
}