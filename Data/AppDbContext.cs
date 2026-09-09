using Microsoft.EntityFrameworkCore;
using VaultContactos.Models;

namespace VaultContactos.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Contacto> Contactos => Set<Contacto>();
}
