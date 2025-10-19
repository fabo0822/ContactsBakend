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

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
		{
			var contacts = await _contactService.GetAllAsync();
			return Ok(contacts);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Contact>> GetById(int id)
		{
			try
			{
				var contact = await _contactService.GetByIdAsync(id);
				return Ok(contact);
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<ActionResult<Contact>> Create([FromBody] Contact contact)
		{
			try
			{
				var created = await _contactService.AddAsync(contact);
				return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, [FromBody] Contact contact)
		{
			if (id != contact.Id)
				return BadRequest(new { message = "Route id and body id must match." });

			try
			{
				await _contactService.UpdateAsync(contact);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

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

		[HttpPatch("{id:int}/favorite")]
		public async Task<IActionResult> SetFavorite(int id, [FromQuery] bool value)
		{
			try
			{
				await _contactService.SetFavoriteAsync(id, value);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}
	}
}
