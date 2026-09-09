using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaultContactos.Data;
using VaultContactos.Models;

namespace VaultContactos.Controllers.Api;

[ApiController]
[Route("api/contactos")]
public class ContactosApiController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Contacto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
    {
        var contactos = await context.Contactos
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Apellido)
            .ToListAsync();

        return Ok(contactos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Contacto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contacto>> GetContacto(int id)
    {
        var contacto = await context.Contactos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return contacto is null ? NotFound() : Ok(contacto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Contacto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Contacto>> PostContacto(Contacto contacto)
    {
        contacto.Id = 0;
        contacto.FechaCreacion = DateTime.UtcNow;
        context.Contactos.Add(contacto);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContacto), new { id = contacto.Id }, contacto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutContacto(int id, Contacto formulario)
    {
        if (id != formulario.Id)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el contacto." });
        }

        var contacto = await context.Contactos.FindAsync(id);
        if (contacto is null)
        {
            return NotFound();
        }

        contacto.Nombre = formulario.Nombre;
        contacto.Apellido = formulario.Apellido;
        contacto.Telefono = formulario.Telefono;
        contacto.Correo = formulario.Correo;
        contacto.Empresa = formulario.Empresa;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContacto(int id)
    {
        var contacto = await context.Contactos.FindAsync(id);
        if (contacto is null)
        {
            return NotFound();
        }

        context.Contactos.Remove(contacto);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<Contacto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Contacto>>> Buscar([FromQuery] string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return BadRequest(new { mensaje = "El parámetro texto es obligatorio." });
        }

        texto = texto.Trim();
        var contactos = await context.Contactos
            .AsNoTracking()
            .Where(c => EF.Functions.Like(c.Nombre, $"%{texto}%") ||
                        EF.Functions.Like(c.Apellido, $"%{texto}%"))
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Apellido)
            .ToListAsync();

        return Ok(contactos);
    }
}
