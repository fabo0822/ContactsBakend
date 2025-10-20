using BusinessLogic.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

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
			// Las validaciones de Data Annotations se ejecutan automáticamente
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

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
			// Validar que el ID sea válido
			if (id <= 0)
			{
				return BadRequest(new { message = "El ID debe ser un número positivo." });
			}

			// Las validaciones de Data Annotations se ejecutan automáticamente
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

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
			// Validar que el ID sea válido
			if (id <= 0)
			{
				return BadRequest(new { message = "El ID debe ser un número positivo." });
			}

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

		// 5. POST /contacts/upload-image - Subir imagen
		[HttpPost("upload-image")]
		[Consumes("multipart/form-data")]
		public async Task<ActionResult<object>> UploadImage([FromForm] IFormFile? file)
		{
			// Soporte robusto: intentar leer desde Request.Form.Files si no llegó con la clave esperada
			file ??= Request?.Form?.Files?.FirstOrDefault();

			// Validar que se haya enviado un archivo
			if (file == null || file.Length == 0)
			{
				return BadRequest(new { message = "No se ha enviado ningún archivo." });
			}

			// Validar tipo de archivo (solo imágenes)
			var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
			var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
			
			if (!allowedExtensions.Contains(fileExtension))
			{
				return BadRequest(new { 
					message = "Tipo de archivo no permitido. Solo se permiten: JPG, JPEG, PNG, GIF, BMP, WEBP" 
				});
			}

			// Validar tamaño del archivo (máximo 5MB)
			const long maxFileSize = 5 * 1024 * 1024; // 5MB
			if (file.Length > maxFileSize)
			{
				return BadRequest(new { 
					message = "El archivo es demasiado grande. El tamaño máximo permitido es 5MB." 
				});
			}

			try
			{
				// Crear directorio de imágenes si no existe
				var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
				if (!Directory.Exists(imagesDirectory))
				{
					Directory.CreateDirectory(imagesDirectory);
				}

				// Generar nombre único para el archivo
				var fileName = $"{Guid.NewGuid()}{fileExtension}";
				var filePath = Path.Combine(imagesDirectory, fileName);

				// Guardar el archivo
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}


				// Generar URL pública de la imagen (absoluta si es posible)
				var relativePath = $"/images/{fileName}";
				string imageUrl;
				if (Request?.Scheme != null && Request?.Host.HasValue == true)
				{
					imageUrl = $"{Request.Scheme}://{Request.Host}{relativePath}";
				}
				else
				{
					imageUrl = relativePath; // fallback
				}

				return Ok(new { 
					message = "Imagen subida exitosamente.",
					imageUrl = imageUrl,
					fileName = fileName,
					fileSize = file.Length
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { 
					message = "Error interno del servidor al subir la imagen.",
					error = ex.Message 
				});
			}
		}
	}
}
