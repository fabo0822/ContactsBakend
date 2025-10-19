using BusinessLogic.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContactsController : ControllerBase
	{
		private readonly IContactService _contactService;

		public ContactsController(IContactService contactService)
		{
			_contactService = contactService;
		}

		// 1. GET /contacts - Obtener todos los contactos
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
		{
			var contacts = await _contactService.GetAllAsync();
			return Ok(contacts);
		}

		// 2. POST /contacts - Crear nuevo contacto
		[HttpPost]
		public async Task<ActionResult<Contact>> Create([FromBody] Contact contact)
		{
			try
			{
				var created = await _contactService.AddAsync(contact);
				return CreatedAtAction(nameof(GetAll), new { }, created);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		// 3. PUT /contacts/{id} - Actualizar contacto (acepta cambios parciales)
		[HttpPut("{id:int}")]
		public async Task<ActionResult<Contact>> Update(int id, [FromBody] Contact contact)
		{
			try
			{
				var updated = await _contactService.UpdateAsync(id, contact);
				return Ok(updated);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		// 4. DELETE /contacts/{id} - Eliminar contacto
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _contactService.DeleteAsync(id);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}
	}
}
