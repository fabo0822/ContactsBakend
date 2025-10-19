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
	}
}
