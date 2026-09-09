using System.ComponentModel.DataAnnotations;

namespace VaultContactos.Models;

public class Contacto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "El nombre no puede superar los 60 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(60, ErrorMessage = "El apellido no puede superar los 60 caracteres.")]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Ingresa un número de teléfono válido.")]
    [StringLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los 120 caracteres.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La empresa es obligatoria.")]
    [StringLength(100, ErrorMessage = "La empresa no puede superar los 100 caracteres.")]
    [Display(Name = "Empresa")]
    public string Empresa { get; set; } = string.Empty;

    [Display(Name = "Fecha de creación")]
    public DateTime FechaCreacion { get; set; }

    public string NombreCompleto => $"{Nombre} {Apellido}";

    public string Iniciales => $"{PrimeraInicial(Nombre)}{PrimeraInicial(Apellido)}";

    private static string PrimeraInicial(string valor) =>
        string.IsNullOrWhiteSpace(valor) ? string.Empty : valor.Trim()[0].ToString().ToUpperInvariant();
}
