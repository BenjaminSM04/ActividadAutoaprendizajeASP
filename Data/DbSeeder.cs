using Microsoft.EntityFrameworkCore;
using VaultContactos.Models;

namespace VaultContactos.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Contactos.AnyAsync())
        {
            return;
        }

        var fechaBase = DateTime.UtcNow;
        context.Contactos.AddRange(
            new Contacto
            {
                Nombre = "Benjamin",
                Apellido = "Saenz",
                Telefono = "+591 71234567",
                Correo = "benjamin.saenz@nova.bo",
                Empresa = "Nova Digital",
                FechaCreacion = fechaBase.AddDays(-12)
            },
            new Contacto
            {
                Nombre = "Samuel",
                Apellido = "Vicente",
                Telefono = "+591 72345678",
                Correo = "Samuel.vicente@andina.bo",
                Empresa = "Andina Studio",
                FechaCreacion = fechaBase.AddDays(-7)
            },
            new Contacto
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = "+591 73456789",
                Correo = "juan.perez@horizonte.bo",
                Empresa = "Horizonte Tech",
                FechaCreacion = fechaBase.AddDays(-2)
            });

        await context.SaveChangesAsync();
    }
}
