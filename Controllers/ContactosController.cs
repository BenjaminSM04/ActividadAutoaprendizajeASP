using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaultContactos.Data;
using VaultContactos.Models;

namespace VaultContactos.Controllers;

public class ContactosController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? buscar)
    {
        IQueryable<Contacto> consulta = context.Contactos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();
            consulta = consulta.Where(c =>
                EF.Functions.Like(c.Nombre, $"%{buscar}%") ||
                EF.Functions.Like(c.Apellido, $"%{buscar}%"));
        }

        ViewData["Buscar"] = buscar;
        var contactos = await consulta
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.Apellido)
            .ToListAsync();

        return View(contactos);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contacto = await context.Contactos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return contacto is null ? NotFound() : View(contacto);
    }

    public IActionResult Create() => View(new Contacto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nombre,Apellido,Telefono,Correo,Empresa")] Contacto contacto)
    {
        if (!ModelState.IsValid)
        {
            return View(contacto);
        }

        contacto.FechaCreacion = DateTime.UtcNow;
        context.Add(contacto);
        await context.SaveChangesAsync();
        TempData["Mensaje"] = "El contacto se guardó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contacto = await context.Contactos.FindAsync(id);
        return contacto is null ? NotFound() : View(contacto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Nombre,Apellido,Telefono,Correo,Empresa")] Contacto formulario)
    {
        if (id != formulario.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(formulario);
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
        TempData["Mensaje"] = "Los cambios se guardaron correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contacto = await context.Contactos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return contacto is null ? NotFound() : View(contacto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contacto = await context.Contactos.FindAsync(id);
        if (contacto is null)
        {
            return NotFound();
        }

        context.Contactos.Remove(contacto);
        await context.SaveChangesAsync();
        TempData["Mensaje"] = "El contacto se eliminó del vault.";
        return RedirectToAction(nameof(Index));
    }
}
